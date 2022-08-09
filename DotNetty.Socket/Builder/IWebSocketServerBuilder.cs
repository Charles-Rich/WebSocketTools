﻿namespace DotNetty.Socket;

/// <summary>
///     WebSocket服务端构建者
/// </summary>
public interface IWebSocketServerBuilder : IGenericServerBuilder<IWebSocketServerBuilder, IWebSocketServer, IWebSocketConnection, string>
{
}