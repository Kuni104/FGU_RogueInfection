using UnityEngine;

public class TestMonoScript : MonoBehaviour
{
    void Start()
    {
        TestGameData gameData = new TestGameData();
        gameData.id = 1;
        gameData.username = "username";
        gameData.password = "password";
        gameData.score = 1;

        TestApiScript.CreateAccount(gameData,
        onSuccess: () => Debug.Log("Saved!"),
        onError: (e) => Debug.LogError("Save failed: " + e)
        );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
