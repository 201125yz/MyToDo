using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MyToDo.Common.converter
{
    internal class IntToBoolConvertercs : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value != null && int.TryParse(value.ToString(), out int result))//如果你导入的是int类型或结果
            {
                if (result == 0)
                    return false;
            }
            return true;
        }

        //转换双向绑定，回传
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //如果回来的是bool
            if (value != null && bool.TryParse(value.ToString(), out bool result))
            {
                if (result)
                    return 1;
            }
            return 0;
        }
    }
}
