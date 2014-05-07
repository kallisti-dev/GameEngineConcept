using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngineConcept.Graphics.Animations
{
    class AnimationTable<Key, S, A> : Animator<S, A>
        where A : IAnimation<S, A>
    {
        private struct TableInfo
        {
            public A animation;
            public IAnimationState<S, A> state;

            public TableInfo(IAnimator<S, A> ator, A ation)
            {
                animation = ation;
                state = ation.CreateState(ator);
            }
        }

        Dictionary<Key, TableInfo> stateTable;

        public AnimationTable(S subject, IEnumerable<KeyValuePair<Key, A>> animations, Key initialKey)
        {
            Subject = subject;
            stateTable = animations.ToDictionary(
                (pair) => pair.Key,
                (pair) => new TableInfo (this, pair.Value)
            );
            SetCurrent(initialKey);
        }

        public void Add(Key key, A animation)
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
