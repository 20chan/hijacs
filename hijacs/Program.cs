using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace hijacs {
    class Program {
        public static string Original() {
            return "Hello world";
        }

        public static string Hijack() {
            return "Hell no world";
        }
        
        static void Main(string[] args) {
            Console.WriteLine("Before hijack:");
            Console.WriteLine(Original());

            var org = typeof(Program).GetMethod(nameof(Original), BindingFlags.Public | BindingFlags.Static);
            var hij = typeof(Program).GetMethod(nameof(Hijack), BindingFlags.Public | BindingFlags.Static);

            HijackMethod(org, hij);
            After();
        }

        static void After() {
            Console.WriteLine("After hijack:");
            Console.WriteLine(Original());
        }

        static void HijackMethod(MethodBase src, MethodBase dest) {
            RuntimeHelpers.PrepareMethod(src.MethodHandle);
            RuntimeHelpers.PrepareMethod(dest.MethodHandle);

            var srcAddr = src.MethodHandle.Value;
            // var destAddr = Marshal.ReadInt32(dest.MethodHandle.Value, 8);
            var destAddr = (long)dest.MethodHandle.GetFunctionPointer();
            Marshal.WriteInt32(srcAddr, 8, (int)destAddr);
        }
    }
}
