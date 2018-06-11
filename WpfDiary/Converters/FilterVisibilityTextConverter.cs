using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using WpfDiary.ViewModels;

namespace WpfDiary.Converters
{
    public class FilterVisibilityTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return (bool)value ? ViewModelConstants.FILTERS_VISIBLE_TEXT : ViewModelConstants.FILTERS_HIDDEN_TEXT;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return ((string)value).Equals(ViewModelConstants.FILTERS_VISIBLE_TEXT) ? true : false;
        }
    }
}
