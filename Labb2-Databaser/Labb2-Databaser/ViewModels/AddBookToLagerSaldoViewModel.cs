using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Printing.IndexedProperties;
using System.Threading.Tasks;
using System.Windows.Media;
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
    private Böcker _selectedBook;
    private Författare _selectedFörfattare;
    private DateTime _releaseDateBook;
    private string _bookTitle;
    private int _bookPages;
    private int _bookCost;
    private string? _bookIsbn;
    private string _bookLanguage;

    public Böcker SelectedBook
    {
        get => _selectedBook;
        set => SetProperty(ref _selectedBook, value);
    }
    public string AddLanguageToBook
    {
        get => _bookLanguage;
        set => SetProperty(ref _bookLanguage, value);
    }

    public Författare SelectedFörfattare
    {
        get => _selectedFörfattare;
        set => SetProperty(ref _selectedFörfattare, value);
    }
    public string? BookIsbn
    {
        get => _bookIsbn;
        set => SetProperty(ref _bookIsbn, value);
    }
    public DateTime BookReleased
    {
        get => _releaseDateBook;
        set => SetProperty(ref _releaseDateBook, value);
    }
    public int BookCost
    {
        get => _bookCost;
        set => SetProperty(ref _bookCost, value);
    }
    public int BookPages
    {
        get => _bookPages;
        set => SetProperty(ref _bookPages, value);
    }

    public string Title
    {
        get => _bookTitle;
        set => SetProperty(ref _bookTitle, value);
    }

    public ObservableCollection<Författare> ShowAllFörfattare
    {
        get => _showAllFörfattare;
        set => SetProperty(ref _showAllFörfattare, value);
    }

    public ObservableCollection<Böcker> ShowAllBooks
    {
        get => _showBooksInLagerSaldo;
        set => SetProperty(ref _showBooksInLagerSaldo, value);
        
    }
    #endregion

    #region IRelayCommands
    public IRelayCommand NavigateToBokSamling { get; }
    public IRelayCommand AddBookToBöckerTable { get; }
    public IRelayCommand RemoveBookFromDb { get; }
    public IRelayCommand UpdateBöckerTableRow { get; }
    #endregion
    public AddBookToLagerSaldoViewModel(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;

        NavigateToBokSamling = new RelayCommand(() =>
            _navigationManager.CurrentViewModel = new BokSamlingViewModel(navigationManager));

        BookReleased = DateTime.Today;

        GetFörfattareFromDb();
        ShowAllBooksInBöckerTable();

        AddBookToBöckerTable = new RelayCommand(() =>
        {
            CreateNewBookForBöckerTable();
        });

        RemoveBookFromDb = new RelayCommand(() =>
        {
            DeleteBookRowFromBöckerTable(SelectedBook);
        });

        UpdateBöckerTableRow = new RelayCommand(() =>
        {
            UpdateBookRowInBöckerTable(SelectedBook);
        });
    }

    #region Methods
    private void ShowAllBooksInBöckerTable()
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

        using (var context = new BokhandelDbContext())
        {
            createNewBookRow.Titel = Title;
            createNewBookRow.FörfattarId = SelectedFörfattare.Id;
            createNewBookRow.Sidor = BookPages;
            createNewBookRow.Pris = BookCost;
            createNewBookRow.Isbn13 = BookIsbn;
            createNewBookRow.UtgivningsDatum = BookReleased;
            createNewBookRow.Språk = AddLanguageToBook;

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

    private void DeleteBookRowFromBöckerTable(Böcker? selectedBook)
    {
        using (var context = new BokhandelDbContext())
        {
            var deleteRow = context.Böckers
                .FirstOrDefault(b => b.Isbn13
                    .Equals(SelectedBook.Isbn13));

            selectedBook = deleteRow;

            context.Böckers.Remove(selectedBook);
            context.SaveChanges();
        }
    }

    private void UpdateBookRowInBöckerTable(Böcker? selectedBook)
    {
        using (var context = new BokhandelDbContext())
        {
            var updateRow = selectedBook;

            updateRow = context.Böckers
                .FirstOrDefault(b => b.Isbn13
                    .Equals(SelectedBook.Isbn13));

            updateRow.Titel = Title;
            updateRow.FörfattarId = SelectedFörfattare.Id;
            updateRow.Pris = BookCost;
            updateRow.Språk = _bookLanguage;
            updateRow.UtgivningsDatum = BookReleased;
            updateRow.Sidor = BookPages;

            context.Böckers.Update(updateRow);
            context.SaveChanges();
        }
    }
    #endregion
}