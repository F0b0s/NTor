using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenSSL.SSL;
using OpenSSL.X509;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            var tcpClient = new TcpClient("178.24.64.33", 443);

            //            using(var stream = tcpClient.GetStream())
            //            {
            //                var clientMessage = Encoding.ASCII.GetBytes("Client hello");
            //                stream.Write(clientMessage, 0, clientMessage.Length);
            //                var bufer = new byte[1000];
            //                var r = stream.Read(bufer, 0, bufer.Length);
            //            }

            //            return;

            Console.WriteLine("Client connected.");
            var sslStream = new SslStream(
                tcpClient.GetStream(),
                false,
                new RemoteCertificateValidationHandler(ValidateServerCertificate),
                null
                );

            //            sslStream.SslProtocol = 
            try
            {
                sslStream.AuthenticateAsClient("default");
            }
            catch (AuthenticationException e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
                if (e.InnerException != null)
                {
                    Console.WriteLine("Inner exception: {0}", e.InnerException.Message);
                }
                Console.WriteLine("Authentication failed - closing the connection.");
                tcpClient.Close();
                return;
            }
        }

        public static bool ValidateServerCertificate(
      object sender,
      X509Certificate certificate,
      X509Chain chain,
      int some, VerifyResult verifyResult)
        {
            //            if (sslPolicyErrors == SslPolicyErrors.None)
            //                return true;

            //Console.WriteLine("Certificate error: {0}", sslPolicyErrors);

            // Do not allow this client to communicate with unauthenticated servers. 
            return false;
        }

    }
}
