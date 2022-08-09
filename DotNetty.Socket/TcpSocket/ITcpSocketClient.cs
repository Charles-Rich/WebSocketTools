namespace DotNetty.Socket;

/// <summary>
///     TcpSocket客户端
/// </summary>
public interface ITcpSocketClient : IBaseTcpSocketClient, ISendBytes, ISendString
{
}