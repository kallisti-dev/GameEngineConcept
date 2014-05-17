using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngineConcept.Components;
using GameEngineConcept.Graphics;
using GameEngineConcept.Graphics.Modes;
using GameEngineConcept.Graphics.VertexBuffers;
using GameEngineConcept.Scenes;

namespace GameEngineConcept
{
    public class GameState : IDrawable, IComponent
    {
        HashSet<IDrawable> drawSet;
        DrawableDepthSet depthSet = new DrawableDepthSet();
        ComponentCollection updateSet = new ComponentCollection();

        public IEnumerable<IDrawable> Drawables { get { return drawSet; } }
        public IEnumerable<IComponent> Components { get { return updateSet; } }

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
                }
            }
        }


        public GameState(GameState parent = null)
        {
            Parent = _parent
            drawSet = new HashSet<IDrawable> { depthSet };
        }


        public void AddDrawables(IEnumerable<IDrawable> drawables)
        {
            foreach (var drawable in drawables) {
                var dd = drawable as IDrawableDepth;
                if (dd != null)
                    depthSet.Add(dd);
                else
                    drawSet.Add(dd);
            }
            if (Parent != null) Parent.AddDrawables(drawables);
        }

        public void AddDrawables(IEnumerable<IDrawableDepth> drawables)
        {
            depthSet.UnionWith(drawables);
            if (Parent != null) Parent.AddDrawables(drawables);
        }

        public void AddDrawables(params IDrawable[] drawables) { AddDrawables(drawables); }
        public void AddDrawables(params IDrawableDepth[] drawables) { AddDrawables(drawables); }


        public void RemoveDrawables(IEnumerable<IDrawable> drawables)
        {
            drawSet.ExceptWith(drawables);
            if (Parent != null) Parent.RemoveDrawables(drawables);
        }

        public void RemoveDrawables(IEnumerable<IDrawableDepth> drawables)
        {
            depthSet.ExceptWith(drawables);
            if (Parent != null) Parent.RemoveDrawables(drawables);
        }

        public void RemoveDrawables(params IDrawable[] drawables) { RemoveDrawables(drawables); }
        public void RemoveDrawables(params IDrawableDepth[] drawables) { RemoveDrawables(drawables); }

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
