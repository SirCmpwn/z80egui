using System;
using System.Runtime.InteropServices;

namespace z80egui.libz80e
{
    // TODO: Split this up into more files based on z80e's actual header layout
    public static unsafe partial class z80e
    {
        [DllImport("z80e")]
        public static extern asic_t *asic_init(TIDeviceType type, IntPtr log);
        [DllImport("z80e")]
        public static extern void asic_free(asic_t *asic);
        [DllImport("z80e")]
        public static extern void mmu_force_write(IntPtr mmu, ushort address, byte value);
        [DllImport("z80e")]
        public static extern byte ti_read_byte(IntPtr mmu, ushort address);
        [DllImport("z80e")]
        public static extern int cpu_execute(IntPtr cpu, int cycles);
        [DllImport("z80e")]
        public static extern IntPtr runloop_init(asic_t *asic);
        [DllImport("z80e")]
        public static extern IntPtr runloop_tick_cycles(IntPtr runloop, int cycles);
        [DllImport("z80e")]
        public static extern IntPtr runloop_tick(IntPtr runloop);
        [DllImport("z80e")]
        public static extern byte bw_lcd_read_screen(IntPtr lcd, int y, int x);
        [DllImport("z80e")]
        public static extern void depress_key(IntPtr keyboard, byte keycode);
        [DllImport("z80e")]
        public static extern void release_key(IntPtr keyboard, byte keycode);
    }

    public enum TIDeviceType
    {
        TI73 = 0,
        TI83p = 1,
        TI83pSE = 2,
        TI84p = 3,
        TI84pSE = 4,
        TI84pCSE = 5
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct z80registers_t
    {
        [FieldOffset(0)]
        public ushort AF;
        [FieldOffset(0)]
        public byte F;
        [FieldOffset(1)]
        public byte A;

        [FieldOffset(2)]
        public ushort BC;
        [FieldOffset(2)]
        public byte C;
        [FieldOffset(3)]
        public byte B;

        [FieldOffset(4)]
        public ushort DE;
        [FieldOffset(4)]
        public byte E;
        [FieldOffset(5)]
        public byte D;
        
        [FieldOffset(6)]
        public ushort HL;
        [FieldOffset(6)]
        public byte L;
        [FieldOffset(7)]
        public byte H;

        [FieldOffset(8)]
        public ushort _AF;
        [FieldOffset(10)]
        public ushort _BC;
        [FieldOffset(12)]
        public ushort _DE;
        [FieldOffset(14)]
        public ushort _HL;
        [FieldOffset(16)]
        public ushort PC;
        [FieldOffset(18)]
        public ushort SP;
        [FieldOffset(20)]
        public ushort IX;
        [FieldOffset(22)]
        public ushort IY;
        [FieldOffset(24)]
        public byte I;
        [FieldOffset(25)]
        public byte R;
        [FieldOffset(26)]
        public ushort WZ;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct z80iodevice_t
    {
        public IntPtr device;
        public IntPtr read_in;
        public IntPtr write_out;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct z80cpu_t
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=0x100)]
        public z80iodevice_t[] devices;
        public z80registers_t registers;
        public byte _status_bits;
        public byte bus;
        public ushort prefix;
        public IntPtr memory;
        public IntPtr read_byte;
        public IntPtr write_byte;
        public int interrupt;
        public IntPtr hook;
        public IntPtr log;

        public bool halted
        {
            get
            {
                return (_status_bits & 32) > 0;
            }
            set
            {
                _status_bits &= 32;
                if (value)
                    _status_bits |= 32;
            }
        }
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct ti_mmu_settings_t
    {
        public ushort ram_pages;
        public ushort flash_pages;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ti_mmu_bank_state_t
    {
        public byte page;
        public int flash;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct flash_write_t
    {
        public uint address;
        public uint address_mask;
        public byte value;
        public byte value_mask;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct ti_mmu_t
    {
        public ti_mmu_settings_t settings;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=4)]
        public ti_mmu_bank_state_t[] banks;
        public byte *ram;
        public byte *flash;
        public int flash_unlocked;
        public int flash_write_index;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=6)]
        public flash_write_t[] flash_writes;
        public IntPtr hook;
        public IntPtr log;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct asic_t
    {
        public int stopped;
        public int device;
        public int battery;
        public int battery_remove_check;
        public int clock_rate;
        public IntPtr _cpu;
        public IntPtr _runloop;
        public IntPtr _mmu;
        public IntPtr _interrupts;
        public IntPtr _timers;
        public IntPtr _hook;
        public IntPtr _log;
        public IntPtr _debugger;

        public z80cpu_t cpu
        {
            get
            {
                return Marshal.PtrToStructure<z80cpu_t>(this._cpu);
            }
            set
            {
                Marshal.StructureToPtr<z80cpu_t>(value, this._cpu, true);
            }
        }

        public ti_mmu_t mmu
        {
            get
            {
                return Marshal.PtrToStructure<ti_mmu_t>(this._mmu);
            }
        }
    }

}