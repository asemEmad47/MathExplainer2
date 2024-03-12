using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdditionScript : MonoBehaviour
{
    [SerializeField] private TMP_InputField FrstNum;
    [SerializeField] private TMP_InputField SecNum;
    [SerializeField] private TextMeshProUGUI FirstNumPlace;
    [SerializeField] private TextMeshProUGUI SecNumPlace;
    [SerializeField] private TextMeshProUGUI Line;
    [SerializeField] private TextMeshProUGUI sign;
    [SerializeField] private TextMeshProUGUI ExplainTxt;
    [SerializeField] private TextMeshProUGUI LangBtnTxt;
    [SerializeField] private TextMeshProUGUI FirstPlaceHolder;
    [SerializeField] private TextMeshProUGUI ScndPlaceHolder;
    [SerializeField] private Button LangBtn;
    private AudioClip[] loop;
    public static AudioSource audioSource;
    private bool Explain = false;
    string SpeakerName = "_Heba_Eng";
    bool IsEng = true;
    public void Start()
    {
        LoadAllAudioClips();
        LangBtn.onClick.AddListener(LangBtnClick);
    }
    public void explain()
    {
        Explain = true;
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(solve());
    }

    public void FillWithZeros(string symb)
    {
        int LenDifference = Mathf.Abs(FrstNum.text.Length - SecNum.text.Length);
        string tempstr = "";
        for (int i = 0; i < LenDifference; i++)
        {
            tempstr += symb;
        }
        if (FrstNum.text.Length > SecNum.text.Length)
        {
            tempstr += SecNum.text;
            SecNumPlace.text = tempstr;
            FirstNumPlace.text = FrstNum.text;
        }
        else
        {
            tempstr += FrstNum.text;
            FirstNumPlace.text = tempstr;
            SecNumPlace.text = SecNum.text;
        }
        string TempFrstNum = "";
        string TempSecNum = "";
        for (int i = 0; i < FirstNumPlace.text.Length; i++)
        {
            TempFrstNum += FirstNumPlace.text[i] + " ";
        }
        for (int i = 0; i < SecNumPlace.text.Length; i++)
        {
            TempSecNum += SecNumPlace.text[i] + " ";
        }
        FirstNumPlace.text = TempFrstNum;
        SecNumPlace.text = TempSecNum;
    }
    public IEnumerator solve()
    {
        if (Explain)
        {
            ResetAllValues();
            FillWithZeros(" ");
            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/first" + SpeakerName)));

            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/write the first number" + SpeakerName)));

            FirstNumPlace.gameObject.SetActive(true);

            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/then" + SpeakerName)));

            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/write the second number" + SpeakerName)));

            SecNumPlace.gameObject.SetActive(true);
            sign.gameObject.SetActive(true);
            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/put zeros in empty digits" + SpeakerName)));

            FillWithZeros("0");
            Line.gameObject.SetActive(true);
        }
        if (FirstNumPlace != null)
        {
            // Populate textInfo manually
            TMP_TextInfo textInfo = FirstNumPlace.GetTextInfo(FirstNumPlace.text);

            // Check if textInfo is available
            if (textInfo != null)
            {
                bool IsCarried = false;
                for (int i = FirstNumPlace.text.Length - 1; i >= 0; i--)
                {
                    string FNum = FirstNumPlace.text[i].ToString();
                    string SNum = SecNumPlace.text[i].ToString();
                    if (!FirstNumPlace.text[i].Equals(' '))
                    {
                        if (Explain)
                        {
                            for (int j = 0; j < loop.Length - 1; j++)
                            {
                                if (IsCarried && j == 0)
                                {
                                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait("1")));

                                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/plus" + SpeakerName)));

                                }
                                if (j == 0)
                                {
                                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(FNum)));

                                    if (IsCarried)
                                    {

                                        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait((int.Parse(FNum) + 1).ToString())));

                                    }
                                    string temp = FirstNumPlace.text.Substring(0, i);
                                    string temp2 = FirstNumPlace.text.Substring(i + 1);
                                    temp += $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>{FNum}</color>";
                                    FirstNumPlace.text = temp + temp2;
                                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/plus" + SpeakerName)));

                                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(SNum)));

                                    temp = SecNumPlace.text.Substring(0, i);
                                    temp2 = SecNumPlace.text.Substring(i + 1);
                                    temp += $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>{SNum}</color>";
                                    SecNumPlace.text = temp + temp2;
                                }
                                audioSource.clip = loop[j];
                                Debug.Log(loop[j].name);
                                audioSource.Play();
                                yield return new WaitForSeconds(audioSource.clip.length);
                                if (j == 0)
                                {
                                    if (IsCarried)
                                        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait((int.Parse(FNum) + 1).ToString())));
                                    else
                                        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(FNum)));
                                }
                                else if (j == 2)
                                {
                                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(SNum)));
                                }
                                else if (j == 3)
                                {
                                    if (IsCarried)
                                        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait((int.Parse(FNum) + 1).ToString())));
                                    else
                                        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(FNum)));
                                }
                            }
                        }

                        if (textInfo.characterInfo != null && i < textInfo.characterInfo.Length)
                        {
                            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

                            Vector3 characterPosition = FirstNumPlace.transform.TransformPoint(charInfo.bottomLeft);

                            int result = int.Parse(FNum.ToString()) + int.Parse(SNum.ToString());

                            if (int.Parse(FNum.ToString()) + int.Parse(SNum.ToString()) < 10)
                            {
                                if (IsCarried)
                                    result += 1;
                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(result.ToString())));
                                if (!IsCarried)
                                {
                                    InstantiateText(result.ToString(), characterPosition.x + 15, characterPosition.y, -350, true);
                                }
                                else
                                {

                                }
                                InstantiateText((result).ToString(), characterPosition.x + 15, characterPosition.y, -350, true);
                                IsCarried = false;
                            }
                            else
                            {
                                if (IsCarried)
                                    result += 1;
                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(result.ToString())));

                                result -= 10;
                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/put" + SpeakerName)));

                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(result.ToString())));

                                InstantiateText((result).ToString(), characterPosition.x + 15, characterPosition.y, -350, true);
                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/and carry up one" + SpeakerName)));

                                if (i - 1 >= 0)
                                {
                                    charInfo = textInfo.characterInfo[i - 1];
                                    characterPosition = FirstNumPlace.transform.TransformPoint(charInfo.bottomLeft);
                                    InstantiateText("1", characterPosition.x - 28, characterPosition.y, 150, false);
                                    IsCarried = true;
                                }
                            }
                        }
                    }

                }
            }
        }
    }
    public void InstantiateText(string txt, float XPos, float YPos, float Distance, bool IsResult)
    {
        if (FirstNumPlace != null)
        {
            GameObject newTextObject = new GameObject("DynamicText");
            TextMeshProUGUI newTextMesh = newTextObject.AddComponent<TextMeshProUGUI>();

            newTextMesh.text = txt;
            newTextMesh.font = FirstNumPlace.font;
            newTextMesh.fontSize = FirstNumPlace.fontSize;
            newTextMesh.color = Color.black;
            newTextMesh.alignment = FirstNumPlace.alignment;
            newTextMesh.fontStyle = FontStyles.Bold;

            newTextObject.transform.SetParent(FirstNumPlace.transform.parent);
            newTextObject.transform.position = new Vector2(XPos, YPos + Distance);

            newTextMesh.raycastTarget = false;
            if (IsResult)
            {
                newTextMesh.fontSize = 85f;
            }
        }
    }
    public void ResetAllValues()
    {
        GameObject additionObject = GameObject.Find("Addition");

        if (additionObject != null)
        {
            TextMeshProUGUI[] textMeshPros = additionObject.GetComponentsInChildren<TextMeshProUGUI>();

            foreach (TextMeshProUGUI textMeshPro in textMeshPros)
            {
                if(!textMeshPro.name.Equals("Placeholder") && !textMeshPro.name.Equals("Text") && !textMeshPro.name.Equals("Text (TMP)")&& !textMeshPro.name.Equals("Sign2")&& !textMeshPro.name.Equals("Sign")&&!textMeshPro.name.Equals("LangBtnTxt") && !textMeshPro.name.Equals("FirstNumPlace") && !textMeshPro.name.Equals("SecNumPlace") && !textMeshPro.name.Equals("Line"))
                    Destroy(textMeshPro);
            }
        }
        Line.gameObject.SetActive(false);
        FirstNumPlace.text ="";
        SecNumPlace.text = "";
        FirstNumPlace.gameObject.SetActive(false) ;
        SecNumPlace.gameObject.SetActive(false) ;
        sign.gameObject.SetActive(false) ;
    }
    public void LoadAllAudioClips()
    {
        loop = Resources.LoadAll<AudioClip>("AdditionTerms/AdditionSound/EngLoop");
    }
    public void LangBtnClick()
    {
        if (IsEng)
        {
            SpeakerName = "_Heba_Egy";
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

            SpeakerName = "_Heba_Eng";
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