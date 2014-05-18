using GameEngineConcept.Components;
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

        static Dictionary<Scene, PendingSceneState> loadedScenes = new Dictionary<Scene, PendingSceneState>();

        //internal state related to a background-loaded scene
        private class PendingSceneState
        {
            public Task task;
            public ResourcePool pool;
            public CancellationTokenSource cancelSource = new CancellationTokenSource();

            public PendingSceneState(Scene s, ResourcePool p)
            {
                pool = p.Copy();
                task = s.Load(pool, cancelSource.Token);
            }
        }

        Dictionary<Scene, GameState> activeScenes = new Dictionary<Scene, GameState>();
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


        public GameState(EngineWindow window, ResourcePool pool, GameState parent = null)
        {
            Parent = parent;
            this.pool = pool;
            this.window = window;
        }


        public Task LoadScene(Scene scene)
        {
            return Task.Run(() => _LoadScene(scene).Wait());
        }

        public void UnloadScene(Scene scene)
        {
            RemoveScene(scene);
            PendingSceneState pending;
            if (loadedScenes.TryGetValue(scene, out pending)) {
                pending.cancelSource.Cancel(true);
                pending.pool.Release();
                loadedScenes.Remove(scene);
            }
        }

        private Task _LoadScene(Scene scene)
        {
            PendingSceneState pendingScene;
            if (loadedScenes.TryGetValue(scene, out pendingScene)) {
                return pendingScene.task;
            }
            else {
                return (loadedScenes[scene] = new PendingSceneState(scene, pool)).task;
            }
        }

        public async Task AddScene(Scene scene)
        {
            await _LoadScene(scene);
            var sceneState = new GameState(window, pool, this);
            scene.Activate(sceneState);
            activeScenes.Add(scene, sceneState);
        }

        public void RemoveScene(Scene scenes)
        {
            foreach (var scene in activeScenes.Keys) {
                GameState sceneState;
                if (activeScenes.TryGetValue(scene, out sceneState)) {
                    sceneState.Parent = null;
                    activeScenes.Remove(scene);
                }
            }
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
