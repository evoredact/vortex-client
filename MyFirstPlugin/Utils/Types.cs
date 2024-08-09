using System;
using System.Linq;
using System.Reflection;

namespace VortexClient.Utils;
public class Types {
    public static Type[] GetTypesInNamespace(Assembly assembly, string nameSpace, bool equality = true) {
        return assembly.GetTypes()
            .Where(t => equality ? String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal) : t.Namespace.StartsWith(nameSpace))
            .ToArray();
    }
}
