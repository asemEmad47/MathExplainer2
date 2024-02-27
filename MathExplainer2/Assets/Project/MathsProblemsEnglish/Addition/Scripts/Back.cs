using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DynamicBack : MonoBehaviour
{
    public static void Back(string SceneName)
    {
        SceneManager.LoadScene(SceneName);

    }
}
