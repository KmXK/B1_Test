using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using Task_02.Exceptions;
using Task_02.Helpers;
using Task_02.Persistence;
using Task_02.Persistence.Entities;
using Task_02.Services.Interfaces;

namespace Task_02.ViewModels;

public partial class MainWindowViewModel(
    AppDbContext context,
    ITurnoverExcelParser turnoverExcelParser) : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<Bank> _banks = new();

    [ObservableProperty]
    private Bank? _selectedBank;

    [RelayCommand]
    private async Task RefreshBanks()
    {
        Banks.Clear();
        SelectedBank = null;
        
        var banks = await context.Set<Bank>().ToListAsync();
        
        banks.ForEach(bank => Banks.Add(bank));

        SelectedBank = Banks.FirstOrDefault();
    }

    [RelayCommand]
    private async Task ImportExcel()
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*",
            Multiselect = false,
            Title = "Select Excel file to import",
            DefaultExt = "xlsx"
        };

        if (openFileDialog.ShowDialog() != true)
        {
            return;
        }

        try
        {
            await turnoverExcelParser.ParseAsync(openFileDialog.FileName);
        }
        catch (ExcelParseException e)
        {
            MessageBoxEx.ShowError(e.GetHumanReadableMessage());
        }
        catch(Exception e)
        {
            MessageBoxEx.ShowError("Unknown error occured while parsing excel file.");
            Console.WriteLine(e);
        }
    }
}