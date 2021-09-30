using System;
using System.ComponentModel;

namespace common.sismo.enums
{
    public class Util
    {
        public static string GetEnumDescription(Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());

            var attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }
    }


    internal class NameAttribute : DescriptionAttribute
    {
        public NameAttribute(string description)
        {
            DescriptionValue = description;
        }
    }

    internal class GroupAttribute : DescriptionAttribute
    {
        public GroupAttribute(string description)
        {
            DescriptionValue = description;
        }
    }

    internal class CodeAttribute : DescriptionAttribute
    {
        public CodeAttribute(string description)
        {
            DescriptionValue = description;
        }
    }

    internal class DefaultValueAttribute : DescriptionAttribute
    {
        public DefaultValueAttribute(string description)
        {
            DescriptionValue = description;
        }
    }
}
