using System;
using System.Threading;

namespace DynamicLoading
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            string assemblyPath = @"D:\Sushil\Castleton\temp\DynamicLoading\DynamicLibrary\bin\Debug\netcoreapp3.0\DynamicLibrary.dll";
            //string assemblyPath = @"D:\Sushil\Castleton\temp\DynamicLoading\RestClient\bin\Debug\netcoreapp3.0\UserConnector.dll";

            WeakReference testAlcWeakRef;
            AssemblyReader reader = new AssemblyReader();
            reader.ReadAssembly(assemblyPath, out testAlcWeakRef);

            int index = 1;
            while (testAlcWeakRef.IsAlive && index < 10)
            {
                Console.WriteLine("Waiting for garbage collection to happen.");
                GC.Collect();
                GC.WaitForPendingFinalizers();
                Thread.Sleep(1000);
                index++;
            }

            System.Diagnostics.Debugger.Break();
            Console.WriteLine($"Test completed, unload success: {!testAlcWeakRef.IsAlive}");
        }
    }
}
