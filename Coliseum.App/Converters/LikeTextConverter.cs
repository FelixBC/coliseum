using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace Coliseum.App.Converters;

public class LikeTextConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool isLiked)
        {
            return isLiked ? "Unlike" : "Like";
        }
        return "Like";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
