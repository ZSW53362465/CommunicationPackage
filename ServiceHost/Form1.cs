using Chioy.Communication.Networking.Common;
using Chioy.Communication.Networking.Interface;
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
        BaseServiceMgr service;
        TcpServiceMgr tcpService;
        HttpServiceMgr httpService;
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
            //启动骨密度 TCP服务
            //BaseService service;
            //TcpServiceMgr tcpService;
            //根据TCP联网方式创建并返回Service
            service = ServiceManagerFactory.Instance().GetService(BindingType.TCP);
            //配置当前使用通信模块的产品
            service.ConfigService( Chioy.Communication.Networking.Common.ProductType.BMD);
            //注册Provider，提供用来提供病人和接收检查结果
            service.RegisterProvider(new MyProvider());
            //客户端断线和新客户端上线是TCPService特有的功能，所以用as来转化Service的类型
            tcpService = service as TcpServiceMgr;
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
            tcpService.SendDataToClient(new ArgumentBase<string>() { Code= Chioy.Communication.Networking.Interface.KRCode.DataFromSvr, Msg = this.textBox1.Text });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tcpService.SendDataToSpecClient(this.listBox1.SelectedItem.ToString(), new Chioy.Communication.Networking.Interface.ArgumentBase<string>() { Code= Chioy.Communication.Networking.Interface.KRCode.DataFromSvr, Msg = this.textBox1.Text });
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //启动骨密度 TCP服务
            //BaseService service;
            //根据HTTP联网方式创建并返回Service
            service = ServiceManagerFactory.Instance().GetService(BindingType.HTTP);
            //配置当前使用通信模块的产品
            service.ConfigService( Chioy.Communication.Networking.Common.ProductType.BMD);
            //注册Provider，提供用来提供病人和接收检查结果
            service.RegisterProvider(new MyProvider());

        }
    }
}
