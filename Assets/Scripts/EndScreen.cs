using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;
using System;
using UnityEngine.EventSystems;
using IneryLibrary.Core.Api.v1;
using IneryLibrary.Core.Providers;
using IneryLibrary.Core;
using IneryLibrary;

public class EndScreen : MonoBehaviour
{
    static EndScreen instance;
    public static EndScreen Instance
    {
        get
        {
            if (instance == null)
                Debug.Log("EndScreen is null");
            return instance;
        }
    }

    [SerializeField]
    TextMeshProUGUI finalScoreText;

    private void Awake()
    {
        instance = this;
    }

    public void ShowFinalScore()
    {
        finalScoreText.text = "Čestitamo!\nVaš skor je: " + ScoreKeeper.Instance.CalculateScore() + "%";

        PushUserData();
    }

    public async void PushUserData()
    {
        Inery inery = new Inery(new IneryConfigurator()
        {
            HttpEndpoint = "https://tas.blockchain-servers.world", //Mainnet
            ChainId = "6aae7eb3a1d8993e6d9865d1eb62f7dff34e2cd9cd7d405d4d5ba96a71d48c9c",
            ExpireSeconds = 60,
            SignProvider = new DefaultSignProvider("5KftZF2nz6eiYy9ZBtGymj75XJWiKJk2f859qdc6kGGMb6boAkb")
        });

        //try
        //{
        //    var result = await inery.CreateTransaction(new IneryLibrary.Core.Api.v1.Transaction()
        //    {
        //        actions = new List<IneryLibrary.Core.Api.v1.Action>()
        //            {
        //                new IneryLibrary.Core.Api.v1.Action()
        //                {
        //                    account = "quiz",
        //                    authorization = new List<PermissionLevel>()
        //                    {
        //                        new PermissionLevel() {actor = "quiz", permission = "active" }
        //                    },
        //                    name = "insertu",
        //                    data = new { username = usernameInput.text, email = emailInput.text, password = HashPassword(passwordInput.text)}
        //                }
        //            }
        //    });
        //    Debug.Log("Uspešno ste se registrovali"); // poruka
        //}

        //catch (Exception e)
        //{
        //    UIManager.Instance.message.text = "Username already exists";
        //    UIManager.Instance.ShowPanel(gameObject.transform);
        //    Debug.Log(e);
        //}
    }

    // private void ScoreIsHigher()
    // 
    //    if (ScoreKeeper.Instance.CalculateScore() > 0)
    //    {
    //        highscoreHandler.SaveHighscoreData(StartScreen.Instance.nameInput.text, ScoreKeeper.Instance.CalculateScore().ToString());
    //    }
    // 
}
