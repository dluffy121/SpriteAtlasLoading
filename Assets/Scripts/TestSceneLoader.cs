using UnityEngine;
using UnityEngine.SceneManagement;

public class TestSceneLoader : MonoBehaviour
{
    public void LoadTestScene(string testSceneName)
    {
        SceneManager.LoadScene(testSceneName);
    }
}
