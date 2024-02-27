using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SolvingTwoEqs : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Answer;
    [SerializeField] private GameObject X1Message;
    [SerializeField] private GameObject Y1Message;
    [SerializeField] private GameObject Res1Message;    

    [SerializeField] private GameObject X2Message;
    [SerializeField] private GameObject Y2Message;
    [SerializeField] private GameObject Res2Message; 

    [SerializeField] private GameObject InfoText; 

    [SerializeField] private TMP_InputField X2;
    [SerializeField] private TMP_InputField Y2;
    [SerializeField] private TMP_InputField Res2;   
    
    [SerializeField] private TMP_InputField X1;
    [SerializeField] private TMP_InputField Y1;
    [SerializeField] private TMP_InputField Res1;

    [SerializeField] private Button PauseBtn;
    [SerializeField] private Button ResumeBtn;
    [SerializeField] private Button Explain;  
    
    
    [SerializeField] private Button Seen1;
    [SerializeField] private Button Seen2;
    [SerializeField] private Button Saad1;
    [SerializeField] private Button Saad2;

    [SerializeField] private Button LangButton;

    [SerializeField] GameObject Circle;
    [SerializeField] GameObject Arrow;


    private bool Pause = false;
    private bool InExplain = false;
    private bool Arab = false;
    private TwoEqsParent parent;
    [SerializeField] private TextMeshProUGUI lineRenderer;

    public void OnDisable()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void PauseNow()
    {
        Pause = true;
        ResumeBtn.gameObject.SetActive(true);
        PauseBtn.gameObject.SetActive(false);
        StartCoroutine(ResumeCoroutine());
    }

    public void Resume()
    {
        Pause = false;
        ResumeBtn.gameObject.SetActive(false);
        PauseBtn.gameObject.SetActive(true);
        // Get or add a LineRenderer component to the GameObject
    }
    void Start()
    {
        PauseBtn.onClick.AddListener(PauseNow);
        ResumeBtn.onClick.AddListener(Resume);
        LangButton.onClick.AddListener(ChangeLang);
    }
    void Update()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
    IEnumerator ResumeCoroutine()
    {
        while (Pause)
        {
            yield return null;
        }
    }
    public static void DisplayLineRender(TextMeshProUGUI lineRenderer , TextMeshProUGUI Answer)
    {
        Answer.ForceMeshUpdate();
        TMP_TextInfo textInfo = Answer.textInfo;

        int lastCharacterIndex = -1;
        int lastLineNumber = -1;

        for (int j = 0; j < textInfo.characterCount; j++)
        {
            if (textInfo.characterInfo[j].lineNumber > lastLineNumber)
            {
                lastLineNumber = textInfo.characterInfo[j].lineNumber;
                lastCharacterIndex = j;
            }
        }

        if (lastCharacterIndex >= 0 && lastCharacterIndex < textInfo.characterInfo.Length)
        {
            // Get the position of the last character
            Vector3 lastCharBottomLeft = textInfo.characterInfo[lastCharacterIndex].bottomLeft;
            Vector3 lastCharTopRight = textInfo.characterInfo[lastCharacterIndex].topRight;
            // Calculate the center of the last character
            Vector3 lastCharCenter = (lastCharBottomLeft + lastCharTopRight) / 2f;
            // Transform the center to world coordinates
            Vector3 worldPosition = Answer.transform.TransformPoint(lastCharCenter);
            lineRenderer.transform.position =new Vector3(worldPosition.x+160,worldPosition.y+35,0);
            lineRenderer.gameObject.SetActive(true);
        }
    }
    public void Solve()
    {
        if (!InExplain)
        {
            if (ValidateInputs(X1.text, Y1.text, Res1.text, X2.text, Y2.text, Res2.text))
            {
                if ((float.Parse(Y1.text) > 0 && (float.Parse(Y2.text) > 0)) || (float.Parse(Y1.text) < 0 && (float.Parse(Y2.text) < 0))&& (float.Parse(Y2.text) % float.Parse(Y1.text) !=0 || float.Parse(Y2.text) % float.Parse(Y2.text) != 0)&&!Arab) {
                    parent = new SubtractionEng(Circle,Arrow,X1,Y1,Res1,X2,Y2,Res2, ref Answer , ref lineRenderer);
                }
                else if((float.Parse(Y1.text) > 0 && (float.Parse(Y2.text) > 0)) || (float.Parse(Y1.text) < 0 && (float.Parse(Y2.text) < 0)) && Arab)
                {
                    //
                }
                else if((float.Parse(Y1.text) > 0 && (float.Parse(Y2.text) < 0)) || (float.Parse(Y1.text) < 0 && (float.Parse(Y2.text) > 0)) && (float.Parse(Y2.text) % float.Parse(Y1.text) == 0 || float.Parse(Y2.text) % float.Parse(Y2.text) == 0) && !Arab)
                {
                    parent = new AdditionEng(Circle, Arrow, X1, Y1, Res1, X2, Y2, Res2, ref Answer, ref lineRenderer);
                }
                else if ((float.Parse(Y1.text) > 0 && (float.Parse(Y2.text) < 0)) || (float.Parse(Y1.text) < 0 && (float.Parse(Y2.text) > 0)) && Arab)
                {
                    //
                }
                parent.Solve();
            }
        }

    }
    public bool ValidateInputs(string X1, string y1, string res1, string X2, string y2, string res2)
    {
        bool isX1Number = float.TryParse(X1, out _);
        bool isY1Number = float.TryParse(y1, out _);
        bool isRes1Number = float.TryParse(res1, out _);        
        
        bool isX2Number = float.TryParse(X2, out _);
        bool isY2Number = float.TryParse(y2, out _);
        bool isRes2Number = float.TryParse(res2, out _);

        if (!isX1Number)
        {
            X1Message.SetActive(true);
            return false;
        }     
        else
            X1Message.SetActive(false);

        if (!isX2Number)
        {
            X2Message.SetActive(true);
            return false;
        }     
        else
            X2Message.SetActive(false);

        if (!isRes1Number)
        {
            Res1Message.SetActive(true);
            return false;
        }
        else
            Res1Message.SetActive(false);

        if (!isY1Number)
        {
            Y1Message.SetActive(true);
            return false;
        }     
        else
            Y1Message.SetActive(false);
        if (!isY2Number)
        {
            Y2Message.SetActive(true);
            return false;
        }
        else
            Y2Message.SetActive(false);
        if (!isRes2Number)
        {
            Res2Message.SetActive(true);
            return false;
        }
        else
            Res2Message.SetActive (false);
        return true;
    }
    public void SolveStepByStep()
    {
        if (ValidateInputs(X1.text, Y1.text, Res1.text, X2.text, Y2.text, Res2.text))
        {
            if ((float.Parse(Y1.text) > 0 && (float.Parse(Y2.text) > 0)) || (float.Parse(Y1.text) < 0 && (float.Parse(Y2.text) < 0)) && !Arab)
            {
                parent = new SubtractionEng(Circle, Arrow, X1, Y1, Res1, X2, Y2, Res2, ref Answer, ref lineRenderer);
            }
            else if ((float.Parse(Y1.text) > 0 && (float.Parse(Y2.text) > 0)) || (float.Parse(Y1.text) < 0 && (float.Parse(Y2.text) < 0)) && Arab)
            {
                //
            }
            else if ((float.Parse(Y1.text) > 0 && (float.Parse(Y2.text) < 0)) || (float.Parse(Y1.text) < 0 && (float.Parse(Y2.text) > 0)) && !Arab)
            {
                parent = new AdditionEng(Circle, Arrow, X1, Y1, Res1, X2, Y2, Res2, ref Answer, ref lineRenderer);
            }
            else if ((float.Parse(Y1.text) > 0 && (float.Parse(Y2.text) < 0)) || (float.Parse(Y1.text) < 0 && (float.Parse(Y2.text) > 0)) && Arab)
            {
                //
            }
            StartCoroutine(SpeakSteps(parent.SolveStepByStep()));
        }
    }
    public IEnumerator SpeakSteps(List<string> writingSteps)
    {
        PauseBtn.gameObject.SetActive(true);
        for (int i = 0; i < writingSteps.Count; i++)
        {
            yield return StartCoroutine(ResumeCoroutine());
            yield return StartCoroutine(parent.SpeakAndWait(writingSteps[i], i));
            if (i == writingSteps.Count - 1)
            {
                InExplain = false;

            }
        }
    }

    public void ChangeLang()
    {
        if (!Arab)
        {
            TextMeshProUGUI textComponent = LangButton.GetComponentInChildren<TextMeshProUGUI>();
            textComponent.alignment = TextAlignmentOptions.Center;
            textComponent.text = "Eng";
            Arab = true;
            Saad1.gameObject.SetActive(true);
            Saad2.gameObject.SetActive(true);
            Seen1.gameObject.SetActive(true);
            Seen2.gameObject.SetActive(true);
            InfoText.GetComponent<TextMeshProUGUI>().text = "                                  =\r\n\r\n                                  =";
            GameObject.Find("Solve").transform.Find("SolveTxt").GetComponent<TextMeshProUGUI>().text = "ﻞﺣ"; 
            GameObject.Find("Explain").transform.Find("ExplainTxt").GetComponent<TextMeshProUGUI>().text = "ﺡﺮﺷﺍ";
        }
        else
        {
            Arab = false;
            TextMeshProUGUI textComponent = LangButton.GetComponentInChildren<TextMeshProUGUI>();
            textComponent.alignment = TextAlignmentOptions.Center;
            textComponent.text = "ﻲﺑﺮﻋ";
            Saad1.gameObject.SetActive(false);
            Saad2.gameObject.SetActive(false);      
            Seen1.gameObject.SetActive(false);
            Seen2.gameObject.SetActive(false);
            InfoText.GetComponent<TextMeshProUGUI>().text = "            X               Y  =\r\n\r\n            X               Y  =";
            GameObject.Find("Solve").transform.Find("SolveTxt").GetComponent<TextMeshProUGUI>().text = "Solve";
            GameObject.Find("Explain").transform.Find("ExplainTxt").GetComponent<TextMeshProUGUI>().text = "Explain";
        }
    }
}