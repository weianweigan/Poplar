using Autodesk.Revit.DB;


namespace BusbarReader.RvtAddin.Reader
{
    public enum BendType
    {
        /// <summary>
        /// 平弯
        /// </summary>
        Vec,
        /// <summary>
        /// 立弯
        /// </summary>
        Hor,

        /// <summary>
        /// 未知，计算错误导致
        /// </summary>
        UnKnown,
    }

    /// <summary>
    /// 折弯
    /// </summary>
    public class BendBusbarSegment : BusbarSegment
    {
        #region Ctor
        public BendBusbarSegment(FamilyInstance element) : base(element)
        {
        }
        #endregion

        #region Properties
        public BendType BendType { get; private set; } = BendType.Vec;
        #endregion

        #region Public Methods
        public IEnumerable<Line3d> GetOrderedLines()
        {
            //第一条起点 和 起点重合
            foreach (var line in Lines)
            {
                if (ConnectUtil.IsCoincident(line.GetEndPoint(0).ToVector3d(), CombineLine.Origin))
                {
                    yield return new Line3d(line.Origin.ToVector3d(), line.Direction.ToVector3d());
                }
                else if (ConnectUtil.IsCoincident(line.GetEndPoint(1).ToVector3d(), CombineLine.Origin))
                {
                    yield return new Line3d(line.Origin.ToVector3d(), line.Direction.ToVector3d()).Invert();
                }
            }

            foreach (var line in Lines)
            {
                if (ConnectUtil.IsCoincident(line.GetEndPoint(0).ToVector3d(), CombineLine.GetEndPt()))
                {
                    yield return new Line3d(line.Origin.ToVector3d(), line.Direction.ToVector3d()).Invert();
                }
                else if (ConnectUtil.IsCoincident(line.GetEndPoint(1).ToVector3d(), CombineLine.GetEndPt()))
                {
                    yield return new Line3d(line.Origin.ToVector3d(), line.Direction.ToVector3d());
                }
            }
        }
        #endregion

        #region Other Methods
        protected override void Solve()
        {
            var geoElement = Element.get_Geometry(new Options()
            {
                View = Element.Document.ActiveView
            });

            var geoInstance = geoElement.FirstOrDefault() as GeometryInstance;
            if (geoInstance == null)
            {
                throw new InvalidOperationException("没有几何信息");
            }

            var geo = geoInstance.GetInstanceGeometry();

            Lines = geo.OfType<Line>().ToList();

            if (Lines.Count != 2)/*(Lines.Count > 2 || Lines.Count == 0)*/
            {
                throw new GuidCurveCountErrorException(Element, 2, Lines.Count);
            }

            if (Lines.Count == 1)
            {
                CombineLine = Lines[0].ToLin3d();
            }
            else
            {
                CombineTwoBendLine();
            }

            //确定折弯类型
            var solids = geo.OfType<Solid>()
                .Where(p => p.Volume > 0)
                .ToList();
            if (solids.Count != 1)
            {
                throw new SolidCountErrorCountException(Element, 1, Lines.Count);
            }

            try
            {
                EnsureBendType(solids.First());
            }
            catch (Exception ex)
            {
                BendType = BendType.UnKnown;
                throw new BendTypeErrorException($"{Element.Id}-{Element.Name} 无法确定折弯类型",ex);
            }
        }

        private void CombineTwoBendLine()
        {
            //组合成一条线
            if (ConnectUtil.IsConnect(Lines[0].GetEndPoint(0), Lines[1]))
            {
                if (ConnectUtil.IsCoincident(Lines[0].GetEndPoint(0), Lines[1].GetEndPoint(0)))
                {
                    var vector = Lines[1].GetEndPoint(1) - Lines[0].GetEndPoint(1);
                    CombineLine = new Line3d(Lines[0].GetEndPoint(1).ToVector3d(), vector.ToVector3d());
                }
                else
                {
                    var vector = Lines[1].GetEndPoint(0) - Lines[0].GetEndPoint(1);
                    CombineLine = new Line3d(Lines[0].GetEndPoint(1).ToVector3d(), vector.ToVector3d());
                }
            }
            else
            {
                if (ConnectUtil.IsCoincident(Lines[0].GetEndPoint(1), Lines[1].GetEndPoint(0)))
                {
                    var vector = Lines[1].GetEndPoint(1) - Lines[0].GetEndPoint(0);
                    CombineLine = new Line3d(Lines[0].GetEndPoint(0).ToVector3d(), vector.ToVector3d());
                }
                else
                {
                    var vector = Lines[1].GetEndPoint(0) - Lines[0].GetEndPoint(0);
                    CombineLine = new Line3d(Lines[0].GetEndPoint(0).ToVector3d(), vector.ToVector3d());
                }
            }
        }

        /// <summary>
        /// 确定折弯类型
        /// </summary>
        private void EnsureBendType(Solid solid)
        {
            var planarFaces = solid.Faces.OfType<PlanarFace>();
            var direction = Lines.First().Direction.ToVector3d();

            PlanarFace sidePlanarFace = null;
            PlanarFace circleSidePlanarFace = null;
            foreach (var face in planarFaces)
            {
                if (face.EdgeLoops.OfType<EdgeArray>()
                    .FirstOrDefault()
                    ?.OfType<Edge>()
                    .All(e => e.AsCurve() is Line) == true)
                {
                    sidePlanarFace = face;
                }
                else
                {
                    //侧面
                    circleSidePlanarFace = face;
                }
                if (sidePlanarFace != null && circleSidePlanarFace != null)
                {
                    break;
                }
            }

            if (sidePlanarFace == null || circleSidePlanarFace == null)
            {
                throw new InvalidOperationException($"{Element.Id}-{Element.Name} 无法计算宽度和厚度");
            }
            else
            {
                SolveWidthAndThickness(sidePlanarFace, out var width, out var thickness);
                var circlewidth = SolveCircleSideWidth(circleSidePlanarFace);
                if (Math.Abs(width - circlewidth) < ConnectUtil.Eplision)
                {
                    BendType = BendType.Hor;
                }
            }
        }

        public override string ToString()
        {
            string name = GetBendTypeName(BendType);

            var lines = GetOrderedLines().ToList();

            var angleR = Vector3d.AngleR(lines[0].Direction,lines[1].Direction);

            var angleD = (angleR / Math.PI) * 180 ;

            return $"{name}：{Math.Round( angleD,2)}";
        }

        private string GetBendTypeName(BendType bendType)
        {
            string name = string.Empty;
            switch (bendType)
            {
                case BendType.Vec:
                    name = "平弯";
                    break;
                case BendType.Hor:
                    name = "立弯";
                    break;
                case BendType.UnKnown:
                    name = $"未知,检查{Element.Id}";
                    break;
            }
            return name;
        }
        #endregion
    }
}

