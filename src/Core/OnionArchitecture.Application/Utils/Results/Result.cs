
using Newtonsoft.Json;

namespace YourCoach.Application.Utils.Results
{
    public class Result : IResult
    {
        public Result()
        {

        }
        public Result(bool success)
        {
            Success = success;
        }
        public Result(bool success, string message) : this(success)
        {
            Message = message;
        }
        public Result(bool success, string message, string code) : this(success)
        {
            Message = message;
            Code = code;
        }
        public Result(bool success, object data) : this(success)
        {
            Data = data;
        }
        public Result(bool success, string message, object data) : this(success)
        {
            Message = message;
            Data = data;
        }
        public Result(bool success, string message, string code, object data) : this(success)
        {
            Message = message;
            Code = code;
            Data = data;
        }
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Code { get; set; }
        public object Data { get; set; }

        public string SerializeData()
        {
            return JsonConvert.SerializeObject(this.Data);
        }
        public string SerializeObject()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
