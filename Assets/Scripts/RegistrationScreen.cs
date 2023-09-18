using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Security.Cryptography;
using System.Text;
using IneryLibrary;
using IneryLibrary.Core.Providers;
using IneryLibrary.Core;
using IneryLibrary.Core.Api.v1;
using UnityEngine.SceneManagement;
using Action = IneryLibrary.Core.Api.v1.Action;
using static UnityEngine.RuleTile.TilingRuleOutput;
using Unity.VisualScripting;

public class RegistrationScreen : MonoBehaviour
{
    private static RegistrationScreen instance;
    public static RegistrationScreen Instance
    {
        get
        {
            if (instance == null)
                Debug.Log("RegistrationScreen is null");
            return instance;
        }
    }

    [Header("UI Components")]
    public TMP_InputField usernameInput;
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_InputField confirmPasswordInput;
    [SerializeField]
    Button registerButton;

    [Header("Keyboard")]
    public GameObject virtualKeyboard;



    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        FieldFilled();
    }

    void FieldFilled()
    {
        if ((usernameInput.text != "" && usernameInput.text.Length > 1) && (emailInput.text != "" && emailInput.text.Length > 1) && (passwordInput.text != "" && passwordInput.text.Length > 1) && (confirmPasswordInput.text != "" && confirmPasswordInput.text.Length > 1))
        {
            registerButton.interactable = true;
        }
        else
        {
            registerButton.interactable = false;
        }
    }

    public async void Registration()
    {
        if (!UsernameCheck() || !EmailCheck() || !PasswordCheck() || !CheckConfirmPassword())
            return;
        else
        {
            // Inery - klasa za interakciju sa čejnom
            // IneryConfigurator - klasa služi za konfigurisanje klase Inery sa čejnom
            Inery inery = new Inery(new IneryConfigurator()
            {
                // HttpEndpoint - Postavlja se url za pristup API-ju blokčejna
                HttpEndpoint = "https://tas.blockchain-servers.world", //Mainnet
                // ChainId - Jedinstveni identifikator za povezivanje na inery čejn
                ChainId = "3b891f1a78a7c27cf5dbaa82d2f30f96d0452262a354b4995b88c162ab066eee",
                // ExpireSeconds - Broj sekundi trajanja transakcije - pre nego što istekne
                ExpireSeconds = 60,
                // SignProvider - Koristi DefaultSignProvider privatni ključ za potpisivanje transakcija
                SignProvider = new DefaultSignProvider("5KftZF2nz6eiYy9ZBtGymj75XJWiKJk2f859qdc6kGGMb6boAkb")
            });

            try
            {
                var result = await inery.CreateTransaction(new IneryLibrary.Core.Api.v1.Transaction()
                {
                    actions = new List<IneryLibrary.Core.Api.v1.Action>()
                    {
                        new IneryLibrary.Core.Api.v1.Action()
                        {
                            account = "quiz",
                            authorization = new List<PermissionLevel>()
                            {
                                new PermissionLevel() {actor = "quiz", permission = "active" }
                            },
                            name = "insertu",
                            data = new { username = usernameInput.text, email = emailInput.text, password = HashPassword(passwordInput.text)}
                        }
                    }
                });
                Debug.Log("Uspešno ste se registrovali"); // poruka
                LoginScreen.Instance.gameObject.SetActive(true);
                UIManagerScene2.Instance.message.text = "Succesfully registered, you can register";
                UIManagerScene2.Instance.ShowPanel(LoginScreen.Instance.gameObject.gameObject.transform);
                gameObject.SetActive(false);
            }

            catch (Exception e)
            {
                UIManagerScene2.Instance.message.text = "Username already exists";
                UIManagerScene2.Instance.ShowPanel(gameObject.transform);
                Debug.Log(e);
            }
        }
    }

    private bool UsernameCheck()
    {
        Regex usernameRegex = new Regex("^[a-z1-5.]{1,12}$");
        UIManagerScene2.Instance.message.text = "";

        if (usernameRegex.IsMatch(usernameInput.text))
        {
            Debug.Log("Validan username");
            return true;
        }
        else
        {
            UIManagerScene2.Instance.message.text = "Username can only have letters, numbers 1-5, dot, 1-12 chars.";
            UIManagerScene2.Instance.ShowPanel(gameObject.transform);
            return false;
        }
    }

    public bool EmailCheck()
    {
        Regex emailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", RegexOptions.IgnoreCase);
        UIManagerScene2.Instance.message.text = "";
        if (emailRegex.IsMatch(emailInput.text))
        {
            Debug.Log("Validan meil");
            return true;
        }
        else
        {
            UIManagerScene2.Instance.message.text = "The email is not valid!";
            UIManagerScene2.Instance.ShowPanel(gameObject.transform);
            return false;
        }
    }

    public bool PasswordCheck()
    {
        Regex passwordRegex = new Regex(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$");
        UIManagerScene2.Instance.message.text = "";
        if (passwordRegex.IsMatch(passwordInput.text))
        {
            Debug.Log("Validan pasword");
            return true;
        }
        else
        {
            // UIManager.Instance.message.text = "The password must be at least 6 characters long and consist of uppercase and lowercase letters, symbols, and numbers";
            UIManagerScene2.Instance.message.text = "Password must have a letter, a number, and be at least 8 characters long";
            UIManagerScene2.Instance.ShowPanel(gameObject.transform);
            return false;
        }
    }

    public bool CheckConfirmPassword()
    {
        if (passwordInput.text.Equals(confirmPasswordInput.text))
        {
            Debug.Log("Potvrđen password");
            return true;
        }
        else
        {
            UIManagerScene2.Instance.message.text = "The passwords do not match";
            UIManagerScene2.Instance.ShowPanel(gameObject.transform);
            return false;
        }
    }

    // Metod za šiforvanje lozinke (SHA256)
    public static string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] hashBytes = sha256.ComputeHash(passwordBytes);
            return Convert.ToBase64String(hashBytes);
        }
    }
}
