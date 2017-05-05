using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using RREngine.Engine.Graphics;

namespace RREngine.Engine.Gui.Controls
{
    public abstract class Control
    {
        #region Helper component indices

        private const int Left = 0;
        private const int Top = 1;
        private const int Right = 2;
        private const int Bottom = 3;

        #endregion

        public GuiController Controller { get; set; }

        private Control _parent = null;

        public Control Parent
        {
            get { return _parent; }
            set
            {
                _parent = value;

                if (_parent != null)
                    Controller = _parent.Controller;
            }
        }

        /// <summary>
        /// Margin applied when the panel is docked in the parent.
        /// </summary>
        public Vector4 Margin { get; set; } = Vector4.Zero;

        /// <summary>
        /// Padding applied to children.
        /// </summary>
        public Vector4 Padding { get; set; } = Vector4.Zero;

        /// <summary>
        /// Docking location of the panel.
        /// </summary>
        public Dock Dock { get; set; } = Dock.None;

        private Vector2 _position = Vector2.Zero;

        public Vector2 Position
        {
            get { return _position; }
            set
            {
                if (_position.X != value.X || _position.Y != value.Y)
                {
                    _position = value;
                    RefreshRequested = true;
                    ParentRefreshRequested = true;
                }
            }
        }

        public float X
        {
            get { return Position.X; }
            set { Position = new Vector2(value, Position.Y); }
        }

        public float Y
        {
            get { return Position.Y; }
            set { Position = new Vector2(Position.X, value); }
        }

        /// <summary>
        /// Absolute on-screen position of the panel.
        /// </summary>
        public Vector2 AbsolutePosition { get; protected set; } = Vector2.Zero;

        private Vector2 _size = new Vector2(64, 64);

        public Vector2 Size
        {
            get { return _size; }
            set
            {
                if (_size.X != value.X || _size.Y != value.Y)
                {
                    _size = value;
                    RefreshRequested = true;
                    ParentRefreshRequested = true;
                }
            }
        }

        public float Width
        {
            get { return Size.X; }
            set { Size = new Vector2(value, Size.Y); }
        }

        public float Height
        {
            get { return Size.Y; }
            set { Size = new Vector2(Size.X, value); }
        }

        /// <summary>
        /// Indicates if panel's layout isn't valid anymore.
        /// Updates the layout in the next frame if true.
        /// </summary>
        public bool RefreshRequested { get; set; } = true;

        public bool ParentRefreshRequested
        {
            get { return Parent?.RefreshRequested ?? false; }
            set
            {
                if (Parent != null)
                    Parent.RefreshRequested = value;
            }
        }

        public ObservableCollection<Control> Children { get; } = new ObservableCollection<Control>();

        public Control(Control parent = null)
        {
            Parent = parent;
            parent?.Children.Add(this);

            Children.CollectionChanged += ChildrenOnChanged;
        }

        private void ChildrenOnChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            RefreshRequested = true;

            if(args.Action == NotifyCollectionChangedAction.Add)
                foreach (var item in args.NewItems)
                    ((Control) item).Parent = this;
        }

        /// <summary>
        /// Returns a list of all children and their children and so on.
        /// </summary>
        public IEnumerable<Control> GetChildrenRecursive()
        {
            var children = Children.AsEnumerable();

            foreach (var child in Children)
                children = children.Concat(child.GetChildrenRecursive());

            return children;
        }

        public event EventHandler Think;

        public void OnThinkInternal()
        {
            OnThink();
            Think?.Invoke(this, EventArgs.Empty);

            foreach (var child in Children)
                child.OnThinkInternal();
        }

        public virtual void OnThink()
        {

        }

        public event EventHandler<IGuiRenderer> Render;

