using BusinessLayer.Interface;
using Microsoft.Extensions.Configuration;
using ModelLayer.Entity;
using ModelLayer.RequestBody;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class UserServiceBusinessLogic : IUserBusiness
    {
        private static string otp;
        private static string email;
        private static User entity;


        private readonly IUserRepo userRepo;

        public UserServiceBusinessLogic(IUserRepo userRepo)
        {
            this.userRepo = userRepo;
        }

        public Task<int> createUser(UserBody entity)
        {
            return userRepo.createUser(entity);
        }

        public Task<string> Login(string email, string password, IConfiguration configuration)
        { 
            return userRepo.Login(email, password, configuration);
        }

        public Task<User> GetUserByEmail(string email)
        { 
            return userRepo.GetUserByEmail(email);
        }

        public async Task ForgotPassword(string email)
        {

            var user = await GetUserByEmail(email);
            if (user != null)
            {
                var Otp = GenerateOneTimePassword();

                UserRepoImpl.Otp = Otp;
                UserRepoImpl.Email = email;
                SendPasswordResetEmail(email, Otp);
            }
            else
            {
                // Handle case where email does not exist in the database
                throw new ArgumentException("User with provided email does not exist.");
            }
        }


        private string GenerateOneTimePassword()
        {
            // Generate a random six-digit OTP
            Random random = new Random();
            int otp = random.Next(100000, 999999);

            return otp.ToString();

        }

        //-----------------------------------

        public async Task SendPasswordResetEmail(string email, string Otp)
        {
            System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
            try
            {
                mailMessage.From = new System.Net.Mail.MailAddress("pdshashank8@outlook.com", "BookStore Management");
                mailMessage.To.Add(email);
                mailMessage.Subject = "Change password for BookStore";
                mailMessage.Body = "This is your otp please enter to change password " + Otp;
                mailMessage.IsBodyHtml = true;
                System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient("smtp-mail.outlook.com");

                // Specifies how email messages are delivered. Here Email is sent through the network to an SMTP server.
                smtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;

                // Set the port for Outlook's SMTP server
                smtpClient.Port = 587; // Outlook SMTP port for TLS/STARTTLS

                // Enable SSL/TLS
                smtpClient.EnableSsl = true;

                string loginName = "pdshashank8@outlook.com";
                string loginPassword = "PDshashank@123";

                System.Net.NetworkCredential networkCredential = new System.Net.NetworkCredential(loginName, loginPassword);
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = networkCredential;

                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught: " + ex.Message);
            }
            finally
            {
                mailMessage.Dispose();
            }
        }

        public Task ResetPassword(string email, string otp, string newPassword)
        {
            return userRepo.ResetPassword(email, otp, newPassword);
        }


    }
}
