using System.Net.Sockets;
using System.Net;
class Server{
    private readonly int headerLength = 32;
    private readonly string headerText = "SIZE:";
    private Socket server;
    private List<Socket> clients;
    public Server(){
        clients = new List<Socket>();
        IPHostEntry host = Dns.GetHostEntry("localhost");
        IPAddress iPAddress = host.AddressList[0];
        IPEndPoint localEndPoint = new IPEndPoint(iPAddress, 11000);

        server = new Socket(iPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        server.Bind(localEndPoint);

        server.Listen(10);
        Console.WriteLine("[Server] Waiting for Connection...");
        Socket client = server.Accept();
        
        
    }

    private async Task Handle(Socket client){
        await Task.Delay(25);
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