using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using BusbarReader.RvtAddin.Reader;


namespace BusbarReader.RvtAddin
{
    public static class ConnectUtil
    {
        public static double Eplision = 0.001;

        public static bool IsConnect(BusbarSegment segment1, BusbarSegment segment2)
        {
            
            return IsConnect(segment1.CombineLine,segment2.CombineLine);
        }

        private static bool IsConnect(Line l1, Line l2)
        {
            return (
                IsCoincident(l1.GetEndPoint(0), l2.GetEndPoint(0)) ||
                IsCoincident(l1.GetEndPoint(1), l2.GetEndPoint(1))
                ) ||
                (
                IsCoincident(l1.GetEndPoint(0), l2.GetEndPoint(1)) ||
                IsCoincident(l1.GetEndPoint(1), l2.GetEndPoint(0))
                );
        }

        private static bool IsConnect(Line3d l1, Line3d l2)
        {
            return (
                IsCoincident(l1.Origin, l2.Origin) ||
                IsCoincident(l1.GetEndPt(), l2.GetEndPt())
                ) ||
                (
                IsCoincident(l1.Origin, l2.GetEndPt()) ||
                IsCoincident(l1.GetEndPt(), l2.Origin)
                );
        }

        public static bool IsConnect(XYZ pt,Line line)
        {
            return IsCoincident(pt, line.GetEndPoint(0)) || IsCoincident(pt, line.GetEndPoint(1));
        }

        public static bool IsConnect(Vector3d pt, Line3d line)
        {
            return IsCoincident(pt, line.Origin) || IsCoincident(pt, line.GetEndPt());
        }

        public static bool IsCoincident(XYZ pt1, XYZ pt2)
        {
            return pt1.DistanceTo(pt2) < Eplision;
        }

        public static bool IsCoincident(Vector3d vector1,Vector3d vector2)
        {
            return vector1.Distance(vector2) < Eplision;
        }
    }
}
