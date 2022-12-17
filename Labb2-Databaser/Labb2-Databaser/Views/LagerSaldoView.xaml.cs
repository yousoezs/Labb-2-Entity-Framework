using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Labb2_Databaser.DbModels;
using Microsoft.EntityFrameworkCore;

namespace Labb2_Databaser.Views
{
    /// <summary>
    /// Interaction logic for LagerSaldoView.xaml
    /// </summary>
    public partial class LagerSaldoView : UserControl
    {
        public LagerSaldoView()
        {
            InitializeComponent();
        }
    }
}
