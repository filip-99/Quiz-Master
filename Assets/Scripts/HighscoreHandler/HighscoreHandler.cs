﻿using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
//using UnityEditor.PackageManager.Requests;
using UnityEngine;
// Potrebno je dodati sledeće biblioteke
using UnityEngine.Networking;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;


public class HighscoreHandler : MonoBehaviour
{

    // Potreban je link do podataka, da bi ga dodelili API-ju
    private string URL = "https://tas.blockchain-servers.world:443/v1/chain/get_table_rows";
    //private string URL_PUSH_ACTION = "https://websocket.inery.io:443/insertscore";
    private string URL_PUSH_ACTION = "https://websocket.inery.io:2555/action/push_action";
    private string URL_SUBSCRIBE = "https://websocket.inery.io:443/subscribe";
    private AesEncryption encrypt = new AesEncryption("CBC", 256);
    private string private_key = "5JoHr2DdCjvCHeh8o7UoWyEKMhgZQGMu";

    // Potrebna je lista koja će sadržati visoke rezultate
    List<HighscoreElement> highscoreList = new List<HighscoreElement>();
    // Potrebno je definisati maksimalan broj elemenata u listi
    public int maxCount = 7;
    // Potrebno je definisati ime datoteke
    public string filename;

    // Delegati se razlikuju od promenjivih po tome što delegati sadrže funkcije, dok promenjive sadrže podatke
    public delegate void OnHighscoreListChanged(List<HighscoreElement> list);

    // Potrebno je kreirati događaj za delegata i sa njim kontrolišemo metode koje se izvršavaju
    // event je statičan, tako da u drugim klasama ne moramo instancirati objekat ove klase
    // Tipa je delegata, koji sadrži reference na metode
    public static event OnHighscoreListChanged onHighscoreListChanged;

    private void Start()
    {
        // Pri pokretanju programa potrebno je da učitamo podatke
        //StartCoroutine(GetDatas());
    }

