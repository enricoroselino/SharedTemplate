using System.Reflection;

namespace Shared.Infrastructure.Helpers;

public static class AssembliesHelper
{
    public static Type[] GetInterfaceTypes<TInterface>(params Assembly[] assemblies)
    {
        if (!typeof(TInterface).IsInterface) throw new ArgumentException("T must be an interface");

        var types = assemblies
            .Where(a => !a.IsDynamic)
            .SelectMany(a => a.GetTypes())
            .Where(x => x is { IsClass: true, IsAbstract: false } && x.IsAssignableTo(typeof(TInterface)))
            .ToArray();

        return types;
    }
}