using System.Net;
using AnotherDotNetGameServer;
using Google.FlatBuffers;
using Test = Base.Test;

namespace ConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            var gameRoom = CreateGameRoom();
            gameRoom.Start();
            CreateClientAndCallServer();
            gameRoom.Stop();
            Console.WriteLine("[Main] Program exited");
        }

        static GameRoom CreateGameRoom()
        {
            return new GameRoom(IPAddress.Any, 1111);
        }

        static void CreateClientAndCallServer()
        {
            var address = "127.0.0.1";
            var port = 1111;
            
            // Console.WriteLine($"[CLIENT] TCP server address: {address}");
            // Console.WriteLine($"[CLIENT] TCP server port: {port}");
            
            var client = new GameClient(address, port);
            
            Console.WriteLine("[CLIENT] Client connecting...");
            client.Connect();
            Console.WriteLine("[CLIENT] Done!");

            TestSinglePacket(client);
            TestOverlapPackets(client);
            
            // Disconnect the client
            Console.WriteLine("[CLIENT] Client disconnecting...");
            client.DisconnectAndStop();
            Console.WriteLine("[CLIENT] Done!");
        }

        private static void TestSinglePacket(GameClient client)
        {
            var littleFlatPacket = CreateFlatPacket(30);
            client.SendPacketAsync("/Calendar/Code", littleFlatPacket);
        }

        private static void TestOverlapPackets(GameClient client)
        {
            var bigFlatPacket = CreateFlatPacket(60000);
            
            for (var i = 0; i < 10; i++)
                client.SendPacketAsync("/Calendar/Code", bigFlatPacket);
        }

        private static byte[] CreateFlatPacket(int stringSize)
        {
            var builder = new FlatBufferBuilder(1024);
            var name = builder.CreateString(string.Join("", Enumerable.Range(0, stringSize).ToList().Select(c => (c%10).ToString())));
            
            Test.StartTest(builder);
            Test.AddDamage(builder, 1000);
            Test.AddName(builder, name);

            var offset = Test.EndTest(builder);
            builder.Finish(offset.Value);

            return builder.SizedByteArray();
        }
    }
}