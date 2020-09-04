using System.Diagnostics;
using System.IO;

namespace BinaryDepths.Extensions
{
    public static class ProcessModuleExtensions
    {
        /// <summary>
        /// Gets the file name of this process module without the path
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        public static string GetFileNameNoPath(this ProcessModule module)
        {
            return Path.GetFileName(module.FileName);
        }
    }
}