using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace icecastSource
{
    public class Libshout
    {
        private const int SUCCESS = 0;
        private const int CONNECTED = -7;
        private int instance;

        public const int FORMAT_OGG = 0;
        public const int FORMAT_MP3 = 1;
        public const int PROTOCOL_HTTP = 0;
        public const int PROTOCOL_XAUDIOCAST = 1;
        public const int PROTOCOL_ICY = 2;
        public const string INFO_BITRATE = "bitrate";
	    public const string INFO_SAMPLERATE = "samplerate";
	    public const string INFO_CHANNELS = "channels";
        public const string INFO_QUALITY = "quality";

        public Libshout() 
        {
            shout_init();
		    this.instance = shout_new();
        }

        IntPtr StrToPtr(string msg)
        {
            return Marshal.StringToHGlobalAuto(msg);
        }

        string PtrToStr(IntPtr intr)
        {
            return Marshal.PtrToStringAnsi(intr);
        }

        #region Wraper Methods

        /// <summary>
        /// Открыть соединение к IceCast. Все параметры УЖЕ должны быть установлены
        /// </summary>
        public void open()
        {
		    if (shout_open(this.instance) != SUCCESS) {
                throw new IOException(GetError());
            }
        }

        /// <summary>
        /// закрыть соединение
        /// </summary>
        public void close()
        {
            if (shout_close(this.instance) == SUCCESS)
            {
                shout_free(this.instance);
            }
            // shout_shutdown();
        }

        /// <summary>
        /// получить версию
        /// </summary>
        /// <returns></returns>
        public string getVersion()
        {
            return shout_version(0, 0, 0);
        }

        

        /// <summary>
        /// Get Icecast host
        /// </summary>
        /// <param name="host">Icecast host</param>
        public void setHost(string host)
        {
            byte[] retArray = Encoding.ASCII.GetBytes(host);
            byte[] retArrayZ = new byte[retArray.Length + 1];
            Array.Copy(retArray, retArrayZ, retArray.Length);
            retArrayZ[retArrayZ.Length - 1] = 0;
              IntPtr retPtr = Marshal.AllocHGlobal(retArrayZ.Length);
              Marshal.Copy(retArrayZ, 0, retPtr, retArrayZ.Length);

            

            IntPtr hostPtr = StrToPtr(host);
            var intr = shout_set_host(this.instance, retPtr);            
            if (intr != SUCCESS)
            {                
                throw new IOException(GetError());
            }
        }

        public string GetError()
        {
           return Marshal.PtrToStringAnsi(shout_get_error(this.instance));
        }
        
        
        /// <summary>
        /// Получить Icecast host        
        /// </summary>
        /// <returns></returns>
        public String getHost()
        {
            return PtrToStr(shout_get_host(this.instance));
        }

        /// <summary>
        /// Set Icecast protocol
        /// </summary>
        /// <param name="protocol"></param>
        public void setProtocol(int protocol) 
        {
		    if (shout_set_protocol(this.instance, protocol) != SUCCESS)
            {
                throw new IOException(GetError());
            }
        }

        /// <summary>
        /// Get Icecast protocol
        /// </summary>
        /// <returns></returns>
        public int getProtocol()
        {
            return shout_get_protocol(this.instance);
        }

        /// <summary>
        /// Set Icecast port
        /// </summary>
        /// <param name="port">Icecast port</param>
        public void setPort(int port) 
        {
		    if (shout_set_port(this.instance, port) != SUCCESS)
            {
                throw new IOException(GetError());
            }
        }

        /// <summary>
        /// Get Icecast port
        /// </summary>
        /// <returns></returns>
        public int getPort()
        {
            return shout_get_port(this.instance);
        }

        /// <summary>
        /// Set Icecast password
        /// </summary>
        /// <param name="password">password</param>
        public void setPassword(string password)
        {
		    if (shout_set_password(this.instance, password) != SUCCESS)
            {
                throw new IOException(GetError());
            }
        }

        /// <summary>
        /// Get Icecast password
        /// </summary>
        /// <returns></returns>
        public String getPassword()
        {
            return PtrToStr(shout_get_password(this.instance));
        }

        /// <summary>
        /// Назначить точку монтирования
        /// </summary>
        /// <param name="mount">точка монтирования</param>
        public void setMount(String mount) 
        {
		    if (shout_set_mount(this.instance, mount) != SUCCESS)
            {
                throw new IOException(GetError());
            }
        }


        /// <summary>
        /// Получить точку монтирования
        /// </summary>
        /// <returns></returns>
        public String getMount()
        {
            return PtrToStr(shout_get_mount(this.instance));
        }

        
        /// <summary>
        /// Установить параметр формата
        /// </summary>
        /// <param name="format">формат</param>
        public void setFormat(int format) 
        {
		    if (shout_set_format(this.instance, format) != SUCCESS)
            {
                throw new IOException(GetError());
            }
        }

        /// <summary>
        /// Получить параметр формата
        /// </summary>
        /// <returns></returns>
        public int getFormat()
        {
            return shout_get_format(this.instance);
        }

        /// <summary>
        /// Отправить данные на IceCast, парсинг для синхронизации информации 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="length"></param>
        public void send(byte[] data, int length) 
        {
		    if (shout_send(this.instance, data, length) != SUCCESS)
            {
                throw new IOException(GetError());
            }
          //  DateTime dt = DateTime.Now;
            shout_sync(this.instance);
        //    var spin = DateTime.Now - dt;
         //   Console.WriteLine(spin.TotalMilliseconds);
        }

        public void sendraw(byte[] data, int length)
        {
            shout_send_raw(this.instance, data, length);
           
            shout_sync(this.instance);
        }
        
        /// <summary>
        /// Установить параметр имя
        /// </summary>
        /// <param name="name">имя</param>
        public void setName(String name) 
        {
		    if (shout_set_name(this.instance, name) != SUCCESS)
            {
                throw new IOException(GetError());
            }
        }

        /// <summary>
        /// Получить параметр имя
        /// </summary>
        /// <returns></returns>
        public String getName()
        {
            return PtrToStr(shout_get_name(this.instance));
        }

        /// <summary>
        /// Установить параметр Url
        /// </summary>
        /// <param name="url"></param>
        public void setUrl(String url)
        {
		    if (shout_set_url(this.instance, url) != SUCCESS)
            {
                throw new IOException(GetError());
            }
        }

        /// <summary>
        /// Получить параметр Url
        /// </summary>
        /// <returns></returns>
        public String getUrl()
        {
            return PtrToStr(shout_get_url(this.instance));
        }

        /// <summary>
        /// Установить параметр Жанр
        /// </summary>
        /// <param name="genre">название жанра</param>
        public void setGenre(String genre)
        {
		    if (shout_set_genre(this.instance, genre) != SUCCESS)
            {
                throw new IOException(GetError());
            }
        }

        /// <summary>
        /// Получить параметр Жанр
        /// </summary>
        /// <returns></returns>
        public String getGenre()
        {
            return PtrToStr(shout_get_genre(this.instance));
        }

        
        /// <summary>
        /// Установить Icecast пользователя
        /// </summary>
        /// <param name="username"></param>
        public void setUser(String username) 
        {
		    if (shout_set_user(this.instance, username) != SUCCESS)
            {
                throw new IOException(GetError());
            }
        }

        /// <summary>
        /// Получить Icecast пользователя
        /// </summary>
        /// <returns></returns>
        public String getUser()
        {
            return PtrToStr(shout_get_user(this.instance));
        }

        
        /// <summary>
        /// Установить Icecast agent
        /// </summary>
        /// <param name="agent"></param>
        public void setAgent(String agent) 
        {
		    if (shout_set_agent(this.instance, agent) != SUCCESS)
            {
                throw new IOException(GetError());
            }
        }

        /// <summary>
        /// Получить Icecast agent
        /// </summary>
        /// <returns></returns>
        public String getAgent()
        {
            return PtrToStr(shout_get_agent(this.instance));
        }

        /// <summary>
        /// Установить Параметр Описание
        /// </summary>
        /// <param name="description"></param>
        public void setDescription(String description) 
        {
		    if (shout_set_description(this.instance, description) != SUCCESS)
            {
                throw new IOException(GetError());
            }
        }

        /// <summary>
        /// Получить Параметр Описание
        /// </summary>
        /// <returns></returns>
        public String getDescription()
        {
            return PtrToStr(shout_get_description(this.instance));
        }

        /// <summary>
        /// Назначить Дамп-файл
        /// </summary>
        /// <param name="dumpfile"></param>
        public void setDumpfile(String dumpfile) 
        {
		    if (shout_set_dumpfile(this.instance, dumpfile) != SUCCESS)
            {
                throw new IOException(GetError());
            }
        }

        /// <summary>
        /// Получить Дамп-файл
        /// </summary>
        /// <returns></returns>
        public String getDumpfile()
        {
            return PtrToStr(shout_get_dumpfile(this.instance));
        }

        /// <summary>
        /// Назначить параметр Info
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void setInfo(String key, String value) 
        {
		    if (shout_set_audio_info(this.instance, key, value) != SUCCESS)
            {
                throw new IOException(GetError());
            }
        }

        /// <summary>
        /// Получить параметр Info
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public String getInfo(String key)
        {
            return PtrToStr(shout_get_audio_info(this.instance, key));
        }

        /// <summary>
        /// Установить параметр MP3-мета
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void setMeta(String key, String value)
        {

            int instanceMeta = shout_metadata_new();
            if (shout_set_metadata(this.instance, instanceMeta) != SUCCESS) {
                throw new IOException(GetError());
            }
            if (shout_metadata_add(instanceMeta, key, value) != SUCCESS) {
                throw new IOException(GetError());
            }
        }

        /// <summary>
        /// Подключение к Icecast установлено ?
        /// </summary>
        /// <returns></returns>
        public bool isConnected()
        {
            return shout_get_connected(this.instance) == CONNECTED ? true : false;
        }

        /// <summary>
        /// Установить параметр public
        /// </summary>
        /// <param name="isPublic"></param>
        public void setPublic(bool isPublic)
        {
            if (shout_set_public(this.instance, isPublic == true ? 1 : 0) != SUCCESS) {
                throw new IOException(GetError());
            }
        }

        
        /// <summary>
        /// Получить параметр public
        /// </summary>
        /// <returns></returns>
        public bool isPublic()
        {
            return shout_get_public(this.instance) == 1 ? true : false;
        }

        
        /// <summary>
        /// Установка Icecast в неблокирующей режим. Должен быть установлен перед открытым
        /// </summary>
        /// <param name="isNonBlocking"></param>
        public void setNonBlocking(bool isNonBlocking)
        {
            if (shout_set_nonblocking(this.instance, isNonBlocking == true ? 1 : 0) != SUCCESS)
            {
                throw new IOException(GetError());
            }
        }

        /// <summary>
        /// Icecast установлен в не блокировки режиме ?
        /// </summary>
        /// <returns></returns>
        public bool isNonBlocking()
        {
            return shout_get_nonblocking(this.instance) == 1 ? true : false;
        }

        /// <summary>
        /// Количество байт, в настоящее время на очереди записи (имеет смысл только в неблокирующем режиме)
        /// </summary>
        /// <returns></returns>
        public int getQueueLen()
        {
            return shout_queuelen(this.instance);
        }

        
        /// <summary>
        /// Получить значение задержки в миллисекундах перед след. отправкй пакета
        /// </summary>
        /// <returns></returns>
        public int getDelay()
        {
            return shout_delay(this.instance);
        }

        public void sync()
        {
            shout_sync(this.instance);
        }
        #endregion

        #region WRAPPER
       

        [DllImport("libshout-3.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern void shout_init();

        [DllImport("libshout-3.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern void shout_shutdown();

        [DllImport("libshout-3.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern string shout_version(int major, int minor, int patch);

        [DllImport("libshout-3.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern int shout_new();

        [DllImport("libshout-3.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern void shout_free(int instance);
        
        [DllImport("libshout-3.dll" , CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]       
        internal static extern IntPtr shout_get_error(int instance);
        
        [DllImport("libshout-3.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int shout_get_errno(int instance);

        [DllImport("libshout-3.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern int shout_get_connected(int instance);

        [DllImport("libshout-3.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern int shout_set_host(int instance, IntPtr host);

        [DllImport("libshout-3.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr shout_get_host(int instance);

        [DllImport("libshout-3.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern int shout_set_port(int instance, int port);

        [DllImport("libshout-3.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern int shout_get_port(int instance);

        [DllImport("libshout-3.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern int shout_set_password(int instance, String password);

        [DllImport("libshout-3.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr shout_get_password(int instance);

        [DllImport("libshout-3.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern int shout_set_mount(int instance, String mount);

        [DllImport("libshout-3.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr shout_get_mount(int instance);

        [DllImport("libshout-3.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern int shout_set_name(int instance, String name);

        [DllImport("libshout-3.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr shout_get_name(int instance);

        [DllImport("libshout-3.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern int shout_set_url(int instance, String url);

        [DllImport("libshout-3.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr shout_get_url(int instance);

        [DllImport("libshout-3.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern int shout_set_genre(int instance, String genre);

        [DllImport("libshout-3.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr shout_get_genre(int instance);

        [DllImport("libshout-3.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern int shout_set_user(int instance, String username);

        [DllImport("libshout-3.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr shout_get_user(int instance);

        [DllImport("libshout-3.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern int shout_set_agent(int instance, String agent);

        [DllImport("libshout-3.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr shout_get_agent(int instance);

        [DllImport("libshout-3.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern int shout_set_description(int instance, String description);

        [DllImport("libshout-3.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr shout_get_description(int instance);

        [DllImport("libshout-3.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern int shout_set_dumpfile(int instance, String dumpfile);

        [DllImport("libshout-3.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr shout_get_dumpfile(int instance);

        [DllImport("libshout-3.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern int shout_set_audio_info(int instance, String key, String value);

        [DllImport("libshout-3.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr shout_get_audio_info(int instance, String key);

        [DllImport("libshout-3.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern int shout_set_public(int instance, int isPublic);

        [DllImport("libshout-3.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern int shout_get_public(int instance);

        [DllImport("libshout-3.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern int shout_set_format(int instance, int format);

        [DllImport("libshout-3.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern int shout_get_format(int instance);

        [DllImport("libshout-3.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern int shout_set_protocol(int instance, int protocol);

        [DllImport("libshout-3.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern int shout_get_protocol(int instance);

        [DllImport("libshout-3.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern int shout_set_nonblocking(int instance, int isNonBlocking);

        [DllImport("libshout-3.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern int shout_get_nonblocking(int instance);

        [DllImport("libshout-3.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern int shout_open(int instance);

        [DllImport("libshout-3.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern int shout_close(int instance);

        [DllImport("libshout-3.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern int shout_send(int instance, byte[] data, int length);

        [DllImport("libshout-3.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern int shout_send_raw(int instance, byte[] data, int length);

        [DllImport("libshout-3.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern int shout_queuelen(int instance);

        [DllImport("libshout-3.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern void shout_sync(int instance);

        [DllImport("libshout-3.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern int shout_delay(int instance);

        [DllImport("libshout-3.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern int shout_metadata_new();

        [DllImport("libshout-3.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern int shout_set_metadata(int instance, int instanceMeta);

        [DllImport("libshout-3.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern int shout_metadata_add(int instanceMeta, String key, String value);

        [DllImport("libshout-3.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern void shout_metadata_free(int instanceMeta);
        #endregion

        enum SHOUTERR
        {
            SHOUTERR_SUCCESS = 0,
            SHOUTERR_INSANE = -1,
            SHOUTERR_NOCONNECT = -2,
            SHOUTERR_NOLOGIN = -3,
            SHOUTERR_SOCKET = -4,
            SHOUTERR_MALLOC = -5,
            SHOUTERR_METADATA = -6,
            SHOUTERR_CONNECTED = -7,
            SHOUTERR_UNCONNECTED = -8,
            SHOUTERR_UNSUPPORTED = -9
        }

        enum SHOUT_FORMAT
        {
            SHOUT_FORMAT_OGG	=0,
            SHOUT_FORMAT_MP3	=1,
            SHOUT_FORMAT_WEBM	=2
        }

        enum SHOUT_PROTOCOL
        {
            SHOUT_PROTOCOL_HTTP = 0,
            SHOUT_PROTOCOL_XAUDIOCAST = 1,
            SHOUT_PROTOCOL_ICY = 2
        }

        public const string SHOUT_AI_BITRATE = "bitrate";
        public const string SHOUT_AI_SAMPLERATE = "samplerate";
        public const string SHOUT_AI_CHANNELS = "channels";
        public const string SHOUT_AI_QUALITY = "quality";
        

        }

    struct shout
    {
        /* hostname or IP of icecast server */
        string host;
        /* port of the icecast server */
        int port;
        /* login password for the server */
        string password;
        /* server protocol to use */
        int protocol;
        /* type of data being sent */
        int format;
        /* audio encoding parameters */
        
        int error;
    }
}
