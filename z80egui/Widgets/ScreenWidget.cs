using System;
using Xwt;
using Xwt.Drawing;
using z80egui.libz80e;

namespace z80egui.Widgets
{
    public class ScreenWidget : Canvas
    {
        private readonly Color PixelOff = Color.FromBytes(0x99, 0xB1, 0x99);
        private readonly Color PixelOn = Colors.Black;

        public ScreenWidget()
        {
        }

        protected override unsafe void OnDraw(Context ctx, Rectangle dirtyRect)
        {
            var pixelWidth = Size.Width / 96;
            var pixelHeight = Size.Height / 64;
            ctx.SetColor(PixelOff);
            ctx.Rectangle(0, 0, Size.Width, Size.Height);
            ctx.Fill();
            ctx.SetColor(PixelOn);
            for (int x = 0; x < 96; x++)
            {
                for (int y = 0; y < 64; y++)
                {
                    byte val = z80e.bw_lcd_read_screen(Program.ASIC->cpu.devices[0x10].device, x, y);
                    if (val != 0)
                    {
                        ctx.Rectangle(x * pixelWidth, y * pixelHeight, pixelWidth, pixelHeight);
                        ctx.Fill();
                    }
                }
            }
            base.OnDraw(ctx, dirtyRect);
        }
    }
}