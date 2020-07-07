﻿#if NETCOREAPP
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using WUX = Windows.UI.Xaml;

namespace Uno.UI.Skia.Platform.GTK
{
	public class GtkHost
	{
		private Func<WUX.Application> _appBuilder;
		private static Gtk.Window _window;

		public static Gtk.Window Window => _window;

		public GtkHost(Func<WUX.Application> appBuilder, string[] args)
		{
			_appBuilder = appBuilder;
		}

		public void Run()
		{
			Gtk.Application.Init();

			_window = new Gtk.Window("Uno Host");
			_window.SetDefaultSize(1024, 800);
			_window.SetPosition(Gtk.WindowPosition.Center);

			_window.DeleteEvent += delegate
			{
				Gtk.Application.Quit();
			};

			Windows.UI.Core.CoreDispatcher.DispatchOverride
			   = d =>
			   {
				   if (Gtk.Application.EventsPending())
				   {
					   Gtk.Application.RunIteration(false);
				   }

				   Gtk.Application.Invoke((s, e) =>
				   {

					   Console.WriteLine("iteration");

					   d();
				   });
			   };

			_window.Realized += (s, e) =>
			{
				WUX.Window.Current.OnNativeSizeChanged(new Windows.Foundation.Size(_window.AllocatedWidth, _window.AllocatedHeight));
			};

			_window.SizeAllocated += (s, e) =>
			{
				WUX.Window.Current.OnNativeSizeChanged(new Windows.Foundation.Size(e.Allocation.Width, e.Allocation.Height));
			};

			var area = new UnoDrawingArea();
			_window.Add(area);

			/* avoids double invokes at window level */
			area.AddEvents((int)(
				Gdk.EventMask.PointerMotionMask
			 | Gdk.EventMask.ButtonPressMask
			 | Gdk.EventMask.ButtonReleaseMask
			));

			_window.ShowAll();

			WUX.Application.Start(_ => _appBuilder());

			Gtk.Application.Run();
		}
	}
}
#endif
