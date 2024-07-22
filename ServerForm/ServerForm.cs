using SocketTopic.Apps;
using SocketTopic.Services;
using System;
using System.Windows.Forms;

namespace ServerForm
{
    public partial class ServerForm : Form
    {
        public ServerForm()
        {
            InitializeComponent();

            Server server = new Server(new TcpSocketWrapper());
            //註冊事件
            server.AfterConnect = SetClientInfo;   
            server.AfterReceiveMsg = f => FileNameTxt.Invoke(new Action(() => FileNameTxt.Text = f.ToString()));

            // 開始監聽
            server.Start();
        }

        private void SetClientInfo(string ip, int port)
        {
            IpTxt.Invoke(new Action(() => IpTxt.Text = ip));
            PortTxt.Invoke(new Action(() => PortTxt.Text = port.ToString()));
        }
    }
}
