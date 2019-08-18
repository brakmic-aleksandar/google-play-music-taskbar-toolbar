using System;
using System.Globalization;
using System.Windows.Data;

namespace WindowsTaskbarAudio.ValueConverter
{
    class BoolToPlayButtonImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool) value)
            {
                return "../Resources/Icons/Pause-32.png";
            }
            return "../Resources/Icons/Play-32.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
