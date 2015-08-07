using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BrokerManager
{
    public partial class BrokerManagerForm : Form
    {
        public BrokerManagerForm()
        {
            InitializeComponent();
            BrokerCommunication comms = new BrokerCommunication();
        }
    }
}
