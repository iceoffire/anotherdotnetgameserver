using System.Net;
using System.Net.Sockets;
using AnotherDotNetGameServer.Interface;
using Google.FlatBuffers;
using Main;
using TcpClient = NetCoreServer.TcpClient;

namespace AnotherDotNetGameServer;

public class GameClient : TcpClient, IGameClient
{
    private bool _stop;

    public GameClient(string address, int port) : base(address, port)
    {
    }

    protected GameClient(DnsEndPoint endpoint) : base(endpoint)
    {
    }

    public bool SendPacketAsync(string route, byte[] packet)
    {
        var builder = new FlatBufferBuilder(1024);
        
        var routePayload = builder.CreateString(route);
        var payload = Packet.CreatePayloadVector(builder, packet);
        
        Packet.StartPacket(builder);
        Packet.AddRoute(builder, routePayload);
        Packet.AddPayload(builder, payload);

        var offset = Packet.EndPacket(builder);
        builder.Finish(offset.Value);
        
        return base.SendAsync(PrepareSendBuffer(builder.SizedByteArray()));
    }

    public override long Send(byte[] buffer)
    {
        throw new Exception("Please send using the SendPacketAsync");
    }

    public override bool SendAsync(byte[] buffer)
    {
        throw new Exception("Please send using the SendPacketAsync");
    }

    public void DisconnectAndStop()
    {
        _stop = true;
        Disconnect();
    }
    
    public void DisconnectAsyncAndStop()
    {
        _stop = true;
        DisconnectAsync();
        while (IsConnected)
            Thread.Yield();
    }
    
    protected override void OnDisconnected()
    {
        Console.WriteLine($"Chat TCP client disconnected a session with Id {Id}");

        // Wait for a while...
        Thread.Sleep(1000);

        // Try to connect again
        if (!_stop)
            ConnectAsync();
    }
    
    protected override void OnReceived(byte[] buffer, long offset, long size)
    {
        
    }
        
    protected override void OnConnected()
    {
        Console.WriteLine($"Chat TCP client connected a new session with Id {Id}");
    }
        
    protected override void OnError(SocketError error)
    {
        Console.WriteLine($"Chat TCP client caught an error with code {error}");
    }
    

    private static byte[] PrepareSendBuffer(byte[] originalBuffer)
    {
        var buffer = new byte[4 + originalBuffer.Length];
        
        Buffer.BlockCopy(BitConverter.GetBytes(originalBuffer.Length), 0, buffer, 0, 4);
        Buffer.BlockCopy(originalBuffer, 0, buffer, 4, originalBuffer.Length);
        
        return buffer;
    }
}