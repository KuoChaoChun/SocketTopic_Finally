using SocketTopic.Interface;
using SocketTopic.Utility;
using System;
using System.IO;
using System.Linq;

namespace SocketTopic.Services
{
    public class Client : IClient
    {
        private ISocketWrapper _socketWrapper;
        private string _savePath;

        public Client(ISocketWrapper socketWrapper, string savePath)
        {
            _socketWrapper = socketWrapper;
            _savePath = savePath;
        }

        public void Connect(string serverIP, int serverPort)
        {
            try
            {
                _socketWrapper.Connect(serverIP, serverPort);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Client_Connect()_Exception: {0}" + ex.Message);
            }
        }

        public void Disconnect()
        {
            _socketWrapper.Close();
        }

        public string SendRequest(string serverIP, int serverPort, string fileName)
        {
            try
            {
                _socketWrapper.Connect(serverIP, serverPort);
                // 傳送要下載的檔案名稱
                _socketWrapper.Send(fileName);
                // 接收成功or失敗
                string msgResult = _socketWrapper.Receive();
                if (msgResult == MsgResultState.Success)
                {
                    // 接收檔案、下載檔案
                    var buffer = new byte[1024];
                    _socketWrapper.Receive(buffer);
                    string filePath = Path.Combine(_savePath, fileName);
                    File.WriteAllBytes(filePath, buffer.ToArray());
                }

                return msgResult;
            }
            catch (Exception ex)
            {
                return $"SendRequest()_Exception: {ex.Message}";
            }
        }

    }
}
