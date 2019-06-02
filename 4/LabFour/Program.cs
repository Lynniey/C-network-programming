using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace SharpPcapLab {

	class Program {
	
		static void Main(string[] args) {
		
			IList<IPAddress> IPs = new List<IPAddress>();
			String answer = "";
			
			while (!(answer.Equals("no"))) {
				
				Console.Write("Enter IP-address: ");
				String IP = Console.ReadLine();
				
				try {
					
					IPs.Add(IPAddress.Parse(IP));
					
				} catch {
					
					continue;
					
				}
				
				Console.Write("\nContinue? (yes/no) ");
				answer = Console.ReadLine();
			}
			
			for (int CurrPort = 0; CurrPort <= 65535; CurrPort++) {
				
				foreach (IPAddress IP in IPs) {
					
					scann(IP, CurrPort);
					
				}
			}
			Console.ReadKey();
		}
		
		
		
		static void scann(IPAddress ipaddres, int port) {
		
			IPEndPoint IPEnd = new IPEndPoint(ipaddres, port);
			
			try {
				
				Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				sock.Connect(IPEnd);
				
				Console.WriteLine(ipaddres.ToString() + " --> " + port + " open");
				sock.Close();
				
			} catch (Exception) {
				
				Console.WriteLine(ipaddres.ToString() + " --> " + port + " close");
				
			}
		}
	}
}

