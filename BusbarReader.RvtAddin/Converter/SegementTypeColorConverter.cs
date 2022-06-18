using BusbarReader.RvtAddin.Reader;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace BusbarReader.RvtAddin.Converter
{
    internal sealed class SegementTypeColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is BusbarSegment segement)
            {
                var color = segement.GetSegmentTypeTypeColor();
                return new SolidColorBrush(color);
            }
            else
            {
                return new SolidColorBrush();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
