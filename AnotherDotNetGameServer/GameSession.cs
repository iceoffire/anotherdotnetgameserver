using AnotherDotNetGameServer.Interface;
using Google.FlatBuffers;
using Main;
using NetCoreServer;
using Buffer = System.Buffer;

namespace AnotherDotNetGameServer;

public class GameSession : TcpSession, IGameSession
{
    private bool _isStillReceiving = false;
    private byte[]? _cumulatedBuffer;
    private long _sizeRemaining;
    private int _cumulatedOffset = 0;

    public GameSession(TcpServer server) : base(server) {}

    protected override void OnDisconnected() {}
    protected override void OnConnected()
    {
        Console.WriteLine($"TCP Session started with Id {Id}.");
    }
    
    protected override void OnReceived(byte[] buffer, long offset, long size)
    {
        if (offset != 0)
            throw new Exception("Should not have offset");
        if (_isStillReceiving)
        {
            var readSize = size;
            if (_sizeRemaining - size < 0)
            {
                Console.WriteLine("[SERVER] Packets Overlap.");
                readSize = _sizeRemaining;
                Buffer.BlockCopy(buffer, 0, _cumulatedBuffer!, _cumulatedOffset, (int)readSize);
                _cumulatedOffset += (int)readSize;
                DoneReadingSinglePacket(_cumulatedBuffer!, _cumulatedOffset);
                var remainingBuffer = new byte[(int)(size - readSize)];
                Buffer.BlockCopy(buffer, (int)readSize, remainingBuffer!, 0, (int)(size - readSize));

                OnReceived(remainingBuffer, 0, size - readSize);
                return;
            }
            Buffer.BlockCopy(buffer, 0, _cumulatedBuffer!, _cumulatedOffset, (int)readSize);

            _cumulatedOffset += (int)readSize;
            _sizeRemaining -= readSize;
            if (_sizeRemaining == 0)
                DoneReadingSinglePacket(_cumulatedBuffer!, _cumulatedOffset);
            if (_sizeRemaining < 0)
                throw new Exception($"Should not be below 0, current value: {_sizeRemaining}");
        }
        else
        {
            var originalPacketSize = BitConverter.ToInt32(buffer);
            if (size > originalPacketSize + 4)
            {
                var completePacket = new byte[originalPacketSize];
                Buffer.BlockCopy(buffer, 4, completePacket, 0, originalPacketSize);
                DoneReadingSinglePacket(completePacket, originalPacketSize);
                
                var remainingBuffer = new byte[(int)(size - originalPacketSize - 4)];
                Buffer.BlockCopy(buffer, originalPacketSize + 4, remainingBuffer!, 0, (int)(size - originalPacketSize - 4));

                OnReceived(remainingBuffer, 0, size - originalPacketSize - 4);
                return;
            }
            if (originalPacketSize + 4 != size)
            {
                _isStillReceiving = true;
                _cumulatedBuffer = new byte[originalPacketSize];
                _sizeRemaining = originalPacketSize - (int)size + 4;
                _cumulatedOffset = (int)size - 4;
                Buffer.BlockCopy(buffer, 4, _cumulatedBuffer, 0, (int)size - 4);
                Console.WriteLine($"[SERVER] Still receiving... It still needs to receive {_sizeRemaining}");
            }
            else
            {
                Console.WriteLine($"[SERVER] Received packet, size: {size}");
                Console.WriteLine($"[SERVER] Real packet size: {originalPacketSize}");
                var packetBuffer = new byte[(int)size - 4];
                Buffer.BlockCopy(buffer, 4, packetBuffer, 0, (int)size - 4);
                DoneReadingSinglePacket(packetBuffer, (int)size - 4);
            }
        }
    }

    private void DoneReadingSinglePacket(byte[] buffer, int totalSize)
    {
        _isStillReceiving = false;
        _cumulatedBuffer = null;
        _sizeRemaining = 0;
        _cumulatedOffset = 0;
        Console.WriteLine($"[SERVER] Done reading single packet, packet size => {totalSize}");
        var cutBuffer = new byte[totalSize];
        Buffer.BlockCopy(buffer, 0, cutBuffer, 0, totalSize);
        OnReadPacketInternal(cutBuffer);
    }

    protected virtual void OnReadPacketInternal(byte[] buffer) => OnReadPacket(Packet.GetRootAsPacket(new ByteBuffer(buffer)));

    protected virtual void OnReadPacket(Packet packet)
    {
        
    }
}