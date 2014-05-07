using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngineConcept.Graphics.Animations
{
    public abstract class BaseAnimationState<Subject, Animation> : IAnimatable
    {
        protected IAnimator<Subject> animator;
        protected Animation animation;

        //currently animated frame
        public int CurrentFrame { get; protected set; }
        //next frame to animate
        public int NextFrame { get; protected set; }

        public BaseAnimationState(IAnimator<Subject> ator, Animation ation) 
        {
            CurrentFrame = NextFrame = 0;
            animator = ator;
            animation = ation;
        }

        public abstract void ToFrame(int i);
        public abstract bool Animate();
    }
}
