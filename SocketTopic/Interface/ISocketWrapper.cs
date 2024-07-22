using System.Net;

namespace SocketTopic.Interface
{
    public interface ISocketWrapper
    {
        void Connect(string ip, int port);
        void Close();
        void Listen(IPAddress address, int port);
        void Send(string message);
        void Send(byte[] data);
        int Receive(byte[] dateBuffer);
        string Receive();
        ISocketWrapper Accept();
        (string ip, int port) GetRemoteInfo();
    }
}

