using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;
using IneryLibrary;
using IneryLibrary.Core.Providers;
using IneryLibrary.Core;
using IneryLibrary.Core.Api.v1;
using Newtonsoft.Json;

public class LoginScreen : MonoBehaviour
{
    static LoginScreen instance;
    public static LoginScreen Instance
    {
        get
        {
            if (instance == null)
                Debug.Log("StartScreen is null");
            return instance;
        }
    }

    [Header("UI Components")]
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    [SerializeField]
    Button playButton;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        FilledFields();
    }

    public async void StartQuiz()
    {
        if (!UsernameCheck() || !PasswordCheck())
        {
            return;
        }
        else
        {
            Inery inery = new Inery(new IneryConfigurator()
            {
                HttpEndpoint = "https://tas.blockchain-servers.world", //Mainnet
                ChainId = "6aae7eb3a1d8993e6d9865d1eb62f7dff34e2cd9cd7d405d4d5ba96a71d48c9c",
                ExpireSeconds = 60,
                SignProvider = new DefaultSignProvider("5KftZF2nz6eiYy9ZBtGymj75XJWiKJk2f859qdc6kGGMb6boAkb")
            });

            try
            {
                // GetTableRows - Metoda za dobijanje redova sa blokèejna
                // GetTableRowsRequest - Je objekat koji odreðuje JSON format
                var result = await inery.GetTableRows(new GetTableRowsRequest()
                {
                    json = true,                        // Ovaj parametar postavlja izlazni format na JSON.
                    code = "quiz",                      // Ovaj parametar postavlja ime raèuna koji poseduje tabelu.
                    scope = "quiz",                     // Ovaj parametar postavlja tekst opsega koji segmentuje skup tabela
                    table = "users",                    // Ovaj parametar postavlja ime tabele
                    index_position = "2",               // Ovaj parametar postavlja index poziciju redova koji se vraæaju.
                    key_type = "i64",                   // Ovaj parametar postavlja tip indeksa. - i64 - podržava ogromnu kolièinu celih brojeva
                    encode_type = "string",             // Ovaj parametar postavlja tip kodiranja za binarne podatke.
                    lower_bound = usernameInput.text,   // Ovaj parametar postavlja donju granicu za izabranu vrednost indeksa.
                    upper_bound = usernameInput.text,   // Ovaj parametar postavlja gornju granicu za izabranu vrednost indeksa.
                    limit = 1                           // Ovaj parametar postavlja maksimalni broj redova koji se vraæaju
                });

                if (result.rows.Count == 0)
                {
                    UIManager.Instance.message.text = "The username does not exist, please try again.";
                    UIManager.Instance.ShowPanel(gameObject.transform);
                    Debug.Log("Pogrešan username");

                    return;
                }

                UserData jsonObj = JsonConvert.DeserializeObject<UserData>(result.rows[0].ToString());

                if ((usernameInput.text.Equals(jsonObj.username)) && (RegistrationScreen.HashPassword(passwordInput.text).Equals(jsonObj.password)))
                    Debug.Log("Korisnik postoji");
                else
                {
                    Debug.Log("Korisnik ne postoji");

                    UIManager.Instance.message.text = "The user does not exist, please try again.";
                    UIManager.Instance.ShowPanel(gameObject.transform);

                    return;
                }

                Debug.Log(result.rows[0].ToString());
            }
            catch (Exception e)
            {
                UIManager.Instance.message.text = "Error";
                UIManager.Instance.ShowPanel(gameObject.transform);
                Debug.Log(e);
                return;
            }

            //Quiz.Instance.
            Quiz.Instance.gameObject.SetActive(true);
            Quiz.Instance.SetUsername(usernameInput.text);
            gameObject.SetActive(false);
        }
    }

    // Metoda proverava da li su polja za unos popunjena
    private void FilledFields()
    {
        if ((usernameInput.text != "" && usernameInput.text.Length > 1) && (passwordInput.text != "" && passwordInput.text.Length > 1))
        {
            playButton.interactable = true;
        }
        else
        {
            playButton.interactable = false;
        }
    }

    private bool UsernameCheck()
    {
        Regex usernameRegex = new Regex("^[a-z1-5.]{1,12}$");
        UIManager.Instance.message.text = "";

        if (usernameRegex.IsMatch(usernameInput.text))
        {
            Debug.Log("Validan username");
            return true;
        }
        else
        {
            UIManager.Instance.message.text = "The username is not valid!";
            UIManager.Instance.ShowPanel(gameObject.transform);
            return false;
        }
    }

    public bool PasswordCheck()
    {
        Regex passwordRegex = new Regex(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$");
        UIManager.Instance.message.text = "";
        if (passwordRegex.IsMatch(passwordInput.text))
        {
            Debug.Log("Validan pasword");
            return true;
        }
        else
        {
            // UIManager.Instance.message.text = "The password must be at least 6 characters long and consist of uppercase and lowercase letters, symbols, and numbers";
            UIManager.Instance.message.text = "The password is not valid!";
            UIManager.Instance.ShowPanel(gameObject.transform);
            return false;
        }
    }
}
