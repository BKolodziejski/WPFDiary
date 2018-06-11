using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using WpfDiary.Models;

namespace WpfDiary.Converters
{
    public class TagStringToSetConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return Utils.TagsSetToString((HashSet<string>)value);
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return Utils.TagsStringToSet(value as string);
        }
    }
}
