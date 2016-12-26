# libShout-csharp
Привязка к libShout на языке C#

Пример испльзования:


class Program
{
    static Libshout icecast;
    static byte[] buff = new byte[4096];
    static int read;

    static void Main(string[] args)
    {
        string filename = "";
        if (args.Count() > 0)
            filename = args[0];
        else return;

        icecast = new Libshout();
        icecast.setProtocol(0);
        icecast.setHost("127.0.0.1");
        icecast.setPort(8000);
        icecast.setPassword("hackme");
        icecast.setFormat(Libshout.FORMAT_MP3);
        icecast.setPublic(true);
        icecast.setName("radio");
        icecast.setMount("/live");
        icecast.open();

        //подключились
        if (icecast.isConnected())
            Console.WriteLine("Connect!");
        else Console.WriteLine(icecast.GetError());

        //читаем файл
        BinaryReader reader = new BinaryReader(File.Open(filename, FileMode.Open));
        int total = 0;
        while (true)
        {
            //читаем буфер
            read = reader.Read(buff, 0, buff.Length); 
            total = total + read;

            Console.WriteLine("Position:  "+reader.BaseStream.Position);
            //если прочитан не весь, то передаем
            if (read > 0)
            {
                icecast.send(buff, read);    //пауза, синхронизация внутри метода
            }
            else break;  //уходим
            
        }

        Console.WriteLine("Done!");
        Console.ReadKey(true);
        icecast.close();            
    }
}
