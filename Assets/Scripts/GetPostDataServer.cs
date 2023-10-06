using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

public class GetPostDataServer : Singleton<GetPostDataServer>
{
    private string URL_PUSH_ACTION = "https://dev-test3.inery.network:443/api/";
    private string urlQuestions = "https://dev-test3.inery.network/api/game/get_questions";


    public static List<Question> questions;
    UserData userData;

    private bool login;

    private void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        GetQuestions();
    }

    public List<Question> GetQuestionsList()
    {
        return questions;
    }

    public bool IsLogged()
    {
        return login;
    }

    public UserData GetUserData()
    {
        return this.userData;
    }

    public List<Question> GetQuestions()
    {
        StartCoroutine(GetQuestions_Coroutine());
        return questions;
    }

    IEnumerator GetQuestions_Coroutine()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(urlQuestions))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            else
            {
                JObject json = JObject.Parse(request.downloadHandler.text);
                JArray rows = json["result"]["rows"] as JArray;

                questions = new List<Question>();

                foreach (var row in rows)
                {
                    Question question = new Question();
                    question.question_id = (int)row["question_id"];
                    question.question = (string)row["question"];
                    question.answers = row["answers"].ToObject<List<string>>();

                    // Dodajte dodatnu logiku za postavljanje taènog odgovora i indeksa taènog odgovora
                    // Na primer, možete postaviti taèan odgovor na prvi odgovor u listi:
                    question.correctAnswer = question.answers[0];
                    question.correctAnswerIndex = 0;

                    questions.Add(question);
                }

                BlockchainData.Instance.GetQuestions(questions);

            }

            yield return new WaitForSeconds(2f);
        }
    }

    public void LoginUser(string username, string password) => StartCoroutine(Login_Coroutine(username, password));

    public IEnumerator Login_Coroutine(string username, string password)
    {
        string url = "https://dev-test3.inery.network/api/user/login";
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);
        form.AddField("security_key", "inery723");

        using (UnityWebRequest request = UnityWebRequest.Post(url, form))
        {
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log(request.downloadHandler.text);

                login = true;


                // SMEŠTANJE PODATAKA UNUTAR PROMENJIVE -------------------------
                // userData = JsonConvert.DeserializeObject<ListUserData>(request.downloadHandler.text).rows[0];
                userData = JsonConvert.DeserializeObject<UserData>(request.downloadHandler.text);

                JObject json = JObject.Parse(request.downloadHandler.text);
                JArray rows = json["result"]["rows"] as JArray;

                userData.user_id = (int)rows[0]["user_id"];
                userData.username = (string)rows[0]["username"];
                userData.email = (string)rows[0]["email"];
                userData.password = (string)rows[0]["password"];
                userData.max_score = (int)rows[0]["max_score"];
                //-----------------------------------------------------------------
            }
        }
    }

    public void SingUpUser(string username, string email, string password) => StartCoroutine(Singup_Coroutine(username, email, password));

    IEnumerator Singup_Coroutine(string username, string email, string password)
    {
        string url = "https://dev-test3.inery.network/api/user/signup";
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("email", email);
        form.AddField("password", password);
        form.AddField("security_key", "inery723");

        using (UnityWebRequest request = UnityWebRequest.Post(url, form))
        {
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log(request.downloadHandler.text);
            }
        }
    }

    public void SetScoreUser(string username, string password, string score) => StartCoroutine(SetScore_Coroutine(username, password, score));

    public IEnumerator SetScore_Coroutine(string username, string password, string score)
    {
        string url = "https://dev-test3.inery.network/api/user/set_score";
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);
        form.AddField("score", score);
        form.AddField("security_key", "inery723");

        using (UnityWebRequest request = UnityWebRequest.Post(url, form))
        {
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log(request.downloadHandler.text);
            }
        }
    }
}

