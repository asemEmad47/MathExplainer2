using UnityEngine;
using UnityEngine.SceneManagement;

public class DynamicBack : MonoBehaviour
{
    public void Back(string SceneName)
    {

        SceneManager.LoadScene(SceneName);

    }
}
