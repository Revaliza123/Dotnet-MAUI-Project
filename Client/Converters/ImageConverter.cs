using System.Globalization;
using ProjectMaui.Client.Helpers;

namespace ProjectMaui.Client.Converters;

public class ImageConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string imagePath)
        {
            return ImageHelper.Resolve(imagePath);
        }
        return "placeholder.jpg";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}