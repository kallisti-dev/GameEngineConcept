using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Diagnostics;

namespace GameEngineConcept.Graphics.Animations
{

    public abstract class Animator<S, A> : IAnimator<S, A>
        where A : IAnimation<S, A>
    {

        public S Subject { get; protected set; }

        public A Animation { get; protected set; }

        public int CurrentFrame { get; protected set; }

        public int NextFrame { get; protected set;}

        protected Animator() { }

        public Animator(S subject, A animation)
        {
            CurrentFrame = NextFrame = 0;
            Subject = subject;
            Animation = animation;
        }

        public abstract void ToFrame(int n);

        public abstract bool Animate();

    }
}
