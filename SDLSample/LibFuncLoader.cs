using System;
using System.Runtime.InteropServices;

namespace SDLSample
{
    /// <summary>
    /// Helps interop by loading libraries and functions for the Windows, Linux and OSX platforms.
    /// </summary>
    public class LibFuncLoader
    {
        private const int RTLD_LAZY = 0x0001;

        #region P/Invokes
        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern IntPtr WinLoadLib(string lpszLib);//Original function name is LoadLibraryW


        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        private static extern IntPtr WinGetFunc(IntPtr hModule, string procName);//Original function name is GetProcAddress


        [DllImport("libdl.so.2")]
        private static extern IntPtr LinuxGetFunc(IntPtr handle, string symbol);//Original function name is dlsym


        [DllImport("libdl.so.2")]
        private static extern IntPtr LinuxLoadLib(string path, int flags);//Original function name is dlopen


        [DllImport("/usr/lib/libSystem.dylib")]
        private static extern IntPtr OSXGetFunc(IntPtr handle, string symbol);//Original function name is dlsym


        [DllImport("/usr/lib/libSystem.dylib")]
        private static extern IntPtr OSXLoadLib(string path, int flags);//Original function name is dlopen
        #endregion


        #region Public Methods
        /// <summary>
        /// Returns a pointer to a library with the given <paramref name="libName"/>.
        /// </summary>
        /// <param name="libName">The name of the library.</param>
        /// <returns></returns>
        public static IntPtr GetLibraryPointer(string libName)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return WinLoadLib(libName);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return LinuxLoadLib(libName, RTLD_LAZY);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return OSXLoadLib(libName, RTLD_LAZY);


            throw new Exception("Operating System Not Recognized");
        }


        /// <summary>
        /// Returns a pointer for the function using the given <paramref name="functionName"/> in the library
        /// using the given <paramref name="libraryPointer"/> of the given type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of method delegate/pointer to return.</typeparam>
        /// <param name="libraryPointer">The pointer to the library that contains the function that matches the given <paramref name="functionName"/>.</param>
        /// <param name="functionName">The name of the function.</param>
        /// <param name="throwIfNotFound">True if an exception should be thrown if the function is not found.</param>
        /// <returns></returns>
        public static T GetFunction<T>(IntPtr libraryPointer, string functionName, bool throwIfNotFound = false)
        {
            IntPtr funcPtr;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                funcPtr = WinGetFunc(libraryPointer, functionName);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
               funcPtr = LinuxGetFunc(libraryPointer, functionName);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                funcPtr = OSXGetFunc(libraryPointer, functionName);
            }
            else
            {
                throw new Exception("Operating System Not Recognized");
            }

            if (funcPtr == IntPtr.Zero)
            {
                if (throwIfNotFound)
                    throw new EntryPointNotFoundException(functionName);

                return default;
            }


            return (T)(object)Marshal.GetDelegateForFunctionPointer(funcPtr, typeof(T));
        }
        #endregion
    }
}
