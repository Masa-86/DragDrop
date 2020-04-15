using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using DragDrop.View.MyEffect;
using DragDrop.ViewModel;

namespace DragDrop
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private DragDropVM vm;
        BoxView boxView;

        public MainPage()
        {
            InitializeComponent();

            vm = new DragDropVM();
            BindingContext = vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            vm.Load.Execute(null);
        }

        private void Drag_Clicked(object sender, EventArgs e)
        {
            Console.WriteLine("Clicked");

            var a = cltDragDrop.ItemsLayout;
            var b = cltDragDrop.ItemsSource;
        }

        private void Drag_Pressed(object sender, EventArgs e)
        {
            Console.WriteLine("Pressed");
        }

        private void Drag_Released(object sender, EventArgs e)
        {
            Console.WriteLine("Released");
        }

        private void TouchEffect_OnTouch(object obj, View.MyEffect.TouchEventArgs args)
        {
            Grid myGrid;

            if (obj is Grid)
            {
                myGrid = (Grid)obj;
            }
            else
            {
                return;
            }

            vm.DragRowUnit = Convert.ToInt32(myGrid.Height);

            switch (args.Type)
            {
                case TouchEventArgs.TouchEventType.Pressed:

                    Console.WriteLine("Tapped:{0},{1}", new object[] { args.MyPoint.X, args.MyPoint.Y });

                    vm.StartPointX = args.MyPoint.X;
                    vm.StartPointY = args.MyPoint.Y;
                    vm.StartRowItem = new DragDropItem(
                        Convert.ToInt32(((Label)myGrid.Children[0]).Text),
                        ((Entry)myGrid.Children[1]).Text);

                    break;

                case TouchEventArgs.TouchEventType.Moved:

                    vm.EndPointX = args.MyPoint.X;
                    vm.EndPointY = args.MyPoint.Y;

                    break;

                case TouchEventArgs.TouchEventType.Released:

                    Console.WriteLine("Released:{0},{1}", new object[] { args.MyPoint.X, args.MyPoint.Y });

                    vm.EndPointX = args.MyPoint.X;
                    vm.EndPointY = args.MyPoint.Y;

                    if(vm.StartPointX != vm.EndPointX || vm.StartPointY != vm.EndPointY)
                    {
                        vm.CmdDragDrop.Execute(null);
                    }

                    break;
            }
        }
    }
}
