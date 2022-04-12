using Autodesk.Revit.DB;


namespace BusbarReader.RvtAddin
{
    public static class XYZExtension
    {
        public static Vector3d ToVector3d(this XYZ xyz)
        {
            return new Vector3d(xyz.X, xyz.Y, xyz.Z);
        }
    }
}
