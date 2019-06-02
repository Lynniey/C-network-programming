using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace SharpPcapLab {
	
	public class Program {
	
		static void sendMessage(int port) {
        
            byte[] readBuf = new byte[1024];
            IPHostEntry ipHostInfo = Dns.GetHostEntry("localhost");
            
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint endpoint = new IPEndPoint(ipAddress, port);
            Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            sender.Connect(endpoint);
            
            Console.Write("Enter message: ");
            string data = Console.ReadLine();
            
            Console.WriteLine("The socket will connect to the {0}\n", sender.RemoteEndPoint.ToString());
            
            byte[] msg = Encoding.UTF8.GetBytes(data);
            int readBufS = sender.Send(msg);
            int readBufR = sender.Receive(readBuf);
            
            Console.WriteLine("The response from the server: {0}\n\n", Encoding.UTF8.GetString(readBuf, 0, readBufR));
            
            if (data.IndexOf("<TheEnd>")==-1) {
            	Console.WriteLine("Stop response");
            }
            	
            sendMessage(port);
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
            Console.WriteLine(" ");

        }
		
		
		
		public static void Main(String[] args) {
			
			try {
				
                sendMessage(11000);
              
            }
			catch (Exception e) {
            
                Console.WriteLine(e.ToString());
                
            }		

		}
	}
}
