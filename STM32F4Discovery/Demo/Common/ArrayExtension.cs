using System;
using System.Text;

namespace Common
{
    public static class ArrayExtension
    {
        public static string Join(this Array @this, string separator)
        {
            if(@this.Length == 0)
                return String.Empty;

            if(@this.Length == 1)
                return @this.GetValue(0).ToString();

            var result = new StringBuilder(@this.GetValue(0).ToString());
            for (int i = 1; i < @this.Length; i++)
            {
                result.Append(separator);
                result.Append(@this.GetValue(i).ToString());
            }

            return result.ToString();
        }
    }
}