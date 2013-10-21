using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace _02350Project.Other
{
    class BooleanToStrokeArrayDashConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DoubleCollection noDash = new DoubleCollection() { 1, 0 };
            DoubleCollection Dash = new DoubleCollection() { 5, 2 };

            return (bool)value ? Dash : noDash;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
