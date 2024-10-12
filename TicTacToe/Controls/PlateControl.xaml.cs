using TicTacToe.Models;

namespace TicTacToe.Controls;

public partial class PlateControl : ContentView
{
    public PlateControl()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty CurrentPlateProperty = BindableProperty.Create(nameof(CurrentPlate), typeof(Plate), typeof(PlateControl));

    public Plate CurrentPlate
    {
        get { return (Plate)GetValue(CurrentPlateProperty); }
        set { 
            SetValue(CurrentPlateProperty, value);
            if (value != null)
            {
                value.PropertyChanged += Value_PropertyChanged;
            }
        }
    }

    private void Value_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        var plate = sender;
    }

    public string PlateColor
    {
        get
        {
            if (CurrentPlate == null)
            {
                return "#FFFFFF";
            }

            if (CurrentPlate.IsWinning)
            {
                return "#00ff00";
            }

            switch (CurrentPlate.PlateState)
            {
                case PlateState.Cicle:
                    return CurrentPlate.NearlyDisappear ? "#20b2aa" : "#30d5c8";
                case PlateState.Cross:
                    return CurrentPlate.NearlyDisappear ? "#ff4d00" : "#ff0000";
                default:
                    return "#8b00ff";
            }
        }
    }
}