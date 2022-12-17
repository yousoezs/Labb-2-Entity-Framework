using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Labb2_Databaser.Managers;
using Labb2_Databaser.ViewModels;
using Labb2_Databaser.Views;

namespace Labb2_Databaser
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly NavigationManager _navigationManager;

        public App()
        {
            _navigationManager = new NavigationManager();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _navigationManager.CurrentViewModel = new MainMenuViewModel(_navigationManager);

            var rootWindow = new RootWindow { DataContext = new RootWindowViewModel(_navigationManager) };
            rootWindow.Show();
            base.OnStartup(e);
        }
    }
}
