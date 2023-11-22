using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject rightAnsUI;
    [SerializeField] GameObject wrongAnsUI;
    [SerializeField] GameObject endGameUI;

    AudioSource alarmAudioSource;
    public AudioSource clockAudioSource;

    [SerializeField] Button addButton;
    [SerializeField] Button divisionButton;
    [SerializeField] Button multiplicationButton;
    [SerializeField] Button subtractButton;

    int score = 0;

    [SerializeField] TMPro.TextMeshProUGUI scoreText;

    Timer timer;

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
        alarmAudioSource = GetComponent<AudioSource>();
        timer = GetComponent<Timer>();
    }

    private void Update()
    {
        if(timer.remainingDuration == 40)
        {
            GameLogic.instance.difficulty = 2;
        }
        if(timer.remainingDuration == 20)
        {
            GameLogic.instance.difficulty = 3;
        }
    }

    void AnswerAddtion()
    {
        userAnswer = "+";
        CheckAnswer();
        GameLogic.instance.GetQuestion();
    }

    void AnswerDivision()
    {
        userAnswer = "/";
        CheckAnswer();
        GameLogic.instance.GetQuestion();
    }
    void AnswerMultiplication()
    {
        userAnswer = "*";
        CheckAnswer();
        GameLogic.instance.GetQuestion();
    }
    void AnswerSubtract()
    {
        userAnswer = "-";
        CheckAnswer();
        GameLogic.instance.GetQuestion();
    }

    void CheckAnswer()
    {
        ChangeUIPosition.instance.ChangePosition();
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
        score += 10;
        UpdateScore(score);
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

    public void DisableButtons()
    {
        addButton.interactable = false;
        subtractButton.interactable = false;
        multiplicationButton.interactable = false;
        divisionButton.interactable = false;
    }
    public void EndGame()
    {
        endGameUI.SetActive(true);
        alarmAudioSource.Play();
        clockAudioSource.Stop();
    }

    void UpdateScore(int score)
    {
        scoreText.text = "Score: " + score.ToString();
    }
}
