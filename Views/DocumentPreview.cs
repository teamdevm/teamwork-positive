using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Skia;
using SkiaSharp;
using System;
using Aspose.Words;

namespace Documently.Views;

class DocumentRenderer : ICustomDrawOperation
{
    public Document? Target { get; set; }
    public Rect Bounds { get; set; }

    public DocumentRenderer()
    {
        Target = null;
    }

    public void Dispose () {}

    public bool Equals (ICustomDrawOperation? other) => other == this;

    // not sure what goes here....
    public bool HitTest (Point p) => false;

    public void Render (IDrawingContextImpl context)
    {
        if (context is ISkiaDrawingContextImpl skia)
        {
            Render(skia.SkCanvas);
        }

        // var canvas = (context as ISkiaDrawingContextImpl)?.SkCanvas;

        // if (canvas is not null)
        // {
        //     Render(canvas);
        // }
    }

    private void Render (SKCanvas canvas)
    {
        if (Target is not null)
        {
            float offX = (float)Bounds.X;
            float offY = (float)Bounds.Y;
            float width = (float)Bounds.Width;
            float offset = 0;

            SKPaint paint = new SKPaint()
            {
                Color = new SKColor(128, 128, 128)                
            };

            for (int i = 0; i < Target.PageCount; ++i)
            {
                offset = offY + i * width * MathF.Sqrt(2);

                Target.RenderToSize(
                    i, canvas,
                    offX, offset,
                    width, float.MaxValue
                );

                if (i > 0)
                {
                    canvas.DrawLine(
                        offX, offset,
                        offX + width, offset,
                        paint
                    );
                }
            }
        }
    }
}

public partial class DocumentPreview : UserControl
{
    private DocumentRenderer renderingLogic;
    private int pageCount;

    public DocumentPreview()
    {
        renderingLogic = new DocumentRenderer();
        this.DataContextChanged += UpdateDocument;
        this.EffectiveViewportChanged += UpdateBounds;
    }

    public override void Render (DrawingContext context)
    {
        context.Custom(renderingLogic);

        // If you want continual invalidation (like a game):
        //Dispatcher.UIThread.InvokeAsync(InvalidateVisual, DispatcherPriority.Background);
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        return new Size(availableSize.Width, pageCount * availableSize.Width * Math.Sqrt(2));
    }

    private void UpdateBounds (object? sender, EventArgs args)
    {
        renderingLogic.Bounds = new Rect(0, 0, this.Bounds.Width, this.Bounds.Height);
    }

    private void UpdateDocument (object? sender, EventArgs args)
    {
        if (DataContext is Document d)
        {
            renderingLogic.Target = d;
            pageCount = d.PageCount;
            InvalidateVisual();
        }
    }
}