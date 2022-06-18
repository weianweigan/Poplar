using System.Windows;
using BusbarReader.RvtAddin.Reader;

namespace BusbarReader.RvtAddin
{
    /// <summary>
    /// BusbarListWindow.xaml 的交互逻辑
    /// </summary>
    public partial class BusbarListWindow : Window
    {
        public BusbarListWindow(List<Busbar> busbars,BusbarDocument document, Autodesk.Revit.UI.UIDocument uiDocument)
        {
            InitializeComponent();

            DataContext = new BusbarListWindowViewModel(busbars,document,uiDocument);
        }
    }
}
