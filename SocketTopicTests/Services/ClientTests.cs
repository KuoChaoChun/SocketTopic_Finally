using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SocketTopic.Interface;
using SocketTopic.Utility;

namespace SocketTopic.Services.Tests
{
    [TestClass()]
    public class ClientTests
    {
        [TestMethod()]
        public void SendRequestTest__FileNotExist()
        {
            // Arrange
            string test_address = "123.123.123.123";
            int test_port = 6666;
            string test_fileName = "test.txt";
            string savePath = "path";
          
            Mock<ISocketWrapper> mockClientSocket = new Mock<ISocketWrapper>();
            mockClientSocket.Setup(c => c.Connect(test_address, test_port)).Verifiable();
            mockClientSocket.Setup(c => c.Send(test_fileName)).Verifiable();
            mockClientSocket.Setup(c => c.Receive()).Returns(MsgResultState.Error);
            mockClientSocket.Setup(c => c.Receive(It.IsAny<byte[]>())).Returns(0).Verifiable();

            var client = new Client(mockClientSocket.Object, savePath);

            // Act
            var res = client.SendRequest(test_address, test_port, test_fileName);

            // Assert
            mockClientSocket.Verify(c => c.Receive(It.IsAny<byte[]>()), Times.Never);
            Assert.AreEqual(MsgResultState.Error, res);
        }
    }
}