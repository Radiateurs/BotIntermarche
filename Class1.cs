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
public class Parse
{
        public string[] parseCommandUser = { "NAME", "PHONE", "ADDRESS", "COMPLEMENT", "CITY", "COUNTRY", "ZIPCODE", "CHOOSENMARKET", "CONNEXIONURL", "DECONNEXIONURL", "ADDINGITEM", "REMOVINGITEM", "MAIL", "PASSWORD", "CARDNUMBER", "CARDSAFETY", "EXPIREDATE" };
        public enum InfoInitUser : int
        {
            NAME,
            PHONE,
            ADDRESS,
            COMPLEMENT,
            CITY,
            COUNTRY,
            ZIPCODE,
            CHOOSENMARKET,
            CONNEXIONURL,
            DECONNEXIONURL,
            ADDINGITEM,
            REMOVINGITEM,
            MAIL,
            PASSWORD,
            CARDNB,
            CARDS,
            EXPIRE,
        }
        public InfoInitUser it;
    }
    public struct CardInfo
    {
        public string _cardNumber;
        public string _cardSafety;
        public string _expireDate;
    }

    public struct LocationInfo
    {
        public string _addr;
        public string _city;
        public string _country;
        public string _zipCode;
        public string _complement;
    }

    public class UserInfo
    {
        private string _name;
        private string _phone;
        private string _mail;
        private string _pswd;
        private string _choosenMarket;
        private string _connexionUrl;
        private string _deconnexionUrl;
        private string _addingItem;
        private string _removingItem;
        private LocationInfo _location; 
        private CardInfo _cardInfo;
        private string JSONInfo;

        public string getName()
        {
            return (_name);
        }
        public string getPhone()
        {
            return (_phone);
        }
        public string getMail()
        {
            return (_mail);
        }
        public string getPasswd()
        {
            return (_pswd);
        }
        public string getChoosenMarket()
        {
            return (_choosenMarket);
        }
        public string getConnexionUrl()
        {
            return (_connexionUrl);
        }
        public string getDeconnexionUrl()
        {
            return (_deconnexionUrl);
        }
        public string getAddingItem()
        {
            return (_addingItem);
        }
        public string getRemovingItem()
        {
            return (_removingItem);
        }

        public string getAddr()
        {
            return (_location._addr);
        }
        public string getCity()
        {
            return (_location._city);
        }
        public string getCountry()
        {
            return (_location._city);
        }
        public string getZipCode()
        {
            return (_location._zipCode);
        }
        public string getComplement()
        {
            return (_location._complement);
        }
        public LocationInfo getLocationInfo()
        {
            return (_location);
        }

        public string getCardNmber()
        {
            return (_cardInfo._cardNumber);
        }
        public string getCardSafety()
        {
            return (_cardInfo._cardSafety);
        }
        public string getExpireDate()
        {
            return (_cardInfo._expireDate);
        }
        public CardInfo getCardInfo()
        {
            return (_cardInfo);
        }

        /*
         * Init the class from a file. Uses the parseUser Class.
         */
        public bool initUser(string confFilePath)
        {
            Parse myParser = new Parse();
            string line;
            string[] splited;

            using (TextReader file = new StreamReader(confFilePath))
            {
                while ((line = file.ReadLine()) != null)
                {
                    splited = line.Split(':');
                    for (myParser.it = Parse.InfoInitUser.NAME; myParser.parseCommandUser[(int)myParser.it] != splited[0] && myParser.it < Parse.InfoInitUser.EXPIRE + 1; myParser.it++) ;
                    Console.WriteLine(splited[0]);
                    switch (myParser.it)
                    {
                        case Parse.InfoInitUser.NAME:
                            _name = splited[1];
                            break;
                        case Parse.InfoInitUser.PHONE:
                            _phone = splited[1];
                            break;
                        case Parse.InfoInitUser.ADDRESS:
                            _location._addr = splited[1];
                            break;
                        case Parse.InfoInitUser.COMPLEMENT:
                            _location._complement = splited[1];
                            break;
                        case Parse.InfoInitUser.CITY:
                            _location._city = splited[1];
                            break;
                        case Parse.InfoInitUser.COUNTRY:
                            _location._country = splited[1];
                            break;
                        case Parse.InfoInitUser.ZIPCODE:
                            _location._zipCode = splited[1];
                            break;
                        case Parse.InfoInitUser.CHOOSENMARKET:
                            int i = 0;
                            while (i < splited.Length - 1)
                            {
                                _choosenMarket += splited[i + 1];
                                i++;
                                if (i < splited.Length - 1)
                                    _choosenMarket += ":";
                            }
                            break;
                        case Parse.InfoInitUser.CONNEXIONURL:
                            int j = 0;
                            while (j < splited.Length - 1)
                            {
                                _connexionUrl += splited[j + 1];
                                j++;
                                if (j < splited.Length - 1)
                                    _connexionUrl += ":";
                            }
                            break;
                        case Parse.InfoInitUser.DECONNEXIONURL:
                            int k = 0;
                            while (k < splited.Length - 1)
                            {
                                _deconnexionUrl += splited[k + 1];
                                k++;
                                if (k < splited.Length - 1)
                                    _deconnexionUrl += ":";
                            }
                            break;
                        case Parse.InfoInitUser.ADDINGITEM:
                            int l = 0;
                            while (l < splited.Length - 1)
                            {
                                _addingItem += splited[l + 1];
                                l++;
                                if (l < splited.Length - 1)
                                    _addingItem += ":";
                            }
                            break;
                        case Parse.InfoInitUser.REMOVINGITEM:
                            int m = 0;
                            while (m < splited.Length - 1)
                            {
                                _removingItem += splited[m + 1];
                                m++;
                                if (m < splited.Length - 1)
                                    _removingItem += ":";
                            }
                            break;
                        case Parse.InfoInitUser.MAIL:
                            _mail = splited[1];
                            break;
                        case Parse.InfoInitUser.PASSWORD:
                            _pswd = splited[1];
                            break;
                        case Parse.InfoInitUser.CARDNB:
                            _cardInfo._cardNumber = splited[1];
                            break;
                        case Parse.InfoInitUser.CARDS:
                            _cardInfo._cardSafety = splited[1];
                            break;
                        case Parse.InfoInitUser.EXPIRE:
                            _cardInfo._expireDate = splited[1];
                            break;
                        default:
                            break;
                    }
                }
            }
            return (true);
        }
    }
}
