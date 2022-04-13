using Autodesk.Revit.DB;

namespace BusbarReader.RvtAddin
{
    public static class UnitConverter
    {
        /// <summary>
        /// 将内部单位(英尺) 转换为 mm
        /// </summary>
        /// <param name="value">以英尺为单位的值</param>
        /// <returns>mm为单位的值</returns>
        /// <remarks>此API在20以后有更改，编译为不同版本</remarks>
        public static double ConvertToMM(double value)
        {
#if R19 || R20
            return UnitUtils.ConvertFromInternalUnits(value, DisplayUnitType.DUT_MILLIMETERS);
#else
            return UnitUtils.ConvertFromInternalUnits(value,UnitTypeId.Millimeters);
#endif
        }

        public static Vector3d ConvertToMM(this Vector3d vector)
        {
            return new Vector3d(ConvertToMM(vector.x), ConvertToMM(vector.y), ConvertToMM(vector.z));
        }

        public static Line3d ConvertToMM(this Line3d line)
        {
            return new Line3d(ConvertToMM(line.Origin), ConvertToMM(line.Direction));
        }
    }
}
