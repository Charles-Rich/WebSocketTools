﻿namespace DotNetty.Socket;

/// <summary>
///     WebSocket客户端构建者
/// </summary>
public interface IWebSocketClientBuilder :
    IGenericClientBuilder<IWebSocketClientBuilder, IWebSocketClient, string>
{
}