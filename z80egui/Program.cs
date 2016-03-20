using System;
using Xwt;
using z80egui.libz80e;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace z80egui
{
    public static unsafe class Program
    {
        public static asic_t *ASIC { get; set; }
        public static IntPtr RunLoop { get; set; }
        public static Stopwatch Stopwatch { get; set; }

        public static void Run(int cycles)
        {
            z80e.runloop_tick_cycles(RunLoop, cycles);
        }

        private static void InitializeCalculator(string[] args)
        {
            ASIC = z80e.asic_init(TIDeviceType.TI84pSE, IntPtr.Zero); // TODO: Parse args for device type
            RunLoop = z80e.runloop_init(ASIC);
            var bytes = File.ReadAllBytes(args[0]);
            Marshal.Copy(bytes, 0, (IntPtr)ASIC->mmu.flash, bytes.Length);
        }

        public static void Main(string[] args)
        {
            if (RuntimeInfo.IsLinux)
            {
                try
                {
                    Application.Initialize(ToolkitType.Gtk3);
                }
                catch
                {
                    Application.Initialize(ToolkitType.Gtk);
                }
            }
            else if (RuntimeInfo.IsMacOSX)
                Application.Initialize(ToolkitType.Gtk); // TODO: Cocoa
            else if (RuntimeInfo.IsWindows)
                Application.Initialize(ToolkitType.Wpf);

            InitializeCalculator(args);
            Stopwatch = new Stopwatch();
            Stopwatch.Start();
            var window = new MainWindow();
            window.Show();
            Application.Run();
            window.Dispose();
        }
    }
}