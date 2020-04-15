using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DragDrop.View.MyEffect
{
    public class TouchEventArgs
    {
        public enum TouchEventType
        {
            Entered,
            Pressed,
            Moved,
            Released,
            Exited,
            Cancelled
        }

        public long Id { get; set; }
        public TouchEventType Type { get; set; }
        public Point MyPoint { get; set; }

        public bool IsInContact { private set; get; }

        public TouchEventArgs(long id, TouchEventType type, float x, float y, bool isInContact)
        {
            Id = id;
            Type = type;
            MyPoint = new Point(x, y);
            IsInContact = isInContact;
        }

        public TouchEventArgs(long id, TouchEventType type, Point point, bool isInContact)
        {
            Id = id;
            Type = type;
            MyPoint = point;
            IsInContact = isInContact;
        }
    }
}
