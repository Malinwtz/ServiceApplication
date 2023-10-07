
using ServiceApplication.MVVM.ViewModels;
using ServiceApplication.Services;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace ServiceApplication
{
   
    public partial class MainWindow : Window
    {
       
        public MainWindow(MainWindowViewModel mainWindowViewModel)
        {
            InitializeComponent();
            DataContext = mainWindowViewModel; //binda contextdelen till mainwindow
    
        }
    }
}






