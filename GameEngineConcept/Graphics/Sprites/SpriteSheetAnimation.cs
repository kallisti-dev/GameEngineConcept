using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Diagnostics;

namespace GameEngineConcept.Graphics.Sprites
{
    //describes the animation of a sprite via sprite sheet manipulations
    public class SpriteSheetAnimation
    {
        public static readonly Point
            rightSlider = new Point(1, 0),
            downSlider = new Point(0, 1);

        public Point StartPoint { get; protected set; }

        //total frames in animation
        public int TotalFrames { get; protected set; }

        //multiplied with FrameOffset to determine frame movement within the sprite texture
        public Point Slider { get; protected set; }

        public SpriteSheetAnimation(int totalFrames, Point startPoint = default(Point)) 
            : this(totalFrames, startPoint, rightSlider) { }

        public SpriteSheetAnimation(Sprite sprite, Point startPoint = default(Point))
            : this(sprite, startPoint, rightSlider) { }

        public SpriteSheetAnimation(int totalFrames, Point startPoint, Point slider)
        {
            TotalFrames = totalFrames;
            StartPoint = startPoint;
            Slider = slider;
        }

        public SpriteSheetAnimation(Sprite sprite, Point startPoint, Point slider)
            : this(0, startPoint, slider)
        {
            Point frameOffset = Slider.Multiply(sprite.GetSize());
            TotalFrames = sprite.Texture.Width / frameOffset.X;
        }
    }
}
