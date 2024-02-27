using System.Collections;
using TMPro;
using UnityEngine;

public class AdditionScript : MonoBehaviour
{
    [SerializeField] private TMP_InputField FrstNum;
    [SerializeField] private TMP_InputField SecNum;
    [SerializeField] private TextMeshProUGUI FirstNumPlace;
    [SerializeField] private TextMeshProUGUI SecNumPlace;
    [SerializeField] private TextMeshProUGUI Line;
    [SerializeField] private TextMeshProUGUI sign;
    private AudioClip[] voiceClips;
    private AudioClip[] Numbers;
    private AudioClip[] loop;
    public static AudioSource audioSource;
    private bool Explain = false;
    public void explain()
    {
        Explain = true;
        audioSource = GetComponent<AudioSource>();
        LoadAllAudioClips();
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
            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/1-write")));

            FirstNumPlace.gameObject.SetActive(true);

            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/2-then")));

            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/3-write the secondNumber")));

            SecNumPlace.gameObject.SetActive(true);
            sign.gameObject.SetActive(true);
            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/4-put zeros in empty digits_Sonya_Eng")));

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

                                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/8-plus")));

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
                                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/8-plus")));

                                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(SNum)));

                                    temp = SecNumPlace.text.Substring(0, i);
                                    temp2 = SecNumPlace.text.Substring(i + 1);
                                    temp += $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>{SNum}</color>";
                                    SecNumPlace.text = temp + temp2;
                                }
                                audioSource.clip = loop[j];
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
                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/5-put")));

                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(result.ToString())));

                                InstantiateText((result).ToString(), characterPosition.x + 15, characterPosition.y, -350, true);
                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/12-and carry up one_Sonya_Eng")));

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
                if(!textMeshPro.name.Equals("Placeholder") && !textMeshPro.name.Equals("Text") && !textMeshPro.name.Equals("Text (TMP)")&& !textMeshPro.name.Equals("Sign2")&& !textMeshPro.name.Equals("Sign")&& !textMeshPro.name.Equals("FirstNumPlace") && !textMeshPro.name.Equals("SecNumPlace") && !textMeshPro.name.Equals("Line"))
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
    void LoadAllAudioClips()
    {
        voiceClips = Resources.LoadAll<AudioClip>("AdditionTerms/AdditionSound");
        Numbers = Resources.LoadAll<AudioClip>("AdditionTerms/EngNums");
        loop = Resources.LoadAll<AudioClip>("AdditionTerms/AdditionSound/Loop");
    }
}