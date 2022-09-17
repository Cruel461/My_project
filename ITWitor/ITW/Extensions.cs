using ITWitor.Models;

using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ITW
{
  public static class Extensions
  {
    public static Array ToEnumerable(this Enum @enum)
    {
      var array = Enum.GetValues(@enum.GetType());
      return array;
    }


    public static TAttribute GetAttribute<TAttribute>(this Enum enumValue)
        where TAttribute : Attribute
    {
      return enumValue.GetType()
                      .GetMember(enumValue.ToString())
                      .First()
                      .GetCustomAttribute<TAttribute>();
    }

    public static string? DisplayName(this Enum @enum)
    {
      return @enum.GetAttribute<DisplayAttribute>()?.Name?.ToString();
    }

    public static string GetDisplayName(this BaseModel baseModel)
    {
      throw new NotImplementedException();

    }
  }
}