using System;
using TMPro;
using UnityEngine;

public class AuthLogic : MonoBehaviour
{
    [SerializeField] private TMP_InputField usernameInputLogin;
    [SerializeField] private TMP_InputField passwordInputLogin;

    [SerializeField] private TMP_InputField usernameInputRegister;
    [SerializeField] private TMP_InputField passwordInputRegister;


    public void OnRegisterButtonClicked()
    {
        Debug.Log("Register button clicked");

        var accountRequest = new AccountRequest
        {
            Username = usernameInputRegister.text,
            Password = passwordInputRegister.text
        };

        Debug.Log(accountRequest.Username);
        Debug.Log(accountRequest.Password);
        ApiManager.Instance.PostCreateAccount(accountRequest);
    }

    public void OnLoginButtonClicked()
    {
        Debug.Log("Login button clicked with username: " + usernameInputLogin.text);

        var accountRequest = new AccountRequest
        {
            Username = usernameInputLogin.text,
            Password = passwordInputLogin.text
        };

        var accountResponse = new AccountResponse();

        ApiManager.Instance.PostLoginAccount(accountRequest, response =>
        {
            accountResponse.accountId = response.accountId;
            accountResponse.accountName = response.accountName;
            accountResponse.score = response.score;
            Debug.Log("Login successful! Account ID: " + accountResponse.accountId);
        }
            , error =>
            {
                Debug.LogError("Login failed: " + error);
            });

    }
}
