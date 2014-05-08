using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngineConcept.Graphics.Animations
{
    class AnimationTable<Key, S, A> : IAnimator<S, A>
        where A : IAnimation<S, A>
    {

        Dictionary<Key, IAnimator<S, A>> stateTable;
        IAnimator<S, A> animator;

        public int CurrentFrame
        {
            get { return animator.CurrentFrame;  }
        }

        public int NextFrame
        {
            get { return animator.NextFrame; }
        }

        public S Subject { get; private set; }

        public A Animation { get; private set; }

        public AnimationTable(S subject, Key initialKey, IEnumerable<KeyValuePair<Key, IAnimation<S, A>>> animations)
        {
            Subject = subject;
            stateTable = animations.ToDictionary(
                (pair) => pair.Key,
                (pair) => pair.Value.CreateAnimator(subject)
            );
            SetCurrent(initialKey);
        }

        public void Add(Key key, IAnimation<S, A> animation)
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
            Animation = animator.Animation;
        }

        public void ToFrame(int n)
        {
            animator.ToFrame(n);
        }

        public bool Animate()
        {
            return animator.Animate();
        }

    }
}
