using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;

namespace System.Reflection
{
    public static class AssemblyExtensions
    {
        /// <summary>
        /// Get the types within the assembly that optionally match the specified predicate
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        /// <remarks>
        /// This method will eat exceptions caused by unresolved dependencies
        /// </remarks>
        /// <remarks>
        /// From StackOverflow https://stackoverflow.com/questions/7889228/how-to-prevent-reflectiontypeloadexception-when-calling-assembly-gettypes
        /// Asked by: M4N https://stackoverflow.com/users/19635/m4n
        /// Used Answer by: sweetfa https://stackoverflow.com/users/490614/sweetfa
        /// </remarks>
        /// <example>
        /// Get all types that can be assigned to IEntityConfiguration:
        /// <code>typeof(IEntityConfiguration).Assembly.GetLoadableTypes(type => typeof(IEntityConfiguration).IsAssignableFrom(type))</code>
        /// </example>
        public static IList<Type> GetLoadableTypes(this Assembly assembly, Predicate<Type> predicate = null)
        {
            Contract.Requires(assembly != null);

            var loadableTypes = new List<Type>();
            Type[] exceptionTypes = null;
            
            try
            {
                loadableTypes = assembly.GetTypes()
                                        .Where(t => t != null && t.Assembly == assembly && predicate?.Invoke(t) == true)
                                        .ToList();
            }
            catch (ReflectionTypeLoadException e)
            {
                exceptionTypes = e.Types;
            }

            if (!(exceptionTypes?.Length > 0))
                return loadableTypes;

            foreach (var type in exceptionTypes)
            {
                try
                {
                    if (type != null && type.Assembly == assembly && predicate?.Invoke(type) == true)
                        loadableTypes.Add(type);
                }
                catch (BadImageFormatException)
                {
                    // Omnomnomnom
                }
            }

            return loadableTypes;
        }
        
        /// <summary>
        /// Gets the interfaces that "implement" the specified generic interface type from the specified assembly
        /// </summary>
        /// <param name="assembly">The assembly</param>
        /// <param name="genericInterfaceTypeDefinition">The generic type definition for the base interface</param>
        /// <returns></returns>
        /// <example>
        /// Get all interfaces that implement ISomeGenericInterface&lt;&gt; in <see cref="Assembly">someAssembly</see>:
        /// <code>GetInterfacesImplementingInterface(someAssembly, typeof(IMoreSpecificGenericInterface&lt;&gt;))</code>
        /// </example>
        /// <exception cref="ArgumentException">Thrown if the specified type is not an interface of a generic type definition</exception>
        public static IList<Type> GetInterfacesImplementingGenericInterface(this Assembly assembly, Type genericInterfaceTypeDefinition)
        {
            if (!genericInterfaceTypeDefinition.IsInterface || !genericInterfaceTypeDefinition.IsGenericTypeDefinition)
                throw new ArgumentException("The specified type must be an interface and a generic type definition", nameof(genericInterfaceTypeDefinition));
            
            bool DoesTypeImplementGenericInterface(Type i)
            {
                if (!i.IsGenericType)
                    return false;
                
                return i.IsGenericType && i.GetGenericTypeDefinition() == genericInterfaceTypeDefinition;
            }

            bool IsTypeAnInterfaceImplementingGenericInterface(Type t)
            {
                return t.IsInterface && t.GetInterfaces().Any(DoesTypeImplementGenericInterface);
            }
            
            return assembly.GetLoadableTypes(IsTypeAnInterfaceImplementingGenericInterface);
        }

        /// <summary>
        /// Gets the Stream for the embedded resource with the given name
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="resourceName"></param>
        /// <returns>The resource stream</returns>
        /// <exception cref="ArgumentException">Thrown if the resource cannot be found in the assembly</exception>
        public static Stream GetEmbeddedResourceStream(this Assembly assembly, string resourceName)
        {
            Contract.Requires(assembly != null, "The assembly cannot be null");
            Contract.Requires(!string.IsNullOrWhiteSpace(resourceName), $"{nameof(resourceName)} must not be null or whitespace");
            
            if (!assembly.GetManifestResourceNames().Contains(resourceName))
                throw new ArgumentException($"Unable to load embedded resource.{Environment.NewLine}{assembly.FullName}{Environment.NewLine}{resourceName}");

            return assembly.GetManifestResourceStream(resourceName);
        }
    }
}