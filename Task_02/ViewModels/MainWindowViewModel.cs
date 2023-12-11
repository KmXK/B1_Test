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
    private ObservableCollection<AccountTurnoverStatementViewModel> _accountTurnoverStatements = new();

    [ObservableProperty]
    private TurnoverStatement? _selectedStatement;

    [ObservableProperty]
    private bool _isLoading = false;

    partial void OnSelectedStatementChanged(TurnoverStatement? oldValue, TurnoverStatement? newValue)
    {
        if (newValue != null)
        {
            IsLoading = true;
            
            var viewModels = GetViewModels(newValue.AccountTurnoverStatements);

            AccountTurnoverStatements.Clear();

            AccountTurnoverStatements.InsertRange(viewModels);

            IsLoading = false;
        }
    }

    private static IEnumerable<AccountTurnoverStatementViewModel> GetViewModels(
        IEnumerable<AccountTurnoverStatement> accountTurnoverStatements)
    {
        var classNumber = 1;
        AccountTurnoverStatementViewModel? classTurnoverStatement = null;

        var groupNumber = 0;
        AccountTurnoverStatementViewModel? groupTurnoverStatement = null;

        foreach (var accountTurnoverStatement in accountTurnoverStatements)
        {
            var className = $"КЛАСС {accountTurnoverStatement.Account.ClassNumber} " +
                            $"{accountTurnoverStatement.Account.BankClass.Name}";
            
            if (groupTurnoverStatement == null || groupNumber != accountTurnoverStatement.AccountNumber / 100)
            {
                if (groupTurnoverStatement != null)
                {
                    yield return groupTurnoverStatement;
                }

                groupNumber = accountTurnoverStatement.AccountNumber / 100;
                groupTurnoverStatement = new AccountTurnoverStatementViewModel(groupNumber.ToString())
                {
                    ClassName = className
                };
            }
            
            if (classTurnoverStatement == null || classNumber != accountTurnoverStatement.Account.ClassNumber)
            {
                if (classTurnoverStatement != null)
                {
                    yield return classTurnoverStatement;
                }

                classNumber = accountTurnoverStatement.Account.ClassNumber;
                classTurnoverStatement = new AccountTurnoverStatementViewModel("ПО КЛАССУ")
                {
                    ClassName = className
                };
            }

            var viewModel = new AccountTurnoverStatementViewModel(accountTurnoverStatement)
            {
                ClassName = className
            };
            
            groupTurnoverStatement.Add(viewModel);
            classTurnoverStatement.Add(viewModel);

            yield return viewModel;
        }

        if (groupTurnoverStatement != null)
        {
            yield return groupTurnoverStatement;
        }

        if (classTurnoverStatement != null)
        {
            yield return classTurnoverStatement;
        }
    }

    [RelayCommand]
    private async Task RefreshBanksAsync()
    {
        IsLoading = true;
        
        TurnoverStatements.Clear();
        SelectedStatement = null;

        var turnoverStatements = await context
            .Set<TurnoverStatement>()
            .Include(x => x.Bank)
            .Include(x => x.AccountTurnoverStatements).ThenInclude(x => x.Account.BankClass)
            .AsNoTracking()
            .ToListAsync();

        TurnoverStatements.InsertRange(turnoverStatements);

        SelectedStatement = TurnoverStatements.FirstOrDefault();
        
        IsLoading = false;
    }

    [RelayCommand]
    private async Task ImportExcelAsync()
    {
        IsLoading = true;
        
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

            await RefreshBanksAsync();
        }
        catch (ExcelParseException e)
        {
            MessageBoxEx.ShowError(e.GetHumanReadableMessage());
        }
        catch(Exception e)
        {
            MessageBoxEx.ShowError("Unknown error occured while parsing excel file.");
        }
        
        IsLoading = false;
    }
}