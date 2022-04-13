using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;

namespace BusbarReader.RvtAddin.Reader
{
    public abstract class BusbarSegment
    {
        #region Ctor
        public BusbarSegment(Element element)
        {
            Element = element;
        }
        #endregion

        #region Static Method
        public static BusbarSegment CreateByElememnt(Element element)
        {
            BusbarSegment segment = default;
            if (element is Duct duct)
            {
                segment = new LineBusbarSegment(duct);
            }
            else if (element is FamilyInstance familyInstance)
            {
                segment = new BendBusbarSegment(familyInstance);
            }

            if (segment != null)
            {
                segment.Solve();
                return segment;
            }
            else
            {
                throw new NotSupportedException(element.GetType().FullName);
            }
        }
        #endregion

        #region Properties
        public Element Element { get; }

        public List<Line> Lines { get; protected set; }

        public Line3d CombineLine { get; protected set; }
        #endregion

        #region Public Methods
        public void InvertCombineLine()
        {
            CombineLine = CombineLine.Invert();
        }
        #endregion

        #region Protected Methods
        protected abstract void Solve();       

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
        #endregion
    }
}
