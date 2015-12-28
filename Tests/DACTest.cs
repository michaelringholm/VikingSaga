using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using VikingSaga.Code;
using VikingSaga.Code.Campaign;
using System.IO;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using System.IO.Compression;
using VikingSaga.Code.Util;

namespace Tests
{
    [TestClass]
    public class DACTest
    {
        [TestMethod]
        public void UploadProfile()
        {
            String url = "http://80.167.157.40:81/viking-saga/save-profile.php";
            
            //client.UploadString("http://80.167.157.40:81/viking-saga", "Hello from .Net");
            //var test = client.DownloadString("http://80.167.157.40:81/viking-saga");

            var cardImageURL = @"heroes\warrior-hero.png";
            var map = MapFactory.CreateMap(MapFactory.MAP1);
            Hero hero = new Warrior { Name = "Ragnar", HP = 10, Mana = 5, Level = 1, XP = 0, Gold = 0, CardImageURL = cardImageURL};
            var profile = new VikingSagaUserProfile { Name = "ethlore", Password = "viking", Gold = 0, Deck = CardFactory.CreateCampaignStarterDeck(), SelectedHero = hero };
            //var deck = CardFactory.CreateCampaignStarterDeck();
            //var profile = new VikingSagaUserProfile { Name = "ethlore", Password = "viking", Gold = 0, SelectedHero = hero };
            profile.Heroes[0] = hero;

            var xml = SerializationHelper.Serialize(profile);
            var zippedData = ZipHelper.Zip(xml);
            String base64ZippedData = Convert.ToBase64String(zippedData);
            
            WebClient client = new WebClient();
            var response = client.DownloadString(url + "?profile_name=" + profile.Name + "&data=" + HttpUtility.UrlEncode(base64ZippedData));
            var unzippedData = ZipHelper.Unzip(Convert.FromBase64String(response));

           /* var json = new JavaScriptSerializer().Serialize(deck);
            WebClient client2 = new WebClient();
            var response2 = client.DownloadString(url + "?data=" + HttpUtility.UrlEncode(json));

            var res = client.UploadString(url, "mydata");
            var res2 = client.UploadFile(url, @"c:\windows\IE11_main.log");*/

            /*var reqparm = new NameValueCollection();
            reqparm.Add("param1", "somevalue");
            reqparm.Add("param2", "othervalue");
            var responsebytes = client.UploadValues(url, "GET", reqparm);
            var responsebody = (new UTF8Encoding()).GetString(responsebytes);

            string myParameters = "param1=value1&param2=value2&param3=value3";

            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                string HtmlResult = wc.UploadString(url + "?val33=jjj", myParameters);
            }*/

        }
    }
}
