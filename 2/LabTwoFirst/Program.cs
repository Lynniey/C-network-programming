using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PacketDotNet;
using SharpPcap;
using System.Net.NetworkInformation;
using System.IO;

namespace SharpPcapLab {
	
    class Program {
		
        static void createPacket() {
			
            Console.Write("Enter name of text file (for example t.txt): ");
            string path = Console.ReadLine();
            string txtFile = File.ReadAllText(path, Encoding.Default);
            
            
            ushort tcpSourcePort = 123;
			ushort tcpDestinationPort = 321;
            var tcpPacket = new TcpPacket(tcpSourcePort, tcpDestinationPort);
            
            tcpPacket.PayloadData = Encoding.UTF8.GetBytes(txtFile);
            
            var ipSourceAddress = System.Net.IPAddress.Parse("127.0.0.1");
            var ipDestinationAddress = System.Net.IPAddress.Parse("127.0.0.2");
            var ipPacket = new IPv4Packet( ipSourceAddress, ipDestinationAddress);

            var sourceHwAddress = PhysicalAddress.Parse("90-90-90-90-90-90");
            var destinationHwAddress = PhysicalAddress.Parse("80-80-80-80-80-80");
            var ethernetPacket = new EthernetPacket(sourceHwAddress, destinationHwAddress, EthernetPacketType.None);
            ipPacket.PayloadPacket = tcpPacket;
            ethernetPacket.PayloadPacket = ipPacket;
            getPacket(ethernetPacket);
            
        }

        
        
        public static void getPacket(Packet packet) {
			
            List<NetworkInterface> INTERFACES = new List<NetworkInterface> { };
            
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces()) {
                
            	INTERFACES.Add(nic);
            	
            }
            
            for (int i = 0; i < INTERFACES.Count; i++) {
                
            	Console.WriteLine("\n\n\t" + i + ". " + INTERFACES[i].Name);
            	
            }
            
            bool flag = true;
            int number;
            CaptureDeviceList deviceList = CaptureDeviceList.Instance;
            string num;
            while (flag) {
            	
                while (true) {
            		
                    Console.Write("\n\nEnter interface number(Enter 9 for exit): ");
                    num = Console.ReadLine();
                    
                    if (!(int.TryParse(num, out number))) {
                    	
                        continue;
                        
                    }
                    
                    number = Convert.ToInt32(num);
                    
                    if (!(number > INTERFACES.Count || number < 0)) {
                        
                    	break;
                    	
                    }
                }
            	
                if (number == INTERFACES.Count) {
            		
                    flag = false;
                    
                }
            	
                else {
            		
                    Console.WriteLine();
                    foreach (ICaptureDevice dev in CaptureDeviceList.Instance) {
                    	
                        if (devName(dev.Name) == INTERFACES[number].Id) {
                    		
                            Console.WriteLine(dev.Name);
                            ICaptureDevice captured = dev;
                            captured.Open();
                            
                            try {
                            	
                                captured.SendPacket(packet);
                                Console.WriteLine("\n-- Packet sent successfuly");
                                Console.WriteLine("\nPress Enter");
                                
                            }
                            
                            catch (Exception e) {
                            	
                                Console.WriteLine("--" + e.Message);
                                
                            }
                            
                            Console.ReadLine();
                            captured.StopCapture();
                            Console.WriteLine("-- Device closed");
                           
                        }
                    }
                }
            }
        }

        static string devName(string str) {
            int count = 0;
            
            for (int i = 0; i < str.Length; i++) {
            	
            	if (str[i] == '{') {
                
                    count = i;
                    
                }
            }
            
            return (str.Substring(count));
            
        }

		static void Main(string[] args) {
        
            createPacket();
            Console.ReadLine();
            
        }
    }
}