using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PacketDotNet;
using SharpPcap;
using System.Net.NetworkInformation;


namespace SharpPcapLab {

	class Program {
    
    	static void Main(string[] args) {
        
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
            		
                    Console.Write("\n\nEnter interface number: ");
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
                            captured.OnPacketArrival += new PacketArrivalEventHandler(OnPacketArrival);
                            captured.Open(DeviceMode.Promiscuous, 1000);
                            captured.Capture();
                            
                        }
                    }
                }
            }
        }

		static void OnPacketArrival(object sender, CaptureEventArgs e) {
        

            Packet packet = Packet.ParsePacket(e.Packet.LinkLayerType, e.Packet.Data);

            var ethPacket = (EthernetPacket)packet.Extract(typeof(EthernetPacket));
            var ipPacket = (IpPacket)packet.Extract(typeof(IpPacket));
            var tcpPacket = (TcpPacket)packet.Extract(typeof(TcpPacket));

            if ((tcpPacket != null) && (ipPacket.SourceAddress.ToString() == "127.0.0.1")) {
            
                Console.WriteLine("Source adress: " + ethPacket.SourceHwAddress.ToString());
                Console.WriteLine("Destination adress: " + ethPacket.DestinationHwAddress.ToString());
                Console.WriteLine("Date: " + e.Packet.Timeval.Date.ToString());
                Console.WriteLine("Lenght: " + e.Packet.Data.Length);
                string data = Encoding.UTF8.GetString(e.Packet.Data);
                Console.WriteLine(data);
                
            }
        }

		static string devName(string str) {
        
            int count = 0;
            for (int i = 0; i < str.Length; i++) {
            
                if (str[i] == '{')
                    count = i;
                
            }
            
            return (str.Substring(count));
        }
    }
}
