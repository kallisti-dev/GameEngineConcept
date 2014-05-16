using System.Drawing;

namespace GameEngineConcept.Graphics.Animations
{
    using ExtensionMethods;

    //describes the animation of a sprite via sprite sheet manipulations
    public class SpriteAnimation : IAnimatable<Sprite>
    {
        public static readonly Point
            rightSlider = new Point(1, 0),
            downSlider = new Point(0, 1);

        public Point StartPoint { get; protected set; }

        //total frames in animation
        public int TotalFrames { get; protected set; }

        //multiplied with FrameOffset to determine frame movement within the sprite texture
        public Point Slider { get; protected set; }

        public SpriteAnimation(int totalFrames, Point startPoint = default(Point)) 
            : this(totalFrames, startPoint, rightSlider) { }

        public SpriteAnimation(Sprite sprite, Point startPoint = default(Point))
            : this(sprite, startPoint, rightSlider) { }

        public SpriteAnimation(int totalFrames, Point startPoint, Point slider)
        {
            TotalFrames = totalFrames;
            StartPoint = startPoint;
            Slider = slider;
        }

        public SpriteAnimation(Sprite sprite, Point startPoint, Point slider)
            : this(0, startPoint, slider)
        {
            Point frameOffset = Slider.Multiply(sprite.GetSize());
            TotalFrames = sprite.Texture.Width / frameOffset.X;
        }

        public  IAnimator<Sprite> CreateAnimator(Sprite sprite)
        {
            return new SpriteAnimator(sprite, this);
        }
    }
}
