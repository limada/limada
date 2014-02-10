using System;
using Xwt;
using Xwt.Drawing;

namespace Samples
{
	public class DragDrop: VBox
	{

	    public class MyCanvas : Canvas {
            protected override void OnMouseEntered (EventArgs args) {
                base.OnMouseEntered(args);
                if (!HasFocus)
                    SetFocus();
            }
	    }

	    Button b2;
		public DragDrop ()
		{
			HBox box = new HBox ();

            var b1 = new MyCanvas(); // new SimpleBox(30);
		    b1.BackgroundColor = Colors.Red;
		    b1.HeightRequest = 30;
		    b1.WidthRequest = 30;
		    var sw = new ScrollView(b1);
            box.PackStart(sw, false);
			
			b2 = new Button ("Drop here");
			box.PackEnd (b2);
			
			b1.ButtonPressed += delegate {
				var d = b1.CreateDragOperation ();
				d.Data.AddValue ("Hola");
				var img = Image.FromResource (GetType(), "class.png");
				d.SetDragImage (img, (int)img.Size.Width, (int)img.Size.Height);
				d.AllowedActions = DragDropAction.All;
				d.Start ();
			};
			
			b2.SetDragDropTarget (TransferDataType.Text, TransferDataType.Uri);
            b1.SetDragDropTarget(TransferDataType.Text, TransferDataType.Uri);
			PackStart (box);
			
			b2.DragDrop += HandleB2DragDrop;
			b2.DragOver += HandleB2DragOver;
            b1.DragDrop += HandleB2DragDrop;
            b1.DragOver += HandleB2DragOver;
		}

		void HandleB2DragOver (object sender, DragOverEventArgs e)
		{
			if (e.Action == DragDropAction.All)
				e.AllowedAction = DragDropAction.Move;
			else
				e.AllowedAction = e.Action;
		}

		void HandleB2DragDrop (object sender, DragEventArgs e)
		{
			Console.WriteLine ("Dropped! " + e.Action);
			Console.WriteLine ("Text: " + e.Data.GetValue (TransferDataType.Text));
			Console.WriteLine ("Uris:");
			foreach (var u in e.Data.Uris)
				Console.WriteLine ("u:" + u);
			e.Success = true;
			b2.Label = "Dropped!";
		}
	}
}

