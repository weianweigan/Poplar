namespace BusbarReader.RvtAddin
{
    public static class Line3dExtension
    {
        public static Vector3d GetEndPt(this Line3d line)
        {
            return (line.Origin + line.Direction);
        }

        public static Line3d Invert(this Line3d line)
        {
            return new Line3d(line.GetEndPt(), line.Direction*-1);
        }
    }
}
