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
    ITurnoverExcelImporter turnoverExcelImporter) : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<TurnoverStatement> _turnoverStatements = new();

    [ObservableProperty]
    private TurnoverStatement? _selectedStatement;

    [RelayCommand]
    private async Task RefreshBanksAsync()
    {
        TurnoverStatements.Clear();
        SelectedStatement = null;
        
        var turnoverStatements = await context
            .Set<TurnoverStatement>()
            .Include(x => x.Bank)
            .ToListAsync();
        
        turnoverStatements.ForEach(bank => TurnoverStatements.Add(bank));

        SelectedStatement = TurnoverStatements.FirstOrDefault();
    }

    [RelayCommand]
    private async Task ImportExcelAsync()
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
            await turnoverExcelImporter.ImportAsync(openFileDialog.FileName);
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