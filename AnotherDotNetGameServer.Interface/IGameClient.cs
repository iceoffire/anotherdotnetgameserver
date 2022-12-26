using System.Net;
using System.Net.Sockets;

namespace AnotherDotNetGameServer.Interface;

public interface IGameClient : IDisposable
{
    Guid Id { get; }
    string Address { get; }
    int Port { get; }
    EndPoint Endpoint { get; }
    Socket Socket { get; }
    
    long BytesPending { get; }
    long BytesSending { get; }
    long BytesSent { get; }
    long BytesReceived { get; }
    
    bool OptionDualMode { get; set; }
    bool OptionKeepAlive { get; set; }
    int OptionTcpKeepAliveTime { get; set; }
    int OptionTcpKeepAliveInterval { get; set; }
    int OptionTcpKeepAliveRetryCount { get; set; }
    bool OptionNoDelay { get; set; }
    int OptionReceiveBufferLimit { get; set; }
    int OptionReceiveBufferSize { get; set; }
    int OptionSendBufferLimit { get; set; }
    int OptionSendBufferSize { get; set; }
    
    bool IsConnecting { get; }
    bool IsConnected { get; }

    bool Connect();
    bool Disconnect();
    bool Reconnect();
    bool ConnectAsync();
    bool DisconnectAsync();
    bool ReconnectAsync();

    bool SendPacketAsync(string route, byte[] packet);
    
    bool IsDisposed { get; }
    bool IsSocketDisposed { get; }
}
