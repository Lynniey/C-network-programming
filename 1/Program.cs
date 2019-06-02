using System;
using System.Collections.Generic;
using System.Text;
using PacketDotNet;
using SharpPcap;

namespace SharpPcapLab {
	
	public class Program {
	
		public static void getDevices() {
			
			string ver = SharpPcap.Version.VersionString;
			Console.WriteLine("SharpPcap {0}, Example1.IfList.cs", ver);

			CaptureDeviceList devices = CaptureDeviceList.Instance;
			
			if(devices.Count < 1)
			{
			    Console.WriteLine("No devices were found on this machine");
			    return;
			}

			Console.WriteLine("\nThe following devices are available on this machine:");
			Console.WriteLine("----------------------------------------------------\n");
		
			foreach(ICaptureDevice dev in devices)
			    Console.WriteLine("{0}\n", dev.ToString());
		}
		
		
		
		private static void device_OnPacketArrival(object sender, CaptureEventArgs e) {
    
			//Packet packets = PacketDotNet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);
			//var ipPacket = (TcpPacket)e.Packet.Extract(typeof(IpPacket)) as IpPacket;
    		
			Packet packet = Packet.ParsePacket(e.Packet.LinkLayerType, e.Packet.Data);
            var ipPacket = IpPacket.GetEncapsulated(packet);
            string srcIp = ipPacket.SourceAddress.ToString();
            string dstIp = ipPacket.DestinationAddress.ToString();
            
			DateTime time = e.Packet.Timeval.Date;
    		int len = e.Packet.Data.Length;
    		Console.WriteLine("Time is {0}:{1}:{2},{3} Len={4}, srcIP={5}, dstIP={6}",
        		time.Hour, time.Minute, time.Second, time.Millisecond, len, srcIp, dstIp);

		}
		
		
		
		public static void caputureDevice() {
			
			CaptureDeviceList devices = CaptureDeviceList.Instance;
            Console.WriteLine("Enter device number");
            
            try
            {
            	int i=int.Parse(Console.ReadLine());
            	
                ICaptureDevice device = devices[i];
                
                device.OnPacketArrival += new SharpPcap.PacketArrivalEventHandler(device_OnPacketArrival);

                int readTimeoutMilliseconds = 1000;
                device.Open(DeviceMode.Promiscuous, readTimeoutMilliseconds);

                Console.WriteLine("-- Listening on {0}, hit 'Enter' to stop...", device.Description);

                device.StartCapture();

                Console.ReadLine();

                device.StopCapture();

                device.Close();

            }
            catch (FormatException e)
            {
            	 Console.WriteLine("You have entered non-numeric characters");
            	 Console.ReadKey();
            }
            
            
			Console.WriteLine("\n\nPress Enter to exit");
            Console.ReadKey();
			
		}
		
		
		
		public static void Main(String[] args) {
			
			Console.WriteLine("1. Retrieve the device list");
			Console.WriteLine("------------------------------\n");
			getDevices();
			
			Console.WriteLine("2-3. Captring packets and information");
			Console.WriteLine("------------------------------\n");
			caputureDevice();
			

		}
	}
}
