﻿using System;
using System.Windows;
using System.Windows.Data;

namespace Spanglish.Util
{

    /*
     * Inverted Bool To Visivility Converter
     * Does the same job as system provided bool to visibility converter, however it inverts the result.
     * Used for binding bool variable to visibility dependency property.
     */
    [ValueConversion(typeof(bool), typeof(bool))]
    public class InvertedBoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(Visibility))
                throw new InvalidOperationException("The target must be a visibility");

            return (bool)value ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}