using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;

namespace BusbarReader.RvtAddin.Reader
{
    public class BusbarDocument
    {
        private readonly List<Element> _selections;

        public BusbarDocument(Document document, List<Element> selections)
        {
            Document = document;
            _selections = selections;
        }

        public Document Document { get; }

        public List<ErrorElement> ErrorElememts { get; set; }

        public List<Busbar> GetBusbar()
        {
            var busbars = new List<Busbar>();

            var busbarSegements = GetSegments();

            var visited = new HashSet<BusbarSegment>();

            while (visited.Count != busbarSegements.Count)
            {
                var start = busbarSegements.FirstOrDefault(p => !visited.Contains(p));
                if (start == null) break;

                var busbar = new Busbar();

                while (start != null)
                {
                    busbar.Add(start);
                    visited.Add(start);
                    start = busbarSegements
                    .FirstOrDefault(p => !visited.Contains(p) && busbar.IsConnect(p));
                }

                //排序
                busbar.Sort();

                //var length = busbar.GetLength();

                busbars.Add(busbar);
            }            

            return busbars;
        }

        private List<BusbarSegment> GetSegments()
        {
            var busbarSegements = new List<BusbarSegment>();
            List<ErrorElement> errorElements = new List<ErrorElement>();

            if (_selections?.Any() == true)
            {
                foreach (var element in _selections)
                {
                    try
                    {
                        var elemt = BusbarSegment.CreateByElememnt(element);
                        busbarSegements.Add(elemt);
                    }
                    catch (Exception ex)
                    {
                        errorElements.Add(new ErrorElement(ex));
                    }
                }
                return busbarSegements;
            }

            var collector = new FilteredElementCollector(Document);
            var lineFileter = new ElementCategoryFilter(BuiltInCategory.OST_DuctCurves);        
            var lineElement = collector.WherePasses(lineFileter);
            foreach (var element in lineElement.OfType<Duct>())
            {
                try
                {
                   var elemt =  BusbarSegment.CreateByElememnt(element);
                    busbarSegements.Add(elemt);
                }
                catch (Exception ex)
                {
                    errorElements.Add(new ErrorElement(ex));
                }
            }

            var bendFileter = new ElementCategoryFilter(BuiltInCategory.OST_DuctFitting);
            collector = new FilteredElementCollector(Document);
            var bendElement = collector.WherePasses(bendFileter);
            foreach (var element in bendElement.OfType<FamilyInstance>())
            {
                try
                {
                    var elemt = BusbarSegment.CreateByElememnt(element);
                    busbarSegements.Add(elemt);
                }
                catch (Exception ex)
                {
                    errorElements.Add(new ErrorElement(ex));
                }
            }

            ErrorElememts = errorElements;

            return busbarSegements;
        }
    }
}
