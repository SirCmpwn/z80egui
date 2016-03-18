using System;
using System.Runtime.InteropServices;

namespace z80egui.libscas
{
    public static unsafe partial class scas
    {
        [DllImport("libscas")]
        public static extern list_t* create_list();
        [DllImport("libscas")]
        public static extern void list_free(list_t* list);
        [DllImport("libscas")]
        public static extern void list_add(list_t* list, void* item);
        [DllImport("libscas")]
        public static extern void list_del(list_t* list, int index);
        [DllImport("libscas")]
        public static extern void list_cat(list_t* list, list_t* source);
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct list_t
    {
        public int capacity;
        public int length;
        public void** items;
    }
}