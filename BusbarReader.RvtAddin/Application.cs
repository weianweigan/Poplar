using Autodesk.Revit.UI;
using BusbarReader.RvtAddin.Commands;

namespace BusbarReader.RvtAddin
{
    [UsedImplicitly]
    public class Application : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            var panel = application.CreatePanel("Panel name", "Poplar");

            var showButton = panel.AddPushButton<Command>("Busbar");
            showButton.SetImage("/BusbarReader.RvtAddin;component/Resources/Icons/RibbonIcon16.png");
            showButton.SetLargeImage("/BusbarReader.RvtAddin;component/Resources/Icons/RibbonIcon32.png");

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            

            return Result.Succeeded;
        }
    }
}