using Autodesk.Revit.DB;


namespace BusbarReader.RvtAddin
{
    public static class RvtLineExtension
    {
        public static Line3d ToLin3d(this Line line, bool invert = false)
        {
            var direction = line.Direction;
            direction = direction.Normalize() * line.Length;

            if (invert)
            {
                var endPt = line.GetEndPoint(1);
                return new Line3d(new Vector3d(endPt.X, endPt.Y, endPt.Z),
                        new Vector3d(-direction.X, -direction.Y, -direction.Z));
            }
            else
            {
                var orign = line.Origin;
                return new Line3d(new Vector3d(orign.X, orign.Y, orign.Z),
                        new Vector3d(direction.X, direction.Y, direction.Z));
            }
        }
      
    }
}
