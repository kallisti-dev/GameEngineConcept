using System;
using GameEngineConcept.Components;
using GameEngineConcept.Graphics;
using System.Collections.Generic;

namespace GameEngineConcept
{
    public class GameState
    {
        DrawableSet drawSet = new DrawableSet();
        ComponentCollection updateSet = new ComponentCollection();



        public IDrawableCollection Drawables { get { return drawSet; } }
        public IComponentCollection  Components { get { return updateSet; } }
        
        public event Action<GameState> OnOverride; //called at the beginning of Override method
        public event Action<GameState> OnRestore;  //called after a Snapshot of this state is restored

        GameState _parent;


        protected EngineWindow window;

        public GameState Parent {
            get
            {
                return _parent;
            }
            set
            {
                if (_parent != null) {
                    _parent.RemoveComponents((IEnumerable<IComponent>)Components);
                    _parent.RemoveDrawables((IEnumerable<IDrawable>)Drawables);
                }
                if (value != null) {
                    value.AddComponents((IEnumerable<IComponent>)Components);
                    value.AddDrawables((IEnumerable<IDrawable>)Drawables);
                }
                _parent = value;
            }
        }


        public GameState(EngineWindow window, GameState parent = null)
        {
            Parent = parent;
            this.window = window;
        }


        public void AddDrawables(IEnumerable<IDrawable> drawables)
        {
            drawSet.UnionWith(drawables);
            if (Parent != null) Parent.AddDrawables(drawables);
        }

        public void AddDrawables(params IDrawable[] drawables) { AddDrawables(drawables); }

        public void AddDrawable(IDrawable drawable) { AddDrawables(drawable); }

        public void RemoveDrawables(IEnumerable<IDrawable> drawables)
        {
            drawSet.ExceptWith(drawables);
            if (Parent != null) Parent.RemoveDrawables(drawables);
        }

        public void RemoveDrawables(params IDrawable[] drawables) { RemoveDrawables(drawables); }

        public void RemoveDrawable(IDrawable drawable) { RemoveDrawables(drawable); }

        public void AddComponents(IEnumerable<IComponent> components)
        {
            updateSet.AddRange(components);
            if (Parent != null) Parent.AddComponents(components);
        }

        public void AddComponents(params IComponent[] components) { AddComponents(components); }

        public void AddComponent(IComponent component) { AddComponents(new[] { component });  }

        public void RemoveComponents(IEnumerable<IComponent> components)
        {
            updateSet.AddRange(components);
            if (Parent != null) Parent.RemoveComponents(components);
        }

        public void RemoveComponents(params IComponent[] components) { RemoveComponents(components); }

        public void RemoveComponent(IComponent component) { RemoveComponents(new[] { component }); }

        //Removes all state from this GameState and returns a GameState.Snapshot that can be used
        //to restore it
        public Snapshot Override()
        {
            OnOverride(this);
            var snapshot = new Snapshot(this, new List<IDrawable>(Drawables), new List<IComponent>(Components));
            RemoveComponents((IEnumerable<IComponent>)Components);
            RemoveDrawables((IEnumerable<IDrawable>)Drawables);
            return snapshot;
        }

        //inner class representing a "snapshot" of an overriden state that can later be restored
        public class Snapshot
        {
            GameState overriden;
            IEnumerable<IDrawable> drawables;
            IEnumerable<IComponent> components;

            internal Snapshot(GameState o, IEnumerable<IDrawable> d, IEnumerable<IComponent> c)
            {
                overriden = o;
                drawables = d;
                components = c;
            }

            public void Restore()
            {
                overriden.AddDrawables(drawables);
                overriden.AddComponents(components);
                overriden.OnRestore(overriden);
            }
        }

    }
}
