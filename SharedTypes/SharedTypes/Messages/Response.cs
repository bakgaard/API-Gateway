using System.Text.Json;

namespace SharedTypes.Messages
{
    public class Response
    {
        public string Result { get; }
        public Dictionary<string, string> Headers { get; }

        public Response(Dictionary<string, string> headers, string result)
        {
            Headers = headers ?? new Dictionary<string, string>(); //Default if nothing is received
            Result = result;
        }
    }
}
