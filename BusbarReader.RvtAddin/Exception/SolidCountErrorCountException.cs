using Autodesk.Revit.DB;

namespace BusbarReader.RvtAddin
{
    public class SolidCountErrorCountException:Exception
    {
        public SolidCountErrorCountException(Element element, int needCount, int actualCount)
          : base($"当前元素：{element.Id}-{element.Name} 实体数量错误，需要：{needCount} 个，实际：{actualCount}个")
        {

        }
    }
}
