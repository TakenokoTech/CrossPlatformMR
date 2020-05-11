namespace Project.Scripts.Runtime.exception
{
    public class ApiException : System.Exception
    {
        public ApiException(string message) : base(message)
        {
        }
    }
}