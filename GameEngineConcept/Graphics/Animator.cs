using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Diagnostics;

namespace GameEngineConcept.Graphics
{

    public class Animator<S, A, AS> : IAnimator<S, A, AS>
        where AS : IAnimationState<S, A, AS>, new()
    {

        public S Subject { get; private set; }

        public A Animation { get; private set; }

        public AS State {get; private set; }

        public Animator(S subject, A animation)
        {
            Subject = subject;
            Animation = animation;
            State = new AS();
            State.Initialize(this, animation);
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
