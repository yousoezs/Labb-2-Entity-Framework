using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Labb2_Databaser.Managers;

public class NavigationManager : ObservableObject
{
    private ObservableObject _currentViewModel;

    public ObservableObject CurrentViewModel
    {
        get { return _currentViewModel; }
        set
        {
            _currentViewModel = value;
            OnCurrentViewModelChanged();
        }
    }

    public event Action CurrentViewModelChanged;

    public void OnCurrentViewModelChanged()
    {
        CurrentViewModelChanged?.Invoke();
    }
}