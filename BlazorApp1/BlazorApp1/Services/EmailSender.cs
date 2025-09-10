using BlazorApp1.Infrastructure.Data;     // ApplicationUser
using Microsoft.AspNetCore.Identity;      // IEmailSender<TUser>
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace BlazorApp1.Services;

public sealed class EmailSender : IEmailSender<ApplicationUser>
{
     private readonly SendGridOptions _opt;
     private readonly ILogger<EmailSender> _logger;

     public EmailSender(IOptions<SendGridOptions> options, ILogger<EmailSender> logger)
     {
          _opt = options.Value;
          _logger = logger;
     }

     public Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
         => SendAsync(email, "Confirmă-ți contul",
             $@"<p>Salut, {user.UserName}!</p>
               <p>Confirmă contul dând click aici:
               <a href=""{confirmationLink}"">Confirmă</a></p>");

     public Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink)
         => SendAsync(email, "Resetare parolă",
             $@"<p>Salut!</p>
               <p>Poți reseta parola aici:
               <a href=""{resetLink}"">Resetează parola</a></p>");

     public Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode)
         => SendAsync(email, "Cod resetare parolă",
             $@"<p>Codul tău de resetare este:
               <strong>{resetCode}</strong></p>");

     // ===== helper comun =====
     private async Task SendAsync(string email, string subject, string htmlMessage)
     {
          if (string.IsNullOrWhiteSpace(_opt.ApiKey))
               throw new InvalidOperationException("SendGrid ApiKey nu este setată (user-secrets).");

          if (string.IsNullOrWhiteSpace(_opt.FromEmail))
               throw new InvalidOperationException("SendGrid FromEmail nu este configurat.");

          var client = new SendGridClient(_opt.ApiKey);

          var from = new EmailAddress(_opt.FromEmail, _opt.FromName ?? "BlazorApp1");
          var to = new EmailAddress(email);

          var msg = MailHelper.CreateSingleEmail(
              from, to, subject,
              plainTextContent: StripHtml(htmlMessage),
              htmlContent: htmlMessage);

          // opțional: fără click tracking
          msg.SetClickTracking(false, false);

          var resp = await client.SendEmailAsync(msg);

          _logger.LogInformation("SendGrid response: {StatusCode}", resp.StatusCode);
          if ((int)resp.StatusCode >= 400)
          {
               var body = await resp.Body.ReadAsStringAsync();
               _logger.LogError("SendGrid error: {Body}", body);
          }
     }

     private static string StripHtml(string html) =>
         string.IsNullOrEmpty(html)
             ? ""
             : System.Text.RegularExpressions.Regex.Replace(html, "<.*?>", string.Empty);
}
