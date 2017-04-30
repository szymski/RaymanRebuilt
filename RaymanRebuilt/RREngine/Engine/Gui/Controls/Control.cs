using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace RREngine.Engine.Gui.Controls
{
    public class Control
    {
        public Control Parent { get; set; }

        public Vector4 Margin { get; set; } = Vector4.Zero;
        public Vector4 Padding { get; set; } = Vector4.Zero;

        public Dock Dock { get; set; } = Dock.None;

        public Vector2 Position { get; set; }
        public Vector2 AbsolutePosition { get; }
        public Vector2 Size { get; set; }

        public bool RefreshRequested { get; set; } = true;

        public ObservableCollection<Control> Children { get; } = new ObservableCollection<Control>();

        public Control()
        {
            Children.CollectionChanged +=ChildrenOnChanged;
        }

        private void ChildrenOnChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            
        }

        public EventHandler Think;

        public void OnThinkInternal()
        {
            OnThink();
            Think?.Invoke(this, EventArgs.Empty);
        }

        public virtual void OnThink()
        {
            
        }

        public EventHandler Render;

        public void OnRenderInternal(IGuiRenderer renderer)
        {
            if (RefreshRequested)
            {
                RefreshRequested = false;
            }

            OnRender();
            Render?.Invoke(this, EventArgs.Empty);

            renderer.Color = new Vector4(1f, 0f, 0f, 1f);
            renderer.DrawRectangleOutline(Position, Size);
        }

        public virtual void OnRender()
        {
            
        }

        public void UpdateLayout()
        {
            
        }
    }
} 
