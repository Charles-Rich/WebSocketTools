using System.Net;
using System.Text;
using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;

namespace DotNetty.Socket;

internal class UdpSocket : IUdpSocket, IChannelEvent
{
    public UdpSocket(int port, UdpSocketEvent socketEvent)
    {
        Port = port;
        _event = socketEvent;
    }

    private UdpSocketEvent _event { get; }
    private IChannel _channel { get; set; }

    public void OnChannelActive(IChannelHandlerContext ctx)
    {
        _event.OnStarted?.Invoke(this);
    }

    public void OnChannelInactive(IChannel channel)
    {
        _event.OnClose?.Invoke(this);
    }

    public void OnChannelReceive(IChannelHandlerContext ctx, object msg)
    {
        var packet = msg as DatagramPacket;
        if (!packet.Content.IsReadable()) return;

        var bytes = packet.Content.ToArray();
        _event.OnRecieve?.Invoke(this, packet.Sender, bytes);
    }

    public void OnException(IChannel channel, Exception exception)
    {
        _event.OnException?.Invoke(exception);
    }

    public int Port { get; }

    public async Task Send(byte[] bytes, EndPoint point)
    {
        try
        {
            var buffer = Unpooled.WrappedBuffer(bytes);
            await _channel.WriteAndFlushAsync(new DatagramPacket(buffer, point));
            await Task.Run(() => { _event.OnSend?.Invoke(this, point, bytes); });
        }
        catch (Exception ex)
        {
            _event.OnException.Invoke(ex);
        }
    }

    public async Task Send(string msgStr, EndPoint point)
    {
        await Send(Encoding.UTF8.GetBytes(msgStr), point);
    }

    public void Close()
    {
        throw new NotImplementedException();
    }

    public void SetChannel(IChannel channel)
    {
        _channel = channel;
    }
}