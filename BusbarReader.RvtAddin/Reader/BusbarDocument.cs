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

                busbar.Sort();

                busbars.Add(busbar);
            }            

            return busbars;
        }

        private List<BusbarSegment> GetSegments()
        {
            var busbarSegements = new List<BusbarSegment>();
            var errorElements = new List<ErrorElement>();

            if (_selections?.Any() == true)
            {
                foreach (var element in _selections)
                {
                    AddSegment(busbarSegements, errorElements, element);
                }
                return busbarSegements;
            }

            var collector = new FilteredElementCollector(Document);
            var lineFileter = new ElementCategoryFilter(BuiltInCategory.OST_DuctCurves);        
            var lineElement = collector.WherePasses(lineFileter);
            foreach (var element in lineElement.OfType<Duct>())
            {
                AddSegment(busbarSegements, errorElements, element);
            }

            var bendFileter = new ElementCategoryFilter(BuiltInCategory.OST_DuctFitting);
            collector = new FilteredElementCollector(Document);
            var bendElement = collector.WherePasses(bendFileter);
            foreach (var element in bendElement.OfType<FamilyInstance>())
            {
                AddSegment(busbarSegements, errorElements, element);
            }

            ErrorElememts = errorElements;

            return busbarSegements;
        }

        private static void AddSegment(List<BusbarSegment> busbarSegements, List<ErrorElement> errorElements, Element element)
        {
            BusbarSegment segment = default;
            try
            {
                segment = BusbarSegment.CreateByElememnt(element);
                busbarSegements.Add(segment);
            }
            catch (BendTypeErrorException ex)
            {
                if (segment != null)
                {
                    busbarSegements.Add(segment);
                }
                errorElements.Add(new ErrorElement(ex, element));
            }
            catch (Exception ex)
            {
                errorElements.Add(new ErrorElement(ex, element));
            }
        }
    }
}
