using System.Net.Sockets;
using NetCoreServer;

namespace AnotherDotNetGameServer.Interface;

public interface IGameRoom : IDisposable
{
    bool Start();
    bool Stop();
    bool Restart();

    bool DisconnectAll();
    TcpSession FindSession(Guid id);

    bool Multicast(byte[] buffer);

    bool IsDisposed { get; }
    bool IsSocketDisposed { get; }
}
