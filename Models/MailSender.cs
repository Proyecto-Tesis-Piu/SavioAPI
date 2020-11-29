using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace MonetaAPI.Models
{
    public static class MailSender
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/gmail-dotnet-quickstart.json
        private static string[] Scopes = { GmailService.Scope.GmailReadonly, GmailService.Scope.GmailSend, GmailService.Scope.GmailCompose, GmailService.Scope.MailGoogleCom };
        private static string ApplicationName = "Moneta API";

        public static void ConfirmMail(string toAddress, string token) {
            UserCredential credential;

            using (var stream =
                new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Gmail API service.
            var service = new GmailService(new BaseClientService.Initializer()
            {
                //ApiKey = "AIzaSyAau3j9w4M4PH5uSUAftJJbrZW6loQZvqE",
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            #region send Message

            //string htmlBody = "<html><body><h1>Alternative Picture</h1><br><img src=\"cid:filename\"></body></html>";
            //AlternateView avHtml = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);
            //LinkedResource inline = new LinkedResource("filename.jpg", MediaTypeNames.Image.Jpeg);
            //inline.ContentId = Guid.NewGuid().ToString();
            //avHtml.LinkedResources.Add(inline);

            MailMessage msg = new MailMessage();
            msg.Subject = "test mail";
            msg.From = new MailAddress("contact@moneta.studio", "Contacto Moneta");
            //msg.To.Add(new MailAddress("erickpacheco114@hotmail.com"));
            //msg.To.Add(new MailAddress("ins-42pdx1n6@isnotspam.com"));
            msg.To.Add(new MailAddress(toAddress));
            //msg.AlternateViews.Add(avHtml);
            msg.BodyTransferEncoding = TransferEncoding.Base64;


            Attachment att = new Attachment(Environment.CurrentDirectory + "\\Assets\\logo.png");
            att.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
            att.ContentDisposition.FileName = "logo.png";
            att.ContentType = new ContentType("image/png");
            att.TransferEncoding = TransferEncoding.Base64;
            att.Name = "logo.png";

            msg.Body = getHtmlBody_confirmEmail(att.ContentId, token);
            msg.IsBodyHtml = true;
            msg.Attachments.Add(att);

            var mimeMessage = MimeKit.MimeMessage.CreateFromMailMessage(msg);

            MemoryStream buffer = new MemoryStream();
            mimeMessage.WriteTo(buffer);
            buffer.Position = 0;

            StreamReader sr = new StreamReader(buffer);
            string rawString = sr.ReadToEnd();

            Message message = new Message
            {
                Raw = Base64UrlEncode(rawString)
            };

            message = service.Users.Messages.Send(message, "contact@moneta.studio").Execute();
            #endregion
        }

        private static string getHtmlBody_confirmEmail(string contentId, string token) {
            string tokenEncoded = HttpUtility.UrlEncode(token);
            string body = "<body style=\"background: #F8F8F8\"><div style=\"background-color: #6a20e1; border-radius: 25px; width: 700px; height: fit-content; padding: 15px 0 25px;\"><img style=\"display: block; margin: 0 auto 20px;\" src=\"cid:{0}\" width=\"100\" height=\"120\"><div style=\"background-color: #fff; width: 100%; padding-top: 20px; padding-bottom: 20px;\"><h2 style=\"margin-left:10%; margin-bottom: 5%\">Gracias por unirte a Moneta Studio!</h2><p style=\"margin-left: 10%;\">Verifica tu cuenta para tener acceso a mas herramientas dentro de Moneta Studio.</p><p style=\"margin-left: 10%;\">Da click en el boton de abajo para confirmar tu correo.</p><a style=\"margin: 0 auto; display: block; border-radius: 25px; color: #ffffff; padding: 10px; background-color: #6a20e1; width: fit-content; cursor: pointer\" href=\"https://www.moneta.studio/user/ConfirmMail/{1}\">Confirmar Correo</a></div></div></body>";
            body = String.Format(body, contentId, tokenEncoded);
            return body;
        }

        private static string Base64UrlEncode(string input)
        {
            var inputBytes = Encoding.UTF8.GetBytes(input);
            // Special "url-safe" base64 encode.
            return Convert.ToBase64String(inputBytes)
              .Replace('+', '-')
              .Replace('/', '_')
              .Replace("=", "");
        }


    }
}
