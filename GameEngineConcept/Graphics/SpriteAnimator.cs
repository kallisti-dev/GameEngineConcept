using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Diagnostics;

namespace GameEngineConcept.Graphics
{

    public enum SpriteFrameDirection {Horizontal, Vertical}

    public class SpriteAnimator
    {
        public static readonly Point
            rightSlider = new Point(1, 0),
            downSlider = new Point(0, 1);

        public Sprite Sprite { get; private set; }

        public Point StartPoint { get; protected set; }

        //currently animated frame
        public int CurrentFrame { get; protected set; }

        //next frame to animate
        public int NextFrame { get; protected set; }

        //total frames in animation
        public int TotalFrames { get; protected set; }

        //integer vector offset between frames
        public Point FrameOffset { get; protected set; }

        //multiplied with FrameOffset to determine frame movement within the sprite texture
        public Point Slider { get; protected set; }

        Point currentTexCoord, nextTexCoord;


        public SpriteAnimator(Sprite sprite, int totalFrames = 0, Point startPoint = default(Point)) : this(sprite, totalFrames, startPoint, rightSlider) 
        {
            if(totalFrames == 0)
                TotalFrames = Sprite.Texture.Width / FrameOffset.X;
        }

        public SpriteAnimator(Sprite sprite, int totalFrames, Point startPoint, Point slider)
        {
            Sprite = sprite;
            nextTexCoord = currentTexCoord = StartPoint = startPoint;
            Slider = slider;
            NextFrame = CurrentFrame = 0;
            TotalFrames = totalFrames;
            FrameOffset = Slider.Multiply(Sprite.GetSize());

        }

        public SpriteAnimator Forward(int nFrames = 1)
        {
            return ToFrame(CurrentFrame + nFrames);
        }

        public SpriteAnimator Reverse(int nFrames = 1)
        {
            return ToFrame(CurrentFrame + nFrames);
        }

        public SpriteAnimator ToFrame(int n)
        {
            //normalize frame input to [0,TotalFrames]
            n %= TotalFrames;
            if (n < 0)
            {
                n = TotalFrames - n;
            }

            var diff = n - CurrentFrame;
            nextTexCoord.Offset(FrameOffset.Multiply(new Point(diff)));
            NextFrame = n;

            return this;
        }

        public bool Animate()
        {
            if (CurrentFrame == NextFrame)
                return false;
            currentTexCoord = nextTexCoord;
            CurrentFrame = NextFrame;
            Sprite.SetTexCoord(currentTexCoord);
            return true;
        }



    }
}
