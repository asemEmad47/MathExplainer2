using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonGameLogic : MonoBehaviour
{
    Button myButton;
    [SerializeField] TMPro.TextMeshProUGUI myText;

    [SerializeField] int lowerSizeLimit = 20;
    [SerializeField] int upperSizeLimit = 50;

    [SerializeField] Color[] buttonColors;

    public static int index;

    static int dummyNum = 1; //used to change tween rotation direction

    // Start is called before the first frame update

    private void Awake()
    {
        myButton = gameObject.GetComponent<Button>();
    }

    private void Start()
    {
        dummyNum *= -1;

        transform.DOScale(new Vector3(2, 2, 2), 1f);
        transform.DORotate(new Vector3(0, 0, 360 * dummyNum), Random.Range(2f, 15f), RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);

        //Change Button Size and Font size randomly
        int randomNum = Random.Range(lowerSizeLimit, upperSizeLimit);
        GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, randomNum * 3);
        GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, randomNum * 3);
        myText.fontSize = randomNum;

        //Change Color Randomly
        gameObject.GetComponent<Image>().color = buttonColors[Random.Range(0, buttonColors.Length)];

        //Give each button a number
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

    private void OnDestroy()
    {
        transform.DOKill();
    }
}
