using GameEngineConcept.Components;
using GameEngineConcept.Graphics;
using System.Collections.Generic;

namespace GameEngineConcept
{
    public class GameState
    {
        DrawableSet drawSet = new DrawableSet();
        ComponentCollection updateSet = new ComponentCollection();

        public IEnumerable<IDrawable> Drawables { get { return drawSet; } }
        public IEnumerable<IComponent> Components { get { return updateSet; } }

        GameState _parent;


        public EngineWindow Window { get; private set; }

        public GameState Parent {
            get
            {
                return _parent;
            }
            set
            {
                if (_parent != null) {
                    _parent.RemoveComponents(Components);
                    _parent.RemoveDrawables(Drawables);
                }
                if (value != null) {
                    value.AddComponents(Components);
                    value.AddDrawables(Drawables);
                }
                _parent = value;
            }
        }


        public GameState(EngineWindow window, GameState parent = null)
        {
            Parent = parent;
            Window = window;
        }


        public void AddDrawables(IEnumerable<IDrawable> drawables)
        {
            drawSet.UnionWith(drawables);
            if (Parent != null) Parent.AddDrawables(drawables);
        }


        public void AddDrawables(params IDrawable[] drawables) { AddDrawables(drawables); }


        public void RemoveDrawables(IEnumerable<IDrawable> drawables)
        {
            drawSet.ExceptWith(drawables);
            if (Parent != null) Parent.RemoveDrawables(drawables);
        }

        public void RemoveDrawables(params IDrawable[] drawables) { RemoveDrawables(drawables); }

        public void AddComponents(IEnumerable<IComponent> components)
        {
            updateSet.UnionWith(components);
            if (Parent != null) Parent.AddComponents(components);
        }

        public void AddComponents(params IComponent[] components) { AddComponents(components); }

        public void RemoveComponents(IEnumerable<IComponent> components)
        {
            updateSet.ExceptWith(components);
            if (Parent != null) Parent.RemoveComponents(components);
        }

        public void RemoveComponents(params IComponent[] components) { RemoveComponents(components); }

    }
}
