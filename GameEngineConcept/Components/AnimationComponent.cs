using System;

namespace GameEngineConcept.Components
{
    using Graphics.Animations;

    public delegate void AnimationEventHandler<S>(AnimationComponent<S> c, IAnimator<S> a);

    /* Game component that manages animation of a given subject */
    public sealed class AnimationComponent<S> : IComponent
    {
        IAnimator<S> animator;
        public S Subject {get; protected set;}
        public bool Loop { get; set; }
        bool _paused;
        public bool Paused { 
            get { return _paused; }
            set
            {
                _paused = value;
                if (_paused)
                    OnPause(this, animator);
                else
                    OnUnpause(this, animator);
            }
        }

        private static AnimationEventHandler<S> @void = (_, __) => { };
        public event AnimationEventHandler<S> OnBegin = @void;     //start of new animation
        public event AnimationEventHandler<S> OnEnd = @void;       //animation has completed and stopped
        public event AnimationEventHandler<S> OnStart = @void;     //start of animation cycle
        public event AnimationEventHandler<S> OnComplete = @void;  //complete animation cycle
        public event AnimationEventHandler<S> OnPause = @void;     //animation pause
        public event AnimationEventHandler<S> OnUnpause = @void;   //animation unpause
        
        public AnimationComponent(S subject)
        {
            Subject = subject;
            animator = null;
        
        }

        public AnimationComponent(S subject, IAnimatable<S> animation) : this(subject)
        {
            BeginAnimation(animation);
        }

        public void BeginAnimation(IAnimatable<S> animation)
        {
            if (animator != null)
                OnEnd(this, animator);
            animator = animation.CreateAnimator(Subject);
            Loop = false;
            _paused = false;
            OnBegin(this, animator);
        }


        public void Update(GameState state)
        {
            if(animator == null)
                return;
            if (!Loop && animator.CurrentFrame >= animator.TotalFrames) {
                OnEnd(this, animator);
                animator = null;
            }
            else if (!Paused) {
                //TODO: add frames per second control
                animator.Forward();
                if (animator.NextFrame == 0)
                    OnStart(this, animator);
                else if (animator.NextFrame >= animator.TotalFrames)
                    OnComplete(this, animator);
                animator.Animate();
            }
        }
    }
}
