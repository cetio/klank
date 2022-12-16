using System.Reflection;

namespace Klank.Generic
{
    public class Encryption
    {
        public int Key = 0;

        public Encryption(int key, int seedOffset = 6)
        {
            if (key is 0)
                throw new Exception("Key must be less or greater than 0.");

            Key = seedOffset ^ key;
        }

        public bool FileEncrypt(string path)
        {
            try
            {
                File.WriteAllText(path,
                    Encrypt(
                        File.ReadAllText(path)
                        )
                );

                return true;
            }
            catch
            {
                throw;
            }
        }

        public bool FileDecrypt(string path)
        {
            try
            {
                File.WriteAllText(path,
                    Decrypt(
                        File.ReadAllText(path)
                        )
                );

                return true;
            }
            catch
            {
                throw;
            }
        }

        public T StructEncrypt<T>(T obj)
        {
            foreach (FieldInfo fi in typeof(T).GetFields())
            {
                if (fi.IsLiteral == true)
                    continue;

                dynamic val = Convert.ChangeType(fi.GetValue(obj), fi.FieldType);
                val = Convert.ChangeType(Encrypt(val), fi.FieldType);
                fi.SetValue((object)obj, val);
            }

            return obj;
        }

        public T StructDecrypt<T>(T obj)
        {
            foreach (FieldInfo fi in typeof(T).GetFields())
            {
                if (fi.IsLiteral == true)
                    continue;

                dynamic val = Convert.ChangeType(fi.GetValue(obj), fi.FieldType);
                val = Convert.ChangeType(Decrypt(val), fi.FieldType);
                fi.SetValue((object)obj, val);
            }

            return obj;
        }

        public string Encrypt(string str)
        {
            char[] chars = str.ToCharArray();
            string fStr = string.Empty;

            for (int i = 0; i < chars.Length; i++)
                fStr += Encrypt(chars[i]);

            return fStr;
        }

        public string Decrypt(string str)
        {
            char[] chars = str.ToCharArray();
            string fStr = string.Empty;

            for (int i = 0; i < chars.Length; i++)
                fStr += Decrypt(chars[i]);

            return fStr;
        }

        public char Encrypt(char c)
            => (char)(Key - c);

        public char Decrypt(char c)
            => (char)(Key - c);

        public long Encrypt(long num)
            => num << Key;

        public long Decrypt(long num)
            => num >> Key;

        public int Encrypt(int num)
            => num << Key;

        public int Decrypt(int num)
            => num >> Key;

        public double Encrypt(double num)
            => num * Key;

        public double Decrypt(double num)
            => num / Key;

        public float Encrypt(float num)
            => num * Key;

        public float Decrypt(float num)
            => num / Key;

        public decimal Encrypt(decimal num)
            => num * Key;

        public decimal Decrypt(decimal num)
            => num / Key;

        public static string Compress(string str)
        {
            string fStr = string.Empty;
            char letter = str[0];
            int count = 1;

            for (int i = 1; i < str.Length; i++)
            {
                if (letter != str[i])
                {
                    AddCompressed(ref fStr, letter, count);

                    letter = str[i];
                    count = 1;
                }
                else
                    count++;
            }

            AddCompressed(ref fStr, letter, count);

            return fStr;
        }

        public static void AddCompressed(ref string str, char letter, int count)
        {
            str += letter;
            str += (char)(count + 0xfff);
        }

        public static string Decompress(string str)
        {
            string fStr = string.Empty;

            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] >= 0xfff)
                {
                    int count = str[i] - 1 - 0xfff;

                    while (count > 0)
                    {
                        fStr += str[i - 1];
                        count--;
                    }
                }
                else
                    fStr += str[i];
            }

            return fStr;
        }
    }
}