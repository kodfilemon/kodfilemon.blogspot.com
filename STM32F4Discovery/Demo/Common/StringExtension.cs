namespace Common
{
    public static class StringExtension
    {
        public static bool StartsWith(this string @this, string value, bool ignoreCase = false)
        {
            if (ReferenceEquals(@this, value))
                return true;

            if(value.Length == 0)
                return true;

            if (ignoreCase)
                return @this.ToLower().IndexOf(value.ToLower()) == 0;

            return @this.IndexOf(value) == 0;
        }

        public static bool EndsWith(this string @this, string value, bool ignoreCase = false)
        {
            if (ReferenceEquals(@this, value))
                return true;

            if (value.Length == 0)
                return true;

            int expectedIndex = @this.Length - value.Length;
            if(expectedIndex < 0)
                return false;

            if (ignoreCase)
                return @this.ToLower().IndexOf(value.ToLower()) == expectedIndex;

            return @this.IndexOf(value) == expectedIndex;
        }
    }
}