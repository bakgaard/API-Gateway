using System.Reflection.PortableExecutable;

namespace SharedTypes.Messages
{
    public class Request
    {
        public string Message { get; }
        public Dictionary<string, string> Headers { get; }

        public Request(Dictionary<string, string> headers, string message)
        {
            Headers = headers ?? new Dictionary<string, string>(); //Default if nothing is received
            Message = message;
        }
    }
}
