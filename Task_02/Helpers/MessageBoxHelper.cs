using System.Windows;

namespace Task_02.Helpers;

public static class MessageBoxEx
{
    public static void ShowError(string errorMessage)
    {
        MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }
}