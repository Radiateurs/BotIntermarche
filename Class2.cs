using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Web;
using Newtonsoft.Json;

namespace BotDriveCart
{
    class WebNavigator
    {
        public class WebCookie
        {
            public CookieContainer FormatReceivedCookies(string SetCookie)
            {
                CookieContainer ret = new CookieContainer();
                string[] cookies;
                string[] sep = { ";,", "/," };

                if (SetCookie == null)
                    return (null);
                cookies = SetCookie.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                foreach (string detailedCookie in cookies)
                {
                    Cookie newCookie = new Cookie();
                    string[] informations;

                    if (!detailedCookie.Contains("HttpOnly"))
                    {
                        informations = detailedCookie.Split(';');
                        newCookie.Name = informations[0].Split('=')[0];
                        newCookie.Value = informations[0].Split('=')[1];
                        newCookie.Domain = "drive.intermarche.com";
                        foreach (string eachInfo in informations)
                        {
                            if (eachInfo.Contains("expires"))
                                newCookie.Expires = Convert.ToDateTime(eachInfo.Split('=')[1]);
                            if (eachInfo.Contains("path") || eachInfo.Contains("Path"))
                                newCookie.Path = eachInfo.Split('=')[1];
                        }
                        ret.Add(newCookie);
                    }
                }
                return (ret);
            }

            public void DumpCookie(CookieContainer cookiesToPrint, Uri domain)
            {
                CookieCollection cookies = cookiesToPrint.GetCookies(domain);

                for (int i = 0; i < cookies.Count; i++)
                {
                    Console.WriteLine(cookies[i].Name + " : " + cookies[i].Value + "; Path : " + cookies[i].Path);
                }
            }
            public void DumpCookie(CookieCollection cookies)
            {
                for (int i = 0; i < cookies.Count; i++)
                {
                    Console.WriteLine(cookies[i].Name + " : " + cookies[i].Value + "; Path : " + cookies[i].Path);
                }
            }
        }

        public WebCookie cookieManager { set ; get;}
        public string data { set; get; }
        public CookieContainer cookie { set; get; }
        public CookieContainer usrCookie { set; get; }
        public CookieCollection usrCookieCollection { set; get; }
        public UserInfo usr { set; get; }
        public string usrCookieHeaders { set; get; }
        bool connected;
        public HttpWebRequest request;
        public HttpWebResponse response;

        public WebNavigator()
        {
            cookie = new CookieContainer();
            usr = new UserInfo();
            connected = false;
            cookieManager = new WebCookie();
        }

        public bool ConnectUserToDrive()
        {
            request = (HttpWebRequest)WebRequest.Create(new Uri(usr.getChoosenMarket()));
            request.Proxy = new WebProxy("127.0.0.1:8080");
            request.Method = "GET";
            request.CookieContainer = cookie;
            response = (HttpWebResponse)request.GetResponse();
            usrCookie = cookieManager.FormatReceivedCookies(response.Headers["Set-cookie"]);
            if (cookie == null)
            {
                Console.WriteLine("No cookie received. Check your connection.");
                return (false);
            }
            request = (HttpWebRequest)WebRequest.Create(new Uri(usr.getConnexionUrl()));
            request.Method = "POST";
            request.ContentType = "application/json; charset=utf-8";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36";
            request.Referer = usr.getChoosenMarket();
            request.Host = "drive.intermarche.com";
            request.Proxy = new WebProxy("127.0.0.1:8080");
            request.AllowAutoRedirect = false;
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.KeepAlive = true;
            request.CookieContainer = cookie;
            StringBuilder beforeData = new StringBuilder("{'txtEmail':" + String.Format("'{0}',", usr.getMail()) + "'txtMotDePasse':" + String.Format("'{0}',", usr.getPasswd()) + "'largeur':'1920','hauteur':'1080','resteConnecte':'false'}");
            request.ContentLength = beforeData.ToString().Length;
            using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(beforeData.ToString());
                writer.Close();
            }
            response = (HttpWebResponse)request.GetResponse();
            using (StreamReader read = new StreamReader(response.GetResponseStream()))
            {
                data = read.ReadToEnd();
                read.Close();
            }
            Console.WriteLine(data);
            if (response.StatusCode != HttpStatusCode.OK)
                return (false);
            connected = true;
            return (true);
        }

