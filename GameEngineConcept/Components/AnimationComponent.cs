using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngineConcept.Components
{
    using Graphics.Animations;

    /* AnimationComponent message types */

    public class BeginAnimation<S> : Message
    {
        public IAnimatable<S> animation;
        public BeginAnimation(IAnimatable<S> animation) {
            this.animation = animation;
        }
    }


    public class PauseAnimation : Message { }

    public class ResumeAnimation : Message { }

    public class LoopAnimation : Message
    {
        public bool loop;
        public LoopAnimation(bool loop = true) { this.loop = loop; }
    }

    /* Base AnimationComponent for use with the phased update system:
     * 
     * collection.Update<AnimationComponent>();
     */
    public abstract class AnimationComponent : IComponent
    {
        public abstract void Update();
    }

    /* Game component that manages animation of a given subject */
    public class AnimationComponent<S> :
        AnimationComponent,
        IReceiver<BeginAnimation<S>>,
        IReceiver<PauseAnimation>,
        IReceiver<ResumeAnimation>,
        IReceiver<LoopAnimation>
    {
        S subject;
        IAnimator<S> animator = null;
        public bool Paused { get; set; }
        public bool Loop {get; set;}

        public AnimationComponent(S subject)
        {
            this.subject = subject;
            Paused = false;
            Loop = false;
        
        }

        public void BeginAnimation(IAnimatable<S> animation)
        {
            animator = animation.CreateAnimator(subject);
            Paused = false;
            Loop = false;
        }


        public override void Update()
        {
            if(animator == null)
                return;
            if (!Loop && animator.CurrentFrame >= animator.TotalFrames) {
                animator = null;
            }
            else if (!Paused) {
                //TODO: add frames per second control
                animator.Forward();
                animator.Animate();
            }
        }

        public void Receive(BeginAnimation<S> msg)
        {
            BeginAnimation(msg.animation);
        }

        public void Receive(PauseAnimation msg)
        {
            Paused = true;
        }

        public void Receive(ResumeAnimation msg)
        {
            Paused = false;
        }

        public void Receive(LoopAnimation msg)
        {
            Loop = msg.loop;
        }
    }
}
