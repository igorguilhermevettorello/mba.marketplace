using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace MBA.Marketplace.Data.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());

            var attribute = field?.GetCustomAttribute<DescriptionAttribute>();

            return attribute?.Description ?? value.ToString();
        }

        public static string GetDisplayName(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());

            var attribute = field?.GetCustomAttributes(typeof(DisplayAttribute), false)
                                 .Cast<DisplayAttribute>()
                                 .FirstOrDefault();

            return attribute?.Name ?? value.ToString();
        }
    }
}
