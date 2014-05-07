using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngineConcept.Graphics.Animations
{
    class AnimationTable<Key, S> : Animator<S>
    {
        private struct TableInfo
        {
            public IAnimation<S> animation;
            public IAnimatable state;

            public TableInfo(IAnimator<S> ator, IAnimation<S> ation)
            {
                animation = ation;
                state = ation.CreateState(ator);
            }
        }

        Dictionary<Key, TableInfo> stateTable;

        public AnimationTable(S subject, IEnumerable<KeyValuePair<Key, IAnimation<S>>> animations, Key initialKey)
        {
            Subject = subject;
            stateTable = animations.ToDictionary(
                (pair) => pair.Key,
                (pair) => new TableInfo (this, pair.Value)
            );
            SetCurrent(initialKey);
        }

        public void Add(Key key, IAnimation<S> animation)
        {
            stateTable[key] = new TableInfo(this, animation);  
        }

        public void Remove(Key key)
        {
            stateTable.Remove(key);
        }

        public void SetCurrent(Key key)
        {
            var info = stateTable[key];
            Animation = info.animation;
            State = info.state;
        }

    }
}
