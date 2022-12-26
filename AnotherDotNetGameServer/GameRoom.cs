using System.Net;
using AnotherDotNetGameServer.Interface;
using NetCoreServer;

namespace AnotherDotNetGameServer;

public class GameRoom : TcpServer, IGameRoom
{
    public GameRoom(IPAddress address, int port) : base(address, port)
    {
    }

    protected override TcpSession CreateSession() => new GameSession(this);
}