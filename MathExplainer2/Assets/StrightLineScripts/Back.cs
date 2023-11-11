using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Back : MonoBehaviour
{
    [SerializeField] private Button backButton;
    [SerializeField] private GameObject MenuPanel;
    [SerializeField] private GameObject StrightLinePanel;

    public void BackBtnChk()
    {
        MenuPanel.active = true;
        StrightLinePanel.active = false;
    }
}
