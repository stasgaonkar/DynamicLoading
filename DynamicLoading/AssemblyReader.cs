using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace DynamicLoading
{
    class AssemblyReader
    {
        /// <summary>
        /// Function called to read the assembly and return the list of publicly
        /// available methods (with their input and output parameters
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void ReadAssembly(string path, out WeakReference alcWeakRef)
        {
            // Create the unloadable DigitalAssemblyLoadContext
            var alc = new DigitalAssemblyLoadContext(path);

            // Load System.ComponentModel.TypeDescriptor to the ALC beforehand,
            // as otherwise the internal caches in it will block unloading the ALC: https://github.com/dotnet/coreclr/issues/26271
            // The netstandard assembly shim also needs to be loaded explicitly for the TypeDescriptor to be loaded.
            var typeDescriptorAssemblyPath = typeof(TypeDescriptor).Assembly.Location;
            alc.LoadFromAssemblyPath(typeDescriptorAssemblyPath);

            var netstandardShimAssemblyPath = Path.Combine(Path.GetDirectoryName(typeDescriptorAssemblyPath), "netstandard.dll");
            alc.LoadFromAssemblyPath(netstandardShimAssemblyPath);

            // Create a weak reference to the AssemblyLoadContext that will allow us to detect
            // when the unload completes.
            alcWeakRef = new WeakReference(alc, trackResurrection: true);

            // Load the plugin assembly into the HostAssemblyLoadContext. 
            // NOTE: the assemblyPath must be an absolute path.
            Assembly assembly = alc.LoadFromAssemblyPath(path);

            // Get a class and its method via reflection.
            Type classType = assembly.GetType("DynamicLibrary.ServiceClient");
            MethodInfo method = classType.GetMethod("Connect");

            var classObj = Activator.CreateInstance(classType);
            var result = method.Invoke(classObj, new object[] { 100 });

            Console.WriteLine("Value of the row count is: " + result);

            alc.Unload();
        }

        private Assembly GetAssemblyFromStream(DigitalAssemblyLoadContext alc, string path)
        {
            Assembly assembly = null;
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                assembly = alc.LoadFromStream(fs);
            }

            return assembly;
        }
    }
}
