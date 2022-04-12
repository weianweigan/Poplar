namespace BusbarReader.RvtAddin
{
    public struct Line3d
    {
        public Vector3d Origin;

        public Vector3d Direction;

        public Line3d(Vector3d origin, Vector3d direction)
        {
            Origin = origin;
            Direction = direction;
        }

        public Vector3d PointAt(double d)
        {
            return Origin + d * Direction;
        }

        public double Project(Vector3d p)
        {
            return (p - Origin).Dot(Direction);
        }

        public double DistanceSquared(Vector3d p)
        {
            double f = (p - Origin).Dot(Direction);
            return (Origin + f * Direction - p).LengthSquared;
        }

        public Vector3d ClosestPoint(Vector3d p)
        {
            double f = (p - Origin).Dot(Direction);
            return Origin + f * Direction;
        }

        public override string ToString()
        {
            return $"<{Origin}>-<{this.GetEndPt()}>";
        }
    }
}