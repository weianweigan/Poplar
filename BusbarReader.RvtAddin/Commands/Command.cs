using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using BusbarReader.RvtAddin.Reader;

namespace BusbarReader.RvtAddin.Commands
{
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiDocument = commandData.Application.ActiveUIDocument;
            var document = uiDocument.Document;

            var selections = uiDocument.Selection.GetElementIds().Select(p => document.GetElement(p)).ToList();

            var busBarDoc = new BusbarDocument(document,selections);
            var busbar = busBarDoc.GetBusbar();

            var window = new BusbarListWindow(busbar, busBarDoc, uiDocument);
            window.ShowDialog();

            return Result.Succeeded;
        }
    }
}