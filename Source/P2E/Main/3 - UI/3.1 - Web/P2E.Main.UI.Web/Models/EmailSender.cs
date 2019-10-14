using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace P2E.Main.UI.Web.Models
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message) => Task.CompletedTask;
    }
}
