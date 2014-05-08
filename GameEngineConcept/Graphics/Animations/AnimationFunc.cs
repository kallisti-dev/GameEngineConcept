using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngineConcept.Graphics.Animations
{
    public enum FrameRoundingType { Floor, Round, Truncate, Ceiling };

    //static functions for creating transformations on existing animations
    public static class AnimationFunc
    {
        public static AnimationFunc<S> ScaleFrames<S>(IAnimatable<S> animation, float scale, FrameRoundingType roundingType = FrameRoundingType.Round)
        {
            Func<float, float> rounder = null;
            switch(roundingType)
            {
                case FrameRoundingType.Round:
                    rounder = FloatMath.Round;
                    break;
                case FrameRoundingType.Truncate:
                    rounder = FloatMath.Truncate;
                    break;
                case FrameRoundingType.Floor:
                    rounder = FloatMath.Floor;
                    break;
                case FrameRoundingType.Ceiling:
                    rounder = FloatMath.Ceiling;
                    break;
            }
            return new AnimationFunc<S>(animation, toFrame: (inner, n) => { inner.ToFrame((int)rounder(scale * n)); });
        }
    }

    //a wrapper around an existing IAnimatable with new functionality given by delegates passed to constructor
    public class AnimationFunc<S> : IAnimatable<S>
    {
        protected readonly static Action<IAnimator<S>, int> nullToFrame = (i, n) => { i.ToFrame(n); };
        protected readonly static Action<IAnimator<S>>  nullAnimate = (i) => { i.Animate(); };

        IAnimatable<S> a;
        protected Action<IAnimator<S>, int> toFrame;
        protected Action<IAnimator<S>>  animate;

        public AnimationFunc(
                IAnimatable<S> animation,  
                Action<IAnimator<S>, int> toFrame = null, 
                Action<IAnimator<S>> animate = null)
        {
            a = animation;
            this.toFrame = toFrame ?? nullToFrame;
            this.animate = animate ?? nullAnimate;
        }

        public IAnimator<S> CreateAnimator(S s)
        {
            return new Animator(this, a.CreateAnimator(s));
        }

        private class Animator : IAnimator<S>
        {
            public S Subject {get {return inner.Subject;}}
            public int CurrentFrame { get; protected set; }
            public int NextFrame { get; protected set; }

            public AnimationFunc<S> parent;
            public IAnimator<S> inner;

            public Animator(AnimationFunc<S> f, IAnimator<S> a)
            {
                parent = f;
                inner = a;
                CurrentFrame = inner.CurrentFrame;
                NextFrame = inner.NextFrame;
            }


            public void ToFrame(int n)
            {
               parent.toFrame(inner, n);
               NextFrame = n;
            }

            public void Animate()
            {
                parent.animate(inner);
                CurrentFrame = NextFrame;
            }
        }
    }
}
