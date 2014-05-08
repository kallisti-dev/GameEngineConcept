using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngineConcept.Graphics.Animations
{
    class AnimatorTable<Key, S> : IAnimator<S>
    {

        Dictionary<Key, IAnimator<S>> stateTable;
        IAnimator<S> animator;

        public int CurrentFrame
        {
            get { return animator.CurrentFrame;  }
        }

        public int NextFrame
        {
            get { return animator.NextFrame; }
        }

        public S Subject { get; private set; }

        IAnimator<S> this[Key key]
        {
            get { return stateTable[key]; }
            set { stateTable[key] = value; }
        }

        public AnimatorTable(S subject, Key initialKey, IEnumerable<KeyValuePair<Key, IAnimatable<S>>> animations)
        {
            Subject = subject;
            stateTable = animations.ToDictionary(
                (pair) => pair.Key,
                (pair) => pair.Value.CreateAnimator(subject)
            );
            SetCurrent(initialKey);
        }

        public void Add(Key key, IAnimatable<S> animation)
        {
            stateTable[key] = animation.CreateAnimator(Subject);
        }

        public void Remove(Key key)
        {
            stateTable.Remove(key);
        }

        public void SetCurrent(Key key)
        {
            animator = stateTable[key];
        }

        public void ToFrame(int n)
        {
            animator.ToFrame(n);
        }

        public void Animate()
        {
            animator.Animate();
        }

    }
}
