using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    public void Registration()
    {
        if (!UsernameCheck() || !EmailCheck() || !PasswordCheck() || !CheckConfirmPassword())
            return;
        else
        {
            GetPostDataServer.Instance.SingUpUser(usernameInput.text, emailInput.text, passwordInput.text);

            BlockchainData.Instance.SetUserPassword(passwordInput.text);

            UIManagerScene2.Instance.message.text = "You have successfully registered";
            UIManagerScene2.Instance.ShowPanel(gameObject.transform);

            ClearInputText(true, true, true, true);
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
            ClearInputText(true, false, false, false);
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
            ClearInputText(false, true, false, false);
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
            ClearInputText(false, false, true, false);

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
            ClearInputText(false, false, false, true);

            return false;
        }
    }

    void ClearInputText(bool username, bool email, bool password, bool confirmPassword)
    {
        switch (true)
        {
            case true when username:
                usernameInput.text = string.Empty;
                break;
            case true when email:
                emailInput.text = string.Empty;
                break;
            case true when password:
                passwordInput.text = string.Empty;
                break;
            case true when confirmPassword:
                confirmPasswordInput.text = string.Empty;
                break;
            default:
                // Ako nijedna od promenljivih nije true, nema čišćenja.
                break;
        }
    }
}
