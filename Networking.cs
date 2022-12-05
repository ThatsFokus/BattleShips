using System.Net.Sockets;
using System.Net;
using System.Text;
public class Client
{

	public async void startClient(){
		IPHostEntry ipHostInfo = await Dns.GetHostEntryAsync(Dns.GetHostName());
		IPAddress ipaddress = ipHostInfo.AddressList[0];
		IPEndPoint ipEndPoint = new(ipaddress, 11_000);

		using Socket client = new(
		    ipEndPoint.AddressFamily, 
		    SocketType.Stream, 
		    ProtocolType.Tcp);

		await client.ConnectAsync(ipEndPoint);
		while (true)
		{
		    // Send message.
		    var message = "Hi friends !<|EOM|>";
		    var messageBytes = Encoding.UTF8.GetBytes(message);
		    _ = await client.SendAsync(messageBytes, SocketFlags.None);
		    Console.WriteLine($"Socket client sent message: \"{message}\"");

		    // Receive ack.
		    var buffer = new byte[1_024];
		    var received = await client.ReceiveAsync(buffer, SocketFlags.None);
		    var response = Encoding.UTF8.GetString(buffer, 0, received);
		    if (response == "<|ACK|>")
		    {
		        Console.WriteLine(
		            $"Socket client received acknowledgment: \"{response}\"");
		        break;
		    }
		    // Sample output:
		    //     Socket client sent message: "Hi friends 👋!<|EOM|>"
		    //     Socket client received acknowledgment: "<|ACK|>"
		}

		client.Shutdown(SocketShutdown.Both);
	}
}

public  class Server{
	public async void hostServer(){
		IPHostEntry ipHostInfo = await Dns.GetHostEntryAsync(Dns.GetHostName());
		IPAddress ipaddress = ipHostInfo.AddressList[0];
		IPEndPoint ipEndPoint = new(ipaddress, 11_000);

		using Socket listener = new(
		    ipEndPoint.AddressFamily,
		    SocketType.Stream,
		    ProtocolType.Tcp);

		listener.Bind(ipEndPoint);
		listener.Listen(100);

		var handler = await listener.AcceptAsync();
		while (true)
		{
		    // Receive message.
		    var buffer = new byte[1_024];
		    var received = await handler.ReceiveAsync(buffer, SocketFlags.None);
		    var response = Encoding.UTF8.GetString(buffer, 0, received);

		    var eom = "<|EOM|>";
		    if (response.IndexOf(eom) > -1 /* is end of message */)
		    {
		        Console.WriteLine(
		            $"Socket server received message: \"{response.Replace(eom, "")}\"");

		        var ackMessage = "<|ACK|>";
		        var echoBytes = Encoding.UTF8.GetBytes(ackMessage);
		        await handler.SendAsync(echoBytes, 0);
		        Console.WriteLine(
		            $"Socket server sent acknowledgment: \"{ackMessage}\"");

		        break;
		    }
		    // Sample output:
		    //    Socket server received message: "Hi friends 👋!"
		    //    Socket server sent acknowledgment: "<|ACK|>"
		}
	}
}