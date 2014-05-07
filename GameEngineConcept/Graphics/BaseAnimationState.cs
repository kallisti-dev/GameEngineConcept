using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngineConcept.Graphics
{
    public abstract class BaseAnimationState<S, A, AS> : IAnimationState<S, A, AS>
        where AS : IAnimationState<S, A, AS>, new()
    {
        protected IAnimator<S, A, AS> animator;
        protected A animation;

        //currently animated frame
        public int CurrentFrame { get; protected set; }
        //next frame to animate
        public int NextFrame { get; protected set; }

        public BaseAnimationState() 
        {
            CurrentFrame = NextFrame = 0;
        }

        public virtual void Initialize(IAnimator<S, A, AS> ator, A ation)
        {
            animator = ator;
            animation = ation;
        }

        public abstract void ToFrame(int i);
        public abstract bool Animate();
    }
}
