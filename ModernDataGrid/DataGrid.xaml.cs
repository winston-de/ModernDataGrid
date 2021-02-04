using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ModernDataGrid
{
    public sealed partial class DataGrid : UserControl
    {
        ObservableCollection<Row> rows = new ObservableCollection<Row>();

        public DataGrid()
        {
            Populate();
            this.InitializeComponent();
        }

        private void Populate()
        {
            for (int i = 0; i < 100; i++)
            {
                rows.Add(new Row()
                {
                    Data = new MockData()
                    {
                        Col1 = RandomString(4),
                        Col2 = RandomString(4),
                        Col3 = RandomString(4),
                    }
                });
            }
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void GridSplitter_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            rows.ToList().ForEach(x =>
            {
                x.Lengths = new GridLength[] {
                    new GridLength(Column1.ActualWidth, GridUnitType.Pixel),
                    new GridLength(Column2.ActualWidth, GridUnitType.Pixel),
                    new GridLength(Column3.ActualWidth, GridUnitType.Pixel),
                };
            });
        }
    }
    class MockData
    {
        public string Col1 { get; set; }
        public string Col2 { get; set; }
        public string Col3 { get; set; }
    }

    class Row : ObservableObject
    {
        public MockData Data { get; set; }


        private GridLength[] lengths =  new GridLength[] 
        {
            new GridLength(1, GridUnitType.Star),
            new GridLength(1, GridUnitType.Star),
            new GridLength(1, GridUnitType.Star),
        };
        public GridLength[] Lengths
        {
            get => lengths;
            set => SetProperty(ref lengths, value);
        }
    }
}
