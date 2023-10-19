using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using MinesweeperV1.DataModel;

namespace MinesweeperV1
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MineField mineField;
        public bool isFlaggedMode;
        private int totalMinesCount = 10;
        private int xSize = 8;
        private int ySize = 8;
        private int totalCellsCount;

        public int flaggedCellsCount = 0;
        public int checkedCellsCount = 0;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            totalCellsCount = xSize * ySize;
            NewGame();
        }

        private void cellButton_Click(object sender, RoutedEventArgs e)
        {
            Button cellButton = sender as Button;
            if (cellButton != null)
            {
                checkedCellsCount++;
                Cell selectedCell = cellButton.DataContext as Cell;
                var cell =
                    (from oneCell in mineField.cells where oneCell.Equals(selectedCell) select oneCell).FirstOrDefault();

                if (!isFlaggedMode)
                {
                    if (cell.IsMined)
                    {
                        Image image = new Image();
                        image.HorizontalAlignment = HorizontalAlignment.Left;
                        image.Height = 48;
                        image.Width = 48;
                        image.Margin = new Thickness(12, 0, 0, 0);
                        image.Source = new BitmapImage(new Uri("Images/mine.jpg", UriKind.Relative));

                        Grid parentGrid = cellButton.Parent as Grid;
                        parentGrid.Children.Remove(cellButton);
                        parentGrid.Children.Add(image);

                        ShowAllMines();
                    }
                    else
                    {
                        cellButton.Content = cell.MinesAroundCount == 0 ? "" : cell.MinesAroundCount.ToString();
                    }
                    cellButton.IsEnabled = false;
                    cellButton.Foreground = new SolidColorBrush(Colors.Blue);
                }
                else
                {
                    mineField.SetFlagged(cell);
                    flaggedCellsCount++;

                    Image image = new Image();
                    image.HorizontalAlignment = HorizontalAlignment.Left;
                    image.Height = 48;
                    image.Width = 48;
                    image.Margin = new Thickness(12, 0, 0, 0);
                    image.Source = new BitmapImage(new Uri("Images/flag-icon.png", UriKind.Relative));

                    isFlaggedMode = false;
                    int remainedMinesCount = totalMinesCount - flaggedCellsCount;
                    remainedMinesCountTxt.Text = "Осталось: " + remainedMinesCount;

                    Grid parentGrid = cellButton.Parent as Grid;
                    parentGrid.Children.Remove(cellButton);
                    parentGrid.Children.Add(image);
                }


                if (checkedCellsCount == totalCellsCount && flaggedCellsCount == totalMinesCount)
                {

                    ShowAllMines();
                    MessageBox.Show("Вы выиграли", "Победа", MessageBoxButton.OKCancel);
                }
            }
        }

        private void ShowAllMines()
        {
            List<Button> buttons = new List<Button>();
            getButtons(cellsListBox, ref buttons);

            foreach (Button cellButton in buttons)
            {
                Cell cellItem = cellButton.DataContext as Cell;
                if (cellItem.IsMined && !cellItem.IsFlagged)
                {
                    Image image = new Image();
                    image.HorizontalAlignment = HorizontalAlignment.Left;
                    image.Height = 48;
                    image.Width = 48;
                    image.Margin = new Thickness(12, 0, 0, 0);
                    image.Source = new BitmapImage(new Uri("Images/mine.jpg", UriKind.Relative));

                    Grid parentGrid = cellButton.Parent as Grid;
                    parentGrid.Children.Remove(cellButton);
                    parentGrid.Children.Add(image);
                }
                cellButton.IsEnabled = false;
            }
        }

        public void getButtons(UIElement parent, ref List<Button> items)
        {
            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            if (childrenCount > 0)
            {
                for (int i = 0; i < childrenCount; i++)
                {
                    UIElement child = VisualTreeHelper.GetChild(parent, i) as UIElement;
                    if (child.GetType() == typeof(Button))
                    {
                        items.Add(child as Button);
                    }
                    getButtons(child, ref items);
                }
            }
        }

        private void NewGame()
        {
            mineField = new MineField(xSize, ySize, totalMinesCount);

            isFlaggedMode = false;
            checkedCellsCount = 0;
            flaggedCellsCount = 0;

            cellsListBox.ItemsSource = mineField.cells;
            cellsListBox.IsEnabled = true;
            remainedMinesCountTxt.Text = string.Empty;
        }

        private void setFlaggedButton_Click(object sender, EventArgs e)
        {
            isFlaggedMode = true;
        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            NewGame();
        }

        private void rulesPanel_Loaded(object sender, RoutedEventArgs e)
        {
            LoadRules();
        }

        private void LoadRules()
        {
            rulesTxt.Text = "Сапер - логическая игра на внимательность. Игровое поле представляет собой сетку, состоящую из ячеек. В поле в произвольных ячейках располагаются мины. Мин определенное количество. Цель игры: последовательно открывая ячейки и отмечая ячейки с минами флажками найти все мины, не открыв ни одной мины. При открытии ячейки с миной игра заканчивается. Если в ячейке нет мины, то при открытии ячейки появляется число, обозначающее количество мин вокруг. Оценивая это число нужно отмечать ячейки с минами флажками и стараться не попасть на мину. В данной игре для того, чтобы установить флажок нужно нажать книпку в панели приложения с изображением флажка и нажать произвольную ячейку. Для открытия ячейки нужно просто нажать на ячейку. Удачи в игре!";
        }

        private void saveSettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            int minesCount;
            if(Int32.TryParse(MinesCountTxt.Text, out minesCount))
            {
                totalMinesCount = minesCount;
                MessageBox.Show("Количество мин сохранено");
                NewGame();
            }
            else
            {
                MessageBox.Show("Вы ввели некорректные данные");
            }
        }

        private void aboutPanel_Loaded(object sender, RoutedEventArgs e)
        {
            aboutTxt.Text = "Сапер\nразработчик: Привалов Владимир.\ne-mail: vovaprivalov@gmail.com,\n twitter: @vovaekb1990";
        }
    }
}