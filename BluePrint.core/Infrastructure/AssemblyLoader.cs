using Microsoft.Extensions.DependencyModel;
using System.Reflection;
namespace BluePrint.core.Infrastructure
{
    public static class AssemblyLoader
    {
        public static IEnumerable<Assembly> GetReferencingAssemblies(string assemblyName)
        {
            return DependencyContext.Default!.CompileLibraries.Select(x => x.Name)
                .Where(x => x.ToLower().Contains(assemblyName.ToLower()))
                .Select(x => Assembly.Load(new AssemblyName(x)));
        }
    }
}
