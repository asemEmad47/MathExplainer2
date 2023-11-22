using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeUIPosition : MonoBehaviour
{
    [SerializeField] RectTransform choices;
    [SerializeField] RectTransform firstPart;
    [SerializeField] RectTransform secondPart;
    [SerializeField] RectTransform emptySpace;

    public static ChangeUIPosition instance;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        ChangePosition();
    }

    public void ChangePosition()
    {
        switch (GameLogic.instance.difficulty)
        {
            case 1:
                choices.localPosition = new Vector3(-45.6f, choices.localPosition.y, choices.localPosition.z);
                firstPart.localPosition = new Vector3(-349.8f, firstPart.localPosition.y, firstPart.localPosition.z);
                secondPart.localPosition = new Vector3(-124f, secondPart.localPosition.y, secondPart.localPosition.z);
                emptySpace.localPosition = new Vector3(0, emptySpace.localPosition.y, emptySpace.localPosition.z);
                return;
            case 2:
                choices.localPosition = new Vector3(-45.6f, choices.localPosition.y, choices.localPosition.z);
                firstPart.localPosition = new Vector3(-323.3f, firstPart.localPosition.y, choices.localPosition.z);
                secondPart.localPosition = new Vector3(-139.9f, secondPart.localPosition.y, choices.localPosition.z);
                emptySpace.localPosition = new Vector3(0, emptySpace.localPosition.y, emptySpace.localPosition.z);
                return;
            case 3:
                choices.localPosition = new Vector3(-123f, choices.localPosition.y, choices.localPosition.z);
                firstPart.localPosition = new Vector3(-407f, firstPart.localPosition.y, firstPart.localPosition.z);
                secondPart.localPosition = new Vector3(-178f, secondPart.localPosition.y, secondPart.localPosition.z);
                emptySpace.localPosition = new Vector3(-77, emptySpace.localPosition.y, emptySpace.localPosition.z);
                return;
        }
    }
}
