using System;
using System.Runtime.InteropServices;

public class NativeLibrary : IDisposable
{
    private IntPtr libraryHandle;
    private string path;

    [DllImport("kernel32")]
    private static extern IntPtr LoadLibrary(string path);

    [DllImport("kernel32")]
    private static extern IntPtr GetProcAddress(IntPtr libraryHandle, string symbolName);

    [DllImport("kernel32")]
    private static extern bool FreeLibrary(IntPtr libraryHandle);

    public T GetDelegate<T>(string functionName)
        where T : class
    {
        IntPtr symbol = GetProcAddress(libraryHandle, functionName);
        if (symbol == IntPtr.Zero)
        {
            throw new Exception("Couldn't get function: " + functionName);
        }

        return Marshal.GetDelegateForFunctionPointer(symbol, typeof(T)) as T;
    }

    public NativeLibrary(string path)
    {
        this.path = path;
        libraryHandle = LoadLibrary(path);
        if (libraryHandle == IntPtr.Zero)
        {
            throw new Exception("Couldn't open native library: " + path);
        }
    }

    public void Dispose()
    {
        FreeLibrary(libraryHandle);
        libraryHandle = IntPtr.Zero;
    }
}