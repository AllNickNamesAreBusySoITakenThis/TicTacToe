using TicTacToe.ViewModels;

namespace TicTacToe
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageViewModel vm)
        {
            InitializeComponent();
            vm.ScreenWidth = Microsoft.Maui.Devices.DeviceDisplay.MainDisplayInfo.Width;
            vm.ScreenHeight = Microsoft.Maui.Devices.DeviceDisplay.MainDisplayInfo.Height;
            BindingContext = vm;
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            var vm = BindingContext as MainPageViewModel;
            if (vm != null)
            {
                vm.ScreenHeight = mainGrid.Height-20;
                vm.ScreenWidth = width;
            }
        }
    }

}
