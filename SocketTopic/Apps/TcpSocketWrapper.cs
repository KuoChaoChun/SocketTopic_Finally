using SocketTopic.Interface;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SocketTopic.Apps
{
    public class TcpSocketWrapper : ISocketWrapper
    {
        private Socket _socket;

        public TcpSocketWrapper() { }

        public TcpSocketWrapper(Socket socket)
        {
            _socket = socket;
        }

        public void Connect(string ip, int port)
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.Connect(ip, port);
        }

        public void Close()
        {
            _socket.Close();
            _socket = null;
        }

        public void Send(string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            _socket.Send(data);
        }

        public void Send(byte[] data)
        {
            _socket.Send(data);
        }

        public void Listen(IPAddress address, int port)
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint endPoint = new IPEndPoint(address, port);
            _socket.Bind(endPoint);
            _socket.Listen(10);
        }

        public int Receive(byte[] dateBuffer)
        {
            return _socket.Receive(dateBuffer);
        }

        public string Receive()
        {
            byte[] buffer = new byte[1024];
            int receivedBytes = Receive(buffer);
            string res = Encoding.UTF8.GetString(buffer, 0, receivedBytes);
            return res;
        }

        public ISocketWrapper Accept()
        {
            return new TcpSocketWrapper(_socket.Accept());
        }

        public (string ip, int port) GetRemoteInfo()
        {
            IPEndPoint clientEndPoint = (IPEndPoint)_socket.RemoteEndPoint;
            return (clientEndPoint.Address.ToString(), clientEndPoint.Port);
        }

    }
}
