using BIS_project.AppData;
using BIS_project.Dtos;
using BIS_project.Helper;
using BIS_project.IServices;
using BIS_project.Models;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using MimeKit;

namespace BIS_project.Services;

public class AuthService : IAuthService
{
    private readonly DataContext _dbContext;    
    private readonly IConfiguration _configuration;

    public AuthService(DataContext data, IConfiguration config)
    {
        _dbContext = data;
        _configuration = config;
    }
    public async  Task<User> UserLogin(UserDto request)
    {
        try
        {
            var user = await _dbContext.Users
                .Include(e=> e.Role)
                .FirstOrDefaultAsync(e =>
                    e.UserName == request.Username &&
                    e.IsActive);
            return user;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
       
       
    }
    public async Task<User> UserRegister(UserRegisterDto request)
    {
        try
        {
            var user = new User
            {
                UserName = request.Username,
                Password = request.Password,
                Firstname = request.Firstname,
                Role = await _dbContext.Roles.FirstOrDefaultAsync(e => e.Role_name == "USER"),
                IsActive = true
            };
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);   
            return null;
        }
    }
    public async Task<User> DriverRegister(UserRegisterDto request)
    {
        try
        {
            var existUser = await _dbContext.Users.Where(e => e.UserName == request.Username).FirstOrDefaultAsync();
            if (existUser != null)
            {
                return new User();
            }
            var user = new User
            {
                UserName = request.Username,
                Password = request.Password,
                Firstname = request.Firstname,
                Role = await _dbContext.Roles.FirstOrDefaultAsync(e => e.Role_name == "DRIVER"),
                IsActive = true
            };
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            await DriverMail(user.UserName, user.UserName, user.Firstname, user.Password);
            return user;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
    
   public async Task<APIResponse> ForgotPassword(string mail)
   {
       try
       {
           var user = await _dbContext.Users.FirstOrDefaultAsync(e => e.UserName == mail);
           if (user == null)
           {
               return new APIResponse(400, null, "User not found!!");
           }
           var newPassword = Guid.NewGuid().ToString().Substring(0, 8);
           user.Password = newPassword;
           await _dbContext.SaveChangesAsync();
           await SendEmail(user.UserName, user.UserName, user.Firstname, newPassword);
           return new APIResponse(200, user, "");

       }
       catch(Exception e)
       {
           Console.WriteLine(e);
           return null;
       }
   }
   private async Task  SendEmail(string toEmail, string username, string Name, string password)
   {
       try 
       {
           var message = new MimeMessage();
           message.From.Add(new MailboxAddress("SilkRoute Express", "abdugafforovazimjon33@gmail.com"));
           message.To.Add(new MailboxAddress(username, toEmail));
           message.Subject = "Password Reset";
           var bodyBuilder = new BodyBuilder();
           bodyBuilder.TextBody = $"Dear {Name},\n\nYou have requested to change the password\n\n" +
                                  $"Username: **{username}**\nPassword: **{password}**" +
                                  $"\n\nYou can enter to you account and change to the new one if you wish" +
                                  $"\n\nWe appreciate your trust in our service and are " +
                                  $"here to assist you with any questions or concerns. If you have any feedback or need assistance, please don't " +
                                  $"hesitate to contact us.\n\nBest regards,\nThe SilkRoute Express Team";
           message.Body = bodyBuilder.ToMessageBody();
           using (var client = new SmtpClient())
           {
               await client.ConnectAsync("smtp.gmail.com", 587, false);
               await client.AuthenticateAsync("abdugafforovazimjon33@gmail.com", "fzazphmwefownyub");
               await client.SendAsync(message);
               await client.DisconnectAsync(true);
           }
       }
       catch (Exception e)
       {
           Console.WriteLine(e);
       }
   }
   private async Task DriverMail(string toEmail, string username, string Name, string password)
   {
       try 
       {
           var message = new MimeMessage();
           message.From.Add(new MailboxAddress("SilkRoute Express", "abdugafforovazimjon33@gmail.com"));
           message.To.Add(new MailboxAddress(username, toEmail));
           message.Subject = "SilkRoute Express Driver";
           var bodyBuilder = new BodyBuilder();
           bodyBuilder.TextBody = $"Dear {Name},\n\nYou have requested to become the driver at SilkRoute Express\n\n" +
                                  $"Username: **{username}**\nPassword: **{password}**" +
                                  $"\n\nYou can enter to you account and change to the new one if you wish" +
                                  $"\n\nWe appreciate your trust in our company and are " +
                                  $"here to assist you with any questions or concerns. If you have any feedback or need assistance, please don't " +
                                  $"hesitate to contact us.\n\nBest regards,\nThe SilkRoute Express Team";
           message.Body = bodyBuilder.ToMessageBody();
           using (var client = new SmtpClient())
           {
               await client.ConnectAsync("smtp.gmail.com", 587, false);
               await client.AuthenticateAsync("abdugafforovazimjon33@gmail.com", "fzazphmwefownyub");
               await client.SendAsync(message);
               await client.DisconnectAsync(true);
           }
       }
       catch (Exception e)
       {
           Console.WriteLine(e);
       }
   }

    public Task<object> RefreshToken()
    {
        throw new NotImplementedException();
    }
}