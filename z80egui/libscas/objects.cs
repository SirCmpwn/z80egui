using System;
using System.Runtime.InteropServices;

namespace z80egui.libscas
{
    public enum SymbolType
    {
        Label = 0,
        Equate = 1
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct object_t
    {
        public list_t *areas;
        public list_t *exports;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct area_t
    {
        public char *name;
        public list_t *late_immediates;
        public list_t *symbols;
        public list_t *source_map;
        public list_t *metadata;
        public byte *data;
        public ulong data_length;
        public ulong data_capacity;
        public ulong final_address;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct source_map_t
    {
        public char *file_name;
        public list_t *entries;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct source_map_entry_t
    {
        public ulong line_number;
        public ulong address;
        public ulong length;
        public char *source_code;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct late_immediate_t
    {
        public IntPtr expression; // TODO: tokenized_expression_t
        public ulong width;
        public ulong address;
        public ulong instruction_address;
        public ulong base_address;
        public int type;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct symbol_t
    {
        public SymbolType type
        {
            get
            {
                return (SymbolType)_type;
            }
            set
            {
                _type = (int)value;
            }
        }

        public int _type;
        public char *name;
        public ulong value;
        public ulong defined_address;
        public int exported;
    }
}