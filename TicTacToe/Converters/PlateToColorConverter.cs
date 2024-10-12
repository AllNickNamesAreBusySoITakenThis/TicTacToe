using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Models;

namespace TicTacToe.Converters
{
    public class PlateToColorConverter : BindableObject, IValueConverter
    {
        //public static readonly BindableProperty IsWinningProperty = BindableProperty.Create(nameof(IsWinning), typeof(bool), typeof(PlateToColorConverter));

        //public bool IsWinning
        //{
        //    get { return (bool)GetValue(IsWinningProperty); }
        //    set { SetValue(IsWinningProperty, value); }
        //}

        //public static readonly BindableProperty NearlyDisappearProperty = BindableProperty.Create(nameof(NearlyDisappear), typeof(bool), typeof(PlateToColorConverter));

        //public bool NearlyDisappear
        //{
        //    get { return (bool)GetValue(NearlyDisappearProperty); }
        //    set { SetValue(NearlyDisappearProperty, value); }
        //}

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "#FFFFFF";
            }

            var plate = (Plate)value;


            if (plate.IsWinning)
            {
                return "#00ff00";
            }

            switch (plate.PlateState)
            {
                case PlateState.Cicle:
                    return plate.NearlyDisappear ? "#20b2aa" : "#30d5c8";
                case PlateState.Cross:
                    return plate.NearlyDisappear ? "#ff4d00" : "#ff0000";
                default:
                    return "#8b00ff";
            }
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
