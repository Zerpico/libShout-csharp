using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace LibshoutSharp
{
    public class Libshout
    {
        private IntPtr instance;
		
		/// <summary> Icecast host </summary>
		public string Host
		{
			get => GetHost();
			set => SetHost(value);
		}
		
		/// <summary> Icecast host </summary>
		public int Port
		{
			get => GetPort();
			set => SetPort(value);
		}
		
		/// <summary> Icecast protocol </summary>
		public SHOUT_PROTOCOL Protocol
		{
			get => GetProtocol();
			set => SetProtocol(value);
		}
		
		/// <summary> User for connection. if not set, default: source </summary>
		public string User
		{
			get => GetUser();
			set => SetUser(value);
		}
		
		/// <summary> Password for connection </summary>
		public string Password
		{
			get => GetPassword();
			set => SetPassword(value);
		}
		
		/// <summary> Mount for connection </summary>
		public string Mount
		{
			get => GetMount();
			set => SetMount(value);
		}
		
		/// <summary> Format icecast for sending </summary>
		public SHOUT_FORMAT Format
		{
			get => GetFormat();
			set => SetFormat(value);
		}
		
		public string Name
		{
			get => GetName();
			set => SetName(value);
		}
		
		public string Url
		{
			get => GetUrl();
			set => SetUrl(value);
		}
		
		public string Genre
		{
			get => GetGenre();
			set => SetGenre(value);
		}		
				
		/// <summary> Icecast agent </summary>
		public string Agent
		{
			get => GetAgent();
			set => SetAgent(value);
		}
		
		/// <summary> Icecast parameter Description </summary>
		public string Description
		{
			get => GetDescription();
			set => SetDescription(value);
		}
		
		public string Dumpfile
		{
			get => GetDumpfile();
			set => SetDumpfile(value);
		}

        public Libshout()
        {
            NativeMethods.shout_init();
            this.instance = NativeMethods.shout_new();
        }

        public Libshout(string hostname, int port, string password, SHOUT_FORMAT format) : this()
        {
            SetProtocol(SHOUT_PROTOCOL.SHOUT_PROTOCOL_HTTP);
            SetHost(hostname);
            SetPort(port);
            SetPassword(password);
            SetFormat(format);

            SetPublic(true);
            SetName("radio");
            SetMount("/live");
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

        /// <summary> Open connection to IceCast. All required parameters must already be set </summary>
        public void Open()
        {
            if (NativeMethods.shout_open(this.instance) != (int)SHOUTERR.SHOUTERR_SUCCESS)
            {
                throw new IOException(GetError());
            }
        }
		
		/// <summary> Check connection </summary>
        public bool IsConnected()
        {
            return NativeMethods.shout_get_connected(this.instance) == (int)SHOUTERR.SHOUTERR_CONNECTED ? true : false;
        }

        /// <summary> Close connection </summary>
        public void Close()
        {
            if (NativeMethods.shout_close(this.instance) == (int)SHOUTERR.SHOUTERR_SUCCESS)
            {
                NativeMethods.shout_free(this.instance);
            }
            // shout_shutdown();
        }

        /// <summary> Get version </summary>
        public string GetVersion()
        {
            return NativeMethods.shout_version(0, 0, 0);
        }


		public string GetError()
        {
            return Marshal.PtrToStringAnsi(NativeMethods.shout_get_error(this.instance));
        }
		
		/// <summary>
        /// Send data on IceCast
        /// </summary>
        /// <param name="data"></param>
        /// <param name="length"></param>
        public void Send(byte[] data, int length)
        {
            if (NativeMethods.shout_send(this.instance, data, length) != (int)SHOUTERR.SHOUTERR_SUCCESS)
            {
                throw new IOException(GetError());
            }
            NativeMethods.shout_sync(this.instance);
        }

        public void SenDraw(byte[] data, int length)
        {
            NativeMethods.shout_send_raw(this.instance, data, length);
            NativeMethods.shout_sync(this.instance);
        }


        //-------------------------------------------------------------------------------------------------------------

        /// <summary> Get Icecast host </summary>
        private void SetHost(string host)
        {
            if (NativeMethods.shout_set_host(this.instance, host) != (int)SHOUTERR.SHOUTERR_SUCCESS)
            {
                throw new IOException(GetError());
            }
        }

        /// <summary> Get Icecast host </summary>
        private string GetHost()
        {
            return PtrToStr(NativeMethods.shout_get_host(this.instance));
        }

        /// <summary> Set Icecast protocol </summary>
        private void SetProtocol(SHOUT_PROTOCOL protocol)
        {
            if (NativeMethods.shout_set_protocol(this.instance, (int)protocol) != (int)SHOUTERR.SHOUTERR_SUCCESS)
            {
                throw new IOException(GetError());
            }
        }

        /// <summary> Get Icecast protocol </summary>
        private SHOUT_PROTOCOL GetProtocol()
        {
            return (SHOUT_PROTOCOL)NativeMethods.shout_get_protocol(this.instance);
        }

        /// <summary> Set Icecast port </summary>
        private void SetPort(int port)
        {
            if (NativeMethods.shout_set_port(this.instance, port) != (int)SHOUTERR.SHOUTERR_SUCCESS)
            {
                throw new IOException(GetError());
            }
        }

        /// <summary> Get Icecast port </summary>
        private int GetPort()
        {
            return NativeMethods.shout_get_port(this.instance);
        }

        /// <summary> Set Icecast password </summary>
        private void SetPassword(string password)
        {
            if (NativeMethods.shout_set_password(this.instance, password) != (int)SHOUTERR.SHOUTERR_SUCCESS)
            {
                throw new IOException(GetError());
            }
        }

        /// <summary> Get Icecast password </summary>
        private string GetPassword()
        {
            return PtrToStr(NativeMethods.shout_get_password(this.instance));
        }

        /// <summary> Set mount point </summary>
        private void SetMount(string mount)
        {
            if (NativeMethods.shout_set_mount(this.instance, mount) != (int)SHOUTERR.SHOUTERR_SUCCESS)
            {
                throw new IOException(GetError());
            }
        }

        /// <summary> Get mount point </summary>
        private string GetMount()
        {
            return PtrToStr(NativeMethods.shout_get_mount(this.instance));
        }


        /// <summary> Set format </summary>
        /// <param name="format">format name</param>
        private void SetFormat(SHOUT_FORMAT format)
        {
            if (NativeMethods.shout_set_format(this.instance, (int)format) != (int)SHOUTERR.SHOUTERR_SUCCESS)
            {
                throw new IOException(GetError());
            }
        }

        /// <summary> Get format </summary>
        private SHOUT_FORMAT GetFormat()
        {
            return (SHOUT_FORMAT)NativeMethods.shout_get_format(this.instance);
        }

        
        /// <summary> Set Name parameter </summary>
        private void SetName(string name)
        {
            if (NativeMethods.shout_set_name(this.instance, name) != (int)SHOUTERR.SHOUTERR_SUCCESS)
            {
                throw new IOException(GetError());
            }
        }

        /// <summary> Get Name parameter </summary>
        private string GetName()
        {
            return PtrToStr(NativeMethods.shout_get_name(this.instance));
        }

        /// <summary> Set Url parameter </summary>
        private void SetUrl(string url)
        {
            if (NativeMethods.shout_set_url(this.instance, url) != (int)SHOUTERR.SHOUTERR_SUCCESS)
            {
                throw new IOException(GetError());
            }
        }

        /// <summary> Get Url parameter</summary>
        private string GetUrl()
        {
            return PtrToStr(NativeMethods.shout_get_url(this.instance));
        }

        /// <summary> Set Url Genre </summary>
        private void SetGenre(string genre)
        {
            if (NativeMethods.shout_set_genre(this.instance, genre) != (int)SHOUTERR.SHOUTERR_SUCCESS)
            {
                throw new IOException(GetError());
            }
        }

        /// <summary> Get Url Genre </summary>
        private string GetGenre()
        {
            return PtrToStr(NativeMethods.shout_get_genre(this.instance));
        }


        /// <summary>Set Icecast UserName</summary>
        private void SetUser(string username)
        {
            if (NativeMethods.shout_set_user(this.instance, username) != (int)SHOUTERR.SHOUTERR_SUCCESS)
            {
                throw new IOException(GetError());
            }
        }

        /// <summary> Get Icecast UserName </summary>
        private string GetUser()
        {
            return PtrToStr(NativeMethods.shout_get_user(this.instance));
        }


        /// <summary> Set Icecast agent </summary>
        private void SetAgent(string agent)
        {
            if (NativeMethods.shout_set_agent(this.instance, agent) != (int)SHOUTERR.SHOUTERR_SUCCESS)
            {
                throw new IOException(GetError());
            }
        }

        /// <summary> Get Icecast agent </summary>
        private string GetAgent()
        {
            return PtrToStr(NativeMethods.shout_get_agent(this.instance));
        }

        /// <summary> Set Parameter Description </summary>
        private void SetDescription(string description)
        {
            if (NativeMethods.shout_set_description(this.instance, description) != (int)SHOUTERR.SHOUTERR_SUCCESS)
            {
                throw new IOException(GetError());
            }
        }

        /// <summary> Get Parameter Description </summary>
        private string GetDescription()
        {
            return PtrToStr(NativeMethods.shout_get_description(this.instance));
        }

        /// <summary> Set Dumpfile </summary>
        private void SetDumpfile(string dumpfile)
        {
            if (NativeMethods.shout_set_dumpfile(this.instance, dumpfile) != (int)SHOUTERR.SHOUTERR_SUCCESS)
            {
                throw new IOException(GetError());
            }
        }

        /// <summary> Get Dumpfile </summary>
        private string GetDumpfile()
        {
            return PtrToStr(NativeMethods.shout_get_dumpfile(this.instance));
        }

        /// <summary> Set Parameter Info </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetInfo(string key, string value)
        {
            if (NativeMethods.shout_set_audio_info(this.instance, key, value) != (int)SHOUTERR.SHOUTERR_SUCCESS)
            {
                throw new IOException(GetError());
            }
        }

        /// <summary>
        /// Get Parameter Info
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public String GetInfo(String key)
        {
            return PtrToStr(NativeMethods.shout_get_audio_info(this.instance, key));
        }

        /// <summary>
        /// Set Parameter MP3-meta
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetMeta(String key, String value)
        {

            var instanceMeta = NativeMethods.shout_metadata_new();
            if (NativeMethods.shout_set_metadata(this.instance, instanceMeta) != (int)SHOUTERR.SHOUTERR_SUCCESS)
            {
                throw new IOException(GetError());
            }
            if (NativeMethods.shout_metadata_add(instanceMeta, key, value) != (int)SHOUTERR.SHOUTERR_SUCCESS)
            {
                throw new IOException(GetError());
            }
        }

       
        /// <summary>
        /// Set parameter public
        /// </summary>
        /// <param name="isPublic"></param>
        public void SetPublic(bool isPublic)
        {
            //I don't know what this parameter is for  
            if (NativeMethods.shout_set_public(this.instance, isPublic == true ? 1 : 0) != (int)SHOUTERR.SHOUTERR_SUCCESS)
            {
                throw new IOException(GetError());
            }
        }


        /// <summary>
        /// Get parameter public
        /// </summary>
        /// <returns></returns>
        public bool IsPublic()
        {
            return NativeMethods.shout_get_public(this.instance) == 1 ? true : false;
        }


        /// <summary>
        /// Set Icecast in NonBlocking mode. IS MUST BE SET BEFORE OPEN
        /// </summary>
        /// <param name="isNonBlocking"></param>
        public void SetNonBlocking(bool isNonBlocking)
        {
            if (NativeMethods.shout_set_nonblocking(this.instance, isNonBlocking == true ? 1 : 0) != (int)SHOUTERR.SHOUTERR_SUCCESS)
            {
                throw new IOException(GetError());
            }
        }

        /// <summary>
        /// Get Icecast is NonBlocking mode
        /// </summary>
        /// <returns></returns>
        public bool IsNonBlocking()
        {
            return NativeMethods.shout_get_nonblocking(this.instance) == 1 ? true : false;
        }

        /// <summary>
        /// Number of bytes currently on the write queue (only makes sense in nonblocking mode)
        /// </summary>
        /// <returns></returns>
        public int GetQueueLen()
        {
            return NativeMethods.shout_queuelen(this.instance);
        }


        /// <summary>
        /// Milliseconds caller should wait before sending again
        /// </summary>
        /// <returns></returns>
        public int GetDelay()
        {
            return NativeMethods.shout_delay(this.instance);
        }

        public void Sync()
        {
            NativeMethods.shout_sync(this.instance);
        }
		
        #endregion

       

        public enum SHOUTERR
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

        public enum SHOUT_FORMAT
        {
            SHOUT_FORMAT_OGG = 0,
            SHOUT_FORMAT_MP3 = 1,
            SHOUT_FORMAT_WEBM = 2
        }

        public enum SHOUT_PROTOCOL
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


}
