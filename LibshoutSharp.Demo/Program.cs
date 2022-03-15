using System;
using System.IO;
using System.Linq;

namespace LibshoutSharp.Demo
{
    class Program
    {
        static Libshout icecast;
        static byte[] buff = new byte[4096];
        static int read;

        static void Main(string[] args)
        {
            LibraryLoader.Instance.LoadDllDirectory();

            string filename = "";
            if (args.Count() > 0)
                filename = args[0];
            else filename = Path.Combine(Environment.CurrentDirectory, "music.mp3");
           
            //set parameters
            icecast = new Libshout();            
            icecast.Protocol = Libshout.SHOUT_PROTOCOL.SHOUT_PROTOCOL_HTTP;
            icecast.Host = "127.0.0.1";
            icecast.Port=8000;
            icecast.Name = "my super radio";
            icecast.Password="hackme";
            icecast.Mount= "/example.ogg";
          //  icecast.User= "source";//or "admin"
            icecast.Format=Libshout.SHOUT_FORMAT.SHOUT_FORMAT_MP3;

			//try open connection
            icecast.Open();
           
            if (!icecast.IsConnected())
			{
				Console.WriteLine(icecast.GetError());
				return;
			}
			
			Console.WriteLine("Connect!");

            //read file
            BinaryReader reader = new BinaryReader(File.Open(filename, FileMode.Open));
            var lenght = reader.BaseStream.Length;
            int total = 0;
            var cursor = Console.GetCursorPosition();
            while (true)
            {
                //read buffer b
                read = reader.Read(buff, 0, buff.Length);
                total = total + read;

                Console.SetCursorPosition(0, cursor.Top);
                Console.WriteLine($"Position:  {(total / (double) lenght) * 100:00.00} %");
                //if not end, then send buffer to icecast 
                if (read > 0)
                {
                    icecast.Send(buff, read);  //sync inside method
                }
                else break;  
            }

            Console.WriteLine("Done!");
            Console.ReadKey(true);
            icecast.Close();
        }
    }
}