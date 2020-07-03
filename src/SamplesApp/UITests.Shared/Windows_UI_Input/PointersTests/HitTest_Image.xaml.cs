using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Uno.UI.Samples.Controls;

namespace UITests.Windows_UI_Input.PointersTests
{
	[Sample("Pointers", "Image")]
	public sealed partial class HitTest_Image : Page
	{
		public HitTest_Image()
		{
			this.InitializeComponent();
#if __WASM__
			foreach (var elt in GetElements())
			{
				elt.PointerPressed += (snd, e) =>
				{
					e.Handled = true;
					LastPressed.Text = elt.Name;
					LastPressedSrc.Text = (e.OriginalSource as FrameworkElement)?.Name ?? "-unknown-";
				};
				elt.PointerMoved += (snd, e) =>
				{
					e.Handled = true;
					LastHovered.Text = elt.Name;
					LastHoveredSrc.Text = (e.OriginalSource as FrameworkElement)?.Name ?? "-unknown-";
				};
			}
#endif
		}

		private IEnumerable<FrameworkElement> GetElements()
		{
			yield return Root;

			yield return TheImage;
		}
	}
}
