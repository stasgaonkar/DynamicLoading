using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace DynamicLoading
{
    class DigitalAssemblyLoadContext : AssemblyLoadContext
    {
        private AssemblyDependencyResolver _resolver;

        public DigitalAssemblyLoadContext(string mainAssemblyToLoadPath) : base(isCollectible: true)
        {
            _resolver = new AssemblyDependencyResolver(mainAssemblyToLoadPath);
       }

        protected override Assembly Load(AssemblyName name)
        {

            string assemblyPath = _resolver.ResolveAssemblyToPath(name);
            if (assemblyPath != null)
            {
                return LoadFromAssemblyPath(assemblyPath);
                // return GetAssemblyFromStream(assemblyPath);
            }

            return null;
        }

        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            string libraryPath = _resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
            if (libraryPath != null)
            {
                return LoadUnmanagedDllFromPath(libraryPath);
            }

            return IntPtr.Zero;
        }

        private Assembly GetAssemblyFromStream(string path)
        {
            Assembly assembly = null;
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                assembly = LoadFromStream(fs);
            }

            return assembly;
        }
    }
}
