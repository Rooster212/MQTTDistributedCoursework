using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MQTTProtocol;
using MQTTProtocol.Packets;

namespace Client
{
    public partial class ClientForm : Form
    {
        private string _connectionHost;
        private int _connectionPort;
        private string _connectionClientId;
        private int _connectionTimeout;
        private List<ListViewItem> _listViewItems = new List<ListViewItem>();
        private readonly ClientManager _clientManager;
        private ConcurrentDictionary<int,ClientManager> testClients = new ConcurrentDictionary<int, ClientManager>();

        private bool _showPings = true;

        public bool testing = false;

        public ClientForm()
        {
            _clientManager = new ClientManager();
            InitializeComponent();
            ResetUiDisconnected();
            InitListView();
            
            //_clientManager.Storage = new ClientStorage();
            var thread = new Thread(UpdateServerList);
            thread.Start();
        }

        private void BindData()
        {
            subscriptionsListBox.DataSource = null;
            subscriptionsListBox.DataSource = _clientManager.SubscribedTopicsList;
        }

        public void ResetUiLoadTestOn()
        {
            ConnectButton.Enabled = false;
            connectedStatus.Text = @"Testing";

            // disable other buttons
            disconnectButton.Enabled = false;
            addSubscriptionButton.Enabled = false;
            addSubscriptionTextBox.Enabled = false;
            removeSubscriptionButton.Enabled = false;
            localhostButton.Enabled = false;

            //enable text boxes for connecting
            clientIdTextBox.Enabled = false;
            connectionAddressTextBox.Enabled = false;
            connectionPortTextBox.Enabled = false;
            connectionTimeoutTextBox.Enabled = false;
            loadTestButton.Enabled = false;

            publishToComboBox.Enabled = false;
            publishPayloadBox.Enabled = false;
        }

        public void ResetUiDisconnected()
        {
            ConnectButton.Enabled = true;
            connectedStatus.Text = @"Disconnected";

            // disable other buttons
            disconnectButton.Enabled = false;
            addSubscriptionButton.Enabled = false;
            addSubscriptionTextBox.Enabled = false;
            removeSubscriptionButton.Enabled = false;
            
            //enable connecting
            clientIdTextBox.Enabled = true;
            connectionAddressTextBox.Enabled = true;
            connectionPortTextBox.Enabled = true;
            connectionTimeoutTextBox.Enabled = true;
            localhostButton.Enabled = true;
            loadTestButton.Enabled = true;

            publishToComboBox.Enabled = false;
            publishPayloadBox.Enabled = false;

            _clientManager.SubscribedTopicsList = new List<string>();
            BindData();
        }

        public void ResetUiConnected()
        {
            ConnectButton.Enabled = false;
            connectedStatus.Text = @"Connected";

            // enable other
            disconnectButton.Enabled = true;
            addSubscriptionButton.Enabled = true;
            addSubscriptionTextBox.Enabled = true;

            //disable text boxes for connecting
            clientIdTextBox.Enabled = false;
            connectionAddressTextBox.Enabled = false;
            connectionPortTextBox.Enabled = false;
            connectionTimeoutTextBox.Enabled = false;
            localhostButton.Enabled = false;
            loadTestButton.Enabled = false;

            publishToComboBox.Enabled = true;
            publishPayloadBox.Enabled = true;

            ClearPublishArea();
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            try
            {
                ConnectButton.Enabled = false;
                loadingImage.Visible = true;
                _connectionHost = connectionAddressTextBox.Text.Trim();
                _connectionPort = int.Parse(connectionPortTextBox.Text.Trim());
                _connectionClientId = clientIdTextBox.Text.Trim();
                _connectionTimeout = int.Parse(connectionTimeoutTextBox.Text.Trim());
                BackgroundWorker bg = new BackgroundWorker();
                bg.DoWork += DoConnect;
                bg.RunWorkerCompleted += DoConnectCompleted;
                bg.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"Exception: " + ex.Message, @"Error occurred");
                ResetUiDisconnected();
            }
        }

        private void DoConnect(object sender, DoWorkEventArgs e)
        {
            _clientManager.Init(
                    _connectionHost,
                    _connectionPort,
                    _connectionClientId,
                    _connectionTimeout,
                    this, false);
        }

        private void DoConnectCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            loadingImage.Invoke(new MethodInvoker(PostConnect));

