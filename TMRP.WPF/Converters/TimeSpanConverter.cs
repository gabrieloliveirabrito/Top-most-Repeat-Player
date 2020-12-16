using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using SConvert = System.Convert;

namespace TMRP.WPF.Converters
{
    [ValueConversion(typeof(long), typeof(string))]
    public class TimeSpanConverter : IValueConverter
    {
        public bool WithMilliseconds { get; set; } = true;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var span = TimeSpan.FromMilliseconds(SConvert.ToInt64(value));

            if (WithMilliseconds)
                return $"{span.Hours:D2}:{span.Minutes:D2}:{span.Seconds:D2}:{span.Milliseconds:D3}";
            else
                return $"{span.Hours:D2}:{span.Minutes:D2}:{span.Seconds:D2}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var span = value.ToString().Split(':');

            if (WithMilliseconds)
                return new TimeSpan(0, SConvert.ToInt32(span[0]), SConvert.ToInt32(span[1]), SConvert.ToInt32(span[2]), SConvert.ToInt32(span[3]));
            else
                return new TimeSpan(0, SConvert.ToInt32(span[0]), SConvert.ToInt32(span[1]), SConvert.ToInt32(span[2]));
        }
    }
}