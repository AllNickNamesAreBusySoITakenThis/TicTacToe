using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Models
{
    public partial class Plate: ObservableObject
    {
        #region Constructor

        public Plate()
        {
            
        }

        public Plate(int _index, int row, int column)
        {
            index = _index;
            rowIndex = row;
            columnIndex = column;
        }

        public Plate(Plate other)
        {
            index = other.index;
            rowIndex = other.rowIndex;
            columnIndex = other.columnIndex;
            plateState = other.plateState;
            timer = other.timer;
            isWinning = other.isWinning;
            nearlyDisappear=other.nearlyDisappear;
        }

        #endregion

        #region Properties

        [ObservableProperty]
        int index = 0;

        [ObservableProperty]
        int rowIndex = 1;

        [ObservableProperty]
        int columnIndex = 1;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(PlateColor))]
        PlateState plateState = PlateState.None;

        [ObservableProperty]
        int timer = 0;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(PlateColor))]
        bool isWinning = false;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(PlateColor))]
        bool nearlyDisappear = false;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(PlateColor))]
        PlateState currentTurn = PlateState.Cross;
        public Color PlateColor
        {
            get
            {     
                if (IsWinning)
                {
                    return (Color)Application.Current.Resources["WinColor"];
                }

                if (NearlyDisappear && CurrentTurn == PlateState)
                {
                    return (Color)Application.Current.Resources["DisappearColor"];
                }

                switch (Application.Current.PlatformAppTheme)
                {
                    case AppTheme.Light:
                        return (Color)Application.Current.Resources["ControlDark"];

                    case AppTheme.Dark:
                    default:
                        return (Color)Application.Current.Resources["ControlDark"];

                }
            }
        }

        #endregion

        #region Metholds

        public override string ToString()
        {
            return $"Plate {Index}. Row - {RowIndex}. Column - {ColumnIndex}";
        }

        #endregion
    }
}
