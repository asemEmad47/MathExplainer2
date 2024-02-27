using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BasicOpScript : MonoBehaviour
{
    public void AdditionPage()
    {
        SceneManager.LoadScene("AdditionScene");
    }
    public void SubtractionPage()
    {
        SceneManager.LoadScene("SlEqsScene");
    }
    public void DevidePage()
    {
        SceneManager.LoadScene("SolvingTwoEqs");
    }
    public void MultiplicationPage()
    {
    }
    public void Back()
    {
        SceneManager.LoadScene("MathProblemPage");
    }
}
