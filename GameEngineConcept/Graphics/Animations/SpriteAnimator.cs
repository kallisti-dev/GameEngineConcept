﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace GameEngineConcept.Graphics.Animations
{
    public class SpriteAnimator : BaseAnimator<Sprite, SpriteAnimation>
    {
        Point currentTexCoord, nextTexCoord;
        Point frameOffset;


        public SpriteAnimator(Sprite sprite, SpriteAnimation animation) : base(sprite, animation)
        {
            nextTexCoord = currentTexCoord = animation.StartPoint;
            frameOffset = animation.Slider.Multiply(sprite.GetSize());
        }

        //shift NextFrame to n
        public override void ToFrame(int n)
        {
            //normalize frame input to [0,TotalFrames]
            n %= Animation.TotalFrames;
            if (n < 0)
            {
                n = Animation.TotalFrames - n;
            }

            var diff = n - CurrentFrame;
            nextTexCoord.Offset(frameOffset.Multiply(new Point(diff)));
            NextFrame = n;
        }

        //make NextFrame the CurrentFrame
        public override void Animate()
        {
            currentTexCoord = nextTexCoord;
            CurrentFrame = NextFrame;
            Subject.SetTexCoord(currentTexCoord);
        }
    }
}
