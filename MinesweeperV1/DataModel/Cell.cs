using System;
using System.ComponentModel;
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
    public class Cell: INotifyPropertyChanged, INotifyPropertyChanging
    {
        private int _id;

        public int Id
        {
            get { return _id; }
            set
            {
                OnPropertyChanging("Id");
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        private int _x;

        public int X
        {
            get { return _x; }
            set
            { _x = value; }
        }

        private int _y;

        public int Y
        {
            get { return _y; }
            set { _y = value; }
        }

        private bool _isFlagged;

        public bool IsFlagged
        {
            get { return _isFlagged; }
            set
            {
                OnPropertyChanging("IsFlagged");
                _isFlagged = value;
                OnPropertyChanged("IsFlagged");
            }
        }

        private bool _isMined;

        public bool IsMined
        {
            get { return _isMined; }
            set { _isMined = value; }
        }

        private int _minesAroundCount;

        public int MinesAroundCount
        {
            get { return _minesAroundCount; }
            set { _minesAroundCount = value; }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        public event PropertyChangingEventHandler PropertyChanging;
    }
}
