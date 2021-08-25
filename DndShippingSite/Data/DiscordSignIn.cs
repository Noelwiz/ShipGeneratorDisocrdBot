using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DndShippingSite.Data
{
    /// <summary>
    /// based on https://www.yogihosting.com/discord-api-asp-net/ 
    /// </summary>
    public class DiscordSignIn
    {
        public IConfiguration Configuration { get; }

        public DiscordSignIn(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void SignIn()
        {
            string client_id = Configuration["client_id"];
            string client_sceret = Configuration["client_secret"];
            string code = "identify";
            string redirect_url = Configuration["redirect_url"];

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("https://discordapp.com/api/oauth2/token");
            webRequest.Method = "POST";
            string parameters = "client_id=" + client_id + "&client_secret=" + client_sceret + "&grant_type=authorization_code&code=" + code + "&redirect_uri=" + redirect_url + "";
            byte[] byteArray = Encoding.UTF8.GetBytes(parameters);
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.ContentLength = byteArray.Length;
            Stream postStream = webRequest.GetRequestStream();

            postStream.Write(byteArray, 0, byteArray.Length);
            postStream.Close();
            WebResponse response = webRequest.GetResponse();
            postStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(postStream);
            string responseFromServer = reader.ReadToEnd();

            string tokenInfo = responseFromServer.Split(',')[0].Split(':')[1];
            string access_token = tokenInfo.Trim().Substring(1, tokenInfo.Length - 3);
        }
    }

    /*
    public string GetUserId()
    {
        //Do http get request to the URL to get the client info in json
        HttpWebRequest webRequest1 = (HttpWebRequest)WebRequest.Create("https://discordapp.com/api/users/@me");
        webRequest1.Method = "Get";
        webRequest1.ContentLength = 0;
        //webRequest1.Headers.Add("Authorization", "Bearer " + access_token);
        webRequest1.ContentType = "application/x-www-form-urlencoded";

        string apiResponse1 = "";
        using (HttpWebResponse response1 = webRequest1.GetResponse() as HttpWebResponse)
        {
            StreamReader reader1 = new StreamReader(response1.GetResponseStream());
            apiResponse1 = reader1.ReadToEnd();
        }
        ///*End

        return apiResponse1; //todo
    }
    */
}
