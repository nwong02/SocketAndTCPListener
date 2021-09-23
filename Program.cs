// Pulled from https://www.c-sharpcorner.com/article/socket-programming-in-C-Sharp/ to investigate server nodes client applcation to receive info

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;


//Socket listener is listening to incoming messages on a specified port and protocol
public class SocketAndTCPListener
{
    public static int Main(String[] args)
    {
        StartServer();
        return 0;
    }


    public static void StartServer()
    {
        // Get Host IP address, a localhost at 127.0.0.1, if host has multiple addresses, we get a list of applicable addresses
        IPHostEntry host = Dns.GetHostEntry("localhost");
        IPAddress ipAddress = host.AddressList[0];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

        try
        {
            // Socket that uses TCP
            Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(localEndPoint);
            listener.Listen(10);

            Console.WriteLine("Waiting for a connection...");
            Socket handler = listener.Accept();

            // Incoming data from client
            string data = null;
            byte[] bytes = null;

            while (true)
            {
                bytes = new byte[1024];
                int bytesRec = handler.Receive(bytes);
                data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                if (data.IndexOf("<EOF>") > -1)
                {
                    break;
                }
            }

            Console.WriteLine("Text received: {0}", data);

            byte[] msg = Encoding.ASCII.GetBytes(data);
            handler.Send(msg);
            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
        
        Console.WriteLine("/n Press any key to continue...");
        Console.ReadKey();
    }
}
