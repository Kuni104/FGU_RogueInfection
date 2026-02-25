using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class TestApiScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is create
        public static TestApiScript Instance { get; private set; }

        private const string BaseUrl = "http://localhost:5095";

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);
        }

        public static void CreateAccount(TestGameData data, System.Action onSuccess, System.Action<string> onError = null)
        {
            string json = JsonUtility.ToJson(data);
            Instance.StartCoroutine(Instance.PostCreateAccount("api/TestGameData/create-account", json, onSuccess, onError));

        }

        public static void SaveGame(TestGameData data, System.Action onSuccess, System.Action<string> onError = null)
        {
            string json = JsonUtility.ToJson(data);
            Instance.StartCoroutine(Instance.PostCoroutine("/api/save", json, onSuccess, onError));
        }

        public static void LoadGame(System.Action<TestGameData> onSuccess, System.Action<string> onError = null)
        {
            Instance.StartCoroutine(Instance.GetCoroutine("/api/load", onSuccess, onError));
        }

        private IEnumerator PostCreateAccount(string endpoint, string json, System.Action onSuccess, System.Action<string> onError)
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);

            using (UnityWebRequest request = new UnityWebRequest(BaseUrl + endpoint, "POST"))
            {
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                    onSuccess?.Invoke();
                else
                    onError?.Invoke(request.error);
            }
        }

        private IEnumerator PostCoroutine(string endpoint, string json, System.Action onSuccess, System.Action<string> onError)
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);

            using (UnityWebRequest request = new UnityWebRequest(BaseUrl + endpoint, "POST"))
            {
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                    onSuccess?.Invoke();
                else
                    onError?.Invoke(request.error);
            }
        }

        private IEnumerator GetCoroutine(string endpoint, System.Action<TestGameData> onSuccess, System.Action<string> onError)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(BaseUrl + endpoint))
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    TestGameData data = JsonUtility.FromJson<TestGameData>(request.downloadHandler.text);
                    onSuccess?.Invoke(data);
                }
                else
                    onError?.Invoke(request.error);
            }
        }
    }
