using System.Windows;
using Task_02.ViewModels;

namespace Task_02;

public partial class MainWindow
{
    private MainWindowViewModel ViewModel => (MainWindowViewModel)DataContext;
    
    public MainWindow(MainWindowViewModel mainWindowViewModel)
    {
        InitializeComponent();
        DataContext = mainWindowViewModel;
    }

    private async void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        await ViewModel.RefreshBanksCommand.ExecuteAsync(null);
    }
}