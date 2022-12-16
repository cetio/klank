using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Keyse
{
    public class KeybindContr
    {
        private Dictionary<Keys, Func<object[], bool>> Binds = new Dictionary<Keys, Func<object[], bool>>();
        private Dictionary<Func<object[], bool>, object[]> Params = new Dictionary<Func<object[], bool>, object[]>();
        private Dictionary<Keys, int> Toggles = new Dictionary<Keys, int>();
        private List<Func<object[], bool>> FuncQueue = new List<Func<object[], bool>>();

        private Timer Timer = new Timer();

        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(Keys vKey);

        public KeybindContr()
        {
            Timer.Interval = 50;
            Timer.Elapsed += Elapse;
        }

        private bool IsKeyDown(Keys key)
        {
            return 0 != (GetAsyncKeyState(key) & 0x8000);
        }

        public void Start()
        {
            Timer.Start();
        }

        public void Stop()
        {
            Timer.Stop();
        }

        public bool Add(Func<object[], bool> func, Keys key, params object[] parameters)
        {
            if (key == Keys.None)
                return false;

            Binds.Add(key, func);
            Params.Add(func, parameters);
            Toggles.Add(key, 2);

            return true;
        }

        public bool Remove(Keys key)
        {
            if (Binds.ContainsKey(key))
            {
                Params.Remove(Binds[key]);
                Binds.Remove(key);
                Toggles.Remove(key);

                return true;
            }
            else
            {
                return false;
            }
        }

        private void Elapse(object sender, ElapsedEventArgs e)
        {
            foreach (Keys key in Binds.Keys)
            {
                if (IsKeyDown(key))
                {
                    if (Toggles[key] >= 2)
                        Binds[key](
                            Params[Binds[key]]
                        );

                    Toggles[key] = 0;
                }
                else
                    Toggles[key]++;
            }

            FuncQueue.Clear();
        }
    }
}
