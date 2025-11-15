using System;
using System.Reflection;

namespace Physiqia
{
    /// <summary>
    /// Lang Enum
    /// </summary>
    public static class LangEnum
    {
        [EnumString("en")]
        public static readonly byte English = 1;

        [EnumString("ru")]
        public static readonly byte Russian = 2;

        [EnumString("jp")]
        public static readonly byte Japanese = 3;

        [EnumString("cn")]
        public static readonly byte Chinese = 4;

        [EnumString("fr")]
        public static readonly byte French = 5;

        /// <summary>
        ///
        /// </summary>
        [AttributeUsage(AttributeTargets.Field)]
        public class EnumStringAttribute : Attribute
        {
            public string Value { get; private set; }

            public EnumStringAttribute(string value)
            {
                Value = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string GetEnumString(int enumValue)
        {
            Type enumType = typeof(LangEnum);
            foreach (var field in enumType.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                if (field.GetValue(null) is byte value && value == enumValue)
                {
                    var attribute = (EnumStringAttribute)
                        Attribute.GetCustomAttribute(field, typeof(EnumStringAttribute));
                    return attribute?.Value ?? enumValue.ToString();
                }
            }
            return enumValue.ToString();
        }
    }
}
