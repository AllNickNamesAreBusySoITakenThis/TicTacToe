using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Models;

namespace TicTacToe.ViewModels
{
    public partial class MainPageViewModel : ObservableObject
    {
        #region Constructor

        public MainPageViewModel()
        {
            Plates = new ObservableCollection<Plate>();
            var index = 0;
            for (int i = 1; i <= 3; i++)
            {
                for (int j = 1; j <= 3; j++)
                {
                    Plates.Add(new Plate(index, i, j));
                    index++;
                }
            }
        }

        #endregion

        #region Properties

        public LocalizationResourceManager LocalizationResourceManager => LocalizationResourceManager.Instance;

        public ObservableCollection<Plate> Plates { get; set; } = new ObservableCollection<Plate>();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(InfoDataString))]
        bool victory = false;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(InfoDataString))]
        bool crossTurn = true;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SquartSize))]
        double screenWidth = 0;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SquartSize))]
        double screenHeight = 0;

        public double SquartSize
        {
            get
            {
                if(ScreenHeight <= 0 || ScreenWidth <= 0)
                {
                    return 0;
                }

                return Math.Min(ScreenHeight - 50, ScreenWidth) / 3;
            }
        }

        public string InfoDataString
        {
            get
            {
                if (Victory)
                {
                    return CrossTurn ? LocalizationResourceManager["Circle_win"].ToString() : LocalizationResourceManager["Cross_win"].ToString();
                }

                return CrossTurn ? LocalizationResourceManager["Cross_turn"].ToString() : LocalizationResourceManager["Circle_turn"].ToString();
            }
        }

        #endregion

        #region Commands

        [RelayCommand]
        void Init()
        {
            Victory = false;
            CrossTurn = true;
        }

        [RelayCommand]
        void PlateClick(int index)
        {
            if (!Victory)
            {
                var nearlyDisappearIndexes = new List<int>();
                var winningIndexes = new List<int>();
                var plateState = PlateState.None;
                foreach (var item in Plates)
                {
                    item.CurrentTurn = item.CurrentTurn == PlateState.Cross ? PlateState.Cicle : PlateState.Cross;
                    if (item.Index == index && item.PlateState == PlateState.None)
                    {
                        item.PlateState = CrossTurn ? PlateState.Cross : PlateState.Cicle;
                        plateState = item.PlateState;
                        item.Timer = 1;
                        ProcessOtherPlates(index, item.PlateState, out nearlyDisappearIndexes, out winningIndexes);
                        CrossTurn = !CrossTurn;
                    }
                }

                for (int i = 0; i < Plates.Count; i++)
                {
                    if (nearlyDisappearIndexes.Contains(i))
                    {
                        Plates[i].NearlyDisappear = true;
                    }
                    else if(plateState == PlateState.None || plateState == Plates[i].PlateState)
                    {
                        Plates[i].NearlyDisappear = false;
                    }

                    if (winningIndexes.Contains(i))
                    {
                        Plates[i].IsWinning = true;
                    }
                    else
                    {
                        Plates[i].IsWinning = false;
                    }
                }
                var tPlates = new List<Plate>(Plates);
                Plates.Clear();
                foreach (var item in tPlates)
                {
                    Plates.Add(item);
                }
            }
        }

        [RelayCommand]
        void Clear()
        {
            Victory = false;
            CrossTurn = true;
            foreach (var item in Plates)
            {
                item.PlateState = PlateState.None;
                item.Timer = 0;
                item.IsWinning = false;
                item.NearlyDisappear = false;
                item.CurrentTurn = PlateState.Cross;
            }
        }

        #endregion

        #region Methods

        void ProcessOtherPlates(int clickedPlateIndex, PlateState setState, out List<int> nearlyDisappearIndexes, out List<int> winningIndexes)
        {
            winningIndexes = new List<int>();
            nearlyDisappearIndexes = new List<int>();

            #region Check fields for disappear

            foreach (var item in Plates)
            {
                if (item.PlateState == setState && item.Index != clickedPlateIndex)
                {
                    if (item.Timer == 3) // set plate to none
                    {
                        item.PlateState = PlateState.None;
                        item.Timer = 0;
                        item.NearlyDisappear = false;
                    }
                    else
                    {
                        item.Timer++;
                        if (item.Timer == 3)
                        {
                            nearlyDisappearIndexes.Add(item.Index);
                        }
                    }
                }
            }

            #endregion

            #region Check winning conditions

            var crossItemsIndexes = new List<int>();
            var circleItemsIndexes = new List<int>();

            foreach (var item in Plates)
            {
                if (item.PlateState == PlateState.Cross)
                {
                    crossItemsIndexes.Add(item.Index);
                }
                else if (item.PlateState == PlateState.Cicle)
                {
                    circleItemsIndexes.Add(item.Index);
                }
            }

            if (CheckWinCondition(crossItemsIndexes))
            {
                winningIndexes = crossItemsIndexes;
                Victory = true;
            }

            if (CheckWinCondition(circleItemsIndexes))
            {
                winningIndexes = circleItemsIndexes;
                Victory = true;
            }

            #endregion
        }

        bool CheckWinCondition(List<int> itemsIndexes)
        {
            if (itemsIndexes.Count != 3)
            {
                return false;
            }
            var rowIndexes = itemsIndexes.Select(x => Plates.First(p => p.Index == x).RowIndex).ToList();
            var colIndexes = itemsIndexes.Select(x => Plates.First(p => p.Index == x).ColumnIndex).ToList();

            //same row all same column for all three
            if (rowIndexes.Distinct().Count() == 1 || colIndexes.Distinct().Count() == 1)
            {
                return true;
            }

            var orderedPlates = itemsIndexes.Select(x => Plates.First(p => p.Index == x)).OrderBy(p => p.RowIndex).ToList();
            var maxDeltaBetweenColumns = 0;
            for (int i = 0; i < orderedPlates.Count; i++)
            {
                if (i != orderedPlates.Count - 1 && Math.Abs(orderedPlates[i].ColumnIndex - orderedPlates[i+1].ColumnIndex)>maxDeltaBetweenColumns)
                {
                    maxDeltaBetweenColumns = Math.Abs(orderedPlates[i].ColumnIndex - orderedPlates[i + 1].ColumnIndex);
                }
            }

            //Different rows and columns for all three
            if (rowIndexes.Distinct().Count() == rowIndexes.Count && colIndexes.Distinct().Count() == colIndexes.Count && maxDeltaBetweenColumns ==1)
            {
                return true;
            }

            return false;
        }

        #endregion
    }
}
