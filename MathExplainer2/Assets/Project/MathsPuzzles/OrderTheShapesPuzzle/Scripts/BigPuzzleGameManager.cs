using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class BigPuzzleGameManager : MonoBehaviour
{
    [SerializeField] GameObject bigPrefab; //It's a button

    [SerializeField] Transform[] spawnPos;

    [SerializeField] GameObject rightAnsUI;
    [SerializeField] GameObject wrongAnsUI;

    [SerializeField] TMPro.TextMeshProUGUI scoreText;

    public int[] numbers; //numbers in the scene

    public int currentNum; //smallest number

    public int index;

    int score = 0;

    public static BigPuzzleGameManager instance;

    public int difficulty = 1;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SpawnBigPuzzle();
    }

    public void SpawnBigPuzzle()
    {
        numbers = GenerateNumbers();
        for(int i = 0; i < spawnPos.Length && i < difficulty + 1; i++)
        {
            Instantiate(bigPrefab, spawnPos[i]);
        }
        Array.Sort(numbers);
        currentNum = numbers[0];
    }

    public void UpdateCurrentNum()
    {
        if(index < ButtonGameLogic.index - 1)
        {
            index++;
            currentNum = numbers[index];
        }
        else
        {
            Right();
        }
    }

    public void Right() { StartCoroutine("RightAnswer"); }
    IEnumerator RightAnswer()
    {
        ButtonGameLogic.index = 0;
        score += 10;
        UpdateScore(score);
        rightAnsUI.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        rightAnsUI.SetActive(false);
        SpawnBigPuzzle();
    }

    public void Wrong() { StartCoroutine("WrongAnswer"); }
    IEnumerator WrongAnswer()
    {
        wrongAnsUI.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        wrongAnsUI.SetActive(false);
        SpawnBigPuzzle();
    }

    void UpdateScore(int score)
    {
        scoreText.text = "Score: " + score.ToString();
    }

    int[] GenerateNumbers()
    {
        index = 0;
        int[] numbers = {0, 0, 0, 0};
        for(int i = 0; i < numbers.Length; i++)
        {
            while (true)
            {
                int randomNum = UnityEngine.Random.Range(1, 100);
                if (!numbers.Contains(randomNum))
                {
                    numbers[i] = randomNum;
                    break;
                }
            }
        }
        return numbers;
    }
}
