﻿using GameEngineConcept.Components;
using GameEngineConcept.Graphics;
using GameEngineConcept.Scenes;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GameEngineConcept
{
    public class GameState
    {
        DrawableSet drawSet = new DrawableSet();
        ComponentCollection updateSet = new ComponentCollection();

        protected EngineWindow window;
        protected ResourcePool pool;

        public IDrawableCollection Drawables { get { return drawSet; } }
        public IComponentCollection  Components { get { return updateSet; } }
        
        public event Action<GameState> OnOverride; //called at the beginning of Override method
        public event Action<GameState> OnRestore;  //called after a Snapshot of this state is restored

        GameState _parent;
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
                    _parent._children.Remove(this);
                }
                if (value != null) {
                    value.AddComponents(Components);
                    value.AddDrawables(Drawables);
                    value._children.Add(this);
                }
                _parent = value;
            }
        }

        public GameState Root
        {
            get
            {
                if (_parent == null)
                    return this;
                else
                    return Parent.Root;
            }
        }

        List<GameState> _children = new List<GameState>();
        public IEnumerable<GameState> Children
        {
            get { return _children; }
        }

        public GameState(EngineWindow window, ResourcePool pool)
        {
            this.pool = pool;
            this.window = window;
        }

        public GameState(GameState parent) : this(parent.window, parent.pool)
        {
            Parent = parent;
        }

        public void AddChildren(IEnumerable<GameState> children)
        {
            foreach (var child in children) { child.Parent = this; }
        }

        public void AddDrawables(IEnumerable<IDrawable> drawables)
        {
            drawSet.UnionWith(drawables);
            if (Parent != null) Parent.AddDrawables(drawables);
        }

        public void AddDrawable(IDrawable drawable) { AddDrawables(new[] { drawable }); }

        public void RemoveDrawables(IEnumerable<IDrawable> drawables)
        {
            drawSet.ExceptWith(drawables);
            if (Parent != null) Parent.RemoveDrawables(drawables);
        }

        public void RemoveDrawable(IDrawable drawable) { RemoveDrawables(new[] { drawable }); }

        public void AddComponents(IEnumerable<IComponent> components)
        {
            updateSet.AddRange(components);
            if (Parent != null) Parent.AddComponents(components);
        }

        public void AddComponent(IComponent component) { AddComponents(new[] { component });  }

        public void RemoveComponents(IEnumerable<IComponent> components)
        {
            updateSet.AddRange(components);
            if (Parent != null) Parent.RemoveComponents(components);
        }

        public void RemoveComponent(IComponent component) { RemoveComponents(new[] { component }); }

        //Removes all state from this GameState and returns a GameState.Snapshot that can be used
        //to restore it
        public ISceneSnapshot Override()
        {
            OnOverride(this);
            var snapshot = new Snapshot(this);
            RemoveComponents((IEnumerable<IComponent>)Components);
            RemoveDrawables((IEnumerable<IDrawable>)Drawables);
            foreach (var child in Children) { child.Parent = null; }
            return snapshot;
        }

        //inner class representing a "snapshot" of an overriden state that can later be restored
        private class Snapshot : ISceneSnapshot
        {
            GameState overriden;
            IEnumerable<IDrawable> drawables;
            IEnumerable<IComponent> components;
            IEnumerable<GameState> children;

            public Snapshot(GameState o)
            {
                overriden = o;
                drawables = new List<IDrawable>(o.Drawables);
                components = new List<IComponent>(o.Components);
                children = new List<GameState>(o.Children);
            }

            public void Restore()
            {
                overriden.AddDrawables(drawables);
                overriden.AddComponents(components);
                overriden.AddChildren(children);
                overriden.OnRestore(overriden);
            }
        }

    }
}
