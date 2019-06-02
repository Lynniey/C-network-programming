using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;

namespace EmailPop
{
    class Program
    {
    	static void Main()
    	{

		TcpClient mail = new TcpClient();
        SslStream sslStream;
        int bytes = -1;

        mail.Connect("pop3.mail.ru",995);
        sslStream = new SslStream(mail.GetStream());

        sslStream.AuthenticateAsClient("pop3.mail.ru");

        byte[] buffer = new byte[2048];

        bytes = sslStream.Read(buffer, 0, buffer.Length);
        Console.WriteLine(Encoding.ASCII.GetString(buffer, 0, bytes));

        
        sslStream.Write(Encoding.ASCII.GetBytes("USER psplab@mail.ru\r\n"));
        bytes = sslStream.Read(buffer, 0, buffer.Length);
        Console.WriteLine(Encoding.ASCII.GetString(buffer, 0, bytes));

                      
        sslStream.Write(Encoding.ASCII.GetBytes("PASS rGKm2QPJ\r\n"));
        bytes = sslStream.Read(buffer, 0, buffer.Length);
        Console.WriteLine(Encoding.ASCII.GetString(buffer, 0, bytes));


        sslStream.Write(Encoding.ASCII.GetBytes("RETR 2" + "\r\n"));
        bytes = sslStream.Read(buffer, 0, buffer.Length);
        Console.WriteLine(Encoding.ASCII.GetString(buffer, 0, bytes));
        

        Console.ReadKey();
        
    	}
    }
}
