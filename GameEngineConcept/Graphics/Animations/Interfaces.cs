using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngineConcept.Graphics.Animations
{

    public interface IAnimatable<S>
    {
         IAnimator<S> CreateAnimator(S subject); 
    }

    public interface IAnimator<S>
    {
        S Subject { get; }

        int CurrentFrame { get; }
        int NextFrame { get; }
        int TotalFrames { get; }

        void ToFrame(int n);
        void Animate();
    }

    public static class IAnimatorExtensions
    {
        public static void Forward<S>(this IAnimator<S> @this, int nFrames = 1)
        {
            @this.ToFrame(@this.CurrentFrame + nFrames);
        }

        public static void Reverse<S>(this IAnimator<S> @this, int nFrames = 1)
        {
            @this.ToFrame(@this.CurrentFrame + nFrames);
        }
    }
}
