namespace ChristmasCoding.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Flurl.Http;
    using Models;
    using Newtonsoft.Json.Linq;

    public class FacebookUserService
    {
        private readonly DatabaseContext db = new DatabaseContext();
        private const string FACEBOOK_API_URL = "https://graph.facebook.com/v2.8/";

        public async Task<FacebookUser> GetUser(string userId)
        {
            return await db.FacebookUsers.FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<bool> AddUser(FacebookUser user)
        {
            if (await db.FacebookUsers.FirstOrDefaultAsync(x => x.UserId == user.UserId) != null)
            {
                return await UpdateUser(user);
            }

            db.FacebookUsers.Add(user);
            await db.SaveChangesAsync();

            return await GetUser(user.UserId) != null;
        }

        public async Task<bool> UpdateUser(FacebookUser user)
        {
            var dbUser = await db.FacebookUsers.FirstOrDefaultAsync(x => x.UserId == user.UserId);
            
            dbUser.AuthToken = user.AuthToken;
            await db.SaveChangesAsync();

            return true;
        }

        public async Task<JObject> GetUsersName(string psid)
        {
            var dbUser = await db.FacebookUsers.FirstOrDefaultAsync(x => x.Psid == psid);

            if (dbUser == null)
            {
                return null;
            }

            var apiUrl = $"{FACEBOOK_API_URL}me?fields=first_name,last_name";

            var result = await apiUrl.WithHeader("Authorization", $"OAuth {dbUser.AuthToken}").GetJsonAsync<JObject>();
    
            return result;
        }

        public async Task<JObject> IsAuthed(string psid)
        {
            var dbUser = await db.FacebookUsers.FirstOrDefaultAsync(x => x.Psid == psid);
            var result = new JObject();

            if (dbUser == null)
            {
                return null;
            }
            
            var response = await $"{FACEBOOK_API_URL}me".AllowHttpStatus("200,401").WithHeader("Authorization", $"OAuth {dbUser.AuthToken}").GetAsync();

            result.Add("isAuthed", response.IsSuccessStatusCode);

            return result;
        }

        public async Task<IEnumerable<string>> GetUsersMovies(string psid)
        {
            IEnumerable<string> result = new List<string>();

            var dbUser = await db.FacebookUsers.FirstOrDefaultAsync(x => x.Psid == psid);

            if (dbUser == null)
            {
                return null;
            }

            var response = await $"{FACEBOOK_API_URL}me/video.watches".WithHeader("Authorization", $"OAuth {dbUser.AuthToken}").GetJsonAsync<JObject>();

            bool next;

            do
            {
                next = response?["paging"]["next"] != null;

                if (response == null) continue;

                result = result.Concat(ExtractMovies(response));

                if (next)
                {
                    response = await $"{response["paging"]["next"]}".WithHeader("Authorization", $"OAuth {dbUser.AuthToken}")
                                    .GetJsonAsync<JObject>();
                }
            } while (next);

            return result;
        }

        private IEnumerable<string> ExtractMovies(JObject response)
        {
            var movies = new List<string>();

            var data = response["data"].ToArray();

            foreach (var movie in data)
            {
                movies.Add(movie["data"]["movie"]["title"].ToString().Normalize());
            }

            return movies;
        }
    }
}
