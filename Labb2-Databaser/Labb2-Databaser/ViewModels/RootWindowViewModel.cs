using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Labb2_Databaser.Managers;

namespace Labb2_Databaser.ViewModels;

public class RootWindowViewModel : ObservableObject
{
    #region Properties & Fields
    private readonly NavigationManager _navigationManager;
    public ObservableObject CurrentViewModel => _navigationManager.CurrentViewModel;
    #endregion

    public RootWindowViewModel(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;
        _navigationManager.CurrentViewModelChanged += CurrentViewModelChanged;
    }

    private void CurrentViewModelChanged()
    {
        OnPropertyChanged(nameof(CurrentViewModel));
    }
}