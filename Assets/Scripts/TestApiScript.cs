using System.Collections;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Networking;

public class TestApiScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is create
    public static TestApiScript Instance {  get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void CreateAccount(TestGameData data)
    {
        StartCoroutine(PostData(data));
    }

    IEnumerator PostData(TestGameData data)
    {
        //TestGameData gameData = new TestGameData();
        //gameData.id = 1;
        //gameData.username = "test";
        //gameData.password = "test";
        //gameData.score = 1;

        string json = JsonUtility.ToJson(data);
        byte[] body = System.Text.Encoding.UTF8.GetBytes(json);

        var request = new UnityWebRequest("http://localhost:5095/api/TestGameData/create-account", "POST");
        request.uploadHandler = new UploadHandlerRaw(body);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
            Debug.LogError(request.error);
        else
            Debug.Log(request.downloadHandler.text);
    }
}