        public bool DeconnectUserToDrive()
        {
            request = (HttpWebRequest)WebRequest.Create(new Uri(usr.getDeconnexionUrl()));
            request.Method = "POST";
            request.ContentType = "application/json; charset=utf-8";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36";
            request.Referer = usr.getChoosenMarket();
            request.Host = "drive.intermarche.com";
            request.Proxy = new WebProxy("127.0.0.1:8080");
            request.AllowAutoRedirect = false;
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.KeepAlive = true;
            request.CookieContainer = cookie;
            request.ContentLength = 0;
            response = (HttpWebResponse)request.GetResponse();
            using (StreamReader read = new StreamReader(response.GetResponseStream()))
            {
                data = read.ReadToEnd();
                read.Close();
            }
            Console.WriteLine(data);
            if (response.StatusCode != HttpStatusCode.OK)
                return (false);
            connected = true;
            return (true);
        }

        public bool AddWishListToCart(WishList wish)
        {
            if (!connected)
                return (false);
            for (int i = 0; i < wish.Lenght(); i++)
            {
                if (!AddItemToCart(wish._items[i]))
                    return (false);
            }
            return (true);
        }

        public bool AddItemToCart(WishList.Item item)
        {
            request = (HttpWebRequest)WebRequest.Create(new Uri(usr.getAddingItem()));
            request.Method = "POST";
            request.ContentType = "application/json; charset=utf-8";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36";
            request.Referer = usr.getChoosenMarket();
            request.Host = "drive.intermarche.com";
            request.AllowAutoRedirect = true;
            request.Accept = "application / json, text / javascript, */*; q=0.01";
            request.KeepAlive = true;
            request.Proxy = new WebProxy("127.0.0.1:8080");
            request.CookieContainer = cookie;
            StringBuilder beforeData = new StringBuilder();
            beforeData.Append("{'idProduit':'" + item.idProduit + "','trackingCode':''}");
            request.ContentLength = beforeData.ToString().Length;
            using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(beforeData.ToString());
            }
            response = (HttpWebResponse)request.GetResponse();
            using (StreamReader read = new StreamReader(response.GetResponseStream()))
            {
                data = read.ReadToEnd();
                read.Close();
            }
            if (response.StatusCode != HttpStatusCode.OK)
                return (false);
            return (true);
        }

        public bool RemoveWishListToCart(WishList wish)
        {
            if (!connected)
                return (false);
            for (int i = 0; i < wish.Lenght(); i++)
            {
                if (!RemoveItemToCart(wish._items[i]))
                    return (false);
            }
            return (true);
        }
        public bool RemoveItemToCart(WishList.Item item)
        {
            request = (HttpWebRequest)WebRequest.Create(new Uri(usr.getRemovingItem()));
            request.Method = "POST";
            request.ContentType = "application/json; charset=utf-8";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36";
            request.Referer = usr.getChoosenMarket();
            request.Host = "drive.intermarche.com";
            request.AllowAutoRedirect = true;
            request.Accept = "application / json, text / javascript, */*; q=0.01";
            request.KeepAlive = true;
            request.Proxy = new WebProxy("127.0.0.1:8080");
            request.CookieContainer = cookie;
            StringBuilder beforeData = new StringBuilder();
            beforeData.Append("{'idProduit':'" + item.idProduit + "','trackingCode':''}");
            request.ContentLength = beforeData.ToString().Length;
            using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(beforeData.ToString());
            }
            response = (HttpWebResponse)request.GetResponse();
            using (StreamReader read = new StreamReader(response.GetResponseStream()))
            {
                data = read.ReadToEnd();
                read.Close();
            }
            Console.WriteLine(data);
            if (response.StatusCode != HttpStatusCode.OK)
                return (false);
            return (true);
        }
    }
}
