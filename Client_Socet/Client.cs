using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class Client
{
    static async Task Main(string[] args)
    {
        IPAddress ipAddress = IPAddress.Parse("192.168.0.103");
        int port = 11000;
        IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

        Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            await sender.ConnectAsync(remoteEP);
            Console.WriteLine($"Connected to {sender.RemoteEndPoint.ToString()}");

            string message = "Hello, server!";
            byte[] msg = Encoding.UTF8.GetBytes(message);
            await sender.SendAsync(new ArraySegment<byte>(msg), SocketFlags.None);

            byte[] bytes = new byte[1024];
            int bytesRec = await sender.ReceiveAsync(new ArraySegment<byte>(bytes), SocketFlags.None);
            string data = Encoding.UTF8.GetString(bytes, 0, bytesRec);

            Console.WriteLine($"At {DateTime.Now.ToShortTimeString()} received from {((IPEndPoint)sender.RemoteEndPoint).Address}: {data}");

            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}