using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace LibshoutSharp
{
    public sealed class LibraryLoader
    {
        /// <summary>
        /// Map processor 
        /// </summary>
        private readonly Dictionary<string, string> processorArchitecturePlatforms = new Dictionary<string, string>()
            {
                { "x86", "x86" },
                { "AMD64", "x64" }
            };

        public string GetProcessArchitecture()
        {
            var procArch = System.Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
            var addressWidth = processorArchitecturePlatforms[procArch ?? string.Empty];
            
            return addressWidth;
        }

        private static LibraryLoader instance;

        private LibraryLoader() { }

        public static LibraryLoader Instance
        {
            get
            {
                if (instance == null)
                    instance = new LibraryLoader();
                return instance;
            }
        }

        public void LoadDllDirectory()
        {
            var cpu = GetProcessArchitecture();
            string dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string source = Path.Combine(dir, cpu);           

            Win32Api.SetDllDirectory(source);
        }

    }

    public static class NativeMethods
    {
        internal const string LibshoutDll = @"libshout-3.dll";                

        [DllImport(LibshoutDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void shout_init();

        [DllImport(LibshoutDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void shout_shutdown();

        [DllImport(LibshoutDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern string shout_version(int major, int minor, int patch);

        [DllImport(LibshoutDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr shout_new();

        [DllImport(LibshoutDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void shout_free(IntPtr instance);

        [DllImport(LibshoutDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr shout_get_error(IntPtr instance);

        [DllImport(LibshoutDll, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int shout_get_errno(IntPtr instance);

        [DllImport(LibshoutDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int shout_get_connected(IntPtr instance);

        [DllImport(LibshoutDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int shout_set_host(IntPtr instance, string host);

        [DllImport(LibshoutDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr shout_get_host(IntPtr instance);

        [DllImport(LibshoutDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int shout_set_port(IntPtr instance, int port);

        [DllImport(LibshoutDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int shout_get_port(IntPtr instance);

        [DllImport(LibshoutDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int shout_set_password(IntPtr instance, String password);

        [DllImport(LibshoutDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr shout_get_password(IntPtr instance);

        [DllImport(LibshoutDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int shout_set_mount(IntPtr instance, String mount);

        [DllImport(LibshoutDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr shout_get_mount(IntPtr instance);

        [DllImport(LibshoutDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int shout_set_name(IntPtr instance, String name);

        [DllImport(LibshoutDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr shout_get_name(IntPtr instance);

        [DllImport(LibshoutDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int shout_set_url(IntPtr instance, String url);

        [DllImport(LibshoutDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr shout_get_url(IntPtr instance);

        [DllImport(LibshoutDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int shout_set_genre(IntPtr instance, String genre);

        [DllImport(LibshoutDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr shout_get_genre(IntPtr instance);

        [DllImport(LibshoutDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int shout_set_user(IntPtr instance, String username);

        [DllImport(LibshoutDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr shout_get_user(IntPtr instance);

        [DllImport(LibshoutDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int shout_set_agent(IntPtr instance, String agent);

        [DllImport(LibshoutDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr shout_get_agent(IntPtr instance);

        [DllImport(LibshoutDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int shout_set_description(IntPtr instance, String description);

        [DllImport(LibshoutDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr shout_get_description(IntPtr instance);

        [DllImport(LibshoutDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int shout_set_dumpfile(IntPtr instance, String dumpfile);

        [DllImport(LibshoutDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr shout_get_dumpfile(IntPtr instance);

        [DllImport(LibshoutDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int shout_set_audio_info(IntPtr instance, String key, String value);

        [DllImport(LibshoutDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr shout_get_audio_info(IntPtr instance, String key);

        [DllImport(LibshoutDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int shout_set_public(IntPtr instance, int isPublic);

        [DllImport(LibshoutDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int shout_get_public(IntPtr instance);

        [DllImport(LibshoutDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int shout_set_format(IntPtr instance, int format);

        [DllImport(LibshoutDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int shout_get_format(IntPtr instance);

        [DllImport(LibshoutDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int shout_set_protocol(IntPtr instance, int protocol);

        [DllImport(LibshoutDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int shout_get_protocol(IntPtr instance);

        [DllImport(LibshoutDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int shout_set_nonblocking(IntPtr instance, int isNonBlocking);

        [DllImport(LibshoutDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int shout_get_nonblocking(IntPtr instance);

        [DllImport(LibshoutDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int shout_open(IntPtr instance);

        [DllImport(LibshoutDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int shout_close(IntPtr instance);

        [DllImport(LibshoutDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int shout_send(IntPtr instance, byte[] data, int length);

        [DllImport(LibshoutDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int shout_send_raw(IntPtr instance, byte[] data, int length);

        [DllImport(LibshoutDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int shout_queuelen(IntPtr instance);

        [DllImport(LibshoutDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void shout_sync(IntPtr instance);

        [DllImport(LibshoutDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int shout_delay(IntPtr instance);

        [DllImport(LibshoutDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr shout_metadata_new();

        [DllImport(LibshoutDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int shout_set_metadata(IntPtr instance, IntPtr instanceMeta);

        [DllImport(LibshoutDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int shout_metadata_add(IntPtr instanceMeta, String key, String value);

        [DllImport(LibshoutDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void shout_metadata_free(IntPtr instanceMeta);
    }
}
