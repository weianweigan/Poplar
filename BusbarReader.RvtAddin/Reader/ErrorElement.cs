namespace BusbarReader.RvtAddin.Reader
{
    public class ErrorElement
    {
        public ErrorElement(Exception exception)
        {
            Exception = exception;
        }

        public Exception Exception { get; private set; }
    }
}
