using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Labb2_Databaser.DbModels;
using Labb2_Databaser.Managers;
using Microsoft.EntityFrameworkCore;

namespace Labb2_Databaser.ViewModels;

public class AddBookToLagerSaldoViewModel : ObservableObject
{
    #region Fields And Properties
    private readonly NavigationManager _navigationManager;
    private ObservableCollection<Böcker> _showBooksInLagerSaldo;

    public ObservableCollection<Böcker> ShowAllBooks
    {
        get
        {
            return _showBooksInLagerSaldo;
        }
        set
        {
            SetProperty(ref _showBooksInLagerSaldo, value);
        }
    }
    #endregion

    #region IRelayCommands
    public IRelayCommand NavigateToBokSamling { get; }
    #endregion
    public AddBookToLagerSaldoViewModel(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;

        NavigateToBokSamling = new RelayCommand(() =>
            _navigationManager.CurrentViewModel = new BokSamlingViewModel(navigationManager));
        ShowAllBooksInLagerSaldo();
    }

    private void ShowAllBooksInLagerSaldo()
    {
        using (var context = new BokhandelDbContext())
        {
            ShowAllBooks = new ObservableCollection<Böcker>();

            foreach (var allBooks in context.Böckers.Include(b => b.Författar))
            {
                ShowAllBooks.Add(allBooks);
            }

        }
    }
}