

namespace BusbarReader.RvtAddin.Reader
{
    public class Busbar
    {

        public Busbar()
        {
        }

        public List<BusbarSegment> Segments { get; } = new List<BusbarSegment>();

        public List<BusbarSegment> SortedSegments { get; private set; }

        public List<Line3d> SortedLines { get; private set; }

        public bool IsConnect(BusbarSegment segment)
        {
            return Segments.Any(p => ConnectUtil.IsConnect(segment, p));
        }

        public double GetLength()
        {
            return SortedLines.Count == 1 ? 
                SortedLines.First().Direction.Length : 
                SortedLines.Select(p => p.Direction.Length).Aggregate((a, b) => a + b);
        }

        public void Add(BusbarSegment busbarSegment)
        {
            Segments.Add(busbarSegment);
        }

        public void Sort()
        {
            //Find Start Point
            BusbarSegment startNode = Segments.OfType<LineBusbarSegment>().FirstOrDefault(p =>
            {
                var line = p.Lines.First();

                bool hasStart = false;
                bool hasEnd = false;
                foreach (var item in Segments)
                {
                    if (p == item)
                    {
                        continue;
                    }
                    else
                    {
                        if (!hasStart &&  ConnectUtil.IsConnect(line.GetEndPoint(0).ToVector3d(), item.CombineLine))
                        {
                            hasStart = true;
                        }
                        if (!hasEnd && ConnectUtil.IsConnect(line.GetEndPoint(1).ToVector3d(), item.CombineLine))
                        {
                            hasEnd = true;
                        }
                        if (hasStart && hasEnd)
                        {
                            return false;
                        }
                    }
                }

                if( !hasStart || !hasEnd)
                {
                    if (hasStart)
                    {
                        p.InvertCombineLine();
                    }
                    return true;
                }
                return false;
            });

            //Sort
            LinkedList<BusbarSegment> sortedSegments = new LinkedList<BusbarSegment>();

            var visited =new HashSet<BusbarSegment>();
            while (startNode != null)
            {
                sortedSegments.AddLast(startNode);
                visited.Add(startNode);

                bool find = false;
                //找下一点
                foreach (var seg in Segments)
                {
                    if (!visited.Contains(seg))
                    {
                        if (ConnectUtil.IsCoincident(seg.CombineLine.Origin,startNode.CombineLine.GetEndPt()))
                        {
                            startNode = seg;
                            find = true;
                            break;
                        }
                        else if (ConnectUtil.IsCoincident(seg.CombineLine.GetEndPt(), startNode.CombineLine.GetEndPt()))
                        {
                            //反转
                            seg.InvertCombineLine();
                            startNode = seg;
                            find=true;
                            break;
                        }
                    }
                }

                if (!find)
                {
                    break;
                }
            }

            SortedSegments = sortedSegments.ToList();

            ToSortLines();
        }

        private void ToSortLines()
        {
            List<Line3d> lines = new List<Line3d>();
            foreach (var segment in SortedSegments)
            {
                if (segment is LineBusbarSegment)
                {
                    lines.Add(segment.CombineLine);
                }
                else if(segment is BendBusbarSegment bendBusbarSegment)
                {
                    lines.AddRange(bendBusbarSegment.GetOrderedLines());
                }
            }
            SortedLines = lines;
        }

        public override string ToString()
        {
            return $"铜牌:{SortedSegments?.Count} 段"; 
        }
    }
}
