using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Infrastructure.Services.Identity;
using Core.ViewModels;
using Core.Entities;
using Infrastructure.Services.EmailService;
using Core.Interfaces;
using Core.DTOs;
using Infrastructure.Services.Identity.JWT;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using System;

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
        private JwtMediateR _jwtMediatR;
        public Account(Microsoft.AspNetCore.Identity.UserManager<User> aspUserManager, IEmailSender emailSender, JwtMediateR jwtMediatR)
        {
            _aspUserManager = aspUserManager;
            _userManager = new UserManager(_aspUserManager);
            _emailSender = emailSender;
            _jwtMediatR = jwtMediatR;
        }
        // GET: api/<Account>
        [HttpGet]
        [Route("api/account/ConfirmEmail/{username}/{token}", Name = "ConfirmEmailAction")]
        public async Task<IActionResult> ConfirmEmail(string username, string token)
        {
            var res = await _userManager.ConfirmEmail(username, token);
            if (res)
            {
                return Content("ایمیل با موفق تایید شد");
            }
            return Content("عملیات موفقیت آمیز نبود");

        }

        [Route("/api/account/login")]
        [HttpPost]
        public async Task<AuthenticationResponse> AuthenticateAsync([FromBody] LoginCommand lcmd)
        {
            var res = await _jwtMediatR.AuthenticateAsync(lcmd);
            return res;
        }

        [Route("/api/account/refrshtoken")]
        [HttpPost]
        public async Task<AuthenticationResponse> RefreshTokenAsync([FromBody] RefreshCommand refcmd)
        {
            return await _jwtMediatR.RefreshToken(refcmd.Token);
        }

        // POST api/<Account>
        [HttpPost]
        [Route("/api/account/register")]
        public async Task<RegisterationResponse> Post([FromBody] RegisterationCommand rcmd)
        {
            var res = await _userManager.Register(rcmd);
            if (res.Success)
            {
                SendEmailAsync(rcmd.Email, rcmd.UserName, res.EmailConfirmationToken);
            }
            res.EmailConfirmationToken = "";
            return res;
        }

        // PUT api/<Account>/5  
        [Route("/api/account/test")]
        [HttpGet]
        [EnableCors]
        public IActionResult Test()
        {
            var demo = User.Identity.Name;
            return new JsonResult("Dude");
        }

        [Authorize]
        [HttpGet]
        [Route("/api/account/isAuth")]
        public IActionResult IsAuth()
        {
            return new JsonResult(true);
        }


        private void SendEmailAsync(string email,string UserName, string token)
        {
            string serverUrl = Url.Link("ConfirmEmailAction", new
            {
                username = UserName,
                token=token

            });
            string body = $"<h1> سلام {UserName} عزیز </h1><br/>" +
        $"<h4> تشکر می کنیم که از پیام رسان پوشرز استفاده می کنید . <br/>" +
        $" شما همچمنین می توانید از طریق راه های زیر با ما در ارتباط باشید <br/>" +
        $"برنامه نویس :<br/>" +
        $"آیدی ایتا : Mamin84 :<br/>" +
        $"آیدی تلگرام : M_AminK :<br/>" +
        $"دیزاینر :<br/>" +
        $" آیدی تلگرام : @Mohammadmd1383 </h4></br></br>" +
        $"<h3><a href='{serverUrl}' >تایید ایمیل </a></h3>";
            _emailSender.SendEmail("Amin@karvizi1384", email, "تایید ایمیل", body, true, "pmsg@pushers.ir", 25, "webmail.pushers.ir");
        }
    }
}
