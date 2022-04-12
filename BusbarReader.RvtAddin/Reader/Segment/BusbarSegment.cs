using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;


namespace BusbarReader.RvtAddin.Reader
{
    public abstract class BusbarSegment
    {
        public BusbarSegment(Element element)
        {
            Element = element;
            Solve();
        }

        public Element Element { get; }

        public List<Line> Lines { get; protected set; }

        public Line3d CombineLine { get; protected set; }

        protected abstract void Solve();

        public static BusbarSegment CreateByElememnt(Element element)
        {
            if (element is Duct duct)
            {
                return new LineBusbarSegment(duct);
            }else if (element is FamilyInstance familyInstance)
            {
                return new BendBusbarSegment(familyInstance);
            }

            throw new NotSupportedException(element.GetType().FullName);
        }

        public void InvertCombineLine()
        {
            CombineLine = CombineLine.Invert();
        }

        protected void SolveWidthAndThickness(PlanarFace face,out double width,out double thickness)
        {
            var lengths = face.EdgeLoops.
                OfType<EdgeArray>().SelectMany(p => p.OfType<Edge>())
                .Select(p => p.ApproximateLength)
                .ToList();

            if (lengths.Any())
            {
                width = lengths.Max();
                thickness = lengths.Min();
            }
            else
            {
                throw new InvalidOperationException($"{Element.Id}-{Element.Name} 无法获取宽度和厚度");
            }
        }

        protected double SolveCircleSideWidth(PlanarFace face)
        {
            var lengths = face.EdgeLoops.
                OfType<EdgeArray>().SelectMany(p => p.OfType<Edge>())
                .Where(e => !e.AsCurve().IsCyclic)
                .Select(p => p.ApproximateLength)
                .ToList();

            if (lengths.Any())
            {
                return lengths.First();
            }
            else
            {
                throw new InvalidOperationException($"{Element.Id}-{Element.Name} 无法获取宽度和厚度");
            }
        }
    }
}
