using System.Drawing;

namespace GameEngineConcept.ExtensionMethods
{
    //this struct allows semi-implicit conversion between Point and Size from System.Drawing within our extension methods
    public struct PointSizeConverter
    {
        internal Point p;

        public static implicit operator PointSizeConverter(Point p)
        {
            return new PointSizeConverter { p = p };
        }

        public static implicit operator PointSizeConverter(Size s)
        {
            return new PointSizeConverter { p = (Point)s };
        }

        public static implicit operator Size(PointSizeConverter @this)
        {
            return (Size)@this.p;
        }

        public static implicit operator Point(PointSizeConverter @this)
        {
            return @this.p;
        }
    }

    //extension methods for Size and Point from System.Drawing
    public static class PointSizeExtensions
    {
        //math operators on Points
        public static PointSizeConverter Multiply(this Point a, PointSizeConverter b)
        {
            return new Point(a.X * b.p.X, a.Y * b.p.Y);
        }

        public static PointSizeConverter Divide(this Point a, PointSizeConverter b)
        {
            return new Point(a.X / b.p.X, a.Y / b.p.Y);
        }

        public static PointSizeConverter Subtract(this Point a, PointSizeConverter b)
        {
            return new Point(a.X - b.p.X, a.Y - b.p.Y);
        }

        public static PointSizeConverter Add(this Point a, PointSizeConverter b)
        {
            return new Point(a.X + b.p.X, a.Y + b.p.Y);
        }

        public static PointSizeConverter Negate(this Point @this)
        {
            return new Point(-@this.X, -@this.Y);
        }

        //math operators on Sizes
        public static PointSizeConverter Multiply(this Size a, PointSizeConverter b)
        {
            return new Point(a.Width * b.p.X, a.Height * b.p.Y);
        }

        public static PointSizeConverter Divide(this Size a, PointSizeConverter b)
        {
            return new Point(a.Width / b.p.X, a.Height / b.p.Y);
        }

        public static PointSizeConverter Subtract(this Size a, PointSizeConverter b)
        {
            return new Point(a.Width - b.p.X, a.Height - b.p.Y);
        }

        public static PointSizeConverter Add(this Size a, PointSizeConverter b)
        {
            return new Point(a.Width + b.p.X, a.Height + b.p.Y);
        }

        public static PointSizeConverter Negate(this Size @this)
        {
            return new Point(-@this.Width, -@this.Height);
        }
    }
}
