using System.Runtime.CompilerServices;
using Random = Klank.Generic.Random;

namespace Klank
{
    public static class EnumerableExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining |
        MethodImplOptions.AggressiveOptimization)]
        public unsafe static int[] Sort(this int[] data)
        {
            fixed (int* pdata = data)
            {
                UnsafeQuickSort(pdata, 0, data.Length - 1);

                return data;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining |
        MethodImplOptions.AggressiveOptimization)]
        private unsafe static void UnsafeQuickSort(int* data, int left, int right)
        {
            int i = left - 1;
            int j = right;

            while (true)
            {
                int d = data[left];
                do i++; while (data[i] < d);
                do j--; while (data[j] > d);

                if (i < j)
                {
                    int tmp = data[i];
                    data[i] = data[j];
                    data[j] = tmp;
                }
                else
                {
                    if (left < j) UnsafeQuickSort(data, left, j);
                    if (j++ < right) UnsafeQuickSort(data, j, right);
                    return;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining |
        MethodImplOptions.AggressiveOptimization)]
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> arr)
        {
            Random random = new Random();
            var shuffled = arr.ToList();

            for (int index = 0; index < shuffled.Count; ++index)
            {
                var randomIndex = random.Next<int>(index, shuffled.Count - 1);
                if (index != randomIndex)
                {
                    var temp = shuffled[index];
                    shuffled[index] = shuffled[randomIndex];
                    shuffled[randomIndex] = temp;
                }
            }

            return shuffled;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining |
        MethodImplOptions.AggressiveOptimization)]
        public static T[] SubArray<T>(this T[] array, int offset, int length)
        {
            T[] result = new T[length];
            Array.Copy(array, offset, result, 0, length);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining |
        MethodImplOptions.AggressiveOptimization)]
        public static int SearchBytes(this byte[] haystack, byte[] needle)
        {
            var len = needle.Length;
            var limit = haystack.Length - len;
            for (var i = 0; i <= limit; i++)
            {
                var k = 0;
                for (; k < len; k++)
                {
                    if (needle[k] != haystack[i + k]) break;
                }
                if (k == len) return i;
            }
            return -1;
        }
    }
}
