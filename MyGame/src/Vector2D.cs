using SwinGameSDK;
using static System.Math;

namespace MyGame
{
    public class Vector2D
    {
        public double x;
        public double y;

        public Vector2D(double x = 0, double y = 0)
        {
            this.x = x;
            this.y = y;
        }

        //converts vector2d to point2d for easier swingame code
        public Point2D asPoint2D()
        {
            var value = new Point2D
            {
                X = (float) x,
                Y = (float) y
            };
            return value;
        }


        //to get distance when given a position vector
        //since distance is |x^2+y^2+..|
        public double Length()
        {
            return Sqrt(x * x + y * y);
        }

        // direction vector from x position to y
        // known as unit vector
        public Vector2D normal()
        {
            var result = new Vector2D();
            var l = Sqrt(x * x + y * y);
            if (l != 0)
            {
                result.x = x / l;
                result.y = y / l;
            }
            // else return (0,0) vector
            return result;
        }

        //overloads for vector2d operations
        //added functionality to allow easier operations without specifying
        //x and y seperately each time
        public static Vector2D operator +(Vector2D v1, Vector2D v2)
        {
            return new Vector2D(v1.x + v2.x, v1.y + v2.y);
        }

        public static Vector2D operator -(Vector2D v1, Vector2D v2)
        {
            return new Vector2D(v1.x - v2.x, v1.y - v2.y);
        }

        public static Vector2D operator *(Vector2D v1, double value)
        {
            return new Vector2D(v1.x * value, v1.y * value);
        }

        public static Vector2D operator *(double value, Vector2D v1)
        {
            return new Vector2D(v1.x * value, v1.y * value);
        }

        public static Vector2D operator /(Vector2D v1, double value)
        {
            return new Vector2D(v1.x / value, v1.y / value);
        }
    }
}