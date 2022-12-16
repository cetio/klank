using System.Reflection;
using System.Security.Cryptography;

namespace Klank.Generic
{
    public class Random
    {
        public Random()
        {

        }

        public T Next<T>(T min, T max)
        {
            if (Convert.ToSingle(min) > Convert.ToSingle(max))
            {
                T d_ = min;
                min = max;
                max = d_;
            }

            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] rand = new byte[4];
            rng.GetBytes(rand);

            float key = System.Math.Abs((float)BitConverter.ToInt16(rand, 0) / short.MaxValue);

            T val = (T)Convert.ChangeType(
                Convert.ToSingle(min) + key * (Convert.ToSingle(max) - Convert.ToSingle(min)),
                typeof(T));

            return val;
        }

        public string Next(int length, string vocab = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz")
        {
            string base_ = string.Empty;

            for (int i = 0; i < length; i++)
            {
                base_ += vocab[Next(0, vocab.Length - 1)];
            }

            return base_;
        }

        public T UniversalNext<T>(int min, int max)
        {
            T shell = Activator.CreateInstance<T>();

            foreach (FieldInfo fi in typeof(T).GetFields())
            {
                fi.SetValue((object)shell, ComplicitNext(fi.FieldType, min, max));
            }

            return shell;
        }

        public dynamic ComplicitNext(Type type, int min, int max)
        {
            if (Convert.ToSingle(min) > Convert.ToSingle(max))
            {
                dynamic d_ = min;
                min = max;
                max = d_;
            }

            if (type == typeof(string))
                return Next(max);

            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] rand = new byte[4];
            rng.GetBytes(rand);

            float key = System.Math.Abs((float)BitConverter.ToInt16(rand, 0) / short.MaxValue);

            dynamic val = Convert.ChangeType(
                Convert.ToSingle(min) + key * (Convert.ToSingle(max) - Convert.ToSingle(min)),
                type);

            return val;
        }
    }
}