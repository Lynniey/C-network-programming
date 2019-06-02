using System;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace SharpPcapLab {
	
	public class Program {
	
		static void tcpListeners() {
        
            IPHostEntry ipHostInfo = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint endpoint = new IPEndPoint(ipAddress, 11000);
            Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            
            try {
            
                listener.Bind(endpoint);
                listener.Listen(50);
                
                while (true) {
                
                    Console.WriteLine("Expected connection port: {0}", endpoint);
                    Socket handler = listener.Accept();
                    string data = null;
                    byte[] readBuf = new byte[1024];
                    int readBufR = handler.Receive(readBuf);
                    data += Encoding.UTF8.GetString(readBuf, 0, readBufR);
                    
                    Console.Write("Text: " + data + "\n");
                    Console.WriteLine("Enter the answer: ");
                    string response = Console.ReadLine();
                    
                    byte[] msg = Encoding.UTF8.GetBytes(response);
                    handler.Send(msg);
                    
                    if (data == "QUIT") {
                    
                        Console.WriteLine("The server closed the connection");
                        break;
                        
                    }
                    
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();

                }
            }
            catch (Exception e) {
            
                Console.WriteLine(e.ToString());
            }
        }
		
		
		static void Main(string[] args) {
        
            tcpListeners();
        }
	}
}
