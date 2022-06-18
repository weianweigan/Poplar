using BusbarReader.RvtAddin.Reader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BusbarReader.RvtAddin
{
    public static class SegmentTypeColorSetting
    {
        public static Color LineColor { get; } = Colors.LightGreen;

        public static Color VecColor { get; } = Colors.LightBlue;

        public static Color HorColor { get; } = Colors.Purple;

        public static Color GetSegmentTypeTypeColor(this BusbarSegment busbarSegment)
        {
            var color = Colors.Transparent;

            if (busbarSegment is LineBusbarSegment)
            {
                color = LineColor;
            }else if(busbarSegment is BendBusbarSegment bendBusbarSegment)
            {
                switch (bendBusbarSegment.BendType)
                {
                    case BendType.Vec:
                        color = VecColor;
                        break;
                    case BendType.Hor:
                        color = HorColor;
                        break;
                    default:
                        return Colors.Transparent;
                }
            }

            return color;
        }
    }
}
