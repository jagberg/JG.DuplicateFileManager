using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace JG.Duplicates.Client.Behaviours
{
    public class FrameworkElementDragBehavior : Behavior<FrameworkElement>
    {
        private bool isMouseClicked = true;

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.MouseDown +=  new MouseButtonEventHandler(AssociatedObject_MouseDown);
            this.AssociatedObject.MouseLeftButtonDown += new MouseButtonEventHandler(AssociatedObject_MouseLeftButtonDown);
            this.AssociatedObject.MouseLeftButtonUp += new MouseButtonEventHandler(AssociatedObject_MouseLeftButtonUp);
            this.AssociatedObject.MouseLeave += new MouseEventHandler(AssociatedObject_MouseLeave);
        }

        void AssociatedObject_MouseDown(object sender, MouseButtonEventArgs e)
        {
            isMouseClicked = true;
            //throw new NotImplementedException();
        }

        void AssociatedObject_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isMouseClicked = true;
        }

        void AssociatedObject_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isMouseClicked = false;
        }

        void AssociatedObject_MouseLeave(object sender, MouseEventArgs e)
        {
            if (isMouseClicked)
            //if (e.LeftButton == MouseButtonState.Pressed)
            {
                //set the item's DataContext as the data to be transferred
                IDraggable dragObject = this.AssociatedObject.DataContext as IDraggable;
                if (dragObject != null && dragObject.DragItem != null)
                {
                    DataObject data = new DataObject();
                    data.SetData(dragObject.DragItem.GetType(), dragObject.DragItem);
                    System.Windows.DragDrop.DoDragDrop(this.AssociatedObject, data, DragDropEffects.Move);
                }
            }
            isMouseClicked = false;
        }
    }
}
