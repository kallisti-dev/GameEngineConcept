using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Diagnostics;

namespace GameEngineConcept.Graphics.Animations
{

    public abstract class BaseAnimator<S, A> : IAnimator<S>
        where A : IAnimatable<S>
    {

        public virtual S Subject { get; protected set; }

        public virtual A Animation { get; protected set; }

        public int CurrentFrame { get; protected set; }

        public int NextFrame { get; protected set;}

        public abstract int TotalFrames { get; }

        protected BaseAnimator() { }

        public BaseAnimator(S subject, A animation)
        {
            CurrentFrame = NextFrame = 0;
            Subject = subject;
            Animation = animation;
        }

        public virtual void ToFrame(int n)
        {
            NextFrame = n;
        }

        public virtual void Animate()
        {
            CurrentFrame = NextFrame;
        }

    }
}
