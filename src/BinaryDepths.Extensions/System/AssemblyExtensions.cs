using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;

namespace System
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
        /// typeof(IEntityConfiguration).Assembly.GetLoadableTypes(type => typeof(IEntityConfiguration).IsAssignableFrom(type))
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
    }
}