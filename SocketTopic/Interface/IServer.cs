using System;

namespace SocketTopic.Interface
{
    public interface IServer
    {
        Action<string, int> AfterConnect { get; set; }
        Action<string> AfterReceiveMsg { get; set; }
        void Start();
        void Stop();
    }
}
