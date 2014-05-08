using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngineConcept.Graphics.Animations
{
    enum FrameRoundingType {Floor, Round, Truncate, Ceiling};

    class FrameScale<S> : IAnimatable<S>
    {
        private class Animator : BaseAnimator<S, FrameScale<S>>
        {
            public IAnimator<S> inner;

            public Animator(S subject, FrameScale<S> animation) : base(subject, animation)
            {
                CurrentFrame = inner.CurrentFrame;
                NextFrame = inner.NextFrame;
            }

            public override void ToFrame(int n)
            {
                inner.ToFrame((int) Animation.rounder(Animation.scale * n));
                base.ToFrame(n);
            }

            public override void Animate()
            {
                inner.Animate();
                base.Animate();
            }
        }

        float scale;
        Func<float, float> rounder;
        IAnimatable<S> a;

        public FrameScale(float frameScale, IAnimatable<S> animation, FrameRoundingType roundingType = FrameRoundingType.Round)
        {
            a = animation;
            scale = frameScale;
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
        }

        public IAnimator<S> CreateAnimator(S subject)
        {
            return new Animator(subject, this);
        }



    }
}
