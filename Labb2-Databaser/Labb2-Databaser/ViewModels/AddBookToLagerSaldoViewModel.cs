using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Printing.IndexedProperties;
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
    private ObservableCollection<Författare> _showAllFörfattare;
    private Författare _selectedFörfattare;
    private LagerSaldo _bookToLagerSaldo;
    private ObservableCollection<Butiker> _showStores;
    private Butiker _selectedStore;
    private DateTime _releaseDateBook;
    private string _bookTitle;
    private int _bookPages;
    private int _bookCost;
    private string? _bookIsbn;
    private string _bookLanguage;

    public Butiker SelectedStore
    {
        get => _selectedStore;
        set
        {
            SetProperty(ref _selectedStore, value);
            GetSelectedStoreFromDb();
        }
    }
    public ObservableCollection<Butiker> ShowStores
    {
        get => _showStores;
        set
        {
            SetProperty(ref _showStores, value);
        }
    }

    public LagerSaldo AddNewBookToLagerSaldo
    {
        get
        {
            return _bookToLagerSaldo;
        }
        set
        {
            SetProperty(ref _bookToLagerSaldo, value);
        }
    }
    public string AddLanguageToBook
    {
        get
        {
            return _bookLanguage;
        }
        set
        {
            SetProperty(ref _bookLanguage, value);
        }
    }

    public Författare SelectedFörfattare
    {
        get
        {
            return _selectedFörfattare;
        }
        set
        {
            SetProperty(ref _selectedFörfattare, value);
        }
    }
    public string? BookIsbn
    {
        get
        {
            return _bookIsbn;
        }
        set
        {
            SetProperty(ref _bookIsbn, value);
        }
    }
    public DateTime BookReleased
    {
        get
        {
            return _releaseDateBook;
        }
        set
        {
            SetProperty(ref _releaseDateBook, value);
        }
    }
    public int BookCost
    {
        get
        {
            return _bookCost;
        }
        set
        {
            SetProperty(ref _bookCost, value);
        }
    }
    public int BookPages
    {
        get
        {
            return _bookPages;
        }
        set
        {
            SetProperty(ref _bookPages, value);
        }
    }

    public string Title
    {
        get
        {
            return _bookTitle;
        }
        set
        {
            SetProperty(ref _bookTitle, value);
        }
    }

    public ObservableCollection<Författare> ShowAllFörfattare
    {
        get
        {
            return _showAllFörfattare;
        }
        set
        {
            SetProperty(ref _showAllFörfattare, value);
        }
    }

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
    public IRelayCommand AddBookToBöckerTable { get; }
    #endregion
    public AddBookToLagerSaldoViewModel(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;

        NavigateToBokSamling = new RelayCommand(() =>
            _navigationManager.CurrentViewModel = new BokSamlingViewModel(navigationManager));

        GetFörfattareFromDb();
        ShowAllBooksInLagerSaldo();
        GetStoresFromDb();

        AddBookToBöckerTable = new RelayCommand(() =>
        {
            CreateNewBookForBöckerTable();
        });
    }

    #region Methods
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
    private void CreateNewBookForBöckerTable()
    {
        var createNewBookRow = new Böcker();

        createNewBookRow.Titel = Title;
        createNewBookRow.FörfattarId = SelectedFörfattare.Id;
        createNewBookRow.Sidor = BookPages;
        createNewBookRow.Pris = BookCost;
        createNewBookRow.Isbn13 = BookIsbn;
        createNewBookRow.UtgivningsDatum = BookReleased;
        createNewBookRow.Språk = AddLanguageToBook;

        using (var context = new BokhandelDbContext())
        {
            context.Böckers.Add(createNewBookRow);
            context.SaveChanges();
        }
    }

    private void GetFörfattareFromDb()
    {
        ShowAllFörfattare = new ObservableCollection<Författare>();
        using (var context = new BokhandelDbContext())
        {
            foreach (var författare in context.Författares.ToList())
            {
                ShowAllFörfattare.Add(författare);
            }
        }
    }

    private void GetSelectedStoreFromDb()
    {
        using (var context = new BokhandelDbContext())
        {
            var storeId = context.Butikers
                .FirstOrDefault(b => b.Id == SelectedStore.Id).Id;
        }
    }

    private void GetStoresFromDb()
    {
        using (var context = new BokhandelDbContext())
        {
            var showStores = context.Butikers.ToList();
            ShowStores = new ObservableCollection<Butiker>(showStores);
        }
    }
    #endregion
}