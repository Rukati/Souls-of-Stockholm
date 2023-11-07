using System;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Configuration;
using Xamarin.Forms;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Xamarin.Essentials;
using System.Diagnostics.Tracing;

namespace ApiRequest
{
    public class Request
    {

        static string server = "https://souls-of-stockholm.onrender.com";

        //-------------------------------GET---------------------------------
        public class Posts
        {
            public int id { get; set; }
            public string name { get; set; }
            public string content { get; set; }
            public User user { get; set; }
        }//                                             |
        static public async Task<List<Posts>> GetAllPosts()//Получение всех постов
        {
            var client = new HttpClient();

            var result = client.GetAsync($"{server}/api/posts").Result;
            Console.WriteLine((int)result.StatusCode);
            var _content = await result.Content.ReadAsStringAsync();

            List<Posts> post = System.Text.Json.JsonSerializer.Deserialize<List<Posts>>(_content);
            return post;
        }
        //-------------------------------------------------------------------
        public class Client
        {
            public int id { get; set; }
            public string username { get; set; }
            public string gender { get; set; }
            public string country { get; set; }
            public int age { get; set; }
        }
        static public async Task<List<Client>> GetInfoProfile()//Получение данных своего профиля
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await SecureStorage.GetAsync("token"));

            var result = client.GetAsync($"{server}/api/authorization/").Result;
            var _content = await result.Content.ReadAsStringAsync();
            var _result = System.Text.Json.JsonSerializer.Deserialize<List<Client>>(_content);
            return _result;
        }
        //--------------------------------------------------------------------
        public class User
        {
            public string username { get; set; }
            public int id { get; set; }
        }//                                              |
        public class Post
        {
            public int id { get; set; }
            public string name { get; set; }
            public string content { get; set; }
            public User user{ get; set; }
        }//                                              |
        public class Comments
        {
            public int id { get; set; }
            public string content { get; set; }
            public int post { get; set; }
            public User user { get; set; }

        }//                                          |
        public class Tags
        {
            public string name { get; set; }
        }
        public class Root
        {
            public Post post { get; set; }
            public Comments[] comments { get; set; }
            public Tags[] tags { get;set; }
        }//                                              |
        static public async Task<Root> GetInfoOnePost(int id)//Получение одного поста
        {
            var client = new HttpClient();
            var result = client.GetAsync($"{server}/api/post/{id}/").Result;
            Console.WriteLine((int)result.StatusCode);
            var _content = await result.Content.ReadAsStringAsync();
            var post = JsonConvert.DeserializeObject<Root>(_content);
            return post;
        }
        //--------------------------------------------------------------------
        public class OtherProfile
        {
            public int id { get; set; }
            public string username { get; set; }
            public int age { get; set; }
            public string gender { get; set; }
            public string country { get; set; }
            public bool is_staff { get; set; }
            public bool is_superuser { get; set; }
        }//                                     |
        static public async Task<OtherProfile> GetInfoOtherProfile(int id)//Получение данных других профилей
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await SecureStorage.GetAsync("token"));

            var result = client.GetAsync($"{server}/api/user/{id}/").Result;
            var _content = await result.Content.ReadAsStringAsync();
            var information = JsonConvert.DeserializeObject<OtherProfile>(_content);
            return information;
        }
        //--------------------------------------------------------------------


        //-------------------------------POST---------------------------------
        public class TokenAuthorization
        {
            public string refresh { get; set; }
            public string access { get; set; }
        } //                                |
        static public async Task<int> PostAuthentication(string username, string password)//Авторизация
        {
            var client = new HttpClient();
            string jsonContent = $@"{{
                ""username"": ""{username}"",
                ""password"": ""{password}""
            }}";

            var content = new StringContent(jsonContent, null, "application/json");

            var result = client.PostAsync($"{server}/api/token/", content).Result;
            var Authorization = System.Text.Json.JsonSerializer.Deserialize< TokenAuthorization>(await result.Content.ReadAsStringAsync());
            await SecureStorage.SetAsync("token", Authorization.access);
            return (int)result.StatusCode;
        }
        //--------------------------------------------------------------------
        static public async Task<int>PostCreatePost(string name, string content, int user)//Создание поста
        {
            string contentUser = $@"{{
                ""name"":""{name}"",
                ""content"":""{content}"",
                ""author"":""{user}""
            }}";

            var body = new StringContent(contentUser, null, "application/json");

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await SecureStorage.GetAsync("token"));
            
            var result = client.PostAsync($"{server}/api/posts/create", body).Result;
            var _content = await result.Content.ReadAsStringAsync();
            return (int)result.StatusCode;
        }//              |
        //--------------------------------------------------------------------
        static public async Task<int> PostAddComment(string content,int user, int post)//Добавление комментов
        {

            string contentUser = $@"{{
                ""content"": ""{content}"",
                ""user"": ""{user}"",
                ""post"": ""{post}""
            }}";

            var body = new StringContent(contentUser, null, "application/json");

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await SecureStorage.GetAsync("token"));

            var result = client.PostAsync($"{server}/api/post/comment/add", body).Result;
            return (int)result.StatusCode;
        }//        |
        //--------------------------------------------------------------------
    }
}
