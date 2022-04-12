using System.Collections.Generic;
using System.IO;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using NUnit.Framework;

namespace BusbarReader.RvtAddin.Reader.Tests
{
    [TestFixture()]
    public class BusbarDocumentTests
    {
        public static string Dir = Path.Combine(typeof(BusbarDocumentTests).Assembly.Location, "TestModel");

        [Test]
        public void VecTest(UIApplication uiApplication, Application application)
        {
            var model = Path.Combine(Dir, "竖直折弯.rvt");
            var busbar = GetBusbar(uiApplication, model,out var doc);
            
            Assert.IsNotNull(busbar);
            Assert.IsTrue(busbar.Count == 1);

            
        }

        [Test]
        public void HorTest(UIApplication uiApplication, Application application)
        {
            var model = Path.Combine(Dir, "水平折弯.rvt");
            var busbar = GetBusbar(uiApplication, model, out var doc);

            Assert.IsNotNull(busbar);
            Assert.IsTrue(busbar.Count == 1);
        }

        [Test]
        public void MutiTest(UIApplication uiApplication, Application application)
        {
            var model = Path.Combine(Dir, "多铜牌.rvt");
            var busbar = GetBusbar(uiApplication, model, out var doc);

            Assert.IsNotNull(busbar);
            Assert.IsTrue(busbar.Count == 1);
        }

        private static List<Busbar> GetBusbar(UIApplication uiApplication, string model,out Document doc)
        {
            doc = uiApplication.Application.OpenDocumentFile(model);

            var busbarDoc = new BusbarDocument(doc);
            var busbar = busbarDoc.GetBusbar();
            return busbar;
        }
    }
}