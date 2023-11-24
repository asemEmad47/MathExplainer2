using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Timer : MonoBehaviour
{

    [SerializeField] private Image uiFill;
    [SerializeField] private TMPro.TextMeshProUGUI uiText;

    public int Duration;

    public int remainingDuration;

    bool isDone = false;

    private void Start()
    {
        Being(Duration);
        uiFill.fillAmount = 1;
    }

    private void Being(int Second)
    {
        remainingDuration = Second;
        StartCoroutine(UpdateTimer());
    }
    private void Update()
    {
        if (!isDone)
        {
            if (uiFill.fillAmount != 0)
            {
                uiFill.fillAmount -= Time.deltaTime * 1;
            }
            else
            {
                uiFill.fillAmount = 1;
            }
        }
    }

    private IEnumerator UpdateTimer()
    {
        while(remainingDuration >= 0)
        {
            uiText.text = remainingDuration.ToString();
            remainingDuration--;
            yield return new WaitForSeconds(1f);
        }
        OnEnd();
    }

    private void OnEnd()
    {
        //DisableButtons();
        //EndGame();
        uiFill.fillAmount = 0;
        isDone = true;
        print("End");
    }
}
