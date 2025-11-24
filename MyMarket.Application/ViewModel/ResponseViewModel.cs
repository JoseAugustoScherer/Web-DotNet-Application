using System.Text.Json.Serialization;

namespace MyMarket.Application.ViewModel;

public class ResponseViewModel
{
    protected ResponseViewModel(int statusCode = 200)
    {
        Message = new HashSet<string>();
        StatusCode = statusCode;
    }

    protected ResponseViewModel(string message, int statusCode) : this(statusCode)
    {
        Message.Add(message);
        StatusCode = statusCode;
    }
    
    protected ResponseViewModel(IEnumerable<string> messages, int statusCode) : this(statusCode)
    {
        Message.UnionWith(messages);
        StatusCode = statusCode;
    }
    
    public ISet<string> Message { get; }
    public int StatusCode { get; set; }
    [JsonIgnore] public bool IsSuccess => Message.Count == 0;
    [JsonIgnore] public bool IsFailure => !IsSuccess;

    public static ResponseViewModel SetStatusCode(int statusCode) => 
        new(statusCode);

    public static ResponseViewModel Ok() => 
        new();

    public static ResponseViewModel Fail(string message, int statusCode) => 
        new(message, statusCode);
    
    public static ResponseViewModel Fail(IEnumerable<string> messages, int statusCode) => 
        new(messages, statusCode);

    public static ResponseViewModel Fail(Exception exception, int statusCode) =>
        new(string.Concat(exception?.Message, " - ", exception?.InnerException?.Message), statusCode);
}

public class ResponseViewModel<TValue> : ResponseViewModel
{
    public ResponseViewModel()
    {
    }

    private ResponseViewModel(string message, int statusCode) : base(message, statusCode)
    {
    }

    private ResponseViewModel(IEnumerable<string> messages, int statusCode) : base(messages, statusCode)
    {
    }
    
    public ResponseViewModel(TValue value, int statusCode)
    {
        Data = value;
        StatusCode = statusCode;
    }
    
    public TValue Data { get; }
    
    public static ResponseViewModel<TValue> Set(TValue value, int statusCode) => 
        new(value, statusCode);
    
    public static ResponseViewModel<TValue> Ok(TValue value) => 
        new(value, 200);

    public static ResponseViewModel<TValue> Fail(string message, int statusCode) => 
        new(message, statusCode);
    
    public static ResponseViewModel<TValue> Fail(IEnumerable<string> messages, int statusCode) => 
        new(messages, statusCode);

    public static ResponseViewModel<TValue> Fail(Exception exception, int statusCode) =>
        new(string.Concat(exception?.Message, " - ", exception?.InnerException?.Message), statusCode);
}