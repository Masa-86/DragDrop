using System;
using System.Collections.Generic;
using System.Text;

namespace DragDrop.ViewModel
{
    public class DragDropItem
    {
        public int RowNo { get; set; }
        public string TaskValue { get; set; }

        public DragDropItem(int rowNo, string taskValue)
        {
            RowNo = rowNo;
            TaskValue = taskValue;
        }

        public static bool operator ==(DragDropItem item1, DragDropItem item2)
        {
            if(item1.RowNo != item2.RowNo)
            {
                return false;
            }

            if (item1.TaskValue != item2.TaskValue)
            {
                return false;
            }

            return true;
        }

        public static bool operator !=(DragDropItem item1, DragDropItem item2)
        {
            if (item1.RowNo == item2.RowNo)
            {
                return false;
            }

            if (item1.TaskValue == item2.TaskValue)
            {
                return false;
            }

            return true;
        }
    }
}
