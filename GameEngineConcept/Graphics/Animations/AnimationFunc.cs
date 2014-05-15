using System;

namespace GameEngineConcept.Graphics.Animations
{
    public enum FrameRoundingType { Floor, Round, Truncate, Ceiling };

    //static extension methods for creating transformations on existing animations
    public static class AnimationFunc
    {
        //applies a transformation function on each frame of the given animation
        public static AnimationFunc <S> TransformFrames<S>(this IAnimatable<S> a, Func<int, int> toFrame, Func<int, int> totalFrames = null)
        {
            return new AnimationFunc<S>(a,
                toFrame: (inner, n) => { inner.ToFrame(toFrame(n)); },
                totalFrames: (inner) => { return totalFrames(inner.TotalFrames); }
            );
        }

        //scales the frames of an animation by a given floating point value
        public static AnimationFunc<S> ScaleFrames<S>(this IAnimatable<S> animation, float scale, Func<float, float> rounder = null)
        {
            if(rounder==null) rounder = FloatMath.Round;
            Func<int, int> f = (n) => (int) rounder(n/scale);
            return TransformFrames(animation, toFrame: f, totalFrames: f);
        }

        public static AnimationFunc<S> OffsetFrames<S>(this IAnimatable<S> animation, int offset)
        {
            return TransformFrames(animation, (n) => n + offset);
        }
    }

    //a wrapper around an existing IAnimatable with new functionality given by delegates passed to constructor
    public class AnimationFunc<S> : IAnimatable<S>
    {
        protected readonly static Action<IAnimator<S>, int> nullToFrame 
            = (i, n) => { i.ToFrame(n); };
        protected readonly static Action<IAnimator<S>> nullAnimate 
            = (i) => { i.Animate(); };
        protected readonly static Func<IAnimator<S>, int> nullTotalFrames 
            = (i) => i.TotalFrames ;

        IAnimatable<S> a;
        protected Action<IAnimator<S>, int> toFrame;
        protected Action<IAnimator<S>> animate;
        protected Func<IAnimator<S>, int> totalFrames;

        public AnimationFunc(
                IAnimatable<S> animation,
                Action<IAnimator<S>, int> toFrame = null, 
                Action<IAnimator<S>> animate = null,
                Func<IAnimator<S>, int> totalFrames = null)
        {
            a = animation;
            this.toFrame = toFrame ?? nullToFrame;
            this.animate = animate ?? nullAnimate;
            this.totalFrames = totalFrames ?? nullTotalFrames;
        }

        public IAnimator<S> CreateAnimator(S s)
        {
            return new Animator(this, a.CreateAnimator(s));
        }

        private class Animator : IAnimator<S>
        {
            public S Subject {get {return inner.Subject;}}
            public int CurrentFrame { get; private set; }
            public int NextFrame { get; private set; }
            public int TotalFrames { get { return parent.totalFrames(inner); } }

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
