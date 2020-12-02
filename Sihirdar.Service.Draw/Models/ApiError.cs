using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Sihirdar.Service.Draw.Models
{
    public class ValidationApiError : ApiError<Dictionary<string, List<string>>>
    {
        public ValidationApiError() : this(new Dictionary<string, List<string>>())
        {
        }

        public ValidationApiError(Dictionary<string, List<string>> dict)
        {
            Code = "Validation";
            Data = dict;
        }

        public void Add(string fieldName, string message)
        {
            List<string> messages;
            if (!Data.TryGetValue(fieldName, out messages))
            {
                messages = new List<string>();
                Data.Add(fieldName, messages);
            }

            messages.Add(message);
        }

        public bool HasErrors()
        {
            return Data.Any();
        }
    }

    public class ApiError<T> : ApiError
    
    {
        [JsonProperty(Order = 2)]
        public T Data { get; set; }
    }

    public class ApiError
    {
        public string Code { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}