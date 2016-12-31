namespace ChristmasCoding.Web.Controllers
{
    using System;
    using System.Web.Mvc;
    using Models;

    public class FacebookController : Controller
    {
        private const string FACEBOOK_API = "https://graph.facebook.com/v2.8/";

        private const string PAGE_ACCESS =
            "EAAXDhmiYX6wBAIzBZC7XfIcw4vcZAJvFF7AcCTwMZAgMCqjEpY4uL2zQEIFjDX3njQyOwBmgKp10sUZAp8PfeOkgAKddfWJmZCRpRgcFATEN3ZB0ZB8BFfQhT2knaUFZAuIpmGFjSuJK24NqBbBlniqRRzRpd0qkVG9ocXMKATTXWAZDZD";

        // GET: Facebook
        public ViewResult Link(string redirect_uri, string account_linking_token)
        {
            var rng = new Random();

            return View(new AccountLink
            {
                RedirectUri = $"{redirect_uri}&authorization_code={rng.Next()}",
                PsidRetreivalUrl = $"{FACEBOOK_API}me?access_token={PAGE_ACCESS}&fields=recipient&account_linking_token={account_linking_token}"
            });
        }

        public ViewResult ReAuth()
        {
            return View();
        }
    }
}