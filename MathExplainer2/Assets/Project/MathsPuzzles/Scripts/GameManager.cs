using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject rightAnsUI;
    [SerializeField] GameObject wrongAnsUI;

    [SerializeField] Button addButton;
    [SerializeField] Button divisionButton;
    [SerializeField] Button multiplicationButton;
    [SerializeField] Button subtractButton;

    string userAnswer;

    public static GameManager instance;

    private void Awake()
    {
        instance = this;

        addButton.onClick.AddListener(AnswerAddtion);
        divisionButton.onClick.AddListener(AnswerDivision);
        multiplicationButton.onClick.AddListener(AnswerMultiplication);
        subtractButton.onClick.AddListener(AnswerSubtract);
    }

    // Start is called before the first frame update
    void Start()
    {
        GameLogic.instance.GenerateQuestion();
    }

    void AnswerAddtion()
    {
        userAnswer = "+";
        CheckAnswer();
        GameLogic.instance.GenerateQuestion();
    }

    void AnswerDivision()
    {
        userAnswer = "/";
        CheckAnswer();
        GameLogic.instance.GenerateQuestion();
    }
    void AnswerMultiplication()
    {
        userAnswer = "*";
        CheckAnswer();
        GameLogic.instance.GenerateQuestion();
    }
    void AnswerSubtract()
    {
        userAnswer = "-";
        CheckAnswer();
        GameLogic.instance.GenerateQuestion();
    }

    void CheckAnswer()
    {
        if(userAnswer == GameLogic.instance.currentOperation)
        {
            StartCoroutine(RightAnswer());
        }
        else
        {
            StartCoroutine(WrongAnswer());
        }
    }

    IEnumerator RightAnswer()
    {
        rightAnsUI.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        rightAnsUI.SetActive(false);
    }

    IEnumerator WrongAnswer()
    {
        wrongAnsUI.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        wrongAnsUI.SetActive(false);
    }
}
