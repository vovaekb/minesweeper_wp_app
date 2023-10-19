using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MinesweeperV1.DataModel
{
    public class MineField
    {
        public ObservableCollection<Cell> cells;

        public List<Cell> minedCells;

        public Random random;

        private int _xSize;

        public int XSize
        {
            get { return _xSize; }
            set { _xSize = value; }
        }

        private int _minesCount;

        public int MinesCount
        {
            get { return _minesCount; }
            set { _minesCount = value; }
        }

        private int _ySize;

        public int YSize
        {
            get { return _ySize; }
            set { _ySize = value; }
        }

        private int _foundedMinesCount;

        public int FoundedMinesCount
        {
            get { return _foundedMinesCount; }
            set { _foundedMinesCount = value; }
        }

        public MineField(int xSize, int ySize, int minesCount)
        {
            random = new Random();
            XSize = xSize;
            YSize = ySize;
            MinesCount = minesCount;
            minedCells = new List<Cell>();
            InitializeField();
        }

        private void InitializeField()
        {
            cells = new ObservableCollection<Cell>();

            for (int i = 0; i < XSize; i++)
            {
                for (int j = 0; j < YSize; j++)
                {
                    Cell cell = new Cell
                    {
                        X = i,
                        Y = j
                    };
                    cells.Add(cell);
                }
            }

            while (minedCells.Count < MinesCount)
            {
                int randomX = random.Next(XSize);
                int randomY = random.Next(YSize);

                var randomCell =
                    (from cell in cells where cell.X.Equals(randomX) && cell.Y.Equals(randomY) select cell).
                        FirstOrDefault();

                if (!minedCells.Contains(randomCell))
                {
                    minedCells.Add(randomCell);

                    randomCell.IsMined = true;
                }
            }

            for (int i = 0; i < XSize; i++)
            {
                for (int j = 0; j < YSize; j++)
                {
                    int minesAroundCount = 0;
                    Cell cell =
                        (from cellItem in cells where cellItem.X.Equals(i) && cellItem.Y.Equals(j) select cellItem).
                            FirstOrDefault();
                    if (!cell.IsMined)
                    {
                        if (i > 0 && j > 0)
                        {
                            Cell topLeftNeighboringCell =
                                (from cellItem in cells
                                 where cellItem.X.Equals(i - 1) && cellItem.Y.Equals(j - 1)
                                 select cellItem).FirstOrDefault();
                            if (topLeftNeighboringCell.IsMined) minesAroundCount++;
                        }
                        if (i > 0)
                        {
                            Cell topNeighboringCell = (from cellItem in cells
                                                       where cellItem.X.Equals(i - 1) && cellItem.Y.Equals(j)
                                                       select cellItem).FirstOrDefault();
                            if (topNeighboringCell.IsMined) minesAroundCount++;
                        }
                        if (i > 0 && j < XSize - 1)
                        {
                            Cell topRightNeighboringCell =
                                (from cellItem in cells
                                 where cellItem.X.Equals(i - 1) && cellItem.Y.Equals(j + 1)
                                 select cellItem).FirstOrDefault();
                            if (topRightNeighboringCell.IsMined) minesAroundCount++;
                        }
                        if (j > 0)
                        {
                            Cell leftNeighboringCell =
                                (from cellItem in cells
                                 where cellItem.X.Equals(i) && cellItem.Y.Equals(j - 1)
                                 select cellItem).FirstOrDefault();
                            if (leftNeighboringCell.IsMined) minesAroundCount++;
                        }
                        if (j < XSize - 1)
                        {
                            Cell rightNeighboringCell =
                                (from cellItem in cells
                                 where cellItem.X.Equals(i) && cellItem.Y.Equals(j + 1)
                                 select cellItem).FirstOrDefault();
                            if (rightNeighboringCell.IsMined) minesAroundCount++;
                        }
                        if (i < YSize - 1 && j > 0)
                        {
                            Cell bottomLeftNeighboringCell =
                                (from cellItem in cells
                                 where cellItem.X.Equals(i + 1) && cellItem.Y.Equals(j - 1)
                                 select cellItem).FirstOrDefault();
                            if (bottomLeftNeighboringCell.IsMined) minesAroundCount++;
                        }
                        if (i < YSize - 1)
                        {
                            Cell bottomNeighboringCell = (from cellItem in cells
                                                          where cellItem.X.Equals(i + 1) && cellItem.Y.Equals(j)
                                                          select cellItem).FirstOrDefault();
                            if (bottomNeighboringCell.IsMined) minesAroundCount++;
                        }
                        if (i < YSize - 1 && j < XSize - 1)
                        {
                            Cell bottomRightNeighboringCell =
                                (from cellItem in cells
                                 where cellItem.X.Equals(i + 1) && cellItem.Y.Equals(j + 1)
                                 select cellItem).FirstOrDefault();
                            if (bottomRightNeighboringCell.IsMined) minesAroundCount++;
                        }

                        cell.MinesAroundCount = minesAroundCount;
                    }
                    else
                    {
                        cell.MinesAroundCount = 100;
                    }
                }
            }
        }

        public void SetFlagged(Cell cell)
        {
            var flaggedCell = (from cellItem in cells where cellItem.Equals(cell) select cellItem).FirstOrDefault();
            flaggedCell.IsFlagged = true;
        }

    }
}
