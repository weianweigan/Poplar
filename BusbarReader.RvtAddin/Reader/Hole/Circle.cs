namespace BusbarReader.RvtAddin.Reader
{
    public class Circle : Hole
    {
        public Circle(Vector3d center, double radius)
        {
            Center = center;
            Radius = radius;
        }

        public double Radius { get; }

        public Vector3d Center { get; }

        public override HoleType Type => HoleType.Circle;

        public override string ToString()
        {
            return $"T:{Center} {Radius}";
        }

        public override string ToString(Vector3d orign)
        {
            return $"T:{UnitConverter.ConvertToMM(Center - orign)},{UnitConverter.ConvertToMM(Radius)}";
        }
    }

    public class SoltArc
    {
        public SoltArc(double radius, Vector3d center, Vector3d startPoint, Vector3d endPoint)
        {
            Radius = radius;
            Center = center;
            StartPoint = startPoint;
            EndPoint = endPoint;
        }

        public double Radius { get; }

        public Vector3d Center { get; }

        public Vector3d StartPoint { get; }

        public Vector3d EndPoint { get; }
    }
}
