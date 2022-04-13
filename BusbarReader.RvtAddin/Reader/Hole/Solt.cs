namespace BusbarReader.RvtAddin.Reader
{
    public class Solt : Hole
    {
        public Solt(List<SoltArc> soltArc, List<Line3d> lines)
        {
            SoltArc = soltArc;
            Lines = lines;
        }

        public override HoleType Type => HoleType.Solt;

        public List<SoltArc> SoltArc { get; }

        public List<Line3d> Lines { get; }

        public override string ToString(Vector3d orign)
        {
            return "Solt";
        }
    }
}
