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
        public BeginAnimation(IAnimatable<S> a) { animation = a; }
    }
    public class PauseAnimation : Message { }
    public class ResumeAnimation : Message { }

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
        IReceiver<ResumeAnimation>
    {
        S subject;
        IAnimator<S> animator = null;
        bool paused = false;

        public AnimationComponent(S subject)
        {
            this.subject = subject;
            animator = null;
        }

        public void Receive(BeginAnimation<S> msg)
        {
            animator = msg.animation.CreateAnimator(subject);
        }

        public void Receive(PauseAnimation msg)
        {
            paused = true;
        }

        public void Receive(ResumeAnimation msg)
        {
            paused = false;
        }

        public override void Update()
        {
            if (animator != null && !paused) {
                animator.Forward();
                animator.Animate();
            }
        }
    }
}
