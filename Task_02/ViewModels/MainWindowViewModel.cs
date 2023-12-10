using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Threading.Channels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Task_02.Persistence;
using Task_02.Persistence.Entities;

namespace Task_02.ViewModels;

public partial class MainWindowViewModel(AppDbContext context) : ObservableObject
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
}