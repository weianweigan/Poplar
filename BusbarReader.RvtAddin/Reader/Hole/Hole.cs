using Autodesk.Revit.DB;

namespace BusbarReader.RvtAddin.Reader
{
    public enum HoleType
    {
        Circle,
        Rectangle,
        Solt,
    }

    public abstract class Hole
    {
        public abstract HoleType Type { get; }

        public abstract string ToString(Vector3d orign);

        public static Hole Create(Edge circle)
        {
            var curve = circle.AsCurve();
            if (!curve.IsCyclic)
            {
                return null;
            }

            var arc = curve as Arc;

            return new Circle(arc.Center.ToVector3d(), arc.Radius);
        }

        internal static Hole Create(EdgeArray loop)
        {
            var array = loop.OfType<Edge>().ToList();
            if (array.Count != 4)
            {
                return null;
            }

            if (array.Any(p => p.AsCurve().IsBound))
            {
                return null;
            }

            var curves = array.Select(p => p.AsCurve()).ToList();

            //腰孔
            if (curves.Any(p => p.IsCyclic))
            {
                var soltArc = curves.OfType<Arc>().Select(p => FromArc(p)).ToList();
                var lines = curves.Where(p => !p.IsCyclic).Select(p => FromCurve(p)).ToList();

                return new Solt(soltArc, lines);
            }
            else
            {
                //矩形
                var lines = curves.Select(p => FromCurve(p)).ToList();

                return new Rectangle(lines);
            }
        }

        private static SoltArc FromArc(Arc p)
        {
            var center = p.Center.ToVector3d();
            var sp = p.GetEndPoint(0).ToVector3d();
            var ep = p.GetEndPoint(1).ToVector3d();

            return new SoltArc(p.Radius, center, sp, ep);
        }

        private static Line3d FromCurve(Curve curve)
        {
            var sp = curve.GetEndPoint(0).ToVector3d();
            var ep = curve.GetEndPoint(1).ToVector3d();

            return new Line3d(sp, ep - sp);
        }
    }
}
