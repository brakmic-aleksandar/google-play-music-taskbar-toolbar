using System;
using System.Globalization;
using System.Windows.Data;

namespace WindowsTaskbarAudio.ValueConverter
{
    class VolumeValueToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ushort v = (ushort) value;
            if (v == 0)
            {
                return "../Resources/Icons/Volume-mute-32.png";
            }
            if (v > 0 && v <= 25)
            {
                return "../Resources/Icons/Volume-low-32.png";
            }
            if (v > 25 && v <= 75)
            {
                return "../Resources/Icons/Volume-medium-32.png";
            }
            if (v > 75 && v <= 100)
            {
                return "../Resources/Icons/Volume-high-32.png";
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
