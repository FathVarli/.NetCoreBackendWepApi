using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using Business.Abstract;
using Core.CrossCuttingConcerns.Logging.Log4Net;
using Core.Utilities.Results;
using Entity.Dtos;

namespace MentalBit.KutuphaneSistemi.Business.Concrete
{
   public class MailManager : IMailService
    {
        private ILoggerService _loggerService;

        public MailManager(ILoggerService loggerService)
        {
            _loggerService = loggerService;
        }

        public IResult SendMail(EmailSenderModel model)
        {

            try
            {
                var to = model.To;
                var subject = model.Subject;
                var body = model.Body;
                var mail = new MailMessage();
                mail.To.Add(to);
                mail.Subject = subject;
                mail.Body = body;
                mail.From = new MailAddress("fath.varl@gmail.com");
                mail.IsBodyHtml = false;
                var smtp = new SmtpClient
                {
                    Port = 587,
                    UseDefaultCredentials = false,
                    EnableSsl = true,
                    Credentials = new System.Net.NetworkCredential("fath.varl@gmail.com", "TR1923")
                };
                smtp.Host = "smtp.gmail.com";
                smtp.Send(mail);
                return new SuccessResult("Mail send success");
            }
            catch (Exception)
            {
                _loggerService.Error("Error to send mail");
                return new ErrorResult("Error to send mail!");
            }
        }
    }
}
