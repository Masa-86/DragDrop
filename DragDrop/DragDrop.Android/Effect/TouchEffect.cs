using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using DragDrop.Droid.Effect;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ResolutionGroupName("Effects")]
[assembly: ExportEffect(typeof(TouchEffect), "TouchEffect")]
namespace DragDrop.Droid.Effect
{
    public class TouchEffect : PlatformEffect
    {
        Android.Views.View view;
        Element element;
        DragDrop.View.MyEffect.TouchEffect tEffect;

        bool capture;
        int[] twoIntArray = new int[2];

        static Dictionary<Android.Views.View, TouchEffect> viewDictionary = new Dictionary<Android.Views.View, TouchEffect>();
        static Dictionary<int, TouchEffect> idToEffectDictionary = new Dictionary<int, TouchEffect>();

        protected override void OnAttached()
        {
            view = Control == null ? Container : Control;
            element = Element;
            if (view != null)
            {
                view.Touch += OnTouch;
                viewDictionary.Add(view, this);
                tEffect = (DragDrop.View.MyEffect.TouchEffect)element.Effects.FirstOrDefault(e => e is DragDrop.View.MyEffect.TouchEffect);
            }
        }

        protected override void OnDetached()
        {
            if (viewDictionary.ContainsKey(view))
            {
                view.Touch -= OnTouch;
                viewDictionary.Remove(view);
            }
        }

        private void OnTouch(object obj, Android.Views.View.TouchEventArgs ev)
        {
            Android.Views.View senderView = obj as Android.Views.View;
            senderView.GetLocationOnScreen(twoIntArray);

            // タップポインターのID
            int actID = ev.Event.ActionIndex;

            // タップした指を識別するID
            int pointID = ev.Event.GetPointerId(actID);

            // 相対座標を取得する場合
            float x = ev.Event.GetX(actID);
            float y = ev.Event.GetY(actID);

            // 絶対座標を取得する場合
            float x_Raw = ev.Event.RawX;
            float y_Raw = ev.Event.RawY;

            Point screenPointerCoords = new Point(x_Raw,
                                                  y_Raw);

            DragDrop.View.MyEffect.TouchEventArgs args;

            switch (ev.Event.Action)
            {
                case MotionEventActions.ButtonPress:
                case MotionEventActions.Down:
                case MotionEventActions.PointerDown:

                    if(!idToEffectDictionary.ContainsKey(pointID))
                    {
                        idToEffectDictionary.Add(pointID, this);
                        FireEvent(this, pointID, View.MyEffect.TouchEventArgs.TouchEventType.Pressed, screenPointerCoords, true);
                    }

                    break;

                case MotionEventActions.ButtonRelease:
                case MotionEventActions.Up:
                case MotionEventActions.Pointer1Up:

                    if (idToEffectDictionary[pointID] != null)
                    {
                        FireEvent(this, pointID, View.MyEffect.TouchEventArgs.TouchEventType.Released, screenPointerCoords, false);
                        idToEffectDictionary.Remove(pointID);
                    }

                    break;

                case MotionEventActions.Move:
                    // Moveイベントは複数同時に取得されるため、ループで処理する
                    for(var i = 0; i < ev.Event.PointerCount;i++)
                    {
                        pointID = ev.Event.GetPointerId(actID);

                        if (idToEffectDictionary[pointID] != null)
                        {
                            FireEvent(this, pointID, View.MyEffect.TouchEventArgs.TouchEventType.Moved, screenPointerCoords, true);
                        }

                    }

                    break;

                case MotionEventActions.Cancel:

                    if (idToEffectDictionary[pointID] != null)
                    {
                        FireEvent(this, pointID, View.MyEffect.TouchEventArgs.TouchEventType.Released, screenPointerCoords, false);
                    }

                    idToEffectDictionary.Remove(pointID);

                    break;
            }
        }

        private void FireEvent(TouchEffect touchEffect, int id, DragDrop.View.MyEffect.TouchEventArgs.TouchEventType eventType, Point pointerLocation, bool isInContact)
        {
            var args = new DragDrop.View.MyEffect.TouchEventArgs(id, eventType, pointerLocation, isInContact);
            tEffect.OnTouchEvent(element, args);
        }
    }
}