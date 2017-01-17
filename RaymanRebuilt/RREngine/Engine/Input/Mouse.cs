using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace RREngine.Engine.Input
{
    public class Mouse
    {
        private InputState[] _buttons = new InputState[Enum.GetValues(typeof(MouseButton)).Cast<int>().Max()];

        private Vector2 _position = Vector2.Zero;
        public Vector2 Position => _position;

        private Vector2 _deltaPosition = Vector2.Zero;
        public Vector2 DeltaPosition => _deltaPosition;

        public float WheelDelta { get; private set; }

        public Mouse()
        {
            Viewport.Current.PostUpdate += PostUpdate;
        }

        #region Getting buttons

        public bool GetButton(MouseButton button)
            => _buttons[(int)button] == InputState.JustPressed || _buttons[(int)button] == InputState.Pressed;

        public bool GetButtonDown(MouseButton button)
            => _buttons[(int)button] == InputState.JustPressed;

        public bool GetButtonUp(MouseButton button)
            => _buttons[(int)button] == InputState.JustReleased;

        #endregion

        private bool _locked = false;
        public bool Locked
        {
            get { return _locked; }
            set
            {
                ChangeLockedRequested?.Invoke(this, new MouseLockedEventArgs(value));
            }
        }

        private bool _cursorVisible = false;
        public bool CursorVisible
        {
            get { return _cursorVisible; }
            set
            {
                ChangeCursorVisibleRequested?.Invoke(this, new CursorVisibleEventArgs(value));
            }
        }

        #region Updating

        /// <summary>
        /// Changes button states from JustPressed to Pressed and JustReleased to released.
        /// Also resets DeltaPosition.
        /// </summary>
        void PostUpdate(object sender, EventArgs e)
        {
            for (int i = 0; i < _buttons.Length; i++)
            {
                if (_buttons[i] == InputState.JustPressed)
                    _buttons[i] = InputState.Pressed;

                if (_buttons[i] == InputState.JustReleased)
                    _buttons[i] = InputState.Released;
            }

            _deltaPosition.X = 0;
            _deltaPosition.Y = 0;
        }

        #endregion

        #region Events

        public event EventHandler<MouseButtonEventArgs> ButtonDown;
        public event EventHandler<MouseButtonEventArgs> ButtonUp;
        public event EventHandler<MouseMoveEventArgs> Move;
        public event EventHandler<MouseWheelEventArgs> WheelChanged;
        public event EventHandler<MouseLockedEventArgs> ChangeLockedRequested;
        public event EventHandler<MouseLockedEventArgs> LockedChanged;
        public event EventHandler<CursorVisibleEventArgs> ChangeCursorVisibleRequested;
        public event EventHandler<CursorVisibleEventArgs> CursorVisibleChanged;

        public void OnButtonDown(MouseButtonEventArgs args)
        {
            _position.X = args.X;
            _position.Y = args.Y;

            _buttons[(int)args.Button] = InputState.JustPressed;

            ButtonDown?.Invoke(this, args);
        }

        public void OnButtonUp(MouseButtonEventArgs args)
        {
            _position.X = args.X;
            _position.Y = args.Y;

            _buttons[(int)args.Button] = InputState.JustReleased;

            ButtonUp?.Invoke(this, args);
        }

        public void OnMove(MouseMoveEventArgs args)
        {
            _deltaPosition.X = args.X - _position.X;
            _deltaPosition.Y = args.Y - _position.Y;

            _position.X = args.X;
            _position.Y = args.Y;

            Move?.Invoke(this, args);
        }

        public void OnWheelChanged(MouseWheelEventArgs args)
        {
            WheelDelta = args.Delta;

            WheelChanged?.Invoke(this, args);
        }

        public void OnLockedChanged(MouseLockedEventArgs args)
        {
            _locked = args.Locked;
            LockedChanged?.Invoke(this, args);
        }

        public void OnCursorVisibleChanged(CursorVisibleEventArgs args)
        {
            _cursorVisible = args.Visible;
            CursorVisibleChanged?.Invoke(this, args);
        }

        #endregion
    }
}
