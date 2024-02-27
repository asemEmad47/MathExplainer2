using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MathProblemScripts : MonoBehaviour
{
    public void BasicOperationPage()
    {
        SceneManager.LoadScene("BasicOpScene");
    }      
    public void SLPage()
    {
        SceneManager.LoadScene("SlEqsScene");
    }       
    public void TwoEqsPage()
    {
        SceneManager.LoadScene("SolvingTwoEqs");
    }    
    public void FactorizationPage()
    {
    }
    public void Back()
    {
        SceneManager.LoadScene("DirectorScene");

    }
}
