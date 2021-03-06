using System.Text;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;


namespace BusbarReader.RvtAddin.Reader
{
    /// <summary>
    /// 直线排
    /// </summary>
    public class LineBusbarSegment : BusbarSegment
    {
        #region Ctor
        public LineBusbarSegment(Duct element) : base(element)
        {
        }
        #endregion

        #region Properties
        public double Width { get; private set; }

        public double ThickNess { get; private set; }

        public XYZ Direction { get; private set; }

        /// <summary>
        /// 孔腔 类型查看：<see cref="HoleType"/>
        /// </summary>
        public List<Hole> Holes { get; private set; }

        /// <summary>
        /// 排头中心点
        /// </summary>
        public Vector3d Orign { get; private set; }
        #endregion

        protected override void Solve()
        {
            var geoElement = Element.get_Geometry(new Options()
            {
                View = Element.Document.ActiveView
            });


            Lines = geoElement.OfType<Line>().ToList();

            if (Lines.Count != 1)
            {
                throw new GuidCurveCountErrorException(Element,2,Lines.Count);
            }

            CombineLine = Lines[0].ToLin3d();
            Direction = Lines[0].Direction.Normalize();

            var solids = geoElement.OfType<Solid>()
                .Where(p => p.Volume > 0)
                .ToList();

            if (Lines.Count != 1)
            {
                throw new SolidCountErrorCountException(Element,1,Lines.Count);
            }
            var solid = solids.First();

            SolveSpec(solid);
        }

        public override string ToString()
        {
            var strBuilder = new StringBuilder();
            strBuilder.Append($"L:{Math.Round(UnitConverter.ConvertToMM(CombineLine.Direction.Length),2)};");
            if (Holes != null)
            {
                foreach (var hole in Holes)
                {
                    strBuilder.Append(hole.ToString(Orign));
                }
            }
            return strBuilder.ToString();
        }

        #region Private Methods
        private void SolveSpec(Solid solid)
        {
            var planarFaces = solid.Faces.OfType<PlanarFace>();

            PlanarFace sidePlanarFace = null;
            PlanarFace holePlanarFace = null;
            foreach (var face in planarFaces)
            {
                if (face.FaceNormal.IsAlmostEqualTo(Direction, ConnectUtil.Eplision))
                {
                    sidePlanarFace = face;
                }
                else if (face.EdgeLoops.Size > 1)
                {
                    holePlanarFace = face;
                }

            }

            if (sidePlanarFace == null)
            {
                throw new InvalidOperationException($"{Element.Id}-{Element.Name} 无法计算宽度和厚度");
            }
            else
            {
                //确定基准点
                var centerOrign = CombineLine.Origin;
                Orign = sidePlanarFace.Project(new XYZ(centerOrign.x, centerOrign.y, centerOrign.z)).XYZPoint.ToVector3d();

                SolveWidthAndThickness(sidePlanarFace,out var width,out var thickness);
                Width = width;
                ThickNess = thickness;
            }

            if (holePlanarFace != null)
            {
                SolveHoles(holePlanarFace);
            }
        }

        private void SolveHoles(PlanarFace face)
        {
            //计算孔腔
            List<Hole> holes = new List<Hole>();
            foreach (EdgeArray loop in face.EdgeLoops)
            {
                Hole hole = null;
                if (loop.Size == 2)
                {
                    var circle = loop.OfType<Edge>().First();
                    hole = Hole.Create(circle);
                }else if(loop.Size == 4)
                {
                    hole = Hole.Create(loop);
                }
                if (hole != null)
                {
                    holes.Add(hole);
                }
            }

            Holes = holes;
        }
        #endregion
    }
}

