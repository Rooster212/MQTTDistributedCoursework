namespace Client
{
    partial class ClientForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClientForm));
            this.connectionSettingsGroupBox = new System.Windows.Forms.GroupBox();
            this.loadTestTextBox = new System.Windows.Forms.TextBox();
            this.loadTestButton = new System.Windows.Forms.Button();
            this.localhostButton = new System.Windows.Forms.Button();
            this.loadServerListGraphic = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            this.connectionTimeoutTextBox = new System.Windows.Forms.TextBox();
            this.loadingImage = new System.Windows.Forms.PictureBox();
            this.connectionAddressTextBox = new System.Windows.Forms.ComboBox();
            this.disconnectButton = new System.Windows.Forms.Button();
            this.clientIdTextBox = new System.Windows.Forms.TextBox();
            this.ConnectButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.connectionPortTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.addSubscriptionTextBox = new System.Windows.Forms.TextBox();
            this.addSubscriptionButton = new System.Windows.Forms.Button();
            this.subscriptionsListBox = new System.Windows.Forms.ListBox();
            this.removeSubscriptionButton = new System.Windows.Forms.Button();
            this.outputListView = new System.Windows.Forms.ListView();
            this.ID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Type = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Topic = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Data = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Time = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.subscriptionsGroupBox = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.publishGroupBox = new System.Windows.Forms.GroupBox();
            this.Publish = new System.Windows.Forms.Button();
            this.clearPublishButton = new System.Windows.Forms.Button();
            this.payloadlabel = new System.Windows.Forms.Label();
            this.publishPayloadBox = new System.Windows.Forms.TextBox();
            this.toTopicLabel = new System.Windows.Forms.Label();
            this.publishToComboBox = new System.Windows.Forms.ComboBox();
            this.mqttClientStatusStrip = new System.Windows.Forms.StatusStrip();
            this.connectedStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.spacerLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.lastPingLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.outputContainer = new System.Windows.Forms.GroupBox();
            this.autoScrollCheckBox = new System.Windows.Forms.CheckBox();
            this.clearOutputButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.showPingsCheckBox = new System.Windows.Forms.CheckBox();
            this.connectionSettingsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.loadServerListGraphic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.loadingImage)).BeginInit();
            this.subscriptionsGroupBox.SuspendLayout();
            this.publishGroupBox.SuspendLayout();
            this.mqttClientStatusStrip.SuspendLayout();
            this.outputContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // connectionSettingsGroupBox
            // 
            this.connectionSettingsGroupBox.Controls.Add(this.loadTestTextBox);
            this.connectionSettingsGroupBox.Controls.Add(this.loadTestButton);
            this.connectionSettingsGroupBox.Controls.Add(this.localhostButton);
            this.connectionSettingsGroupBox.Controls.Add(this.loadServerListGraphic);
            this.connectionSettingsGroupBox.Controls.Add(this.label6);
            this.connectionSettingsGroupBox.Controls.Add(this.connectionTimeoutTextBox);
            this.connectionSettingsGroupBox.Controls.Add(this.loadingImage);
            this.connectionSettingsGroupBox.Controls.Add(this.connectionAddressTextBox);
            this.connectionSettingsGroupBox.Controls.Add(this.disconnectButton);
            this.connectionSettingsGroupBox.Controls.Add(this.clientIdTextBox);
            this.connectionSettingsGroupBox.Controls.Add(this.ConnectButton);
            this.connectionSettingsGroupBox.Controls.Add(this.label3);
            this.connectionSettingsGroupBox.Controls.Add(this.connectionPortTextBox);
            this.connectionSettingsGroupBox.Controls.Add(this.label2);
            this.connectionSettingsGroupBox.Controls.Add(this.label1);
            this.connectionSettingsGroupBox.Location = new System.Drawing.Point(12, 12);
            this.connectionSettingsGroupBox.Name = "connectionSettingsGroupBox";
            this.connectionSettingsGroupBox.Size = new System.Drawing.Size(355, 156);
            this.connectionSettingsGroupBox.TabIndex = 0;
            this.connectionSettingsGroupBox.TabStop = false;
            this.connectionSettingsGroupBox.Text = "Connection Settings";
            // 
            // loadTestTextBox
            // 
            this.loadTestTextBox.Location = new System.Drawing.Point(135, 125);
            this.loadTestTextBox.Name = "loadTestTextBox";
            this.loadTestTextBox.Size = new System.Drawing.Size(27, 20);
            this.loadTestTextBox.TabIndex = 14;
            this.loadTestTextBox.Text = "50";
            // 
            // loadTestButton
            // 
            this.loadTestButton.Location = new System.Drawing.Point(168, 123);
            this.loadTestButton.Name = "loadTestButton";
            this.loadTestButton.Size = new System.Drawing.Size(75, 23);
            this.loadTestButton.TabIndex = 13;
            this.loadTestButton.Text = "Load Test";
            this.loadTestButton.UseVisualStyleBackColor = true;
            this.loadTestButton.Click += new System.EventHandler(this.loadTestButton_Click);
            // 
            // localhostButton
            // 
            this.localhostButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.localhostButton.Location = new System.Drawing.Point(125, 15);
            this.localhostButton.Name = "localhostButton";
            this.localhostButton.Size = new System.Drawing.Size(41, 23);
            this.localhostButton.TabIndex = 12;
            this.localhostButton.Text = "local";
            this.localhostButton.UseVisualStyleBackColor = true;
            this.localhostButton.Click += new System.EventHandler(this.localhostButton_Click);
            // 
            // loadServerListGraphic
            // 
            this.loadServerListGraphic.Cursor = System.Windows.Forms.Cursors.AppStarting;
            this.loadServerListGraphic.Image = global::Client.Properties.Resources.ajax_loader;
            this.loadServerListGraphic.Location = new System.Drawing.Point(168, 19);
            this.loadServerListGraphic.Name = "loadServerListGraphic";
            this.loadServerListGraphic.Size = new System.Drawing.Size(16, 16);
            this.loadServerListGraphic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.loadServerListGraphic.TabIndex = 11;
            this.loadServerListGraphic.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 100);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(139, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Connect Timeout (Seconds)";
            // 
            // connectionTimeoutTextBox
            // 
            this.connectionTimeoutTextBox.Location = new System.Drawing.Point(187, 97);
            this.connectionTimeoutTextBox.Name = "connectionTimeoutTextBox";
            this.connectionTimeoutTextBox.Size = new System.Drawing.Size(161, 20);
            this.connectionTimeoutTextBox.TabIndex = 9;
            this.connectionTimeoutTextBox.Text = "2";
            // 
            // loadingImage
            // 
            this.loadingImage.Cursor = System.Windows.Forms.Cursors.AppStarting;
            this.loadingImage.Image = global::Client.Properties.Resources.ajax_loader;
            this.loadingImage.Location = new System.Drawing.Point(81, 126);
            this.loadingImage.Name = "loadingImage";
            this.loadingImage.Size = new System.Drawing.Size(16, 16);
            this.loadingImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.loadingImage.TabIndex = 8;
            this.loadingImage.TabStop = false;
            this.loadingImage.Visible = false;
            // 
            // connectionAddressTextBox
            // 
            this.connectionAddressTextBox.FormattingEnabled = true;
            this.connectionAddressTextBox.Location = new System.Drawing.Point(186, 17);
            this.connectionAddressTextBox.Name = "connectionAddressTextBox";
            this.connectionAddressTextBox.Size = new System.Drawing.Size(162, 21);
            this.connectionAddressTextBox.TabIndex = 7;
            this.connectionAddressTextBox.Text = "mqtt.dcs.net.hull.ac.uk";
            // 
            // disconnectButton
            // 
            this.disconnectButton.Location = new System.Drawing.Point(275, 123);
            this.disconnectButton.Name = "disconnectButton";
            this.disconnectButton.Size = new System.Drawing.Size(73, 23);
            this.disconnectButton.TabIndex = 5;
            this.disconnectButton.Text = "Disconnect";
            this.disconnectButton.UseVisualStyleBackColor = true;
            this.disconnectButton.Click += new System.EventHandler(this.disconnectButton_Click);
            // 
            // clientIdTextBox
            // 
            this.clientIdTextBox.Location = new System.Drawing.Point(187, 70);
            this.clientIdTextBox.Name = "clientIdTextBox";
            this.clientIdTextBox.Size = new System.Drawing.Size(161, 20);
            this.clientIdTextBox.TabIndex = 4;
            this.clientIdTextBox.Text = "201102762";
            // 
            // ConnectButton
            // 
            this.ConnectButton.Location = new System.Drawing.Point(9, 123);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(66, 23);
            this.ConnectButton.TabIndex = 1;
            this.ConnectButton.Text = "Connect";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Client ID";
            // 
            // connectionPortTextBox
            // 
            this.connectionPortTextBox.Location = new System.Drawing.Point(187, 44);
            this.connectionPortTextBox.Name = "connectionPortTextBox";
            this.connectionPortTextBox.Size = new System.Drawing.Size(161, 20);
            this.connectionPortTextBox.TabIndex = 2;
            this.connectionPortTextBox.Text = "5000";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Connection Port";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Connection Address/IP";
            // 
            // addSubscriptionTextBox
            // 
            this.addSubscriptionTextBox.Location = new System.Drawing.Point(9, 20);
            this.addSubscriptionTextBox.Name = "addSubscriptionTextBox";
            this.addSubscriptionTextBox.Size = new System.Drawing.Size(185, 20);
            this.addSubscriptionTextBox.TabIndex = 3;
            this.addSubscriptionTextBox.Text = "sensors/ldr/1";
            // 
            // addSubscriptionButton
            // 
            this.addSubscriptionButton.Location = new System.Drawing.Point(200, 19);
            this.addSubscriptionButton.Name = "addSubscriptionButton";
            this.addSubscriptionButton.Size = new System.Drawing.Size(148, 23);
            this.addSubscriptionButton.TabIndex = 4;
            this.addSubscriptionButton.Text = "Add Subscription";
            this.addSubscriptionButton.UseVisualStyleBackColor = true;
            this.addSubscriptionButton.Click += new System.EventHandler(this.addSubscriptionButton_Click);
            // 
            // subscriptionsListBox
            // 
            this.subscriptionsListBox.FormattingEnabled = true;
            this.subscriptionsListBox.Location = new System.Drawing.Point(9, 46);
            this.subscriptionsListBox.Name = "subscriptionsListBox";
            this.subscriptionsListBox.Size = new System.Drawing.Size(185, 186);
            this.subscriptionsListBox.TabIndex = 5;
            this.subscriptionsListBox.SelectedValueChanged += new System.EventHandler(this.SubscriptionListBoxSelected);
            // 
            // removeSubscriptionButton
            // 
            this.removeSubscriptionButton.Location = new System.Drawing.Point(200, 48);
            this.removeSubscriptionButton.Name = "removeSubscriptionButton";
            this.removeSubscriptionButton.Size = new System.Drawing.Size(148, 23);
            this.removeSubscriptionButton.TabIndex = 6;
            this.removeSubscriptionButton.Text = "Remove Subscription";
            this.removeSubscriptionButton.UseVisualStyleBackColor = true;
            this.removeSubscriptionButton.Click += new System.EventHandler(this.removeSubscriptionButton_Click);
            // 
            // outputListView
            // 
            this.outputListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ID,
            this.Type,
            this.Topic,
            this.Data,
            this.Time});
            this.outputListView.Location = new System.Drawing.Point(15, 39);
            this.outputListView.Name = "outputListView";
            this.outputListView.Size = new System.Drawing.Size(622, 570);
            this.outputListView.TabIndex = 7;
            this.outputListView.UseCompatibleStateImageBehavior = false;
            this.outputListView.View = System.Windows.Forms.View.Details;
            // 
            // ID
            // 
            this.ID.DisplayIndex = 1;
            this.ID.Text = "ID";
            // 
            // Type
            // 
            this.Type.DisplayIndex = 0;
            this.Type.Text = "Type";
            this.Type.Width = 78;
            // 
            // Topic
            // 
            this.Topic.Text = "Topic";
            this.Topic.Width = 148;
            // 
            // Data
            // 
            this.Data.Text = "Data";
            this.Data.Width = 192;
            // 
            // Time
            // 
            this.Time.Text = "Time";
            this.Time.Width = 139;
            // 
            // subscriptionsGroupBox
            // 
            this.subscriptionsGroupBox.Controls.Add(this.label10);
            this.subscriptionsGroupBox.Controls.Add(this.label9);
            this.subscriptionsGroupBox.Controls.Add(this.label8);
            this.subscriptionsGroupBox.Controls.Add(this.label7);
            this.subscriptionsGroupBox.Controls.Add(this.subscriptionsListBox);
            this.subscriptionsGroupBox.Controls.Add(this.addSubscriptionTextBox);
            this.subscriptionsGroupBox.Controls.Add(this.addSubscriptionButton);
            this.subscriptionsGroupBox.Controls.Add(this.removeSubscriptionButton);
            this.subscriptionsGroupBox.Location = new System.Drawing.Point(12, 174);
            this.subscriptionsGroupBox.Name = "subscriptionsGroupBox";
            this.subscriptionsGroupBox.Size = new System.Drawing.Size(355, 244);
            this.subscriptionsGroupBox.TabIndex = 9;
            this.subscriptionsGroupBox.TabStop = false;
            this.subscriptionsGroupBox.Text = "Manage Subscriptions";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(229, 161);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(84, 13);
            this.label10.TabIndex = 10;
            this.label10.Text = "RGB LED Topic";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(222, 84);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(96, 13);
            this.label9.TabIndex = 9;
            this.label9.Text = "Light Sensor Topic";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(228, 177);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(87, 13);
            this.label8.TabIndex = 8;
            this.label8.Text = "actuators/led/01";
            this.label8.Click += new System.EventHandler(this.label8_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(236, 97);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(70, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "sensors/ldr/1";
            this.label7.Click += new System.EventHandler(this.label7_Click);
            // 
            // publishGroupBox
            // 
            this.publishGroupBox.Controls.Add(this.Publish);
            this.publishGroupBox.Controls.Add(this.clearPublishButton);
            this.publishGroupBox.Controls.Add(this.payloadlabel);
            this.publishGroupBox.Controls.Add(this.publishPayloadBox);
            this.publishGroupBox.Controls.Add(this.toTopicLabel);
            this.publishGroupBox.Controls.Add(this.publishToComboBox);
            this.publishGroupBox.Location = new System.Drawing.Point(12, 424);
            this.publishGroupBox.Name = "publishGroupBox";
            this.publishGroupBox.Size = new System.Drawing.Size(355, 203);
            this.publishGroupBox.TabIndex = 10;
            this.publishGroupBox.TabStop = false;
            this.publishGroupBox.Text = "Publish";
            // 
            // Publish
            // 
            this.Publish.Location = new System.Drawing.Point(200, 174);
            this.Publish.Name = "Publish";
            this.Publish.Size = new System.Drawing.Size(148, 23);
            this.Publish.TabIndex = 5;
            this.Publish.Text = "Publish";
            this.Publish.UseVisualStyleBackColor = true;
            this.Publish.Click += new System.EventHandler(this.Publish_Click);
            // 
            // clearPublishButton
            // 
            this.clearPublishButton.Location = new System.Drawing.Point(9, 174);
            this.clearPublishButton.Name = "clearPublishButton";
            this.clearPublishButton.Size = new System.Drawing.Size(148, 23);
            this.clearPublishButton.TabIndex = 4;
            this.clearPublishButton.Text = "Clear";
            this.clearPublishButton.UseVisualStyleBackColor = true;
            this.clearPublishButton.Click += new System.EventHandler(this.clearPublishButton_Click);
            // 
            // payloadlabel
            // 
            this.payloadlabel.AutoSize = true;
            this.payloadlabel.Location = new System.Drawing.Point(9, 48);
            this.payloadlabel.Name = "payloadlabel";
            this.payloadlabel.Size = new System.Drawing.Size(45, 13);
            this.payloadlabel.TabIndex = 3;
            this.payloadlabel.Text = "Payload";
            // 
            // publishPayloadBox
            // 
            this.publishPayloadBox.Location = new System.Drawing.Point(68, 46);
            this.publishPayloadBox.Multiline = true;
            this.publishPayloadBox.Name = "publishPayloadBox";
            this.publishPayloadBox.Size = new System.Drawing.Size(280, 125);
            this.publishPayloadBox.TabIndex = 2;
            // 
            // toTopicLabel
            // 
            this.toTopicLabel.AutoSize = true;
            this.toTopicLabel.Location = new System.Drawing.Point(9, 22);
            this.toTopicLabel.Name = "toTopicLabel";
            this.toTopicLabel.Size = new System.Drawing.Size(53, 13);
            this.toTopicLabel.TabIndex = 1;
            this.toTopicLabel.Text = "Publish to";
            // 
            // publishToComboBox
            // 
            this.publishToComboBox.FormattingEnabled = true;
            this.publishToComboBox.Location = new System.Drawing.Point(68, 19);
            this.publishToComboBox.Name = "publishToComboBox";
            this.publishToComboBox.Size = new System.Drawing.Size(280, 21);
            this.publishToComboBox.TabIndex = 0;
            // 
            // mqttClientStatusStrip
            // 
            this.mqttClientStatusStrip.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.mqttClientStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectedStatus,
            this.spacerLabel,
            this.lastPingLabel});
            this.mqttClientStatusStrip.Location = new System.Drawing.Point(0, 636);
            this.mqttClientStatusStrip.Name = "mqttClientStatusStrip";
            this.mqttClientStatusStrip.Size = new System.Drawing.Size(1030, 22);
            this.mqttClientStatusStrip.TabIndex = 11;
            this.mqttClientStatusStrip.Text = "Not Connected";
            // 
            // connectedStatus
            // 
            this.connectedStatus.Name = "connectedStatus";
            this.connectedStatus.Size = new System.Drawing.Size(88, 17);
            this.connectedStatus.Text = "Not Connected";
            // 
            // spacerLabel
            // 
            this.spacerLabel.Name = "spacerLabel";
            this.spacerLabel.Size = new System.Drawing.Size(927, 17);
            this.spacerLabel.Spring = true;
            // 
            // lastPingLabel
            // 
            this.lastPingLabel.Name = "lastPingLabel";
            this.lastPingLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // outputContainer
            // 
            this.outputContainer.Controls.Add(this.autoScrollCheckBox);
            this.outputContainer.Controls.Add(this.clearOutputButton);
            this.outputContainer.Controls.Add(this.label5);
            this.outputContainer.Controls.Add(this.label4);
            this.outputContainer.Controls.Add(this.showPingsCheckBox);
            this.outputContainer.Controls.Add(this.outputListView);
            this.outputContainer.Location = new System.Drawing.Point(378, 12);
            this.outputContainer.Name = "outputContainer";
            this.outputContainer.Size = new System.Drawing.Size(637, 615);
            this.outputContainer.TabIndex = 12;
            this.outputContainer.TabStop = false;
            this.outputContainer.Text = "Output";
            // 
            // autoScrollCheckBox
            // 
            this.autoScrollCheckBox.AutoSize = true;
            this.autoScrollCheckBox.Checked = true;
            this.autoScrollCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autoScrollCheckBox.Location = new System.Drawing.Point(473, 15);
            this.autoScrollCheckBox.Name = "autoScrollCheckBox";
            this.autoScrollCheckBox.Size = new System.Drawing.Size(77, 17);
            this.autoScrollCheckBox.TabIndex = 12;
            this.autoScrollCheckBox.Text = "Auto-Scroll";
            this.autoScrollCheckBox.UseVisualStyleBackColor = true;
            // 
            // clearOutputButton
            // 
            this.clearOutputButton.Location = new System.Drawing.Point(556, 11);
            this.clearOutputButton.Name = "clearOutputButton";
            this.clearOutputButton.Size = new System.Drawing.Size(75, 23);
            this.clearOutputButton.TabIndex = 11;
            this.clearOutputButton.Text = "Clear";
            this.clearOutputButton.UseVisualStyleBackColor = true;
            this.clearOutputButton.Click += new System.EventHandler(this.clearOutputButton_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.DarkRed;
            this.label5.Location = new System.Drawing.Point(149, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Red = Recieve";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.LimeGreen;
            this.label4.Location = new System.Drawing.Point(70, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Green = Send";
            // 
            // showPingsCheckBox
            // 
            this.showPingsCheckBox.AutoSize = true;
            this.showPingsCheckBox.Checked = true;
            this.showPingsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showPingsCheckBox.Location = new System.Drawing.Point(368, 15);
            this.showPingsCheckBox.Name = "showPingsCheckBox";
            this.showPingsCheckBox.Size = new System.Drawing.Size(82, 17);
            this.showPingsCheckBox.TabIndex = 8;
            this.showPingsCheckBox.Text = "Show Pings";
            this.showPingsCheckBox.UseVisualStyleBackColor = true;
            this.showPingsCheckBox.CheckedChanged += new System.EventHandler(this.showPingsCheckBox_CheckedChanged);
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1030, 658);
            this.Controls.Add(this.outputContainer);
            this.Controls.Add(this.mqttClientStatusStrip);
            this.Controls.Add(this.publishGroupBox);
            this.Controls.Add(this.subscriptionsGroupBox);
            this.Controls.Add(this.connectionSettingsGroupBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ClientForm";
            this.Text = "MQTT Client";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_Closed);
            this.connectionSettingsGroupBox.ResumeLayout(false);
            this.connectionSettingsGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.loadServerListGraphic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.loadingImage)).EndInit();
            this.subscriptionsGroupBox.ResumeLayout(false);
            this.subscriptionsGroupBox.PerformLayout();
            this.publishGroupBox.ResumeLayout(false);
            this.publishGroupBox.PerformLayout();
            this.mqttClientStatusStrip.ResumeLayout(false);
            this.mqttClientStatusStrip.PerformLayout();
            this.outputContainer.ResumeLayout(false);
            this.outputContainer.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox connectionSettingsGroupBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox connectionPortTextBox;
        private System.Windows.Forms.Button ConnectButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox clientIdTextBox;
        private System.Windows.Forms.Button disconnectButton;
        private System.Windows.Forms.TextBox addSubscriptionTextBox;
        private System.Windows.Forms.Button addSubscriptionButton;
        private System.Windows.Forms.ListBox subscriptionsListBox;
        private System.Windows.Forms.Button removeSubscriptionButton;
        private System.Windows.Forms.ListView outputListView;
        private System.Windows.Forms.GroupBox subscriptionsGroupBox;
        private System.Windows.Forms.GroupBox publishGroupBox;
        private System.Windows.Forms.Label payloadlabel;
        private System.Windows.Forms.TextBox publishPayloadBox;
        private System.Windows.Forms.Label toTopicLabel;
        private System.Windows.Forms.ComboBox publishToComboBox;
        private System.Windows.Forms.Button Publish;
        private System.Windows.Forms.Button clearPublishButton;
        private System.Windows.Forms.ComboBox connectionAddressTextBox;
        private System.Windows.Forms.StatusStrip mqttClientStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel connectedStatus;
        private System.Windows.Forms.ColumnHeader ID;
        private System.Windows.Forms.ColumnHeader Type;
        private System.Windows.Forms.ColumnHeader Topic;
        private System.Windows.Forms.ColumnHeader Data;
        private System.Windows.Forms.ColumnHeader Time;
        private System.Windows.Forms.GroupBox outputContainer;
        private System.Windows.Forms.CheckBox showPingsCheckBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ToolStripStatusLabel spacerLabel;
        private System.Windows.Forms.ToolStripStatusLabel lastPingLabel;
        private System.Windows.Forms.Button clearOutputButton;
        private System.Windows.Forms.PictureBox loadingImage;
        private System.Windows.Forms.TextBox connectionTimeoutTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox autoScrollCheckBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.PictureBox loadServerListGraphic;
        private System.Windows.Forms.Button localhostButton;
        private System.Windows.Forms.TextBox loadTestTextBox;
        private System.Windows.Forms.Button loadTestButton;

    }
}

