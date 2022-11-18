using System.Net.Sockets;
using System.Net;
using System.Text;
class Server{
    private readonly int headerLength = 32;
    private readonly string headerText = "SIZE:";
    private Socket server;
    private List<Socket> clients;
    private bool serverRunning;
    public Server(){
        clients = new List<Socket>();
        serverRunning = true;
        IPHostEntry host = Dns.GetHostEntry("localhost");
        IPAddress iPAddress = host.AddressList[0];
        IPEndPoint localEndPoint = new IPEndPoint(iPAddress, 11000);

        server = new Socket(iPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        server.Bind(localEndPoint);
        server.ReceiveTimeout = 1000;
    }

    public void StartServer(){
        runServer();
    }

    private async Task runServer(){
        Console.WriteLine("[SERVER] Waiting for Connection...");
        while(serverRunning){
            Socket client = server.Accept();
            if (client != null){
                Console.WriteLine($"[SERVER] {client.LocalEndPoint.Serialize()[0]} connected");
                clients.Add(client);
                handleClient(client);
            }
        }
        foreach(var client in clients){
            client.Close();
        }
        server.Close();
    }
    private async Task stopServer(){
        while (true){
            if(Console.ReadLine() == "stop") break;
            await Task.Delay(1000);
        }
        serverRunning = false;
    }

    private async Task handleClient(Socket client){
        await Task.Delay(25);
        byte[] buffer = new byte[headerLength];
        client.Receive(buffer);
        if(buffer != null){
            //read the header length and receive second message
            
        }

    }
}

class Client{
    private readonly int headerLength = 32;
    private readonly string headerText = "SIZE:";
    bool isConnected;
    Socket socket;
    public Client(){
        
    }

    public bool Connect(){


        if (!isConnected){
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Connection Failed");
            Console.ResetColor();
            return false;
        }
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Connected");
        Console.ResetColor();

        return true;
    }
    public void sendShot(){

    }

    public (int left, int top) takeHit(){
        (int left, int top) pos;
        pos.left = 0;
        pos.top = 0;


        return pos;
    }


}