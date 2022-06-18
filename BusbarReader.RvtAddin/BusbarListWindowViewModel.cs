using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using BusbarReader.RvtAddin.Reader;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BusbarReader.RvtAddin
{
    public class BusbarListWindowViewModel : ObservableObject
    {
        #region Fields
        private Busbar _selectedBusbar;
        private BusbarSegment _selectedSegment;
        private ErrorElement _selectErrorElement;
        private string _statusBarMsg;
        #endregion

        #region Ctor
        public BusbarListWindowViewModel(List<Busbar> busbars, BusbarDocument document,
            UIDocument uiDocument)
        {
            Busbars = busbars;
            Document = document;
            UiDocument = uiDocument;

            StatusBarMsg = $"Num:{busbars?.Count}";
        }
        #endregion

        #region Properties
        public BusbarDocument Document { get; }

        public UIDocument UiDocument { get; }

        public List<Busbar> Busbars { get; set; }

        public Busbar SelectedBusbar
        {
            get => _selectedBusbar; set
            {
                _selectedBusbar = value;
                OnPropertyChanged(nameof(SelectedBusbar));
                OnPropertyChanged("SelectedBusbar.SortedSegments");
                SelectElement(SelectedBusbar?.SortedSegments.Select(p => p.Element).ToList());
            }
        }

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
        #endregion

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

        public string StatusBarMsg { get => _statusBarMsg; set => SetProperty(ref _statusBarMsg, value); }

    }
}
