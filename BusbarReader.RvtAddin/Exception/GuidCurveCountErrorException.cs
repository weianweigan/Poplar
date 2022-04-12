using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace BusbarReader.RvtAddin
{
    public class GuidCurveCountErrorException:Exception
    {
        public GuidCurveCountErrorException(Element element,int needCount,int actualCount)
            : base($"当前元素：{element.Id}-{element.Name} 引导线数量错误，需要：{needCount} 个，实际：{actualCount}个")
        {

        }
    }
}
