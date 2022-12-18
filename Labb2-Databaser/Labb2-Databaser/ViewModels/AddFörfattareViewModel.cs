﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Documents;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Labb2_Databaser.DbModels;
using Labb2_Databaser.Managers;
using Microsoft.EntityFrameworkCore;

namespace Labb2_Databaser.ViewModels;

public class AddFörfattareViewModel : ObservableObject
{
    #region Fields And Properties
    private readonly NavigationManager _navigationManager;
    private ObservableCollection<Författare> _showAllFörfattare;
    private string _addFörfattareNamn;
    private string _addFörfattareEfterNamn;
    private DateTime _addFörfattareFödelseDatum;
    private Författare _selectedFörfattare;
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
    public DateTime AddFörfattareFödelseDatum
    {
        get
        {
            return _addFörfattareFödelseDatum;
        }
        set
        {
            SetProperty(ref _addFörfattareFödelseDatum, value);
        }
    }
    public string AddFörfattareEfterNamn
    {
        get
        {
            return _addFörfattareEfterNamn;
        }
        set
        {
            SetProperty(ref _addFörfattareEfterNamn, value);
        }
    }

    public string AddFörfattareNamn
    {
        get
        {
            return _addFörfattareNamn;
        }
        set
        {
            SetProperty(ref _addFörfattareNamn, value);
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
    #endregion

    #region IRelayCommands
    public IRelayCommand NavigateToBokSamling { get; }
    public IRelayCommand AddNewRowFörfattareDb { get; }
    public IRelayCommand RemoveFörfattare { get; }
    public IRelayCommand UpdateFörfattareToDb { get; }
    #endregion
    public AddFörfattareViewModel(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;

        NavigateToBokSamling = new RelayCommand(() =>
            _navigationManager.CurrentViewModel = new BokSamlingViewModel(navigationManager));

        AddNewRowFörfattareDb = new RelayCommand(() =>
        {
            AddFörfattareToDb();
        });

        RemoveFörfattare = new RelayCommand(() =>
        {
            var selectedFörfattare = GetSelectedFörfattareFromDb();
            RemoveFörfattareFromDb(selectedFörfattare);
        });

        UpdateFörfattareToDb = new RelayCommand(() =>
        {
            var selectedFörfattare = GetSelectedFörfattareFromDb();
            UpdateFörfattareTable(selectedFörfattare);
        });

        ShowFörfattareInDb();
    }
    #region Methods

    private void ShowFörfattareInDb()
    {
        using (var context = new BokhandelDbContext())
        {
            ShowAllFörfattare = new ObservableCollection<Författare>();
            foreach (var författare in context.Författares.ToList())
            {
                ShowAllFörfattare.Add(författare);
            }
        }
    }

    private void AddFörfattareToDb()
    {

        using (var context = new BokhandelDbContext())
        {
            var författareCreateRow = new Författare();

            författareCreateRow.Id = context.Författares.Max(f => f.Id) + 1;

            författareCreateRow.Förnamn = AddFörfattareNamn;
            författareCreateRow.Efternamn = AddFörfattareEfterNamn;
            författareCreateRow.Födelsedatum = AddFörfattareFödelseDatum;

            context.Författares.Add(författareCreateRow);
            context.SaveChanges();
        }
    }

    private void RemoveFörfattareFromDb(List<Författare> författareRemoved)
    {
        using (var context = new BokhandelDbContext())
        {
            var removeFörfattare = context.Författares
                .First(f => f.Id
                    .Equals(SelectedFörfattare.Id));

            context.Författares.Remove(removeFörfattare);
            context.SaveChanges();
        }
    }

    private List<Författare> GetSelectedFörfattareFromDb()
    {
        using (var context = new BokhandelDbContext())
        {
            var författare = context.Författares
                .Where(f => f.Id
                    .Equals(SelectedFörfattare.Id))
                .ToList();

            return författare;
        }
    }

    private void UpdateFörfattareTable(List<Författare> selectedFörfattareToUpdate)
    {
        using (var context = new BokhandelDbContext())
        {
            var updatedFörfattare = context.Författares
                .First(f => f.Id
                    .Equals(SelectedFörfattare.Id));

            updatedFörfattare.Förnamn = AddFörfattareNamn;
            updatedFörfattare.Efternamn = AddFörfattareEfterNamn;
            updatedFörfattare.Födelsedatum = AddFörfattareFödelseDatum;

            context.Författares.Update(updatedFörfattare);
            context.SaveChanges();
        }
    }
    #endregion
}