using UnityEngine;

public class TestMonoScript : MonoBehaviour
{
    void Start()
    {
        TestGameData gameData = new TestGameData
        {
            id = 1,
            username = "test",
            password = "test",
            score = 1,
        };

        TestApiScript.Instance.CreateAccount(gameData);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
