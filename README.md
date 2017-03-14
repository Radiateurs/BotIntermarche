# BotIntermarche
Un bot permettant de faire ses courses sur un drive inter marché via différent fichier de configuration.

A bot that can shop online from differents configuration files.

The purpose of this bot take place in different steps :

    1. Log in the user.
    2. Either add or remove items.
    3. Log out the user.
  
The different configuration files are :

    1. info.user(.txt) -> The user info.
    2. wishList_01(.json) -> the wishList of item that the bot should add in the user cart.
  
info.user(.txt) :
-----------------

This file contains all the usefull informations. This files has a specific way to be written wich is the following :

      NAME:           -> Name of the user // Not mandatory
      PHONE:          -> Phone number // Not mandatory
      ADRESS:         -> Shipping adress // Mandatory in futur versions.
      COMPLEMENT:     -> More informations about the shipping adress like the appartement number, floor or other. // Mandatory in futur versions.
      CITY:           -> Shipping city // Mandatory in futur versions.
      COUNTRY:        -> Shipping country // Mandatory in futur versions.
      ZIPCODE:        -> Shipping zipcode // Mandatory in futur versions.
      CHOOSENMARKET:  -> URL of the online market choose by the user // MUST be http://drive.intermarche.com for this version.
      CONNEXIONURL:   -> URL to log in the user. // MUST be http://drive.intermarche.com/Connexion for this version.
      DECONNEXIONURL: -> URL to log out the user. // MUST be http://drive.intermarche.com/Deconnexion for this version.
      ADDINGITEM:     -> URL to add item in the cart. // MUST be http://drive.intermarche.com/Plus for this version.
      REMOVINGITEM:   -> URL to remove item in the cart. // MUST be http://drive.intermarche.com/Moins for this version.
      MAIL:           -> The mail adress to log in the user. // Mandatory in this version.
      PASSWORD:       -> The password to log in the user. // Mandatory in this version.
      CARDNUMBER:     -> The number of user's payement card. // Not Mandatory in this version.
      CARDSAFETY:     -> The user's payement card safety number code. // Not mandatory in this version.
      EXPIREDATE:     -> Date of expiration of the user's payement card. // Not mandatory in this version.
  
All the inforamtions aren't crypted. It's your purpose to keep those information well hidden.
This software doesn't copy your personnal informations.

wishList(.json) :
-----------------

This file constains the differents items that should be added in the user's cart.
It's a JSON files so the items are object with the following attributs :

      int id:             -> the ID of the item. // Not important.
      string idProduit:   -> the id of the product that the user want to buy. // REALLY important. Most important informations for this section.
      string name:        -> the name of the product.
      string mark:        -> mark of the product.
      string descritpion: -> description of the product.
      string price:       -> price of the product.
      string shelf:       -> shelf of the product.
      bool liter:         -> Informs if the product is sell in liter or not.
      bool kilo:          -> Informs if the product is sell in kilogram or not.
      bool unity:         -> Informs if the product is sell at the unit or not.
      string pricePer:    -> Describes the price per unit, liter or kilo.
      string quantity:    -> Describes the quantity of the product.
      string capacity:    -> Describes the capacity of the product.
      bool promotion:     -> Informs if the product is in sale.
      string shopAdress:  -> Informs where the product is from.

Those informations are present in the database of the online market. To get them, you should scrap the web site or ask for the database.

Enjoy !
