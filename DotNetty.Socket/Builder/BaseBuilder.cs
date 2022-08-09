﻿namespace DotNetty.Socket;

internal abstract class BaseBuilder<TBuilder, TTarget> : IBuilder<TBuilder, TTarget> where TBuilder : class
{
    public BaseBuilder(int port)
    {
        _port = port;
    }

    protected int _port { get; }

    public abstract Task<TTarget> BuildAsync();

    public abstract TBuilder OnException(Action<Exception> action);
}