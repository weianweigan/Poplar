using Autodesk.Revit.DB;

namespace BusbarReader.RvtAddin.Reader
{
    public class ErrorElement
    {
        public ErrorElement(Exception exception,Element element)
        {
            Exception = exception;
            Element = element;
        }

        public Exception Exception { get; private set; }

        public Element Element { get; }

        public override string ToString()
        {
            return $"{Exception.Message}";
        }
    }
}
