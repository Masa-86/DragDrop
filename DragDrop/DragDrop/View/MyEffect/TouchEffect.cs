using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using static DragDrop.View.MyEffect.TouchEvent;

namespace DragDrop.View.MyEffect
{
    public class TouchEffect : RoutingEffect
    {
        public TouchEffect() : base("Effects.TouchEffect")
        {

        }

        public event TouchEventHundler OnTouch;

        public void OnTouchEvent(object obj, TouchEventArgs args)
        {
            OnTouch?.Invoke(obj, args);
        }
    }
}
