using System;
using System.Reflection;

namespace SyndicationCore {
    public static class VersionHelper {
        public static string ForType<T>() {
            var typeInfo = System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(T));
            return typeInfo.Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
        }
    }
}