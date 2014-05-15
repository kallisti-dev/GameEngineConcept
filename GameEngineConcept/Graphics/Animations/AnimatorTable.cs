using System.Collections.Generic;
using System.Linq;

namespace GameEngineConcept.Graphics.Animations
{
    class AnimatorTable<Key, S> : IAnimator<S>
    {

        Dictionary<Key, IAnimator<S>> stateTable;
        IAnimator<S> currentAnimator;

        public int CurrentFrame
        {
            get { return currentAnimator.CurrentFrame;  }
        }

        public int NextFrame
        {
            get { return currentAnimator.NextFrame; }
        }

        public int TotalFrames
        {
            get { return currentAnimator.TotalFrames; }
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

        public bool Remove(Key key)
        {
            return stateTable.Remove(key);
        }

        public void SetCurrent(Key key)
        {
            currentAnimator = stateTable[key];
        }

        public void ToFrame(int n)
        {
            currentAnimator.ToFrame(n);
        }

        public void Animate()
        {
            currentAnimator.Animate();
        }

    }
}
