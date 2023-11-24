using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonGameLogic : MonoBehaviour
{
    Button myButton;
    [SerializeField] TMPro.TextMeshProUGUI myText;

    [SerializeField] int lowerSizeLimit = 20;
    [SerializeField] int upperSizeLimit = 50;

    public static int index;

    // Start is called before the first frame update

    private void Awake()
    {
        myButton = gameObject.GetComponent<Button>();
    }

    private void Start()
    {
        int randomNum = Random.Range(lowerSizeLimit, upperSizeLimit);
        GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, randomNum*3);
        GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, randomNum * 3);
        myText.fontSize = randomNum;
        myText.text = BigPuzzleGameManager.instance.numbers[index++].ToString();
    }

    private void OnEnable()
    {
        myButton.onClick.AddListener(CheckNumber);
    }

    private void OnDisable()
    {
        myButton.onClick.RemoveListener(CheckNumber);
    }

    void CheckNumber()
    {
        bool check = int.Parse(myText.text) == BigPuzzleGameManager.instance.currentNum;
        if (check)
        {
            BigPuzzleGameManager.instance.UpdateCurrentNum();
        }
        else
        {
            GameObject[] remainingPuzzles = GameObject.FindGameObjectsWithTag("Puzzle");
            foreach(GameObject puzzle in remainingPuzzles)
            {
                Destroy(puzzle);
            }
            BigPuzzleGameManager.instance.Wrong();
            index = 0;
        }
        Destroy(gameObject);
    }
}
