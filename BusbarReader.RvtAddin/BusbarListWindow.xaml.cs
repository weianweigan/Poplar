using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Autodesk.Revit.DB;
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
                SelectElement(SelectedBusbar?.SortedSegments.Select(p => p.Element).ToList());
            }
        }

        public BusbarDocument Document { get; }

        public UIDocument UiDocument { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void SelectElement(List<Element> elements)
        {
            if (elements != null)
            {
                UiDocument.Selection.SetElementIds(elements
                    .Select(p => p.Id)
                    .ToList());

                UiDocument.RefreshActiveView();
            }
        }

        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }

            return false;
        }

        private ErrorElement _selectErrorElement;

        public ErrorElement SelectErrorElement
        {
            get => _selectErrorElement; set
            {
                SetProperty(ref _selectErrorElement, value);
                if (_selectErrorElement != null)
                {
                    SelectElement(new List<Element>() { _selectErrorElement.Element });
                }
            }
        }

        private BusbarSegment _selectedSegment;

        public BusbarSegment SelectedSegment
        {
            get => _selectedSegment; set
            {
                SetProperty(ref _selectedSegment, value);
                if (_selectedSegment != null)
                {
                    SelectElement(new List<Element>() { _selectedSegment.Element});
                }
            }
        }
    }
}
