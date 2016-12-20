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
        BaseService service;
        TCPService tcpService;
        HttpService httpService;
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_Start_Click(object sender, EventArgs e)
        {
            if (service != null && service.ServiceState == System.ServiceModel.CommunicationState.Opened)
            {
                MessageBox.Show("服务已启动");
                return;
            }
            service = ServiceManager.Instance().GetService(BindingType.TCP);
            service.ConfigService( Chioy.Communication.Networking.Common.ProductType.BMD);
            //service.RegisterProvider(new MyProvider());
            tcpService = service as TCPService;
            tcpService.ClientLost += Service_ClientLost;
            tcpService.NewClientSubscribed += Service_NewClientSubscribed;
            service.ExceptionEvent += Service_ExceptionEvent;
            MessageBox.Show("服务启动成功");
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
            listBox1.Invoke(new MethodInvoker(delegate () {
                listBox1.Items.Remove(e.Data);
            }));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tcpService.SendDataToClient(new Chioy.Communication.Networking.Interface.ArgumentBase<string>() { Msg = this.textBox1.Text });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tcpService.SendDataToSpecClient(this.listBox1.SelectedValue.ToString(), new Chioy.Communication.Networking.Interface.ArgumentBase<string>() { Msg = this.textBox1.Text });
        }

        private void button3_Click(object sender, EventArgs e)
        {
            httpService = ServiceManager.Instance().GetService(BindingType.HTTP) as HttpService;
            httpService.ConfigService( Chioy.Communication.Networking.Common.ProductType.BMD);
            httpService.RegisterProvider(new MyProvider());



        }
    }
}
