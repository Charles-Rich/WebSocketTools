using DotNetty.Codecs.Http;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;

namespace DotNetty.Socket;

internal class WebSocketServerBuilder :
    BaseGenericServerBuilder<IWebSocketServerBuilder, IWebSocketServer, IWebSocketConnection, string>,
    IWebSocketServerBuilder
{
    public WebSocketServerBuilder(int port, string path)
        : base(port)
    {
        _path = path;
    }

    private string _path { get; }

    public override async Task<IWebSocketServer> BuildAsync()
    {
        var tcpServer = new WebSocketServer(_port, _path, _event);
        var serverChannel = await new ServerBootstrap()
            .Group(new MultithreadEventLoopGroup(), new MultithreadEventLoopGroup())
            .Channel<TcpServerSocketChannel>()
            .Option(ChannelOption.SoBacklog, 8192)
            .Option(ChannelOption.TcpNodelay, true)
            .ChildHandler(new ActionChannelInitializer<IChannel>(channel =>
            {
                var pipeline = channel.Pipeline;
                pipeline.AddLast(new HttpServerCodec());
                pipeline.AddLast(new HttpObjectAggregator(65536));
                pipeline.AddLast(new CommonChannelHandler(tcpServer));
            })).BindAsync(_port);
        _event.OnServerStarted?.Invoke(tcpServer);
        tcpServer.SetChannel(serverChannel);

        return await Task.FromResult(tcpServer);
    }
}