using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using DragDrop.iOS.Effect;

[assembly: ResolutionGroupName("Effects")]
[assembly: ExportEffect(typeof(TouchEffect), "TouchEffect")]
namespace DragDrop.iOS.Effect
{
    public class TouchEffect : PlatformEffect
    {
        UIView view;
        TouchRecognizer touchRecognizer;

        /// <summary>
        /// イベントのアタッチ
        /// </summary>
        protected override void OnAttached()
        {
            view = Control == null ? Container : Control;
            view.UserInteractionEnabled = true;

            DragDrop.View.MyEffect.TouchEffect effect = (DragDrop.View.MyEffect.TouchEffect)Element.Effects.FirstOrDefault(e => e is DragDrop.View.MyEffect.TouchEffect);

            if (effect != null && view != null)
            {
                touchRecognizer = new TouchRecognizer(Element, view, effect);

                view.AddGestureRecognizer(touchRecognizer);
            }
        }

        /// <summary>
        /// イベントのデタッチ
        /// </summary>
        protected override void OnDetached()
        {
            if (touchRecognizer != null)
            {

                touchRecognizer.Detach();
                view.RemoveGestureRecognizer(touchRecognizer);
            }
        }
    }
}