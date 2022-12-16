using BenchmarkDotNet.Attributes;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Klank.Generic
{
    public unsafe static class KlankHash
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining |
        MethodImplOptions.AggressiveOptimization)]
        public static long Hash(this object obj)
        {
            TypedReference tRef = __makeref(obj);
            nint rPtr = **(nint**)&tRef + 8;

            int size = Marshal.SizeOf(obj);
            Span<byte> bytes = new((void*)rPtr, size);
            long rhash = size;

            for (int i = 0; i < size; i++)
            {
                if (bytes[i] is 0)
                    continue;

                int mul = 1 - ((bytes[i] << 2) * 32 | i);
                rhash *= mul;
                rhash += mul;
            }

            return rhash;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining |
        MethodImplOptions.AggressiveOptimization)]
        public static long Hash(this string str)
        {
            fixed (char* sPtr = str)
            {
                int size = str.Length * 2;
                Span<byte> bytes = new(sPtr, size);
                long rhash = size;

                for (int i = 0; i < size; i++)
                {
                    if (bytes[i] is 0)
                        continue;

                    int mul = 1 - ((bytes[i] << 2) * 32 | i);
                    rhash *= mul;
                    rhash += mul;
                }

                return rhash;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining |
        MethodImplOptions.AggressiveOptimization)]
        public static double HashSE(this object obj, int seed = 0x2A947)
        {
            TypedReference tRef = __makeref(obj);
            nint rPtr = **(nint**)&tRef + 8;

            int size = Marshal.SizeOf(obj);
            Span<byte> bytes = new((void*)rPtr, size);
            long rhash = 1;

            for (int i = 0; i < size; i++)
            {
                if (bytes[i] is 0)
                    continue;

                int mul = 1 - ((bytes[i] << 6) * seed | i);
                rhash *= mul;
                rhash += mul;
            }

            return Math.SCos(rhash ^ size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining |
        MethodImplOptions.AggressiveOptimization)]
        public static double HashSE(this string str, int seed = 0x2A947)
        {
            fixed (char* sPtr = str)
            {
                int size = str.Length * 2;
                Span<byte> bytes = new(sPtr, size);
                long rhash = 1;

                for (int i = 0; i < size; i++)
                {
                    if (bytes[i] is 0)
                        continue;

                    int mul = 1 - ((bytes[i] << 6) * seed | i);
                    rhash *= mul;
                    rhash += mul;
                }

                return Math.SCos(rhash ^ size);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining |
        MethodImplOptions.AggressiveOptimization)]
        public static string HashToAlpha(this object hash)
        {
            string base_ = string.Empty;
            TypedReference tRef = __makeref(hash);
            nint rPtr = **(nint**)&tRef + 8;

            int size = Marshal.SizeOf(hash);
            Span<byte> bytes = new((void*)rPtr, size);

            for (int i = 0; i < size; i++)
            {
                base_ += (char)(65 + bytes[i] % 25);

                if (i < size - 1)
                    base_ += (char)(65 + (bytes[i] ^ bytes[i + 1]) % 25);
                else
                    base_ += (char)(65 + (bytes[i] ^ bytes[0]) % 25);
            }

            return base_;
        }

        [GlobalSetup]
        public static void Benchmark()
        {
            //BenchmarkRunner.Run<CollisionTester>();
            _ = new CollisionTester();
        }
    }

    public unsafe class CollisionTester
    {
        private Dictionary<long, object> hashes = new();
        private int collisions = 0;
        public CollisionTester()
        {
            Run();
        }

        [Benchmark]
        public void Run()
        {
            hashes.Clear();
            collisions = 0;

            foreach (string word in File.ReadAllLines("C:\\Users\\stake\\Downloads\\words.txt"))
            {
                long hash = word.Hash();

                if (!hashes.TryAdd(hash, word))
                {
                    collisions++;
                }
            }

            for (int i = short.MinValue; i < short.MaxValue; i++)
            {
                long hash = i.Hash();

                if (!hashes.TryAdd(hash, i))
                {
                    collisions++;
                }
            }

            Debug.WriteLine(collisions + " Collisions!");
        }
    }
}
