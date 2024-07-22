namespace SocketTopic.Interface
{
    public interface IClient
    {
        void Connect(string serverIP, int serverPort);
        void Disconnect();
        string SendRequest(string serverIP, int serverPort, string fileName);
    }
}
