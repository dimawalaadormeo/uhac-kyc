using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UHACK
{
    public static class Utilities
    {
        //For creating CSV files
        public static bool createCSV(string accountNumber, string firstLine, string secondLine, string thirdLine)
        {
            try
            {
                string filePath = @"D:\temp\" + accountNumber + ".csv";
                string delimiter = ",";

                string[][] output = new string[][]{  
                    new string[]{firstLine},  
	                new string[]{secondLine},  
                    new string[]{thirdLine}
	            };
                int length = output.GetLength(0);
                StringBuilder sb = new StringBuilder();
                for (int index = 0; index < length; index++)
                    sb.AppendLine(string.Join(delimiter, output[index]));

                File.WriteAllText(filePath, sb.ToString());

                return true;
            }
            catch
            {
                return false;
            }
        }
        
        //Save snapshot to a file
        public static bool saveImageToFile(PictureBox pictureBox, string fileName)
        {
            try
            {
                pictureBox.Image.Save(@"D:\temp\" + fileName, ImageFormat.Jpeg);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string uploadFileToServer(string fileName)
        {
            
            WebClient client = new WebClient();

            NetworkCredential networkCredentials = new NetworkCredential("root", "");

            Uri uri = new Uri(@"http://localhost:8080/01/" + fileName);

            client.Credentials = networkCredentials;
            byte[] arrReturn = client.UploadFile(uri, fileName);

            return arrReturn.ToString();

        }
    }

    public class UDPClient
    {
        static Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
            ProtocolType.Udp);
        static IPAddress send_to_address = IPAddress.Parse("127.0.0.1");
        static IPEndPoint sending_end_point = new IPEndPoint(send_to_address, 15001);
        
        public static bool sendUDP(string message)
        {
            try
            {
                byte[] send_buffer = Encoding.ASCII.GetBytes(message);
                sending_socket.SendTo(send_buffer, sending_end_point);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
