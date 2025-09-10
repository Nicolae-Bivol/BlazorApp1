namespace BlazorApp1.Services;

public sealed class SendGridOptions
{
     public string? ApiKey { get; set; }
     public string? FromEmail { get; set; }
     public string? FromName { get; set; }
}
