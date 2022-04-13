namespace BusbarReader.RvtAddin.Reader
{
    public class Rectangle : Hole
    {
        public Rectangle(List<Line3d> lines)
        {
            Lines = lines;
        }
        public override HoleType Type => HoleType.Rectangle;

        public List<Line3d> Lines { get;  }

        public override string ToString()
        {
            return $"Rect";
        }

        public override string ToString(Vector3d orign)
        {
            return $"Rect";
        }
    }
}
