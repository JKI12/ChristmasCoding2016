namespace ChristmasCoding.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Data;
    using Data.Models;
    using Newtonsoft.Json.Linq;

    public class FUserController : ApiController
    {
        private readonly FacebookUserService fus = new FacebookUserService();

        public async Task<IHttpActionResult> Post(FacebookUser user)
        {
            if (!ModelState.IsValid || user == null)
            {
                return BadRequest();
            }

            var result = await fus.AddUser(user);

            switch (result)
            {
                case true:
                    return Ok();
                default:
                    return BadRequest();
            }
        }

        // Get Users Likes (Add route) -> User Id -> /me returns IHttpAction

        // Get Name
        [Route("api/fuser/GetName/{id}")]
        public async Task<IHttpActionResult> GetName(string id)
        {
            var result = await fus.GetUsersName(id);

            if (result == null)
            {
                return BadRequest("User doesn't exist");
            }

            return Ok(JObject.FromObject(result));
        }

        [Route("api/fuser/GetAuth/{id}")]
        public async Task<IHttpActionResult> GetAuth(string id)
        {
            var result = await fus.IsAuthed(id);

            if (result == null)
            {
                return BadRequest("User doesn't exist");
            }

            return Ok(result);
        }
    }
}
