using System;
using System.Globalization;
using System.Windows.Data;

namespace Krang.Client.Presentation
{
    public sealed class LongToSpeedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var byteCount = (int)value;
            string[] suf = { "B/s", "KB/s", "MB/s", "GB/s", "TB/s", "PB/s", "EB/s" };

            if (byteCount == 0)
                return "0 " + suf[0];

            var bytes = Math.Abs(byteCount);
            var place = System.Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            var num = Math.Round(bytes / Math.Pow(1024, place), 1);

            return string.Format("{0} {1}", Math.Sign(byteCount) * num, suf[place]);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
