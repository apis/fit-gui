using System;
using Gtk;
using WebKit;

public partial class MainWindow: Gtk.Window
{	
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();
	//var x = this.scrolledwindow1

		WebView webView = new WebView();
		webView.Open ("http://mono-project.com");
        this.scrolledwindow1.Add(webView);
		this.ShowAll();
	}
	
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}
}
