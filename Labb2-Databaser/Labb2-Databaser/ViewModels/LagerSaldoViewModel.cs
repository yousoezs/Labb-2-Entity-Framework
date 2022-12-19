﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Azure.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Labb2_Databaser.DbModels;
using Labb2_Databaser.Managers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Labb2_Databaser.ViewModels;

public class LagerSaldoViewModel : ObservableObject
{
    #region Fields And Properties
    private readonly NavigationManager _navigationManager;
    private int? _updateValue;
    private ObservableCollection<LagerSaldo> _allBooks;
    private ObservableCollection<Butiker> _storesList;
    private ObservableCollection<LagerSaldo> _books;
    private Butiker _storeSelected;
    private Butiker _currentStore;
    private LagerSaldo _book;

    public ObservableCollection<LagerSaldo> AllBooks
    {
        get => _allBooks;
        set => SetProperty(ref _allBooks, value);
    }
    public int? UpdateValue
    {
        get => _updateValue;
        set => SetProperty(ref _updateValue, value);
    }
    public LagerSaldo SelectedBook
    {
        get => _book;
        set => SetProperty(ref _book, value);
    }
    public Butiker CurrentBookStore
    {
        get => _currentStore;
        set => SetProperty(ref _currentStore, value);
    }

    public ObservableCollection<Butiker> StoresList
    {
        get => _storesList;
        set => SetProperty(ref _storesList, value);
    }

    public Butiker StoreSelected
    {
        get => _storeSelected;
        set
        {
            SetProperty(ref _storeSelected, value);
            ShowStoreBalance();
            CurrentBookStore = value;
        }
    }
    public ObservableCollection<LagerSaldo> Books
    {
        get => _books;
        set => SetProperty(ref _books, value);
    }
    #endregion

    #region IRelayCommands

    public IRelayCommand NavigateBack { get; }
    public IRelayCommand UpdateQuantity { get; }
    public IRelayCommand RemoveBookFromLagerSaldo { get; }

    #endregion

    public LagerSaldoViewModel(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;

        NavigateBack = new RelayCommand(() =>
            _navigationManager.CurrentViewModel = new MainMenuViewModel(navigationManager));
        ShowAllBooks();
        ShowListedCurrencies();


        UpdateQuantity = new RelayCommand( async () =>
        {
            var book = await GetBookFromDatabase();
            var lagerSaldosInSelectedStore = await GetLagerSaldosForSelectedStoreFromDatabase();
            bool bookExists = lagerSaldosInSelectedStore.Exists(bs => bs.Isbn == book.Isbn13);

            if (bookExists)
            {
                UpdateLagerSaldoQuantity(lagerSaldosInSelectedStore, book.Isbn13);
            }

            if (!bookExists)
            {
                CreateRowForLagerSaldo();
            }
        });

        RemoveBookFromLagerSaldo = new RelayCommand(async () =>
        {
            var book = await GetBookFromDatabase();
            var lagerSaldosInSelectedStore = await GetLagerSaldosForSelectedStoreFromDatabase();
            DeleteRowFromLagerSaldo(lagerSaldosInSelectedStore, book.Isbn13);
        });
    }

    #region Methods
    private async Task ShowListedCurrencies()
    {
        using (var context = new BokhandelDbContext())
        {
            var store = await context.Butikers.ToListAsync();
            StoresList = new ObservableCollection<Butiker>(store);
        }
    }
    private async Task ShowStoreBalance()
    {
        using (var context = new BokhandelDbContext())
        {
            var lagerSaldo = await context.LagerSaldos
                .Include(b => b.IsbnNavigation)
                .Where(l => l.ButikId.Equals(StoreSelected.Id))
                .ToListAsync();
            Books = new ObservableCollection<LagerSaldo>(lagerSaldo);
        }
    }

    private async Task ShowAllBooks()
    {
        using (var context = new BokhandelDbContext())
        {
            AllBooks = new ObservableCollection<LagerSaldo>();
            var books = await context.LagerSaldos
                .Include(l => l.IsbnNavigation)
                .ToListAsync();

            foreach (var book in books)
            {
                AllBooks.Add(book);
            }
        }
    }

    private async Task<Böcker?> GetBookFromDatabase()
    {
        using (var context = new BokhandelDbContext())
        {
            var selectedBook = await context.Böckers
                .FirstOrDefaultAsync(b => b.Isbn13
                    .Equals(SelectedBook.Isbn));
            return selectedBook;
        }
    }

    private async Task<List<LagerSaldo>> GetLagerSaldosForSelectedStoreFromDatabase()
    {
        using (var context = new BokhandelDbContext())
        {
            var selectLagerSaldo = await context.LagerSaldos
                .Where(l => l.ButikId
                    .Equals(CurrentBookStore.Id))
                .ToListAsync();
            return selectLagerSaldo;
        }
    }

    private async Task UpdateLagerSaldoQuantity(List<LagerSaldo>? lagerSaldosInSelectedStore, string isbn13)
    {
        var lagerSaldoToUpdate = lagerSaldosInSelectedStore
            .FirstOrDefault(l => l.Isbn
                .Equals(isbn13));

        using (var context = new BokhandelDbContext())
        {
            var lagerSaldoDb = await context.LagerSaldos.FirstAsync(ls =>
                ls.Isbn == lagerSaldoToUpdate.Isbn && ls.ButikId == lagerSaldoToUpdate.ButikId);

            lagerSaldoDb.Antal += UpdateValue;

            context.LagerSaldos.Update(lagerSaldoDb);
            context.SaveChanges();
        }
    }

    private void CreateRowForLagerSaldo()
    {
        var lagerSaldoCreateRow = new LagerSaldo();

        lagerSaldoCreateRow.Isbn = SelectedBook.Isbn;
        lagerSaldoCreateRow.ButikId = CurrentBookStore.Id;
        lagerSaldoCreateRow.Antal = UpdateValue;

        using (var context = new BokhandelDbContext())
        {
            context.LagerSaldos.Add(lagerSaldoCreateRow);
            context.SaveChanges();
        }
    }

    private void DeleteRowFromLagerSaldo(List<LagerSaldo>? lagerSaldosInSelectedStore, string isbn13)
    {
        var lagerSaldoToDelete = lagerSaldosInSelectedStore
            .FirstOrDefault(l => l.Isbn
                .Equals(isbn13));

        using (var context = new BokhandelDbContext())
        {
            var lagerSaldoDb = context.LagerSaldos.First(ls => ls.Isbn
                .Equals(lagerSaldoToDelete.Isbn) && ls.ButikId
                .Equals(lagerSaldoToDelete.ButikId));

            context.LagerSaldos.Remove(lagerSaldoDb);
            context.SaveChanges();
        }
    }
    #endregion
}