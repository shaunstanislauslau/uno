using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Uno.UI.Samples.Controls;
#if __IOS__ || __MACOS__ || __ANDROID__
using _FrameworkElement = Windows.UI.Xaml.IFrameworkElement;
#else
using _FrameworkElement = Windows.UI.Xaml.FrameworkElement;
#endif

namespace UITests.Windows_UI_Input.PointersTests
{
	[Sample("Pointers", "Image")]
	public sealed partial class HitTest_Image : Page
	{
		public HitTest_Image()
		{
			this.InitializeComponent();

			foreach (var elt in GetElements())
			{
				elt.PointerPressed += (snd, e) =>
				{
					e.Handled = true;
					LastPressed.Text = elt.Name;
					LastPressedSrc.Text = (e.OriginalSource as _FrameworkElement)?.Name ?? "-unknown-";
				};
				elt.PointerMoved += (snd, e) =>
				{
					e.Handled = true;
					LastHovered.Text = elt.Name;
					LastHoveredSrc.Text = (e.OriginalSource as _FrameworkElement)?.Name ?? "-unknown-";
				};
			}
		}

		private IEnumerable<_FrameworkElement> GetElements()
		{
			yield return Root;

			yield return TheImage;
		}
	}
}
