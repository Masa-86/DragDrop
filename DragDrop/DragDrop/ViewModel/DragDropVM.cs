using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

using Xamarin.Forms;

using System.ComponentModel;

namespace DragDrop.ViewModel
{
    public class DragDropVM : INotifyPropertyChanged
    {
        public ObservableCollection<DragDropItem> DragDropItems { get; set; }

        public Command Load;
        public Command AddButtonClick;
        public Command CmdDragDrop;

        public event PropertyChangedEventHandler PropertyChanged;

        public double StartPointX { get; set; }
        public double StartPointY { get; set; }
        public DragDropItem StartRowItem { get; set; }

        public double EndPointX { get; set; }
        public double EndPointY { get; set; }
        public DragDropItem EndRowItem { get; set; }


        public int DragRowUnit { get; set; }

        public DragDropVM()
        {
            Load = new Command(() => Loading());
            CmdDragDrop = new Command(() => ExecDragDrop());
            DragDropItems = new ObservableCollection<DragDropItem>();

            for(var i = 0; i < 5; i++)
            {
                var item = new DragDropItem(i + 1, "item" + (i+1).ToString());
                DragDropItems.Add(item);
            }
        }

        private void Loading()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DragDropItems"));
        }

        private void ExecDragDrop()
        {
            var dragY = EndPointY - StartPointY;
            var moveRow = 0;
            var startIndex = 0;

            if (dragY >= DragRowUnit / 2 && dragY < DragRowUnit)
            {
                moveRow = 1;
            }
            else if (dragY < (-1 * DragRowUnit) / 2 && dragY >= (-1 * DragRowUnit))
            {
                moveRow = -1;
            }
            else if(dragY > DragRowUnit * (DragDropItems.Count + 1))
            {
                moveRow = DragDropItems.Count - startIndex;
            }
            else
            {
                moveRow = Convert.ToInt32(dragY / DragRowUnit);
            }

            for(var i = 0; i < DragDropItems.Count; i++)
            {
                if(DragDropItems[i] == StartRowItem)
                {
                    startIndex = i;
                }
            }
            var endIndex = startIndex + moveRow;

            if(startIndex < 0 || startIndex >= DragDropItems.Count)
            {
                return;
            }

            if (endIndex < 0 || endIndex >= DragDropItems.Count)
            {
                return;
            }

            DragDropItems.Move(startIndex, endIndex);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DragDropItems"));
        }
    }
}
