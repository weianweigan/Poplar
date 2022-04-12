using System.ComponentModel;
using System.Windows;
using Autodesk.Revit.UI;
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

    public class BusbarListWindowViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        private Busbar _selectedBusbar;

        public BusbarListWindowViewModel(List<Busbar> busbars, BusbarDocument document, 
            Autodesk.Revit.UI.UIDocument uiDocument)
        {
            Busbars = busbars;
            Document = document;
            UiDocument = uiDocument;
        }

        public List<Busbar> Busbars { get; set; }

        public Busbar SelectedBusbar
        {
            get => _selectedBusbar; set
            {
                _selectedBusbar = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedBusbar)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedBusbar.SortedSegments"));
                SelectElement();
            }
        }

        public BusbarDocument Document { get; }

        public UIDocument UiDocument { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void SelectElement()
        {
            if (SelectedBusbar != null)
            {
                UiDocument.Selection.SetElementIds(SelectedBusbar.SortedSegments
                    .Select(p => p.Element.Id)
                    .ToList());

                UiDocument.RefreshActiveView();
            }
        }
    }
}
