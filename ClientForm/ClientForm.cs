using SocketTopic.Apps;
using SocketTopic.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientForm
{
    public partial class ClientForm : Form
    {
        private Client _client;

        public string ServerIp => IpTxt.Text.Trim();
        public string Port => PortTxt.Text.Trim();
        public string FileName => FileNameTxt.Text.Trim();

        public ClientForm()
        {
            InitializeComponent();
            SavePathTxt.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            TcpSocketWrapper tcpSocketWrapper = new TcpSocketWrapper();
            _client = new Client(tcpSocketWrapper, SavePathTxt.Text);
        }

        private void RequestBtn_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(PortTxt.Text.Trim(), out int serverPort))
            {
                MessageBox.Show("請輸入有效的Port。");
                return;
            }

            Task.Run(() =>
            {
                string result = _client.SendRequest(ServerIp, serverPort, FileName);
                _client.Disconnect();
                AppendLog(result);
            });
        }

        private void AppendLog(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => ResultTxt.AppendText(message + Environment.NewLine)));
            }
            else
            {
                ResultTxt.AppendText(message + Environment.NewLine);
            }
        }
    }
}
