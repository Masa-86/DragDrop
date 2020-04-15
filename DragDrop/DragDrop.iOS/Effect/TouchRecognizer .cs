using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Forms;

namespace DragDrop.iOS.Effect
{
    public class TouchRecognizer : UIGestureRecognizer
    {
        private Element element;        // Forms element for firing events
        private UIView view;            // iOS UIView 
        private DragDrop.View.MyEffect.TouchEffect touchEffect;

        static Dictionary<UIView, TouchRecognizer> viewDictionary =
            new Dictionary<UIView, TouchRecognizer>();

        static Dictionary<long, TouchRecognizer> idToTouchDictionary =
            new Dictionary<long, TouchRecognizer>();

        public TouchRecognizer(Element element, UIView view, DragDrop.View.MyEffect.TouchEffect touchEffect)
        {
            this.element = element;
            this.view = view;
            this.touchEffect = touchEffect;

            viewDictionary.Add(view, this);
        }

        public void Detach()
        {
            viewDictionary.Remove(view);
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);

            foreach (UITouch touch in touches.Cast<UITouch>())
            {
                long id = touch.Handle.ToInt64();
                FireEvent(this, id, DragDrop.View.MyEffect.TouchEventArgs.TouchEventType.Pressed, touch, true);

                if (!idToTouchDictionary.ContainsKey(id))
                {
                    idToTouchDictionary.Add(id, this);
                }
            }
        }

        public override void TouchesMoved(NSSet touches, UIEvent evt)
        {
            base.TouchesMoved(touches, evt);

            foreach (UITouch touch in touches.Cast<UITouch>())
            {
                long id = touch.Handle.ToInt64();

                FireEvent(idToTouchDictionary[id], id, DragDrop.View.MyEffect.TouchEventArgs.TouchEventType.Moved, touch, true);
            }
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);

            foreach (UITouch touch in touches.Cast<UITouch>())
            {
                long id = touch.Handle.ToInt64();

                FireEvent(idToTouchDictionary[id], id, DragDrop.View.MyEffect.TouchEventArgs.TouchEventType.Released, touch, false);

                idToTouchDictionary.Remove(id);
            }
        }

        public override void TouchesCancelled(NSSet touches, UIEvent evt)
        {
            base.TouchesCancelled(touches, evt);

            foreach (UITouch touch in touches.Cast<UITouch>())
            {
                long id = touch.Handle.ToInt64();

                if (idToTouchDictionary[id] != null)
                {
                    FireEvent(idToTouchDictionary[id], id, DragDrop.View.MyEffect.TouchEventArgs.TouchEventType.Released, touch, false);
                }
                idToTouchDictionary.Remove(id);
            }
        }

        private void FireEvent(TouchRecognizer recognizer, long id, DragDrop.View.MyEffect.TouchEventArgs.TouchEventType actionType, UITouch touch, bool isInContact)
        {
            // Convert touch location to Xamarin.Forms Point value
            CGPoint cgPoint = touch.LocationInView(recognizer.View);
            Point xfPoint = new Point(cgPoint.X, cgPoint.Y);

            // Get the method to call for firing events
            Action<Element, DragDrop.View.MyEffect.TouchEventArgs> onTouchAction = recognizer.touchEffect.OnTouchEvent;

            // Call that method
            onTouchAction(recognizer.element,
                new DragDrop.View.MyEffect.TouchEventArgs(id, actionType, xfPoint, isInContact));
        }
    }
}