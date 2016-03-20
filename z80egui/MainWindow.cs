using System;
using Xwt;
using z80egui.Widgets;

namespace z80egui
{
    public class MainWindow : Window
    {
        public ScreenWidget LCD { get; set; }
        public TimeSpan LastTick { get; set; }

        public MainWindow()
        {
            Title = "z80e";
            Closed += (sender, e) => Application.Exit();
            var box = new VBox();
            LCD = new ScreenWidget();
            LCD.WidthRequest = 96 * 4;
            LCD.HeightRequest = 64 * 4;
            box.PackStart(LCD);
            Content = box;
            LastTick = Program.Stopwatch.Elapsed;
            Application.TimeoutInvoke(1000 / 60, Tick);
        }

        public bool Tick()
        {
            var now = Program.Stopwatch.Elapsed;
            int cycles = (int)((now - LastTick).TotalSeconds * 15000000.0);
            Console.WriteLine("Simulating {0} cycles", cycles);
            Program.Run(cycles);
            LCD.QueueDraw();
            LastTick = now;
            return true;
        }
    }
}