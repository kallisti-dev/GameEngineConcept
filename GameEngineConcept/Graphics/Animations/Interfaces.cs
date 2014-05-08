using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngineConcept.Graphics.Animations
{
    public interface IAnimatable
    {
        int CurrentFrame { get; }
        int NextFrame { get; }

        void ToFrame(int n);
        bool Animate();
    }

    public static class IAnimatableExtensions
    {
        public static void Forward(this IAnimatable @this, int nFrames = 1)
        {
            @this.ToFrame(@this.CurrentFrame + nFrames);
        }

        public static void Reverse(this IAnimatable @this, int nFrames = 1)
        {
            @this.ToFrame(@this.CurrentFrame + nFrames);
        }
    }

    public interface IAnimation<S, A>
        where A : IAnimation<S, A>
    {
         IAnimator<S, A> CreateAnimator(S subject); 
    }

    public interface IAnimator<S, A> : IAnimatable
        where A : IAnimation<S, A>
    {
        S Subject { get; }
        A Animation { get; }
    }
}
