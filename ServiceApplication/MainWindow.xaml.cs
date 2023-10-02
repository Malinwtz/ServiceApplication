﻿using ServiceApplication.MVVM.Models;
using ServiceApplication.MVVM.ViewModels;
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




//public ObservableCollection<DeviceItem> Devices { get; set; } = new ObservableCollection<DeviceItem>()
//{ 
//    // Hårdkodad lista med devices som nedan kopplas till frontend för att loopas ut i vyn. 
//    new DeviceItem { DeviceId = "1", DeviceType = "light", Placement = "Kitchen", IsActive = true },
//    new DeviceItem { DeviceId = "2", DeviceType = "ledstrip", Placement = "Livingroom", IsActive = false },
//    new DeviceItem { DeviceId = "3", DeviceType = "fan", Placement = "Bedroom", IsActive = false },
//};


//public MainWindow()
//{
//    InitializeComponent(); //allt måste ske efter denna metod

//    //Denna mappning behövs för att mappa frontend till backend för att listan ska kunna visas.
//    this.DataContext = this;
//    // Kopplar listan Devices till frontendlistan
//    // DeviceList.ItemsSource = Devices;

//}


////Om man trycker ner musknappen så kommer denna funktion att köras.
//private void Window_MouseDown(object sender, MouseButtonEventArgs e) //för att kunna flytta fönstret, kanske bara använda i debugläge och inte i den färdiga applikationen
//{
//    if (e.ChangedButton == MouseButton.Left)
//    {
//        this.DragMove();
//    }
//}