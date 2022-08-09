﻿using System.Threading.Tasks;

namespace DotNetty.Socket;

/// <summary>
///     发送字符串
/// </summary>
public interface ISendString
{
    /// <summary>
    ///     发送字符串
    /// </summary>
    /// <param name="msgStr">字符串</param>
    /// <returns></returns>
    Task Send(string msgStr);
}