    /*
    // Za uzimanje podataka pomoću API-ja koristimo sledeću metodu
    IEnumerator GetDatas()
    {
        // Potrebno je da definišemo zahtev
        using (UnityWebRequest request = UnityWebRequest.Get(URL))
        {
            // Šaljemo zahtev
            yield return request.SendWebRequest();

            // Kada pošaljemo zahtev proveravamo vezu sa "serverom"
            if (request.result == UnityWebRequest.Result.ConnectionError)
                Debug.LogError(request.error);
            // U koliko nemamo grešku dodeljujemo vrednost promenjivoj json
            else
            {
                // Samo request.downloadHandler.text sadrži podatke u JSON formatu
                string json = request.downloadHandler.text;
                // Sada je potrebno razčlaniti podatke
                SimpleJSON.JSONNode status = SimpleJSON.JSON.Parse(json);

                // Razložene podatke dodeljujemo elementu pojedinačnoi smeštamo u listu
                for (int i = 0; i < status.Count; i++)
                {
                    HighscoreElement el = new HighscoreElement(status[i]["playerName"], status[i]["points"]);
                    highscoreList.Add(el);
                }

                if (onHighscoreListChanged != null)
                {
                    onHighscoreListChanged.Invoke(highscoreList);
                }
            }
        }
    }
    */
    // Potrebna je metoda za čuvanje podataka na serveru
    IEnumerator PostData()
    {
        highscoreList.Clear();
        // Potrebna nam je pomoćna promenjia za postavljanje podataka na WEB Server, pored API-ja
        /*using (UnityWebRequest request = UnityWebRequest.Post(URL, "{\"code\":\"inerygame\",\"table\":\"scores\",\"scope\":\"inerygame\",\"lower_bound\":\"0\",\"limit\":\"" + maxCount + "\",\"json\":\"true\"}"))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError)
                Debug.LogError(request.error);
            else
            {
                Debug.Log(request.downloadHandler.text);
                string json = request.downloadHandler.text;
                JSONNode status = SimpleJSON.JSON.Parse(json);
                // Da bi izostavili prvi element, potrebno je da uđemo u objekat JSONa i pokupimo podatke
                foreach (JSONNode node in status["rows"])
                {
                    HighscoreElement ellement = new HighscoreElement(node["username"], node["score"]);
                    highscoreList.Add(ellement);
                }

                while (highscoreList.Count > maxCount)
                {
                    highscoreList.RemoveAt(maxCount);
                }

                if (onHighscoreListChanged != null)
                {
                    onHighscoreListChanged.Invoke(highscoreList);
                }
            }


        }*/
        var request = new UnityWebRequest(URL, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes("{\"code\":\"inerygame\",\"table\":\"scores\",\"scope\":\"inerygame\",\"lower_bound\":\"0\",\"limit\":\"" + maxCount + "\",\"json\":\"true\"}");
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        using (request)
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError)
                Debug.LogError(request.error);
            else
            {
                //Debug.Log(request.downloadHandler.text);
                string json = request.downloadHandler.text;
                JSONNode status = SimpleJSON.JSON.Parse(json);
                // Da bi izostavili prvi element, potrebno je da uđemo u objekat JSONa i pokupimo podatke
                foreach (JSONNode node in status["rows"])
                {
                    HighscoreElement ellement = new HighscoreElement(node["username"], node["score"]);
                    highscoreList.Add(ellement);
                }

                while (highscoreList.Count > maxCount)
                {
                    highscoreList.RemoveAt(maxCount);
                }

                if (onHighscoreListChanged != null)
                {
                    onHighscoreListChanged.Invoke(highscoreList);
                }
            }


        }
    }

    //--------------------- POST METODA ---------------------

    IEnumerator InsertData(string username, string score, string coins)
    {
        // Potrebna nam je pomoćna promenjia za postavljanje podataka na WEB Server, pored API-ja
        //Staro System.Text.Encoding.UTF8.GetString(encrypt.Encrypt("{\"account\":\"inerygame\",\"action\":\"insertscore\",\"data\":{\"username\":\"" + username + "\", \"score\":\"" + score + "\", \"coins\":\"" + coins + "\"}}", private_key))
        //EncryptString("{\"type\": \"push_action\",\"action_name\": \"insertscore\",\"account\": \"inerygame\",\"data\": {\"username\" : \"aaa\", \"score\" : 55, \"coins\" : 5}}", private_key, "iLHtRkhssiB2KTvY")
        string req = "{\"request\": \"" + EncryptString("{\"type\": \"push_action\",\"action_name\": \"insertscore\",\"account\": \"inerygame\",\"data\": {\"username\" : \"" + username + "\", \"score\" : " + score + ", \"coins\" : " + coins + "}}", private_key, "iLHtRkhssiB2KTvY") + "\"}";

        using (UnityWebRequest request = UnityWebRequest.Put(URL_PUSH_ACTION, req))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError)
                Debug.LogError(request.error);
            else
            {
                GetHighscoreData();
            }


        }
    }



    IEnumerator SubscribeData(string email, InputField emailf, Text message)
    {
        var request = new UnityWebRequest(URL_SUBSCRIBE, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(email);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        //request.SetRequestHeader("Content-Type", "application/json");

        using (request)
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError)
                Debug.LogError(request.error);
            else
            {
                if (request.downloadHandler.text == "True")
                {
                    //Debug.Log("Successfully subscribed");
                    message.text = "Successfully subscribed";
                    emailf.text = "";
                }
                else
                {
                    //Debug.Log(request.downloadHandler.text);
                    message.text = request.downloadHandler.text;
                }
            }


        }
    }

    public void Subscribe(string email, InputField emailf, Text message)
    {
        StartCoroutine(SubscribeData(email, emailf, message));
    }

    // Potrebno je da se proveri skor pre upisivanja
    // element ime i broj poena
    public void AddHighscoreIfPossible(HighscoreElement element)
    {
        // Potrebnno je da prođemo kroz listu i da ubacimo najviši skor, u koliko je veći od nekih elemenata u listi
        for (int i = 0; i < maxCount; i++)
        {
            // U koliko nema dovoljno elemenata u listi dodaće se postignuti rezultat i u koliko je broj poena veći od određenog elementa u listi
            if (i >= highscoreList.Count || element.points > highscoreList[i].points)
            {
                // Dodajemo element u listu na poziciju elementa od koga je veći
                highscoreList.Insert(i, element);


                // Takođe treba da se pobrinemo da u koliko se skor unese u listu, broj korisnika ne pređe zadati broj
                while (highscoreList.Count > maxCount)
                {
                    // RemoveAt() - metoda briše element na zadatoj poziciji
                    highscoreList.RemoveAt(maxCount);
                }


                // Potrebno je da trenutne podatke u listi sačuvamo u JSON fajlu
                StartCoroutine(PostData());
                /*
                                // Ovde se takođe poziva delegat
                                if (onHighscoreListChanged != null)
                                {
                                    onHighscoreListChanged.Invoke(highscoreList);
                                }
                */
                // Break se koristi za iskakanje iz if i for petlje, dakle kada je uslov jednom ispunjen izaćiće iz petlje
                break;
            }
        }
    }

    public void GetHighscoreData()
    {
        StartCoroutine(PostData());
    }
    public void SaveHighscoreData(string username, string score, string coins)
    {
        StartCoroutine(InsertData(username, score, coins));
    }

    //-------------------------------------------------------------------------------------------------------------------------------
    /*
        // Potrebne su metode za učitavanje i čuvanje podataka u listu:
        private void SaveHighscore()
        {
            // Pozivamo metodu iz statik klase FileHendler, za čuvanje podataka u JSON fajl preko liste
            // Za parametre navodimo definisanu listu i ime datoteke
            FileHendler.SaveToJSON<HighscoreElement>(highscoreList, filename);
        }

        private void LoadHighscores()
        {
            // Za čitanje podataka iz JSON fajla koristimo drugu metodu iz iste klase FileHendler
            // highscoreList = FileHendler.ReadFromJSON<HighscoreElement>(filename);

            // U koliko lista sadrži više elemenata od broja elemenata zadatih promenjivom maxCount izvršavaće se sledeći kod
            while (highscoreList.Count > maxCount)
            {
                // RemoveAt() - metoda briše element na zadatoj poziciji
                highscoreList.RemoveAt(maxCount);
            }

            // Potrebno je da aktiviramo delegat u koliko nije null tj. u koliko mu je dodeljena metoda
            if (onHighscoreListChanged != null)
            {
                onHighscoreListChanged.Invoke(highscoreList);
            }
        }
    */

    public static string Decrypt(string cipherData, string keyString, string ivString)
    {
        byte[] key = Encoding.UTF8.GetBytes(keyString);
        byte[] iv = Encoding.UTF8.GetBytes(ivString);

        try
        {
            using (var rijndaelManaged =
                   new RijndaelManaged { Key = key, IV = iv, Mode = CipherMode.CBC })
            using (var memoryStream =
                   new MemoryStream(Convert.FromBase64String(cipherData)))
            using (var cryptoStream =
                   new CryptoStream(memoryStream,
                       rijndaelManaged.CreateDecryptor(key, iv),
                       CryptoStreamMode.Read))
            {
                return new StreamReader(cryptoStream).ReadToEnd();
            }
        }
        catch (CryptographicException e)
        {
            Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
            return null;
        }
        // You may want to catch more exceptions here...
    }

    /// <summary>
    /// aes-256-cbc 加密
    /// </summary>
    /// <param name="message"></param>
    /// <param name="KeyString"></param>
    /// <param name="IVString"></param>
    /// <returns></returns>
    public static string EncryptString(string message, string KeyString, string IVString)
    {
        byte[] Key = ASCIIEncoding.UTF8.GetBytes(KeyString);
        byte[] IV = ASCIIEncoding.UTF8.GetBytes(IVString);

        string encrypted = null;
        RijndaelManaged rj = new RijndaelManaged();
        rj.Key = Key;
        rj.IV = IV;
        rj.Mode = CipherMode.CBC;

        try
        {
            MemoryStream ms = new MemoryStream();

            using (CryptoStream cs = new CryptoStream(ms, rj.CreateEncryptor(Key, IV), CryptoStreamMode.Write))
            {
                using (StreamWriter sw = new StreamWriter(cs))
                {
                    sw.Write(message);
                    sw.Close();
                }
                cs.Close();
            }
            byte[] encoded = ms.ToArray();
            encrypted = Convert.ToBase64String(encoded);

            ms.Close();
        }
        catch (CryptographicException e)
        {
            Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
            return null;
        }
        catch (UnauthorizedAccessException e)
        {
            Console.WriteLine("A file error occurred: {0}", e.Message);
            return null;
        }
        catch (Exception e)
        {
            Console.WriteLine("An error occurred: {0}", e.Message);
        }
        finally
        {
            rj.Clear();
        }
        return encrypted;
    }

}