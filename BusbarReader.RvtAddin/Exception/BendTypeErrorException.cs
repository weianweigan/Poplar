namespace BusbarReader.RvtAddin
{
    public class BendTypeErrorException : Exception
    {
        public BendTypeErrorException(string message, Exception innerException) : 
            base(message, innerException)
        {

        }
    }
}
