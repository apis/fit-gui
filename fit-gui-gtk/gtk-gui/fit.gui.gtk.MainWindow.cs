
// This file has been generated by the GUI designer. Do not modify.
namespace fit.gui.gtk
{
	public partial class MainWindow
	{
		private global::Gtk.VBox vbox1;
		private global::Gtk.Alignment alignment2;
		private global::Gtk.HBox hbox1;
		private global::Gtk.Button buttonStartStop;
		private global::Gtk.Button buttonAddFolder;
		private global::Gtk.Button buttonEditFolder;
		private global::Gtk.Button buttonDeleteFolder;
		private global::Gtk.HPaned hpaned1;
		private global::Gtk.ScrolledWindow GtkScrolledWindow;
		private global::Gtk.TreeView treeview1;
		private global::Gtk.VBox vbox3;
		private global::Gtk.HBox hbox2;
		private global::Gtk.ToggleButton togglebuttonShowSpecification;
		private global::Gtk.ToggleButton togglebuttonShowResult;
		private global::Gtk.ScrolledWindow scrolledwindow1;
		private global::Gtk.Alignment alignment1;
		private global::Gtk.ProgressBar progressbarMain;
		
		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget fit.gui.gtk.MainWindow
			this.Name = "fit.gui.gtk.MainWindow";
			this.Title = global::Mono.Unix.Catalog.GetString ("MainWindow");
			this.WindowPosition = ((global::Gtk.WindowPosition)(4));
			// Container child fit.gui.gtk.MainWindow.Gtk.Container+ContainerChild
			this.vbox1 = new global::Gtk.VBox ();
			this.vbox1.Name = "vbox1";
			this.vbox1.Spacing = 6;
			// Container child vbox1.Gtk.Box+BoxChild
			this.alignment2 = new global::Gtk.Alignment (0.5F, 0.5F, 1F, 1F);
			this.alignment2.Name = "alignment2";
			this.alignment2.LeftPadding = ((uint)(5));
			this.alignment2.TopPadding = ((uint)(5));
			this.alignment2.RightPadding = ((uint)(5));
			// Container child alignment2.Gtk.Container+ContainerChild
			this.hbox1 = new global::Gtk.HBox ();
			this.hbox1.Name = "hbox1";
			this.hbox1.Spacing = 6;
			// Container child hbox1.Gtk.Box+BoxChild
			this.buttonStartStop = new global::Gtk.Button ();
			this.buttonStartStop.WidthRequest = 80;
			this.buttonStartStop.CanFocus = true;
			this.buttonStartStop.Name = "buttonStartStop";
			this.buttonStartStop.UseUnderline = true;
			// Container child buttonStartStop.Gtk.Container+ContainerChild
			global::Gtk.Alignment w1 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w2 = new global::Gtk.HBox ();
			w2.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w3 = new global::Gtk.Image ();
			w2.Add (w3);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w5 = new global::Gtk.Label ();
			w5.LabelProp = global::Mono.Unix.Catalog.GetString ("Start");
			w5.UseUnderline = true;
			w2.Add (w5);
			w1.Add (w2);
			this.buttonStartStop.Add (w1);
			this.hbox1.Add (this.buttonStartStop);
			global::Gtk.Box.BoxChild w9 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.buttonStartStop]));
			w9.Position = 0;
			w9.Expand = false;
			w9.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.buttonAddFolder = new global::Gtk.Button ();
			this.buttonAddFolder.WidthRequest = 80;
			this.buttonAddFolder.CanFocus = true;
			this.buttonAddFolder.Name = "buttonAddFolder";
			this.buttonAddFolder.UseUnderline = true;
			// Container child buttonAddFolder.Gtk.Container+ContainerChild
			global::Gtk.Alignment w10 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w11 = new global::Gtk.HBox ();
			w11.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w12 = new global::Gtk.Image ();
			w11.Add (w12);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w14 = new global::Gtk.Label ();
			w14.LabelProp = global::Mono.Unix.Catalog.GetString ("GtkButton");
			w14.UseUnderline = true;
			w11.Add (w14);
			w10.Add (w11);
			this.buttonAddFolder.Add (w10);
			this.hbox1.Add (this.buttonAddFolder);
			global::Gtk.Box.BoxChild w18 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.buttonAddFolder]));
			w18.Position = 1;
			w18.Expand = false;
			w18.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.buttonEditFolder = new global::Gtk.Button ();
			this.buttonEditFolder.WidthRequest = 80;
			this.buttonEditFolder.CanFocus = true;
			this.buttonEditFolder.Name = "buttonEditFolder";
			this.buttonEditFolder.UseUnderline = true;
			// Container child buttonEditFolder.Gtk.Container+ContainerChild
			global::Gtk.Alignment w19 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w20 = new global::Gtk.HBox ();
			w20.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w21 = new global::Gtk.Image ();
			w20.Add (w21);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w23 = new global::Gtk.Label ();
			w23.LabelProp = global::Mono.Unix.Catalog.GetString ("GtkButton");
			w23.UseUnderline = true;
			w20.Add (w23);
			w19.Add (w20);
			this.buttonEditFolder.Add (w19);
			this.hbox1.Add (this.buttonEditFolder);
			global::Gtk.Box.BoxChild w27 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.buttonEditFolder]));
			w27.Position = 2;
			w27.Expand = false;
			w27.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.buttonDeleteFolder = new global::Gtk.Button ();
			this.buttonDeleteFolder.WidthRequest = 80;
			this.buttonDeleteFolder.CanFocus = true;
			this.buttonDeleteFolder.Name = "buttonDeleteFolder";
			this.buttonDeleteFolder.UseUnderline = true;
			// Container child buttonDeleteFolder.Gtk.Container+ContainerChild
			global::Gtk.Alignment w28 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w29 = new global::Gtk.HBox ();
			w29.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w30 = new global::Gtk.Image ();
			w29.Add (w30);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w32 = new global::Gtk.Label ();
			w32.LabelProp = global::Mono.Unix.Catalog.GetString ("GtkButton");
			w32.UseUnderline = true;
			w29.Add (w32);
			w28.Add (w29);
			this.buttonDeleteFolder.Add (w28);
			this.hbox1.Add (this.buttonDeleteFolder);
			global::Gtk.Box.BoxChild w36 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.buttonDeleteFolder]));
			w36.Position = 3;
			w36.Expand = false;
			w36.Fill = false;
			this.alignment2.Add (this.hbox1);
			this.vbox1.Add (this.alignment2);
			global::Gtk.Box.BoxChild w38 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.alignment2]));
			w38.Position = 1;
			w38.Expand = false;
			w38.Fill = false;
			// Container child vbox1.Gtk.Box+BoxChild
			this.hpaned1 = new global::Gtk.HPaned ();
			this.hpaned1.CanFocus = true;
			this.hpaned1.Name = "hpaned1";
			this.hpaned1.Position = 172;
			// Container child hpaned1.Gtk.Paned+PanedChild
			this.GtkScrolledWindow = new global::Gtk.ScrolledWindow ();
			this.GtkScrolledWindow.Name = "GtkScrolledWindow";
			this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
			this.treeview1 = new global::Gtk.TreeView ();
			this.treeview1.CanFocus = true;
			this.treeview1.Name = "treeview1";
			this.GtkScrolledWindow.Add (this.treeview1);
			this.hpaned1.Add (this.GtkScrolledWindow);
			global::Gtk.Paned.PanedChild w40 = ((global::Gtk.Paned.PanedChild)(this.hpaned1 [this.GtkScrolledWindow]));
			w40.Resize = false;
			// Container child hpaned1.Gtk.Paned+PanedChild
			this.vbox3 = new global::Gtk.VBox ();
			this.vbox3.Name = "vbox3";
			this.vbox3.Spacing = 6;
			// Container child vbox3.Gtk.Box+BoxChild
			this.hbox2 = new global::Gtk.HBox ();
			this.hbox2.Name = "hbox2";
			this.hbox2.Spacing = 6;
			// Container child hbox2.Gtk.Box+BoxChild
			this.togglebuttonShowSpecification = new global::Gtk.ToggleButton ();
			this.togglebuttonShowSpecification.WidthRequest = 180;
			this.togglebuttonShowSpecification.CanFocus = true;
			this.togglebuttonShowSpecification.Name = "togglebuttonShowSpecification";
			this.togglebuttonShowSpecification.UseUnderline = true;
			this.togglebuttonShowSpecification.Active = true;
			// Container child togglebuttonShowSpecification.Gtk.Container+ContainerChild
			global::Gtk.Alignment w41 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w42 = new global::Gtk.HBox ();
			w42.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w43 = new global::Gtk.Image ();
			w43.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-file", global::Gtk.IconSize.Menu);
			w42.Add (w43);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w45 = new global::Gtk.Label ();
			w45.LabelProp = global::Mono.Unix.Catalog.GetString ("Show specification");
			w45.UseUnderline = true;
			w42.Add (w45);
			w41.Add (w42);
			this.togglebuttonShowSpecification.Add (w41);
			this.hbox2.Add (this.togglebuttonShowSpecification);
			global::Gtk.Box.BoxChild w49 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.togglebuttonShowSpecification]));
			w49.Position = 0;
			w49.Expand = false;
			w49.Fill = false;
			// Container child hbox2.Gtk.Box+BoxChild
			this.togglebuttonShowResult = new global::Gtk.ToggleButton ();
			this.togglebuttonShowResult.WidthRequest = 180;
			this.togglebuttonShowResult.CanFocus = true;
			this.togglebuttonShowResult.Name = "togglebuttonShowResult";
			this.togglebuttonShowResult.UseUnderline = true;
			// Container child togglebuttonShowResult.Gtk.Container+ContainerChild
			global::Gtk.Alignment w50 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w51 = new global::Gtk.HBox ();
			w51.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w52 = new global::Gtk.Image ();
			w52.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-revert-to-saved", global::Gtk.IconSize.Menu);
			w51.Add (w52);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w54 = new global::Gtk.Label ();
			w54.LabelProp = global::Mono.Unix.Catalog.GetString ("Show result");
			w54.UseUnderline = true;
			w51.Add (w54);
			w50.Add (w51);
			this.togglebuttonShowResult.Add (w50);
			this.hbox2.Add (this.togglebuttonShowResult);
			global::Gtk.Box.BoxChild w58 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.togglebuttonShowResult]));
			w58.Position = 1;
			w58.Expand = false;
			w58.Fill = false;
			this.vbox3.Add (this.hbox2);
			global::Gtk.Box.BoxChild w59 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.hbox2]));
			w59.Position = 0;
			w59.Expand = false;
			w59.Fill = false;
			// Container child vbox3.Gtk.Box+BoxChild
			this.scrolledwindow1 = new global::Gtk.ScrolledWindow ();
			this.scrolledwindow1.CanFocus = true;
			this.scrolledwindow1.Name = "scrolledwindow1";
			this.scrolledwindow1.ShadowType = ((global::Gtk.ShadowType)(1));
			this.vbox3.Add (this.scrolledwindow1);
			global::Gtk.Box.BoxChild w60 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.scrolledwindow1]));
			w60.Position = 1;
			this.hpaned1.Add (this.vbox3);
			this.vbox1.Add (this.hpaned1);
			global::Gtk.Box.BoxChild w62 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.hpaned1]));
			w62.Position = 2;
			// Container child vbox1.Gtk.Box+BoxChild
			this.alignment1 = new global::Gtk.Alignment (0.5F, 0.5F, 1F, 1F);
			this.alignment1.Name = "alignment1";
			this.alignment1.LeftPadding = ((uint)(4));
			this.alignment1.RightPadding = ((uint)(4));
			this.alignment1.BottomPadding = ((uint)(6));
			// Container child alignment1.Gtk.Container+ContainerChild
			this.progressbarMain = new global::Gtk.ProgressBar ();
			this.progressbarMain.Name = "progressbarMain";
			this.progressbarMain.Text = global::Mono.Unix.Catalog.GetString ("zxczxczxc");
			this.alignment1.Add (this.progressbarMain);
			this.vbox1.Add (this.alignment1);
			global::Gtk.Box.BoxChild w64 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.alignment1]));
			w64.Position = 3;
			w64.Expand = false;
			w64.Fill = false;
			this.Add (this.vbox1);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.DefaultWidth = 580;
			this.DefaultHeight = 315;
			this.Hide ();
			this.DeleteEvent += new global::Gtk.DeleteEventHandler (this.OnDeleteEvent);
			this.buttonStartStop.Clicked += new global::System.EventHandler (this.OnButtonStartStopClicked);
			this.buttonAddFolder.Clicked += new global::System.EventHandler (this.OnButtonAddFolderClicked);
			this.buttonEditFolder.Clicked += new global::System.EventHandler (this.OnButtonAddFolderClicked);
			this.buttonDeleteFolder.Clicked += new global::System.EventHandler (this.OnButtonAddFolderClicked);
			this.togglebuttonShowSpecification.Clicked += new global::System.EventHandler (this.OnTogglebuttonShowSpecificationClicked);
			this.togglebuttonShowResult.Clicked += new global::System.EventHandler (this.OnTogglebuttonShowResultClicked);
		}
	}
}
