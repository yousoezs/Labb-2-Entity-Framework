using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Labb2_Databaser.DbModels;
using Labb2_Databaser.Managers;
using Microsoft.EntityFrameworkCore;

namespace Labb2_Databaser.ViewModels;

public class BokSamlingViewModel : ObservableObject
{
    #region Fields And Properties
    private readonly NavigationManager _navigationManager;
    private ObservableCollection<LagerSaldo> _booksFromLagerSaldo;
    private ObservableCollection<Författare> _showAllFörfattareFromDb;

    public ObservableCollection<Författare> ShowAllFörfattareFromDb
    {
        get => _showAllFörfattareFromDb;
        set => SetProperty(ref _showAllFörfattareFromDb, value);
    }

    public ObservableCollection<LagerSaldo> BooksFromLagerSaldo
    {
        get => _booksFromLagerSaldo;
        set => SetProperty(ref _booksFromLagerSaldo, value);
    }

    #endregion

    #region IRelayCommands
    public IRelayCommand NavigateBackToMainMenu { get; }
    public IRelayCommand AddFörfattareToDb { get; }
    public IRelayCommand AddBookToLagerSaldoNavigation { get; }
    #endregion
    public BokSamlingViewModel(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;

        NavigateBackToMainMenu = new RelayCommand(() =>
            _navigationManager.CurrentViewModel = new MainMenuViewModel(navigationManager));

        AddFörfattareToDb = new RelayCommand(() =>
            _navigationManager.CurrentViewModel = new AddFörfattareViewModel(navigationManager));

        AddBookToLagerSaldoNavigation = new RelayCommand(() =>
            _navigationManager.CurrentViewModel = new AddBookToLagerSaldoViewModel(navigationManager));

        ShowBooksFromLagerSaldo();
        ShowFörfattareFromDb();
    }
    #region Methods
    private void ShowBooksFromLagerSaldo()
    {
        using (var context = new BokhandelDbContext())
        {
            BooksFromLagerSaldo = new ObservableCollection<LagerSaldo>();

            foreach (var books in context.LagerSaldos.Include(ls => ls.IsbnNavigation))
            {
                BooksFromLagerSaldo.Add(books);
            }
        }
    }

    private void ShowFörfattareFromDb()
    {
        using (var context = new BokhandelDbContext())
        {
            ShowAllFörfattareFromDb = new ObservableCollection<Författare>();
            foreach (var författare in context.Författares.ToList())
            {
                ShowAllFörfattareFromDb.Add(författare);
            }

        }
    }
    #endregion
}