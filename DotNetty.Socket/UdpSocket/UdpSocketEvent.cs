using System.Net;

namespace DotNetty.Socket;

internal class UdpSocketEvent
{
    public Action<IUdpSocket> OnStarted { get; set; }
    public Action<IUdpSocket, EndPoint, byte[]> OnRecieve { get; set; }
    public Action<IUdpSocket, EndPoint, byte[]> OnSend { get; set; }
    public Action<IUdpSocket> OnClose { get; set; }
    public Action<Exception> OnException { get; set; }
}