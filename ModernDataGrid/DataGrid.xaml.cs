using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
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

        MockDataViewModel MockDataViewModel = new MockDataViewModel();
        public DataGrid()
        {
            MockDataViewModel.Populate();
            this.InitializeComponent();
        }

        private void GridSplitter_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            MockDataViewModel.ColumnViewModels[0].Length = new GridLength(Column1.ActualWidth, GridUnitType.Pixel);
            MockDataViewModel.ColumnViewModels[1].Length = new GridLength(Column2.ActualWidth, GridUnitType.Pixel);
            MockDataViewModel.ColumnViewModels[2].Length = new GridLength(Column3.ActualWidth, GridUnitType.Pixel);
        }
    }
    class MockData
    {
        public string[] Data { get; set; }
    }

    class Row : ObservableObject
    {
        public string[] Data { get; set; }


        private ColumnViewModel[] columnViewModels;
        public ColumnViewModel[] ColumnViewModels
        {
            get => columnViewModels;
            set => SetProperty(ref columnViewModels, value);
        }
    }

    class MockDataViewModel : ObservableObject
    {
        ObservableCollection<Row> rows = new ObservableCollection<Row>();
        public ObservableCollection<Row> Rows
        {
            get => rows;
            set => SetProperty(ref rows, value);
        }

        public ColumnViewModel[] ColumnViewModels { get; set; } = new ColumnViewModel[]
        {
            new ColumnViewModel(),
            new ColumnViewModel(),
            new ColumnViewModel(),
        };

        public RelayCommand<string> SortPressed => new RelayCommand<string>(x => Sort(int.Parse(x)));

        public void Sort(int columnNum)
        {
            Rows = new ObservableCollection<Row>(Rows.ToList().OrderBy(x => x.Data[columnNum]));
        }

        public void Populate()
        {
            for (int i = 0; i < 1000; i++)
            {
                Rows.Add(new Row()
                {
                    Data = new string[]
                    {
                        RandomString(4),
                        RandomString(4),
                        RandomString(4),
                    },
                    ColumnViewModels = ColumnViewModels,
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

    }

    class ColumnViewModel : ObservableObject
    {
        private GridLength length = new GridLength(1, GridUnitType.Star);
        public GridLength Length
        {
            get => length;
            set => SetProperty(ref length, value);
        }
    }
}
