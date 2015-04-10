using System;
using System.Text;

namespace DemoSDCard2
{
    internal static class ButtonEventConverter
    {
        private const char Separator = ',';
        private const string TrueString = "1";
        private const string FalseString = "0";
        
        public static string ToString(ButtonEvent value)
        {
            var sb = new StringBuilder();

            var str = ToString(value.EventTime);
            sb.Append(str);
            sb.Append(Separator);

            str = ToString(value.State);
            sb.Append(str);

            string result = sb.ToString();
            return result;
        }

        private static string ToString(DateTime value)
        {
            const string dateTimeFormat = "yyyy-MM-dd hh:mm:ss";
            string result = value.ToString(dateTimeFormat);
            return result;
        }

        private static string ToString(Boolean value)
        {
            string result = value ? TrueString : FalseString;
            return result;
        }

        public static ButtonEvent ToEntity(string value)
        {
            string[] tokens = value.Split(Separator);

            DateTime dt = ToDateTime(tokens[0]);
            bool state = ToState(tokens[1]);

            var result = new ButtonEvent(dt, state);
            return result;
        }

        private static bool ToState(string value)
        {
            bool result = value.Equals(TrueString);
            return result;
        }

        private static DateTime ToDateTime(string value)
        {
            string[] tokens = value.Split(' ');
            
            string[] dateParts = tokens[0].Split('-');
            int y = Int32.Parse(dateParts[0]);
            int mo = Int32.Parse(dateParts[1]);
            int d = Int32.Parse(dateParts[2]);

            string[] timeParts = tokens[1].Split(':');
            int h = Int32.Parse(timeParts[0]);
            int mi = Int32.Parse(timeParts[1]);
            int s = Int32.Parse(timeParts[2]);

            var result = new DateTime(y, mo, d, h, mi, s);
            return result;
        }
    }
}