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

        public string PlateColor
        {
            get
            {     
                if (IsWinning)
                {
                    return "#00ff00";
                }

                switch (PlateState)
                {
                    case PlateState.Cicle:
                        return NearlyDisappear ? "#808080" : "#30d5c8";
                    case PlateState.Cross:
                        return NearlyDisappear ? "#808080" : "#ff0000";
                    default:
                        return "#8b00ff";
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
