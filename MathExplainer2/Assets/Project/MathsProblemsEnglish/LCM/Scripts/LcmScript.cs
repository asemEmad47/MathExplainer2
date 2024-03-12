using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LcmScript : MonoBehaviour
{

    [SerializeField] private TMP_InputField FrstNum;
    [SerializeField] private TMP_InputField SecNum;
    [SerializeField] private TextMeshProUGUI ExplainTxt;
    [SerializeField] private TextMeshProUGUI LangBtnTxt;
    [SerializeField] private TextMeshProUGUI FirstPlaceHolder;
    [SerializeField] private TextMeshProUGUI ScndPlaceHolder;
    [SerializeField] private Button LangBtn;
    [SerializeField] private GameObject Circle;
    [SerializeField] private GameObject Line;
    [SerializeField] private GameObject Square;
    private AudioClip[] loop;
    public static AudioSource audioSource;
    private bool Explain = false;

    private int NumOfCircles = 0;
    private int NumOfLines = 0;
    private int NumOfSquares = 0;
    List<float> FirstNumList = new List<float>();
    List<float> SecNumList = new List<float>();

    bool IsEng = true;
    public void Start()
    {
        Circle.SetActive(false); Line.SetActive(false);
        LoadAllAudioClips();
        LangBtn.onClick.AddListener(LangBtnClick);
    }
    public void explain()
    {
        Explain = true;
        audioSource = GetComponent<AudioSource>();
        //StartCoroutine(solve());
    }


    public void solve()
    {
        float FNum = float.Parse(FrstNum.text);
        float SNum = float.Parse(SecNum.text);

        InstatiateCircle(FrstNum.transform.position.x, FrstNum.transform.position.y - 150);
        InstantiateText(FrstNum.text);
        GetLcm(FNum, true, FrstNum.transform.position.x-120, FrstNum.transform.position.y - 250);

        InstatiateCircle(FrstNum.transform.position.x, GameObject.Find("Circle" + (NumOfCircles - 1)).transform.position.y - 50);
        InstantiateText(SecNum.text);
        GetLcm(SNum, false, FrstNum.transform.position.x-120, GameObject.Find("Circle" + (NumOfCircles - 1)).transform.position.y-150);
        GetAns();

    }
    public TextMeshProUGUI InstantiateText(string txt)
    {
        GameObject newTextObject = new GameObject("DynamicText");
        TextMeshProUGUI newTextMesh = newTextObject.AddComponent<TextMeshProUGUI>();
        newTextMesh.transform.SetParent(GameObject.Find("Panel").transform);

        newTextMesh.text = txt;
        newTextMesh.fontSize = 52;
        newTextMesh.color = Color.white;
        newTextMesh.transform.position = new Vector3(GameObject.Find("Circle" + (NumOfCircles-1)).transform.position.x+70, GameObject.Find("Circle" + (NumOfCircles - 1)).transform.position.y,newTextMesh.transform.position.z);
        return newTextMesh;
    }
    public void InstatiateCircle( float XAxis,float YAxis)
    {
        GameObject newCircle = Instantiate(Circle);
        newCircle.SetActive(true);
        newCircle.transform.SetParent(GameObject.Find("Panel").transform);
        newCircle.name = "Circle" + NumOfCircles;
        newCircle.transform.position = new Vector3(XAxis, YAxis, newCircle.transform.position.z);
        NumOfCircles++; // Increment after instantiation
        InstantiateLines(); // Call InstantiateLines after NumOfCircles is incremented
    }
    public void GetAns()
    {
        FirstNumList.Sort();
        SecNumList.Sort();
        int FirstArrCounter = 0;    
        int SecArrCounter = 0;
        while (FirstArrCounter<FirstNumList.Count || SecArrCounter < SecNumList.Count)
        {
            InstantiateSquare();
            TextMeshProUGUI newTextMesh ;
            if (SecArrCounter == SecNumList.Count||(FirstArrCounter < FirstNumList.Count && FirstNumList[FirstArrCounter] < SecNumList[SecArrCounter])){
                newTextMesh = InstantiateText((FirstNumList[FirstArrCounter]).ToString());
                FirstArrCounter++;

            }
            else if (FirstArrCounter == FirstNumList.Count||(SecArrCounter < SecNumList.Count  && FirstNumList[FirstArrCounter] > SecNumList[SecArrCounter]))
            {
                newTextMesh = InstantiateText(SecNumList[SecArrCounter].ToString());
                SecArrCounter++;
            }
            else
            {
                newTextMesh = InstantiateText(FirstNumList[FirstArrCounter].ToString());
                TextMeshProUGUI newTextMesh2 = InstantiateText((SecNumList[SecArrCounter]).ToString());
                newTextMesh2.transform.SetParent(GameObject.Find("Square" + (NumOfSquares-1)).transform);
                newTextMesh2.color = Color.black;

                newTextMesh2.transform.position = new Vector3(GameObject.Find("Square" + (NumOfSquares - 1)).transform.position.x +50, GameObject.Find("Square" + (NumOfSquares - 1)).transform.position.y -50, newTextMesh.transform.position.z);
                FirstArrCounter++;
                SecArrCounter++;

            }

            newTextMesh.transform.SetParent(GameObject.Find("Square" + (NumOfSquares-1)).transform);
            newTextMesh.color = Color.black;
            newTextMesh.transform.position = new Vector3(GameObject.Find("Square" + (NumOfSquares - 1)).transform.position.x+50, GameObject.Find("Square" + (NumOfSquares - 1)).transform.position.y+50, newTextMesh.transform.position.z);
        }
    }

    public void InstantiateLines()
    {
        GameObject circle = GameObject.Find("Circle" + (NumOfCircles - 1)); // Find the last created circle
        if (circle != null)
        {
            GameObject LeftLine = Instantiate(Line);
            LeftLine.SetActive(true);
            LeftLine.transform.SetParent(GameObject.Find("Panel").transform);
            LeftLine.name = "LeftLine" + NumOfLines;
            LeftLine.transform.Rotate(Vector3.forward, -30f);
            LeftLine.transform.position = new Vector3(
                circle.transform.position.x - 50f,
                circle.transform.position.y - 30f,
                LeftLine.transform.position.z
            );

            GameObject rightLine = Instantiate(Line);
            rightLine.SetActive(true);
            rightLine.transform.SetParent(GameObject.Find("Panel").transform);
            rightLine.name = "RightLine" + NumOfLines;
            rightLine.transform.position = new Vector3(
                circle.transform.position.x + 50f,
                circle.transform.position.y - 30f,
                rightLine.transform.position.z
            );
            NumOfLines++;
            rightLine.transform.Rotate(Vector3.forward, 30f);
        }
        else
        {
            Debug.LogWarning("Circle" + (NumOfCircles - 1) + " not found!");
        }
    }
    public void InstantiateSquare()
    {
        GameObject newSquare = Instantiate(Square);
        newSquare.name = "Square" + NumOfSquares;
        newSquare.transform.SetParent(GameObject.Find("Panel").transform);
        if (NumOfSquares == 0)
        {
            GameObject circle = GameObject.Find("Circle" + (NumOfCircles - 1)); // Find the last created circle
            newSquare.transform.position = new Vector3(
            GameObject.Find("Circle0").transform.position.x,
            circle.transform.position.y - 200f,
            newSquare.transform.position.z
            );
        }
        else
        {

            GameObject Square = GameObject.Find("Square" + (NumOfSquares - 1)); // Find the last created circle
            if(Square.transform.position.x + 200 + Square.GetComponent<RectTransform>().rect.width < Screen.width)
            {
                newSquare.transform.position = new Vector3(
                Square.transform.position.x + 200,
                Square.transform.position.y,
                newSquare.transform.position.z
                );
            }
            else
            {
                newSquare.transform.position = new Vector3(
                GameObject.Find("Circle0").transform.position.x,
                Square.transform.position.y-Square.GetComponent<RectTransform>().rect.height-200,
                newSquare.transform.position.z
                );
            }
        }
        NumOfSquares++;
    }
    public void GetLcm(float Num,bool isFirstNum,float XAxis,float YAxis)
    {
        if (Num <= 2)
        {
            DestroyLine();
            return;
        }
        else if (Num % 2 == 0)
        {
            InstatiateCircle(XAxis,YAxis);
            InstantiateText("2");
            DestroyLine();
            InstatiateCircle(XAxis+240, YAxis);
            InstantiateText((Num / 2).ToString());
            YAxis -= 150;
            if (isFirstNum)
            {
                FirstNumList.Add(2);
            }
            else
            {
                SecNumList.Add(2);
            }
            GetLcm(Num / 2, isFirstNum,XAxis+140,YAxis);
        }
        else
        {
            for (int i = 3; i < Num; i++)
            {
                if (i == Num - 1 || i == Num)
                {
                    DestroyLine();
                    return;
                }
                if (Num % i == 0)
                {
                    InstatiateCircle(XAxis-50, YAxis);
                    InstantiateText(i.ToString());
                    if (i == 3)
                        DestroyLine();
                    InstatiateCircle(XAxis+250, YAxis);

                    InstantiateText((Num / i).ToString());
                    YAxis -= 150;
                    if (isFirstNum)
                    {
                        FirstNumList.Add(i);
                        FirstNumList.Add(Num / i);
                    }
                    else
                    {
                        SecNumList.Add(i);
                        SecNumList.Add(Num / i);
                    }
                    GetLcm(Num / i, isFirstNum, XAxis + 250, YAxis);
                    GetLcm(i, isFirstNum, XAxis + 250, YAxis);
                }
            }

        }
    }
    public void DestroyLine()
    {
        Destroy(GameObject.Find("RightLine" + (NumOfLines - 1)));
        Destroy(GameObject.Find("LeftLine" + (NumOfLines - 1)));
    }
    public void LoadAllAudioClips()
    {
        loop = Resources.LoadAll<AudioClip>("AdditionTerms/AdditionSound/EngLoop");
    }
    public void LangBtnClick()
    {
        if (IsEng)
        {
            loop = Resources.LoadAll<AudioClip>("AdditionTerms/AdditionSound/ArabLoop");
            ExplainTxt.text = "ﺡﺮﺷﺍ";
            LangBtnTxt.text = "Eng";
            FirstPlaceHolder.text = "ﻝﻭﻻﺍ ﺩﺪﻌﻟﺍ ﺐﺘﻛﺍ";
            ScndPlaceHolder.text = "ﻲﻧﺎﺜﻟﺍ ﺩﺪﻌﻟﺍ ﺐﺘﻛﺍ";
            IsEng = false;
            AdditionVoiceSpeaker.NumPlace = "EgyNums";
        }
        else
        {

            loop = Resources.LoadAll<AudioClip>("AdditionTerms/AdditionSound/EngLoop");
            LangBtnTxt.text = "ﻱﺮﺼﻣ";
            ExplainTxt.text = "Explain";
            FirstPlaceHolder.text = "First Number";
            ScndPlaceHolder.text = "Second Number";
            AdditionVoiceSpeaker.NumPlace = "EngNums";
            IsEng = true;
        }
    }
}
