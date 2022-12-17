using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Labb2_Databaser.Managers;

namespace Labb2_Databaser.ViewModels;

public class MainMenuViewModel : ObservableObject
{
    #region Fields And Properties
    private NavigationManager _navigationManager;
    #endregion

    #region IRelayCommands

    public IRelayCommand NavigateLagerSaldo { get; }
    public IRelayCommand NavigateBokSamling { get; }

    #endregion
    public MainMenuViewModel(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;

        NavigateLagerSaldo = 
            new RelayCommand(() => _navigationManager.CurrentViewModel = new LagerSaldoViewModel(navigationManager));
        NavigateBokSamling = new RelayCommand(() =>
            _navigationManager.CurrentViewModel = new BokSamlingViewModel(navigationManager));
    }
}