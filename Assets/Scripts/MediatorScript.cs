using IneryLibrary.Core.Api.v1;
using IneryLibrary.Core.Providers;
using IneryLibrary.Core;
using IneryLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System;
using Json.Lib;
using System.Security.Cryptography;
using System.Text;

public class MediatorScript : Singleton<MediatorScript>
{

    [SerializeField] string startScene;
    public UserData data;

    protected override void Awake()
    {
        base.Awake();
    }

    public async Task<UserData> GetRow(string usernameInput)
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
                lower_bound = usernameInput,   // Ovaj parametar postavlja donju granicu za izabranu vrednost indeksa.
                upper_bound = usernameInput,   // Ovaj parametar postavlja gornju granicu za izabranu vrednost indeksa.
                limit = 1                           // Ovaj parametar postavlja maksimalni broj redova koji se vraæaju
            });

            if (result.rows.Count == 0)
            {
                UIManagerScene2.Instance.message.text = "The username does not exist, please try again.";
                UIManagerScene2.Instance.ShowPanel(gameObject.transform);
                Debug.Log("Pogrešan username");

                return null;
            }

            UserData jsonObj = JsonConvert.DeserializeObject<UserData>(result.rows[0].ToString());
            return jsonObj;
        }
        catch (Exception e)
        {
            UIManagerScene2.Instance.message.text = "Error";
            UIManagerScene2.Instance.ShowPanel(gameObject.transform);
            Debug.Log(e);
            return null;
        }
    }

    public void SaveUserData(UserData userData)
    {
        data = userData;

        PlayerPrefs.DeleteKey("UserData");
        string json = JsonUtility.ToJson(userData);
        PlayerPrefs.SetString("UserData", json);
        PlayerPrefs.Save();
    }

    public UserData LoadUserData()
    {
        UserData userData = new UserData();
        if (PlayerPrefs.HasKey("UserData"))
        {
            string json = PlayerPrefs.GetString("UserData");
            userData = JsonUtility.FromJson<UserData>(json);
        }
        return userData;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(startScene);
    }

    public async void SaveScore(UserData userData, int highScore)
    {
        {
            Inery inery = new Inery(new IneryConfigurator()
            {
                HttpEndpoint = "https://tas.blockchain-servers.world",
                ChainId = "3b891f1a78a7c27cf5dbaa82d2f30f96d0452262a354b4995b88c162ab066eee",
                ExpireSeconds = 60,
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
                            data = new { username = userData.username, scorre = highScore}
                        }
                    }
                });
                Debug.Log("Sačuvan Skor na Blokčejnu");
            }

            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
    }
}
