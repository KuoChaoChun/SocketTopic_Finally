using SocketTopic.Interface;
using SocketTopic.Utility;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SocketTopic.Services
{
    public class Server : IServer
    {
        private ISocketWrapper _socketWrapper;
        private bool _isRunning = false;
        private string BaseDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Files");

        public Action<string, int> AfterConnect { get; set; }
        public Action<string> AfterReceiveMsg { get; set; }

        public Server(ISocketWrapper socketWrapper)
        {
            _socketWrapper = socketWrapper;
        }

        public void Start()
        {
            Task.Run(() =>
            {
                try
                {
                    _socketWrapper.Listen(IPAddress.Any, 8081);
                    _isRunning = true;

                    while (_isRunning)
                    {
                        var client = _socketWrapper.Accept();
                        // 觸發已連線事件
                        AfterConnect?.Invoke(client.GetRemoteInfo().ip, client.GetRemoteInfo().port);
                        // 讀取檔案名稱
                        string fileName = client.Receive();
                        // 觸發取得檔案名稱事件
                        AfterReceiveMsg?.Invoke(fileName);

                        string filePath = Path.Combine(BaseDirectory, fileName);
                        if (File.Exists(filePath))
                        {
                            // 先傳送成功訊息
                            client.Send(MsgResultState.Success);
                            // 再傳送檔案
                            byte[] fileBytes = File.ReadAllBytes(filePath);
                            client.Send(fileBytes);
                        }
                        else
                        {
                            // 傳送失敗訊息
                            client.Send(MsgResultState.Error);
                        }

                        client.Close();
                    }
                }
                catch (SocketException ex) when (ex.SocketErrorCode == SocketError.AddressAlreadyInUse)
                {
                    Console.WriteLine("port已被占用，請選擇其他port。");
                }
            });
        }

        public void Stop()
        {
            _isRunning = false;
            _socketWrapper.Close();
        }

    }
}