        public void OnRenderInternal(IGuiRenderer renderer)
        {
            if (RefreshRequested)
            {
                RefreshRequested = false;
                UpdateLayout();
                OnPerformLayout();
            }

            var mat = renderer.Matrix;
            renderer.Matrix *= Matrix4.CreateTranslation(new Vector3(Position));

            OnRender(renderer);
            Render?.Invoke(this, renderer);

            if (renderer.DrawDebug)
            {
                renderer.Color = new Vector4(1f, 0f, 0f, 1f);
                renderer.DrawRectangleOutline(Vector2.Zero, Size);
            }

            foreach (var child in Children)
            {
                child.OnRenderInternal(renderer);
            }

            renderer.Matrix = mat;

            // Called twice to update the layout when it changes after updating positions and sizes.
            if (RefreshRequested)
            {
                RefreshRequested = false;
                UpdateLayout();
                OnPerformLayout();
            }
        }

        public virtual void OnRender(IGuiRenderer renderer)
        {

        }

        public void UpdateLayout()
        {
            Vector4 dockOffset = Padding;

            foreach (var child in Children)
            {
                if (child.Dock == Dock.Top)
                {
                    child.Width = Width - child.Margin[Left] - child.Margin[Right] -
                        dockOffset[Left] - dockOffset[Right];

                    child.X = child.Margin[Left] + dockOffset[Left];
                    child.Y = child.Margin[Top] + dockOffset[Top];

                    dockOffset[Top] += child.Height + child.Margin[Top] + child.Margin[Bottom];
                }
                else if (child.Dock == Dock.Bottom)
                {
                    child.Width = Width - child.Margin[Left] - child.Margin[Right] -
                        dockOffset[Left] - dockOffset[Right];

                    child.X = child.Margin[Left] + dockOffset[Left];
                    child.Y = Height - child.Height - child.Margin[Bottom] - dockOffset[Bottom];

                    dockOffset[Bottom] += child.Height + child.Margin[Top] + child.Margin[Bottom];
                }
                else if (child.Dock == Dock.Left)
                {
                    child.Height = Height - child.Margin[Top] - child.Margin[Bottom] -
                        dockOffset[Top] - dockOffset[Bottom];

                    child.X = child.Margin[Left] + dockOffset[Left];
                    child.Y = child.Margin[Top] + dockOffset[Top];

                    dockOffset[Left] += child.Width + child.Margin[Left] + child.Margin[Right];
                }
                else if (child.Dock == Dock.Right)
                {
                    child.Height = Height - child.Margin[Top] - child.Margin[Bottom] -
                        dockOffset[Top] - dockOffset[Bottom];

                    child.X = Width - child.Width - child.Margin[Right] - dockOffset[Right];
                    child.Y = child.Margin[Top] + dockOffset[Top];

                    dockOffset[Right] += child.Width + child.Margin[Left] + child.Margin[Right];
                }
                else if (child.Dock == Dock.Fill)
                {
                    child.Width = Width - child.Margin[Left] - child.Margin[Right] -
                        dockOffset[Left] - dockOffset[Right];
                    child.Height = Height - child.Margin[Top] - child.Margin[Bottom] -
                        dockOffset[Top] - dockOffset[Bottom];

                    child.X = dockOffset[Left];
                    child.Y = dockOffset[Top];
                }

                child.AbsolutePosition = AbsolutePosition + child.Position;
            }
        }

        public event EventHandler PerformLayout; 

        public virtual void OnPerformLayout()
        {
            PerformLayout?.Invoke(this, EventArgs.Empty);
        }

        #region Input events

        public bool Hovered { get; set; }

        public virtual void OnMouseMoved(MouseMoveEventArgs args)
        {
            if (args.X >= AbsolutePosition.X && args.Y >= AbsolutePosition.Y &&
                args.X <= AbsolutePosition.X + Width && args.Y <= AbsolutePosition.Y + Height)
            {
                if (!Hovered)
                {
                    Hovered = true;
                    OnMouseHover();
                }
            }
            else if (Hovered)
            {
                Hovered = false;
                OnMouseLeave();
            }
        }

        public virtual void OnMouseHover()
        {

        }

        public virtual void OnMouseLeave()
        {

        }

        public virtual void OnMouseButtonDown(MouseButtonEventArgs args)
        {

        }

        public virtual void OnMouseButtonUp(MouseButtonEventArgs args)
        {

        }

        #endregion
    }
}
