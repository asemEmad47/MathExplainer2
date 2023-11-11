using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private Button StrightLineBtn;
    [SerializeField] private GameObject MenuPanel;
    [SerializeField] private GameObject StrightLinePanel;

    public void StrightLineChk()
    {
        MenuPanel.active = false;
        StrightLinePanel.active = true;
    }
}
