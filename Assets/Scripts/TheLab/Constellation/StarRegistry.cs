using System;
using System.Globalization;
using System.Reflection;

namespace Physiqia.TheLab.Constellation
{
    public static class StarRegistry
    {
        /// <summary>
        ///
        /// </summary>
        private const string Namespace = "Physiqia.TheLab.Constellation";

        /// <summary>
        ///
        /// </summary>
        /// <param name="hrCode"></param>
        /// <returns></returns>
        public static IStarHR Get(string hrCode)
        {
            var className = hrCode.Replace(" ", "").ToUpperInvariant();
            var fullTypeName = $"{Namespace}.{className}";
            var type = Type.GetType(fullTypeName);

            if (type == null)
                return null;

            return Activator.CreateInstance(type) as IStarHR;
        }
    }
}
