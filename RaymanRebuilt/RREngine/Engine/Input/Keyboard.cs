using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RREngine.Engine.Input
{
    public class Keyboard
    {
        enum KeyState
        {
            JustPressed,
            Pressed,
            JustReleased,
            Released
        }

        private KeyState[] _keyStates = new KeyState[Enum.GetValues(typeof(KeyboardKey)).Cast<int>().Max()];

        public KeyboardKey[] PressedKeys = new KeyboardKey[0];

        public Keyboard()
        {
            Viewport.Current.PreUpdate += PreUpdate;
            Viewport.Current.PostUpdate += PostUpdate;
        }

        void PreUpdate(object sender, EventArgs e)
        {
            var pressedKeys = new List<KeyboardKey>();

            for(int i = 0; i < _keyStates.Length; i++)
                if(_keyStates[i] == KeyState.JustPressed || _keyStates[i] == KeyState.Pressed)
                    pressedKeys.Add((KeyboardKey)i);

            PressedKeys = pressedKeys.ToArray();
        }

        void PostUpdate(object sender, EventArgs e)
        {
            for (int i = 0; i < _keyStates.Length; i++)
            {
                if (_keyStates[i] == KeyState.JustPressed)
                    _keyStates[i] = KeyState.Pressed;

                if (_keyStates[i] == KeyState.JustReleased)
                    _keyStates[i] = KeyState.Released;
            }
        }

        #region Events

        public event EventHandler<KeyEventArgs> KeyDown;
        public event EventHandler<KeyEventArgs> KeyUp;

        public void OnKeyDown(KeyEventArgs args)
        {
            _keyStates[(int)args.Key] = KeyState.JustPressed;

            KeyDown?.Invoke(this, args);
        }

        public void OnKeyUp(KeyEventArgs args)
        {
            _keyStates[(int)args.Key] = KeyState.JustReleased;

            KeyUp?.Invoke(this, args);
        }

        #endregion
    }
}
