using System.Net.Sockets;
using System.Net;
using System.Text;
class Server{
	private readonly int headerLength = 32;
	private readonly string headerText = "SIZE:";
	private readonly int port = 11000;
	private Socket server;
	private List<Socket> clients;
	private bool serverRunning;
	private Decoder decoder;
	private Encoder encoder;
	public Server(){
		clients = new List<Socket>();
		serverRunning = true;
		IPHostEntry host = Dns.GetHostEntry("localhost");
		IPAddress iPAddress = host.AddressList[0];
		IPEndPoint localEndPoint = new IPEndPoint(iPAddress, port);

		server = new Socket(iPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
		server.Bind(localEndPoint);
		server.ReceiveTimeout = 1000;
		decoder = System.Text.Encoding.UTF8.GetDecoder();
		encoder = System.Text.Encoding.UTF8.GetEncoder();
	}

	public void StartServer(){
		runServer();
	}

	private void runServer(){
		
		server.Listen();
		Console.WriteLine("[SERVER] Waiting for Connection...");
		stopServer();
		try{
			while(serverRunning){
				Socket client = server.Accept();
				if (client != null){
					Console.WriteLine($"[SERVER] {client.LocalEndPoint.Serialize()[0]} connected");
					clients.Add(client);
					handleClient(client);
				}
			}
		}
		catch{
			//ignore this shit
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
		server.Close();
	}

	private async Task handleClient(Socket client){
		while(client.Connected){
			await Task.Delay(25);
			byte[] bytebuffer = new byte[headerLength];
			client.Receive(bytebuffer);
			if(bytebuffer != new byte[headerLength]){
				//read the header length and receive second message
				char[] charbuffer = new char[decoder.GetCharCount(bytebuffer, 0, bytebuffer.Length)];
				int actualamountofchars = decoder.GetChars(bytebuffer, 0, bytebuffer.Length, charbuffer, 0);
				char[] useful = new char[charbuffer.Length - headerText.Length];
				charbuffer.CopyTo(useful, headerText.Length);
				int size = Convert.ToInt32(useful);
				bytebuffer = new byte[size];
				client.Receive(bytebuffer);

				//create and encrypt header
				string myheadermessage = headerText;
				myheadermessage += bytebuffer.Length.ToString();
				char[] myheader = myheadermessage.ToCharArray();
				byte[] myheaderbuffer = new byte[encoder.GetByteCount(myheader, 0, myheader.Length, true)];
				encoder.GetBytes(myheader, 0, myheader.Length, myheaderbuffer, 0, true);
				foreach(Socket c in clients){
					if (c == client)continue;
					c.Send(myheaderbuffer);
					await Task.Delay(50);
					c.Send(bytebuffer);
				}
			}
		}
	}
}

class Client{
	private readonly int headerLength = 32;
	private readonly string headerText = "SIZE:";
	private readonly int port = 11000;
	bool isConnected;
	Socket socket;
	public Client(){
		IPHostEntry host = Dns.GetHostEntry("localhost");
		IPAddress iPAddress = host.AddressList[0];
		IPEndPoint localEndPoint = new IPEndPoint(iPAddress, port);
		socket = new Socket(iPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
		socket.Bind(localEndPoint);
	}

	public bool Connect(byte[] addressValues){
		IPAddress address = new IPAddress(addressValues);
		try{
		socket.Connect(address, port);

			if (!socket.Connected){
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
		catch(SocketException e){
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Connection Failed");
				Console.ResetColor();
				Console.WriteLine(e);
		
				return true;
		}
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