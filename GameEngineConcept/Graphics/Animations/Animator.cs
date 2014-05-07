using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Diagnostics;

namespace GameEngineConcept.Graphics.Animations
{

    public class Animator<S, A> : IAnimator<S, A>
        where A : IAnimation<S, A>
    {

        public S Subject { get; protected set; }

        public A Animation { get; protected set; }

        public IAnimationState<S, A> State {get; protected set; }

        protected Animator() { }

        public Animator(S subject, A animation)
        {
            Subject = subject;
            Animation = animation;
            State = animation.CreateState(this);
        }

        public void Forward(int nFrames = 1)
        {
            ToFrame(State.CurrentFrame + nFrames);
        }

        public void Reverse(int nFrames = 1)
        {
            ToFrame(State.CurrentFrame + nFrames);
        }

        public void ToFrame(int n)
        {

            State.ToFrame(n);
        }

        public bool Animate()
        {
            return State.Animate();
        }

    }
}
