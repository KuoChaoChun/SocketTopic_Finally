using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SocketTopic.Interface;
using SocketTopic.Utility;
using System.Net;

namespace SocketTopic.Services.Tests
{
    [TestClass()]
    public class ServerTests
    {
        [TestMethod()]
        public void StartTest()
        {
            // Arrange
            string test_address = "123.123.123.123";
            int test_port = 6666;
            string test_fileName = "test.txt";
            Mock<ISocketWrapper> mockServerSocket = new Mock<ISocketWrapper>();
            Mock<ISocketWrapper> mockClientSocket = new Mock<ISocketWrapper>();

            mockClientSocket.Setup(c => c.GetRemoteInfo()).Returns((test_address, test_port));
            mockClientSocket.Setup(c => c.Receive()).Returns(test_fileName);
            mockClientSocket.Setup(c => c.Send(MsgResultState.Success)).Verifiable();
            mockClientSocket.Setup(c => c.Send(MsgResultState.Error)).Verifiable();

            mockServerSocket.Setup(s => s.Listen(It.IsAny<IPAddress>(), It.IsAny<int>())).Verifiable();
            mockServerSocket.Setup(s => s.Accept()).Returns(mockClientSocket.Object);

            var server = new Server(mockServerSocket.Object);

            // Act
            server.Start();

            server.Stop();
            // Assert
            mockClientSocket.Verify(c => c.Send(MsgResultState.Success), Times.Never);
        }
    }
}