namespace YourCoach.Application.Utils.Results
{
    public interface IResult
    {
        bool Success { get; set; }
        string Message { get; set; }
        string Code { get; set; }
        object Data { get; set; }
    }
}
