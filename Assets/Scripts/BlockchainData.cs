using IneryLibrary.Core.Api.v1;
using IneryLibrary.Core.Providers;
using IneryLibrary.Core;
using IneryLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Json.Lib;

public class BlockchainData : MonoBehaviour
{
    static BlockchainData instance;
    public static BlockchainData Instance
    {
        get
        {
            if (instance == null)
                Debug.Log("BlockchainData is null");
            return instance;
        }
    }

    public List<Question> questions = new List<Question>();


    public void SaveQuestions(List<Question> questions)
    {
        PlayerPrefs.DeleteKey("Questions");

        // Serijalizuj listu u JSON format
        string json = JsonConvert.SerializeObject(questions);
        // Saèuvaj JSON string u PlayerPrefs
        PlayerPrefs.SetString("Questions", json);

        // Obavezno pozovite PlayerPrefs.Save() kako biste saèuvali promene
        PlayerPrefs.Save();
    }

    public async Task<List<Question>> GetQuestionsAsync()
    {
        Inery inery = new Inery(new IneryConfigurator()
        {
            HttpEndpoint = "https://tas.blockchain-servers.world", //Mainnet
            ChainId = "3b891f1a78a7c27cf5dbaa82d2f30f96d0452262a354b4995b88c162ab066eee",
            ExpireSeconds = 60,
            SignProvider = new DefaultSignProvider("5KftZF2nz6eiYy9ZBtGymj75XJWiKJk2f859qdc6kGGMb6boAkb")
        });

        try
        {
            var result = await inery.GetTableRows(new GetTableRowsRequest()
            {
                json = true,
                code = "quiz",
                scope = "quiz",
                table = "questions",
                index_position = "0",
                key_type = "i64",
                encode_type = "string",
                limit = 1
            });
            int i = 0;

            foreach (var row in result.rows)
            {
                questions.Add(JsonConvert.DeserializeObject<Question>(row.ToString()));
                questions[i].correctAnswer = questions[i].answers[0];
                i++;
            }

            i = 0;
            // Mešanje pitanja u listi
            for (i = questions.Count - 1; i >= 0; i--)
            {
                int rnd = UnityEngine.Random.Range(0, i);

                // Swap the elements at indices i and rnd.
                Question temp = questions[i];
                questions[i] = questions[rnd];
                questions[rnd] = temp;
            }

            i = 0;
            // Mešanje odgovora u listi
            for (i = questions.Count - 1; i >= 0; i--)
            {
                for (int j = questions[i].answers.Count - 1; j > 0; j--)
                {
                    int rndAns = UnityEngine.Random.Range(0, j);
                    // Debug.Log(rndAns);

                    // Swap the elements at indices i and rnd.
                    string temp = questions[i].answers[j];
                    questions[i].answers[j] = questions[i].answers[rndAns];
                    questions[i].answers[rndAns] = temp;
                }

            }
            SaveQuestions(questions);
            return questions;
        }
        catch (Exception e)
        {
            Debug.Log(e);
            return null;
        }
    }

    private async void Awake()
    {
        instance = this;

        questions.Clear();
        questions = await GetQuestionsAsync();
    }

    public List<Question> LoadList()
    {
        // Uèitaj JSON string iz PlayerPrefs
        string json = PlayerPrefs.GetString("Questions");

        // Deserijalizuj JSON string u listu
        questions = JsonConvert.DeserializeObject<List<Question>>(json);

        return questions;
    }
}
