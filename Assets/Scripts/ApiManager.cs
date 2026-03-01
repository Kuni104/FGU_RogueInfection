using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class ApiManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static ApiManager Instance { get; private set; }

    [SerializeField]
    private string baseUrl = "https://localhost:7229";

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    public void GetAccount(System.Action<string> onSuccess, System.Action<string> onError)
    {
        StartCoroutine(GetAccountCoroutine("/api/account/get-account", onSuccess, onError));
    }

    private IEnumerator GetAccountCoroutine(string endpoint, System.Action<string> onSuccess, System.Action<string> onError)
    {
        using var request = UnityWebRequest.Get(baseUrl + endpoint);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            onError?.Invoke(request.error);
            yield break;
        }

        onSuccess?.Invoke(request.downloadHandler.text);
    }

    /* ===================== POST ===================== */

    public void PostCreateAccount(AccountRequest data)
    {
        StartCoroutine(PostCreateAccountCoroutine(data));
    }

    private IEnumerator PostCreateAccountCoroutine(AccountRequest data)
    {
        Debug.Log("Posting create account...");
        string json = JsonUtility.ToJson(data);
        byte[] body = Encoding.UTF8.GetBytes(json);

        Debug.Log(data.Username);
        Debug.Log(data.Password);
        Debug.Log("JSON payload: " + json);
        Debug.Log(body);

        var request = new UnityWebRequest("https://localhost:7229/api/account/create-account", "POST");
        request.uploadHandler = new UploadHandlerRaw(body);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();
        Debug.Log("Response received for create account.");
        if (request.result != UnityWebRequest.Result.Success)
            Debug.LogError(request.error);
        else
            Debug.Log(request.downloadHandler.text);
    }

    public void PostLoginAccount(AccountRequest data, System.Action<AccountResponse> onSuccess, System.Action<string> onError)
    {
        StartCoroutine(PostLoginAccountCoroutine(data, onSuccess, onError));
    }

    private IEnumerator PostLoginAccountCoroutine(AccountRequest data, System.Action<AccountResponse> onSuccess, System.Action<string> onError)
    {
        Debug.Log("Posting login account...");
        string json = JsonUtility.ToJson(data);
        byte[] body = Encoding.UTF8.GetBytes(json);

        Debug.Log(data.Username);
        Debug.Log(data.Password);
        Debug.Log("JSON payload: " + json);
        Debug.Log(body);

        using var request = new UnityWebRequest("https://localhost:7229/api/account/login", "POST");
        request.uploadHandler = new UploadHandlerRaw(body);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            onError?.Invoke(request.error);
            yield break;
        }

        try
        {
            AccountResponse responseObj =
                JsonUtility.FromJson<AccountResponse>(request.downloadHandler.text);

            Debug.Log(responseObj.accountId);
            Debug.Log(responseObj.accountName);
            Debug.Log(responseObj.score);

            onSuccess?.Invoke(responseObj);
        }
        catch
        {
            onError?.Invoke("JSON parse failed");
        }
    }
}
