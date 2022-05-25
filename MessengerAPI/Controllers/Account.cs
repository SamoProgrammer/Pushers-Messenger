using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Infrastructure.Services.Identity;
using Core.ViewModels;
using Core.Entities;
using Infrastructure.Services.EmailService;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MessengerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Account : ControllerBase
    {
        private UserManager _userManager;
        private IEmailSender _emailSender;
        private Microsoft.AspNetCore.Identity.UserManager<User> _aspUserManager;
        public Account(Microsoft.AspNetCore.Identity.UserManager<User> aspUserManager, IEmailSender emailSender)
        {
            _aspUserManager = aspUserManager;
            _userManager = new UserManager(_aspUserManager);
            _emailSender = emailSender;
        }
        // GET: api/<Account>
        [HttpGet]
        [Route("api/account/ConfirmEmail/{username}/{token}")]
        public async Task ConfirmEmail(string username, string token)
        {
            await _userManager.ConfirmEmail(username, token);
        }

        // GET api/<Account>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<Account>
        [HttpPost]
        public async Task<RegisterationResponse> Post([FromBody] RegisterationCommand registercmd)
        {
            var res = await _userManager.Register(registercmd);
            if (res.Success)
            {
                SendEmailAsync(registercmd.UserName, res.EmailConfirmationToken);
            }
            res.EmailConfirmationToken = "";
            return res;
        }

        // PUT api/<Account>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<Account>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }



        private void SendEmailAsync(string UserName, string token)
        {
            string body = $"<h1> سلام {UserName} عزیز </h1><br/>" +
        $"<h4> تشکر می کنیم که از پیام رسان پوشرز استفاده می کنید . <br/>" +
        $" شما همچمنین می توانید از طریق راه های زیر با ما در ارتباط باشید <br/>" +
        $"برنامه نویس :<br/>" +
        $"آیدی ایتا : Mamin84 :<br/>" +
        $"آیدی تلگرام : M_AminK :<br/>" +
        $"دیزاینر :<br/>" +
        $" آیدی تلگرام : @Mohammadmd1383 </h4></br></br>" +
        $"<h3><a href='{Url.ActionLink("ConfirmEmail")}/{UserName}/{token}' >تایید ایمیل </a></h3>";
            _emailSender.SendEmail("Callofdutyblackops4", "arcamingamer@gmail.com", "تایید ایمیل", body, true, "aminkarvizi1384@gmail.com", 587, "smtp.gmail.com");
        }
    }
}
