using System.Net.Sockets;
using NetCoreServer;

namespace AnotherDotNetGameServer.Interface;

public interface IGameSession : IDisposable
{
    Guid Id { get; }
    TcpServer Server { get; }
    Socket Socket { get; }
    long BytesPending { get; }
    long BytesSending { get; }
    long BytesSent { get; }
    long BytesReceived { get; }
    int OptionReceiveBufferLimit { get; set; }
    int OptionReceiveBufferSize { get; set; }
    int OptionSendBufferLimit { get; set; }
    int OptionSendBufferSize { get; set; }
    bool IsConnected { get; }
    
    bool Disconnect();
    long Send(byte[] buffer);
    bool SendAsync(byte[] buffer);
    
    bool IsDisposed { get; }
    bool IsSocketDisposed { get; }
}
