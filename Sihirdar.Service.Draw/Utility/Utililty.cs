using System;
using System.Globalization;

namespace Sihirdar.Service.Draw.Utility
{
    public static class Utililty
    {
        public static string ApiKeyGenerator()
        {
            return Guid.NewGuid().ToString("D");
        }

        public static string GenerateCardNumber()
        {
            //Random class ile pin turetilince ayni keyler donuyor.
            var buffer = Guid.NewGuid().ToByteArray();
            var convert = BitConverter.ToInt64(buffer, 0); //burada string'e cevirildiginde hata olusuyor.
            var numbers = convert.ToString("0000-0000-0000-0000").Substring(3, 19);

            return numbers;
        }

        public static bool DateTimeParsing(string strDateTime, out DateTime dateTime)
        {
            var formats = new[] { "dd.MM.yyyy", "dd.MM.yyyy HH:mm:ss", "dd.MM.yyyy HH:mm" };

            if (!string.IsNullOrEmpty(strDateTime))
                return DateTime.TryParseExact(strDateTime, formats, CultureInfo.InstalledUICulture, DateTimeStyles.None,
                    out dateTime);

            dateTime = new DateTime(1970, 1, 1);
            return false;
        }
    }
}
