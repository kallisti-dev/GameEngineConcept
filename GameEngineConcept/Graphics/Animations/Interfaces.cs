using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngineConcept.Graphics.Animations
{
    public interface IAnimationState<Subject, Animation>
        where Animation : IAnimation<Subject, Animation>
    {
        int CurrentFrame { get; }
        int NextFrame { get; }

        void ToFrame(int n);
        bool Animate();
    }

    public interface IAnimation<Subject, Animation>
        where Animation : IAnimation<Subject, Animation>
    {
        IAnimationState<Subject, Animation> CreateState(IAnimator<Subject, Animation> animator); 
    }

    public interface IAnimator<S, A>
        where A : IAnimation<S, A>
    {
        S Subject { get; }
        A Animation { get; }
        IAnimationState<S, A> State { get; }

        void Forward(int n);
        void Reverse(int n);

        void ToFrame(int n);
        bool Animate();
    }
}
