using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace GameEngineConcept.Graphics.Animations
{
    public class SpriteAnimationState : BaseAnimationState<Sprite, SpriteAnimation>
    {
        Point currentTexCoord, nextTexCoord;
        Point frameOffset;


        public SpriteAnimationState(IAnimator<Sprite, SpriteAnimation> ator, SpriteAnimation ation) : base(ator, ation)
        {
            nextTexCoord = currentTexCoord = animation.StartPoint;
            frameOffset = animation.Slider.Multiply(animator.Subject.GetSize());
        }

        //shift NextFrame to n
        public override void ToFrame(int n)
        {
            //normalize frame input to [0,TotalFrames]
            n %= animation.TotalFrames;
            if (n < 0)
            {
                n = animation.TotalFrames - n;
            }

            var diff = n - CurrentFrame;
            nextTexCoord.Offset(frameOffset.Multiply(new Point(diff)));
            NextFrame = n;
        }

        //make NextFrame the CurrentFrame
        public override bool Animate()
        {
            if (CurrentFrame == NextFrame)
                return false;
            currentTexCoord = nextTexCoord;
            CurrentFrame = NextFrame;
            animator.Subject.SetTexCoord(currentTexCoord);
            return true;
        }
    }
}
