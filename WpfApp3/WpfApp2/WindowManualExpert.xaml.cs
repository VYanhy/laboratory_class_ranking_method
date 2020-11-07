using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DragDropEffects = System.Windows.DragDropEffects;
using DragEventArgs = System.Windows.DragEventArgs;
using DragEventHandler = System.Windows.DragEventHandler;
using MessageBox = System.Windows.MessageBox;

namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для WindowManualExpert.xaml
    /// </summary>
    public partial class WindowManualExpert : Window
    {
        public ChoiceRow[] choiceRows;

        public delegate void FileNameHandler(string message);
        public event FileNameHandler Notify;

        public delegate Point GetPosition(IInputElement element);
        int rowIndex = -1;

        public WindowManualExpert()
        {
            InitializeComponent();
            productsDataGrid.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(productsDataGrid_PreviewMouseLeftButtonDown);
            productsDataGrid.Drop += new DragEventHandler(productsDataGrid_Drop);
        }

        public WindowManualExpert(int i)
        {
            InitializeComponent();
            this.Title = (i + 1).ToString() + " експерт";
            over_title.Text += " " + (i + 1).ToString();
            productsDataGrid.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(productsDataGrid_PreviewMouseLeftButtonDown);
            productsDataGrid.Drop += new DragEventHandler(productsDataGrid_Drop);
        }

        void productsDataGrid_Drop(object sender, DragEventArgs e)
        {
            if (rowIndex < 0)
                return;
            int index = this.GetCurrentRowIndex(e.GetPosition);
            if (index < 0)
                return;
            if (index == rowIndex)
                return;
            if (index == productsDataGrid.Items.Count - 1)
            {
                MessageBox.Show("This row-index cannot be drop");
                return;
            }
            ProductCollection productCollection = Resources["ProductList"] as ProductCollection;
            Product changedProduct = productCollection[rowIndex];
            productCollection.RemoveAt(rowIndex);
            productCollection.Insert(index, changedProduct);

            productsDataGrid.Items.Refresh();
        }

        void productsDataGrid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            rowIndex = GetCurrentRowIndex(e.GetPosition);
            if (rowIndex < 0)
                return;
            productsDataGrid.SelectedIndex = rowIndex;
            Product selectedEmp = productsDataGrid.Items[rowIndex] as Product;
            if (selectedEmp == null)
                return;
            DragDropEffects dragdropeffects = DragDropEffects.Move;
            if (DragDrop.DoDragDrop(productsDataGrid, selectedEmp, dragdropeffects) != DragDropEffects.None)
            {
                productsDataGrid.SelectedItem = selectedEmp;
            }
        }

        private bool GetMouseTargetRow(Visual theTarget, GetPosition position)
        {
            Rect rect = VisualTreeHelper.GetDescendantBounds(theTarget);
            Point point = position((IInputElement)theTarget);
            return rect.Contains(point);
        }

        private DataGridRow GetRowItem(int index)
        {
            if (productsDataGrid.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
                return null;
            return productsDataGrid.ItemContainerGenerator.ContainerFromIndex(index) as DataGridRow;
        }

        private int GetCurrentRowIndex(GetPosition pos)
        {
            int curIndex = -1;
            for (int i = 0; i < productsDataGrid.Items.Count; i++)
            {
                DataGridRow itm = GetRowItem(i);
                if (GetMouseTargetRow(itm, pos))
                {
                    curIndex = i;
                    break;
                }
            }
            return curIndex;
        }

        void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void filePathWrite_GotFocus(object sender, RoutedEventArgs e)
        {
            hintPath.Visibility = Visibility.Hidden;
        }

        private void filePathWrite_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (filePathWrite.Text.Length == 0)
                hintPath.Visibility = Visibility.Visible;
        }

        private void fileNameWrite_GotFocus(object sender, RoutedEventArgs e)
        {
            hintName.Visibility = Visibility.Hidden;
        }

        private void fileNameWrite_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (fileNameWrite.Text.Length == 0)
                hintName.Visibility = Visibility.Visible;
        }

        private void filePathWrite_LostFocus(object sender, RoutedEventArgs e)
        {
            if (filePathWrite.Text.Length == 0)
                hintPath.Visibility = Visibility.Visible;
        }

        private void fileNameWrite_LostFocus(object sender, RoutedEventArgs e)
        {
            if (fileNameWrite.Text.Length == 0)
                hintName.Visibility = Visibility.Visible;
        }

        private void GetFromDataGrid()
        {
            Dictionary<int, string> objectListFromDataGrid = new Dictionary<int, string>(Ranking.m);
            for (int i = 0; i < productsDataGrid.Items.Count; i++)
            {
                objectListFromDataGrid.Add(i, productsDataGrid.Items.GetItemAt(i).ToString());
                //Console.WriteLine(objectListFromDataGrid[i + 1]);
            }

            choiceRows = new ChoiceRow[Ranking.m];
            ProductCollection products = new ProductCollection();

            string pattern;
            for (int i = 0; i <= Ranking.m; i++)
            {
                for (int j = 0; j <= Ranking.m; j++)
                {
                    pattern = @"^" + (i + 1).ToString() + ".[\\s\\w]*$";

                    if (Regex.IsMatch(objectListFromDataGrid[j], pattern, RegexOptions.IgnoreCase))
                    {
                        choiceRows[i] = new ChoiceRow(products.ElementAt(i), j + 1);
                        /*Console.WriteLine(
                            "choiceRows id: " + 
                            choiceRows[i - 1].Product.ProductId.ToString() + ", name: " + 
                            choiceRows[i - 1].Product.ProductName + ", rate: " + 
                            choiceRows[i - 1].Rate);*/
                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GetFromDataGrid();

            WindowResultPairwiseComparisonMatrix window = new WindowResultPairwiseComparisonMatrix(choiceRows);
            window.Show();
        }

        private void Button_Click_WriteToFile(object sender, RoutedEventArgs e)
        {
            try
            {
                GetFromDataGrid();

                string filePath = $"{filePathWrite.Text}\\{fileNameWrite.Text}.xlsx";
                Ranking.WriteExpertRanks(filePath, choiceRows);
                Notify?.Invoke(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void filePathWrite_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (!string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    filePathWrite.Text = fbd.SelectedPath;
                }
            }
        }
    }
}
