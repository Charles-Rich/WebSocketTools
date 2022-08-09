using System;
using DotNetty.Transport.Channels;

namespace DotNetty.Socket;

internal interface IChannelEvent
{
    void OnChannelActive(IChannelHandlerContext ctx);
    void OnChannelReceive(IChannelHandlerContext ctx, object msg);
    void OnChannelInactive(IChannel channel);
    void OnException(IChannel channel, Exception exception);
}