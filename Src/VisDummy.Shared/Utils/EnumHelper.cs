using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace VisDummy.Shared.Utils
{
    public static class EnumHelper
    {
        public static string GetDescription(Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            DescriptionAttribute attribute = field.GetCustomAttribute<DescriptionAttribute>();
            return attribute == null ? value.ToString() : attribute.Description;
        }

        public static IEnumerable<ValueDescription> GetAllValuesAndDescriptions(Type enumType)
        {
            if (!enumType.IsEnum)
                throw new ArgumentException("Type must be an enum");

            return Enum.GetValues(enumType).Cast<Enum>()
                .Select(e => new ValueDescription()
                {
                    Value = e,
                    Description = GetDescription(e)
                }).ToList();
        }
    }

    public class ValueDescription
    {
        public object Value { get; set; }
        public string Description { get; set; }
    }
}
