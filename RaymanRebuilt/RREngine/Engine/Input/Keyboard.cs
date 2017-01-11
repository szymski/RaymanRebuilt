using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RREngine.Engine.Input
{
    public class Keyboard
    {
        private InputState[] _inputStates = new InputState[Enum.GetValues(typeof(KeyboardKey)).Cast<int>().Max()];

        public KeyboardKey[] PressedKeys = new KeyboardKey[0];

        public Keyboard()
        {
            Viewport.Current.PreUpdate += PreUpdate;
            Viewport.Current.PostUpdate += PostUpdate;
        }

        #region Getting keys

        public bool GetKey(KeyboardKey key)
            => _inputStates[(int)key] == InputState.JustPressed || _inputStates[(int)key] == InputState.Pressed;

        public bool GetKeyDown(KeyboardKey key)
            => _inputStates[(int)key] == InputState.JustPressed;

        public bool GetKeyUp(KeyboardKey key)
            => _inputStates[(int)key] == InputState.JustReleased;

        #endregion

        #region Updating

        /// <summary>
        /// Updates PressedKeys array.
        /// </summary>
        void PreUpdate(object sender, EventArgs e)
        {
            var pressedKeys = new List<KeyboardKey>();

            for (int i = 0; i < _inputStates.Length; i++)
                if (_inputStates[i] == InputState.JustPressed || _inputStates[i] == InputState.Pressed)
                    pressedKeys.Add((KeyboardKey)i);

            PressedKeys = pressedKeys.ToArray();
        }

        /// <summary>
        /// Changes key states from JustPressed to Pressed and JustReleased to released.
        /// </summary>
        void PostUpdate(object sender, EventArgs e)
        {
            for (int i = 0; i < _inputStates.Length; i++)
            {
                if (_inputStates[i] == InputState.JustPressed)
                    _inputStates[i] = InputState.Pressed;

                if (_inputStates[i] == InputState.JustReleased)
                    _inputStates[i] = InputState.Released;
            }
        }

        #endregion

        #region Events

        public event EventHandler<KeyEventArgs> KeyDown;
        public event EventHandler<KeyEventArgs> KeyUp;

        public void OnKeyDown(KeyEventArgs args)
        {
            _inputStates[(int)args.Key] = InputState.JustPressed;

            KeyDown?.Invoke(this, args);
        }

        public void OnKeyUp(KeyEventArgs args)
        {
            _inputStates[(int)args.Key] = InputState.JustReleased;

            KeyUp?.Invoke(this, args);
        }

        #endregion
    }
}
