using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using Newtonsoft.Json;

namespace BotDriveCart
{
    public class WishList
    {
        public class Item
        {
            public uint id { get; set;}
            public uint idProduit { get; set; }
            public string name { get; set;}
            public string mark { get; set;}
            public string description { get; set;}
            public string price { get; set;}
            public string shelf { get; set;}
            public string categorie { get; set;}
            public bool liter { get; set;}
            public bool kilo { get; set;}
            public bool unity { get; set;}
            public string pricePer { get; set;}
            public string quantity { get; set;}
            public string capacity { get; set;}
            public bool promotion { get; set;}
            public string shopAdress { get; set;}
            DateTime scrapingDate { get; set;}
            Uri     url { get; set;}
            Uri     mainURL { get; set;}
        }

        public List<Item> _items { get; set; }

        public int Lenght()
        {
            return (_items.Count);
        }

        public bool initWishList()
        {
            using (StreamReader file = new StreamReader("./wishList_01.json"))
            {
                string fullFile = file.ReadToEnd();
                _items = JsonConvert.DeserializeObject<List<Item>>(fullFile);
            }
            return (true);
        }
    }

    public class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            WebNavigator nv = new WebNavigator();
            WishList wish = new WishList();

            Console.WriteLine("Begin of program.");
            try
            {
                nv.usr.initUser("./info.user.txt");
                wish.initWishList();
                if (nv.ConnectUserToDrive())
                {
                    if (!nv.AddWishListToCart(wish))
                        Console.WriteLine("An error occured at adding the wishlist to cart");
                    if (!nv.DeconnectUserToDrive())
                        Console.WriteLine("An error occured at the deconnexion");
                }
                else
                    Console.WriteLine("An error occured at connexion");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
