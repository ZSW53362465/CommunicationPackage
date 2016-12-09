using Chioy.Communication.Networking.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServiceHost
{
    public partial class Form1 : Form
    {
        TCPService service;
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_Start_Click(object sender, EventArgs e)
        {
            var service = ServiceManager.Instance().GetService(BindingType.TCP);
            var tcpService = service as TCPService;
            tcpService.ConfigService();
            tcpService.ClientLost += Service_ClientLost;
            tcpService.NewClientSubscribed += Service_NewClientSubscribed;
            service.ExceptionEvent += Service_ExceptionEvent;
            //var service = new TCPService();
            //service.StartKRSvc();
        }

        private void Service_ExceptionEvent(Chioy.Communication.Networking.Common.KRException ex)
        {
        }

        private void Service_NewClientSubscribed(object sender, Chioy.Communication.Networking.Models.DataEventArgs e)
        {
            listBox1.Items.Add(e.Data.ToString());
        }

        private void Service_ClientLost(object sender, Chioy.Communication.Networking.Models.DataEventArgs e)
        {
            listBox1.Items.Remove(e.Data.ToString());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            service.SendDataToClient(new Chioy.Communication.Networking.Interface.ArgumentBase<string>() { Msg = this.textBox1.Text });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            service.SendDataToSpecClient(this.listBox1.SelectedValue.ToString(), new Chioy.Communication.Networking.Interface.ArgumentBase<string>() { Msg = this.textBox1.Text });
        }
    }
}
