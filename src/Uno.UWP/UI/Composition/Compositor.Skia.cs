
using SkiaSharp;
using System.Diagnostics;
using System.Numerics;

namespace Windows.UI.Composition
{
	public partial class Compositor
	{
		internal ContainerVisual RootVisual { get; set; }

		//public CompositionSurfaceBrush CreateSurfaceBrush()
		//{
		//	throw new global::System.NotImplementedException("The member CompositionSurfaceBrush Compositor.CreateSurfaceBrush() is not implemented in Uno.");
		//}

		//public CompositionSurfaceBrush CreateSurfaceBrush(ICompositionSurface surface)
		//{
		//	throw new global::System.NotImplementedException("The member CompositionSurfaceBrush Compositor.CreateSurfaceBrush(ICompositionSurface surface) is not implemented in Uno.");
		//}

		internal void Render(SKSurface surface, SKImageInfo info)
		{
			var sw = Stopwatch.StartNew();

			if (RootVisual != null)
			{
				foreach (var visual in RootVisual.Children)
				{
					RenderVisual(surface, info, visual);
				}
			}

			sw.Stop();

			// global::System.Console.WriteLine($"Render time {sw.Elapsed}");
		}

		private static void RenderVisual(SKSurface surface, SKImageInfo info, Visual visual)
		{
			if (visual.Opacity != 0)
			{
				surface.Canvas.Save();

				var visualMatrix = surface.Canvas.TotalMatrix;

				SKMatrix.PreConcat(ref visualMatrix, SKMatrix.MakeTranslation(visual.Offset.X, visual.Offset.Y));

				if (visual.RotationAngleInDegrees != 0)
				{
					SKMatrix.PreConcat(ref visualMatrix, SKMatrix.MakeRotationDegrees(visual.RotationAngleInDegrees, visual.CenterPoint.X, visual.CenterPoint.Y));
				}

				if (visual.TransformMatrix != Matrix4x4.Identity)
				{
					SKMatrix.PreConcat(ref visualMatrix, visual.TransformMatrix.ToSKMatrix44().Matrix);
				}

				surface.Canvas.SetMatrix(visualMatrix);

				visual.Render(surface, info);

				switch (visual)
				{
					case SpriteVisual spriteVisual:
						foreach (var inner in spriteVisual.Children)
						{
							RenderVisual(surface, info, inner);
						}
						break;

					case ContainerVisual containerVisual:
						foreach (var inner in containerVisual.Children)
						{
							RenderVisual(surface, info, inner);
						}
						break;
				}

				surface.Canvas.Restore();
			}
		}
	}
}