            if (_clientManager.Communication == null)
            {
                MessageBox.Show(@"Error occurred whilst connecting");
                Invoke(new MethodInvoker(() => UpdateLastPingTimeLabel("Error connecting at ", DateTime.Now)));
            }
            else if (_clientManager.Communication.Connected)
            {
                ResetUiConnected();
                Invoke(new MethodInvoker(() => UpdateLastPingTimeLabel("Connected at ", DateTime.Now)));
            }
        }

        private void PostConnect()
        {
            loadingImage.Visible = false;
            if (_clientManager.Communication == null || !_clientManager.Communication.Connected)
            {
                ConnectButton.Enabled = true;
            }
        }

        private void disconnectButton_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => _clientManager.Disconnect());
            thread.Start();
        }

        private void addSubscriptionButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(addSubscriptionTextBox.Text)) return; // we don't want to add empty subscriptions
            var subscribePacket = new Subscribe(_clientManager.NextPacketId, addSubscriptionTextBox.Text.Trim());
            _clientManager.Communication.AddPacketToSendQueue(subscribePacket);
            _clientManager.SubscribedTopicsList.Add(addSubscriptionTextBox.Text.Trim());
            addSubscriptionTextBox.Text = string.Empty;
            BindData();
            UpdateSubscribedComboBox();
        }

        private void removeSubscriptionButton_Click(object sender, EventArgs e)
        {
            removeSubscriptionButton.Enabled = false;
            if (subscriptionsListBox.SelectedValue == null) return;
            string selectedTopic = subscriptionsListBox.SelectedValue.ToString();
            _clientManager.Communication.AddPacketToSendQueue(new Unsubscribe(_clientManager.NextPacketId, selectedTopic));
            _clientManager.SubscribedTopicsList.Remove(selectedTopic);
            BindData();
            UpdateSubscribedComboBox();
        }

        private void SubscriptionListBoxSelected(object sender, EventArgs e)
        {
            if (subscriptionsListBox.SelectedIndex != -1)
                removeSubscriptionButton.Enabled = true;
            else
                removeSubscriptionButton.Enabled = false;
        }

        private void InitListView()
        {
            outputListView.LabelEdit = false;
            outputListView.FullRowSelect = true;
            outputListView.AllowColumnReorder = false;
        }

        public void AddListViewItem(bool recieve, MQTTPacketTypes packetType, int? packetId, string topic, string data, DateTime time)
        {
            if (testing) return;
            // type id topic data time
            ListViewItem newItem = new ListViewItem(new[]
            {
                (packetId != null ? ((int) packetId).ToString() : ""),
                GetPacketTypeString(packetType),
                topic,
                data,
                time.ToShortDateString() + " " + time.ToLongTimeString()
            }) {BackColor = recieve ? Color.FromArgb(148, 255, 114, 114) : Color.FromArgb(148, 61, 218, 61)};
            _listViewItems.Add(newItem);

            if (packetType == MQTTPacketTypes.PINGRESP)
                UpdateLastPingTimeLabel(@"Last successful ping at ",time);

            if (autoScrollCheckBox.Checked)
                newItem.EnsureVisible();

            if (_showPings // if it is a ping and we are allowed to show it just let it through
                || (packetType != MQTTPacketTypes.PINGREQ && packetType != MQTTPacketTypes.PINGRESP)) // or if it isn't a ping
            {
                outputListView.Invoke((MethodInvoker) delegate
                {
                    AddListViewItem(newItem);
                });
            }
        }

        private void AddListViewItem(ListViewItem item)
        {
            outputListView.Items.Add(item);
        }

        private void UpdateLastPingTimeLabel(string label, DateTime time)
        {
            if (testing) return;
            try
            {
                lastPingLabel.Text = label + time.ToLongTimeString();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Trace.WriteLine("Some error in WinForms happened that I can't find the cause of...");
            }
            
        }

        private string GetPacketTypeString(MQTTPacketTypes packetType)
        {
            string packetTypeString;
            switch (packetType)
            {
                case MQTTPacketTypes.CONNACK:
                    packetTypeString = "Connack";
                    break;
                case MQTTPacketTypes.PUBLISH:
                    packetTypeString = "Publish";
                    break;
                case MQTTPacketTypes.SUBACK:
                    packetTypeString = "Suback";
                    break;
                case MQTTPacketTypes.UNSUBACK:
                    packetTypeString = "Unsuback";
                    break;
                case MQTTPacketTypes.PINGRESP:
                    packetTypeString = "Pingresp";
                    break;
                case MQTTPacketTypes.DISCONNECT:
                    packetTypeString = "Disconnect";
                    break;
                case MQTTPacketTypes.UNSUBSCRIBE:
                    packetTypeString = "Unsubscribe";
                    break;
                case MQTTPacketTypes.SUBSCRIBE:
                    packetTypeString = "Subscribe";
                    break;
                case MQTTPacketTypes.CONNECT:
                    packetTypeString = "Connect";
                    break;
                case MQTTPacketTypes.PINGREQ:
                    packetTypeString = "Pingreq";
                    break;
                case MQTTPacketTypes.PUBACK:
                    packetTypeString = "Puback";
                    break;
                default:
                    packetTypeString = "Unknown/Unimplemented";
                    break;

            }
            return packetTypeString;
        }

        private void clearPublishButton_Click(object sender, EventArgs e)
        {
            ClearPublishArea();
            UpdateSubscribedComboBox();
        }

        private void Publish_Click(object sender, EventArgs e)
        {
            var publishPacket = new Publish(publishToComboBox.Text, publishPayloadBox.Text);
            _clientManager.Communication.AddPacketToSendQueue(publishPacket);
            ClearPublishArea();
            UpdateSubscribedComboBox();
        }

        private void ClearPublishArea()
        {
            publishPayloadBox.Text = string.Empty;
            publishToComboBox.Text = string.Empty;
            UpdateSubscribedComboBox();
        }

        private void UpdateSubscribedComboBox()
        {
            List<string> publishToList = new List<string>();

            if (_clientManager.SubscribedTopicsList == null || _clientManager.SubscribedTopicsList.Count <= 0) return;

            Parallel.ForEach(_clientManager.SubscribedTopicsList, item => publishToList.Add(item));
            publishToComboBox.DataSource = new BindingSource(publishToList, null);
        }

        private void UpdateServerList()
        {
            Invoke(new Action(() =>loadServerListGraphic.Visible = true));
            
            try
            {
                string[] serverList = ClientCommunication.GetServers();
                Invoke(new Action(() =>
                {
                    Thread.Sleep(MQTTBase.THREAD_SLEEP_REST_TIME);
                    connectionAddressTextBox.Items.Clear();
                    if (serverList != null) connectionAddressTextBox.Items.AddRange(serverList);
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"Error getting server list from server",@"Error");
                Trace.WriteLine(ex.Message);
            }
            Invoke(new Action(() => loadServerListGraphic.Visible = false));
        }

        private void clearOutputButton_Click(object sender, EventArgs e)
        {
            outputListView.Items.Clear();
        }

        private void showPingsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var checkbox = (CheckBox) sender;
            _showPings = checkbox.Checked;
        }

        private void MainForm_Closed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void localhostButton_Click(object sender, EventArgs e)
        {
            Invoke(new Action(() => connectionAddressTextBox.Text = @"127.0.0.1"));
        }

        private void loadTestButton_Click(object sender, EventArgs e)
        {
            int numOfLoadTestClients;

            if (int.TryParse(loadTestTextBox.Text, out numOfLoadTestClients))
            {
                var thread = new Thread(() => PerformTest(numOfLoadTestClients));
                thread.Start();
            }
            else
            {
                MessageBox.Show(@"Invalid number for number of test clients",@"Error");
            }
        }

        private void PerformTest(int numOfLoadTestClients)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            this.Invoke(new Action(ResetUiLoadTestOn));
            testing = true;
            for (int i = 0; i < numOfLoadTestClients; i++)
            {
                var cM = new ClientManager();
                string host = @"127.0.0.1";
                int port = 5000;
                string clientId = "testClient"+i;
                int timeout = 2;
                this.Invoke(new Action(() =>
                {
                    host = connectionAddressTextBox.Text.Trim();
                    port = int.Parse(connectionPortTextBox.Text.Trim());
                    timeout = int.Parse(connectionTimeoutTextBox.Text.Trim()); 
                }));
                
                cM.Init(
                    host,
                    port,
                    clientId,
                    timeout,
                    this, true);
                testClients.GetOrAdd(i, cM);
                Thread.Sleep(MQTTBase.THREAD_SLEEP_REST_TIME);
            }
            // wait a bit
            Thread.Sleep(500);
            for (int i = 0; i < numOfLoadTestClients; i++)
            {
                ClientManager manager;
                if (testClients.TryGetValue(i, out manager))
                {
                    manager.Disconnect(true);
                }
            }
            stopwatch.Stop();
            MessageBox.Show(String.Format("Load test completed in {0}ms with {1} clients", stopwatch.ElapsedMilliseconds, numOfLoadTestClients),
                @"Load test complete!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            // wait a bit
            Thread.Sleep(1000);
            testing = false;
            Invoke(new Action(ResetUiDisconnected));
        }

        private void label7_Click(object sender, EventArgs e)
        {
            Invoke(new Action(() => addSubscriptionTextBox.Text = @"sensors/ldr/1"));
        }

        private void label8_Click(object sender, EventArgs e)
        {
            Invoke(new Action(() => addSubscriptionTextBox.Text = @"actuators/led/01"));
            Invoke(new Action(() => publishToComboBox.Text = @"actuators/led/01"));
        }
    }
}
