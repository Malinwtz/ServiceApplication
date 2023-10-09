using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using System;
namespace ServiceApplication.MVVM.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject 
    {
        private readonly IServiceProvider _serviceProvider; 
        public MainWindowViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            CurrentViewModel = _serviceProvider.GetRequiredService<HomeViewModel>(); 
            // Såhär slipper vi lägga in homeviewmodel som en ny instans.
        }


        [ObservableProperty] //autogenererar currentviewmodeldelen med getters och setters och onpropertychanged
        private ObservableObject? _currentViewModel;  //vi får vår public-del (CurrentViewModel) automatiskt från ObsObj

    }
}
