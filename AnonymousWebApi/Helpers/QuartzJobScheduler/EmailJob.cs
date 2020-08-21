using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace AnonymousWebApi.Helpers.QuartzJobScheduler
{
    [DisallowConcurrentExecution]
    public class EmailJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            //using (var message = new MailMessage("vinayvalsan2018@gmail.com", "vinayvvv444@gmail.com"))
            //{
            //    message.Subject = "Test";
            //    message.Body = "Test at " + DateTime.Now;
            //    using (SmtpClient client = new SmtpClient
            //    {
            //        EnableSsl = true,
            //        Host = "smtp.gmail.com",
            //        Port = 587,
            //        Credentials = new NetworkCredential("vinayvalsan2018@gmail.com", "**********")
            //    })
            //    {
            //        client.Send(message);
            //    }
            //}
            return Task.CompletedTask;
        }
    }
}
