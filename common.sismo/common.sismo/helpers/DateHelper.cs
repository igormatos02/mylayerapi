using System;
using System.Globalization;


namespace common.sismo.helpers
{
    public static class DateHelper
    {
        //public static dynamic GetDBValue(dynamic var, DateType type)
        //{
        //    if (var == null || var == "")
        //    {
        //        return DBNull.Value;
        //    }

        //    if (type == DateType.InvertedDate)
        //        return IntertedStringToDate(var);
        //    else if (type == DateType.NormalDate)
        //        return StringToDate(var);
        //    else if (type == DateType.InvertedDateTime)
        //        return IntertedStringToDateTime(var);
        //    else if (type == DateType.NormalDateTime)
        //        return StringToDateTime(var);
        //    else return var;

        //}

        public static DateTime StringToDate(string date)
        {
            var dtfi = new DateTimeFormatInfo
            {
                ShortDatePattern = "dd/MM/yyyy",
                DateSeparator = "/"
            };
            return Convert.ToDateTime(date, dtfi);
        }

        public static DateTime IntertedStringToDate(string date)
        {
            var dtfi = new DateTimeFormatInfo
            {
                ShortDatePattern = "yyyy/MM/dd",
                DateSeparator = "/"
            };
            return Convert.ToDateTime(date, dtfi);
        }

        public static DateTime StringToDateTime(string date)
        {
            var dtfi = new DateTimeFormatInfo
            {
                ShortDatePattern = "dd/MM/yyyy HH:mm:ss",
                DateSeparator = "/"
            };
            return Convert.ToDateTime(date, dtfi);
        }

        public static DateTime IntertedStringToDateTime(string date)
        {
            var dtfi = new DateTimeFormatInfo
            {
                ShortDatePattern = "yyyy/MM/dd HH:mm:ss",
                DateSeparator = "/"
            };
            return Convert.ToDateTime(date, dtfi);
        }

        public static string StringToDbString(string date)
        {
            var dtfi = new DateTimeFormatInfo
            {
                ShortDatePattern = "dd/MM/yyyy",
                DateSeparator = "/"
            };
            return Convert.ToDateTime(date, dtfi).ToString("MM-dd-yyyy");
        }

        public static string DbStringToString(string date)
        {
            var dtfi = new DateTimeFormatInfo
            {
                ShortDatePattern = "MM-dd-yyyy",
                DateSeparator = "-"
            };
            return Convert.ToDateTime(date, dtfi).ToString("dd/MM/yyyy");
        }

        public static DateTime StringToDate(string date, string fullDateTimePattern)
        {
            var dtfi = new DateTimeFormatInfo
            {
                FullDateTimePattern = fullDateTimePattern
            };
            return Convert.ToDateTime(date, dtfi);
        }

        public static string DateToString(DateTime date)
        {
            return date.ToString("dd/MM/yyyy");
        }

        public static string DateTimeToString(DateTime date)
        {
            return date.ToString("dd/MM/yyyy HH:mm");
        }

        public static string DateToDbString(DateTime date)
        {
            return date.ToString("MM-dd-yyyy");
        }
        public static dynamic GetDBValue(dynamic var, String type)
        {
            if (var == null || ((type == "InvertedDate" || type == "NormalDate" || type == "InvertedDateTime" || type == "NormalDateTime") && var == ""))
            {
                // if (type == "InvertedDate" || type == "NormalDate" || type == "InvertedDateTime" || type == "NormalDateTime")
                //   return DateTime.Now.Date;
                //else
                return DBNull.Value;
            }

            else
            {
                if (type == "InvertedDate")
                    return DateHelper.IntertedStringToDate(var);
                else if (type == "NormalDate")
                    return DateHelper.StringToDate(var);
                if (type == "InvertedDateTime")
                    return DateHelper.IntertedStringToDateTime(var);
                else if (type == "NormalDateTime")
                    return DateHelper.StringToDateTime(var);
                else return var;
            }
        }
        public static string GetShortMonthName(Int32 month)
        {
            String value = "";

            if (month == 1) value = "Jan";
            if (month == 2) value = "Fev";
            if (month == 3) value = "Mar";
            if (month == 4) value = "Abr";
            if (month == 5) value = "Mai";
            if (month == 6) value = "Jun";
            if (month == 7) value = "Jul";
            if (month == 8) value = "Ago";
            if (month == 9) value = "Set";
            if (month == 10) value = "Out";
            if (month == 11) value = "Nov";
            if (month == 12) value = "Dez";

            return value;
        }
    }
}
