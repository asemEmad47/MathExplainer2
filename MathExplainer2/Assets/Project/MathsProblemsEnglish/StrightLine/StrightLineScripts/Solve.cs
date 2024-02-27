using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Solve : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Answer;
    [SerializeField] private GameObject XMessage;
    [SerializeField] private GameObject YMessage;
    [SerializeField] private GameObject SlopeMessage;
    [SerializeField] private TMP_InputField XAxis;
    [SerializeField] private TMP_InputField YAxis;
    [SerializeField] private TMP_InputField Numerator;
    [SerializeField] private TMP_InputField Denominator;
    [SerializeField] private Button PauseBtn;
    [SerializeField] private Button ResumeBtn;
    [SerializeField] private Button SolveQ;
    [SerializeField] private Button Explain;
    [SerializeField] GameObject Circle;
    [SerializeField] GameObject Arrow;
    private AudioClip[] voiceClips;
    private AudioClip[] Numbers;
    private AudioSource audioSource;
    public bool CalledFromAnotherScript = false;
    public int type = 0;
    private int clipIndex = 0;
    private bool IsAnimatedPart = false;
    private bool Pause = false;
    private bool InExplain = false;
    public static bool IsAnimatedPart2 = false;
    private string deno;
    private string nue;
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
    }

    void Start()
    {
        PauseBtn.onClick.AddListener(PauseNow);
        ResumeBtn.onClick.AddListener(Resume);
        SolveQ.onClick.AddListener(SolveQuestion);
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

    public void SetSolve(GameObject Xmsg, GameObject Ymsg, GameObject Slpmsg, TMP_InputField X, TMP_InputField y, string numerator, string denom, TextMeshProUGUI Ans, bool Another, int ty, GameObject circle, GameObject arrow , Button pause1 , Button Resume1)
    {
        XMessage = Xmsg;
        YMessage = Ymsg;
        SlopeMessage = Slpmsg;
        XAxis = X;
        YAxis = y;
        nue = numerator;
        deno = denom;
        Answer = Ans;
        CalledFromAnotherScript = Another;
        type = ty;
        this.Circle = circle;
        this.Arrow = arrow;
        this.PauseBtn = pause1;
        this.ResumeBtn = Resume1;
        PauseBtn.onClick.AddListener(PauseNow);
        ResumeBtn.onClick.AddListener(Resume);
    }
    public void SolveQuestion()
    {
        if (!InExplain)
        {
            ResetAns();
            if (deno == null || deno.Equals("") || nue == null || nue.Equals(""))
            {
                nue = Numerator.text;
                deno = Denominator.text;
            }
            if (ValidateInputs(XAxis.text, YAxis.text, nue, deno))
            {
                float XVal = float.Parse(XAxis.text), YVal = float.Parse(YAxis.text);
                int DenoVal = int.Parse(deno), NueVal = int.Parse(nue);
                float Result = (float)NueVal / DenoVal;
                float MX = XVal * Result;
                if (NueVal % DenoVal != 0)
                    Answer.text += $"<line-height=25%>{FractionCalculator.GetFrec(false, "m = ", NueVal, DenoVal)}</line-height>\n";
                else
                    Answer.text += $"<line-height=25%>{"m = " + NueVal}</line-height>\n";
                Answer.text += $"<line-height=25%>y = mx + c</line-height>\n";
                if (NueVal % DenoVal != 0)
                    Answer.text += $"<line-height=25%>{FractionCalculator.GetFrec(false, "y = ", NueVal, DenoVal, "x + c")}</line-height>\n";
                else
                    Answer.text += $"<line-height=25%>{"y = " + NueVal + "x + c"}</line-height>\n";
                if (NueVal % DenoVal != 0)
                    Answer.text += $"<line-height=25%>{FractionCalculator.GetFrec(false, YVal.ToString() + " = ", NueVal, DenoVal, " X " + XVal + " + c")} </line-height>\n";
                else
                    Answer.text += $"<line-height=25%>{YVal.ToString() + " = " + XVal + "X + c"} </line-height>\n";
                FractionCalculator.MultiplyFractionByInteger(ref NueVal, ref DenoVal, (int)XVal);
                if (NueVal % DenoVal == 0)
                {
                    Answer.text += $"<line-height=25%>{YAxis.text} = {MX} + c </line-height>\n" +
                    $"<line-height=25%>{YAxis.text} - {MX} = c </line-height>\n" +
                    $"<line-height=25%>c = {YAxis.text} - {MX}</line-height>\n" +
                    $"<line-height=25%>c = {YVal - MX}</line-height>\n";
                    if ((YVal - MX) > 0)
                        Answer.text += $"<line-height=25%>{FractionCalculator.GetFrec(false, "y = ", int.Parse(nue), int.Parse(deno), "x + " + (YVal - MX))} </line-height>\n";
                    else
                        Answer.text += $"<line-height=25%>{FractionCalculator.GetFrec(false, "y = ", int.Parse(nue), int.Parse(deno), "x   " + (YVal - MX))} </line-height>\n";
                }
                else
                {
                    Answer.text += $"<line-height=25%>{FractionCalculator.GetFrec(false, YAxis.text + " = ", NueVal, DenoVal, " + c")}</line-height>\n" +
                    $"<line-height=25%>{FractionCalculator.GetFrec(false, YAxis.text + " - ", NueVal, DenoVal, " = c")}</line-height>\n" +
                    $"<line-height=25%>{FractionCalculator.GetFrec(false, "c = " + YAxis.text + " - ", NueVal, DenoVal)}</line-height>\n";
                    FractionCalculator.SubtractFractions((int)YVal, 1, ref NueVal, ref DenoVal);
                    Answer.text += $"<line-height=40%>{FractionCalculator.GetFrec(false, "c = ", NueVal, DenoVal)}</line-height>\n";
                    if ((YVal - MX) > 0)
                        Answer.text += $"<line-height=40%>{FractionCalculator.GetFrec(false, "y = ", int.Parse(nue), int.Parse(deno), "x + ", NueVal, DenoVal)}</line-height>\n";
                    else
                        Answer.text += $"<line-height=40%>{FractionCalculator.GetFrec(false, "y = ", int.Parse(nue), int.Parse(deno), "x ", NueVal, DenoVal)} </line-height>\n";
                }
            }
        }

    }
    public bool ValidateInputs(string X, string y, string s1, string s2)
    {
        bool isXNumber = float.TryParse(X, out _);
        bool isYNumber = float.TryParse(y, out _);
        bool isNueNumber = float.TryParse(s1, out _);
        bool isDenoNumber = float.TryParse(s2, out _);
        float XNum = float.Parse(X);
        float YNum = float.Parse(y);
        float NueNum = float.Parse(s1);
        float DenoNum = float.Parse(s2);
        if (isXNumber && isYNumber && isNueNumber && isDenoNumber)
        {
            XMessage.active = false;
            YMessage.active = false;
            SlopeMessage.active = false;
            if (!CalledFromAnotherScript)
                Answer.text = "";
            return true;
        }
        else if (XNum > 100 || YNum > 100 || DenoNum > 100 || NueNum > 100)
        {
            if (XNum > 100)
            {
                XMessage.active = true;
            }
            if (YNum > 100)
            {
                YMessage.active = true;
            }
            if (NueNum > 100 || DenoNum > 100)
            {
                SlopeMessage.active = true;
            }

            return false;
        }
        else
        {
            if (!isXNumber)
            {
                XMessage.active = true;
            }
            else
                XMessage.active = false;
            if (!isYNumber)
            {
                YMessage.active = true;
            }
            else
                YMessage.active = false;
            if (!isNueNumber || !isDenoNumber)
            {
                SlopeMessage.active = true;
            }
            else
                SlopeMessage.active = false;
            if (!CalledFromAnotherScript)
                Answer.text = "";
            return false;
        }
    }
    public void SolveStepByStep()
    {
        if(!InExplain)
        {
            InExplain = true;
            ResetAns();
            if (deno == null || deno.Equals("") || nue == null || nue.Equals(""))
            {
                nue = Numerator.text;
                deno = Denominator.text;
            }
            audioSource = GetComponent<AudioSource>();
            LoadAllAudioClips();
            VoiceSpeaker.LoadAllAudioClips();
            if (ValidateInputs(XAxis.text, YAxis.text, nue, deno))
            {
                float XVal = float.Parse(XAxis.text), YVal = float.Parse(YAxis.text);
                int DenoVal = int.Parse(deno), NueVal = int.Parse(nue);
                float Result = (float)NueVal / DenoVal;
                float MX = XVal * Result;
                List<string> WritingSteps = new List<string>();
                if (DenoVal < 0)
                {
                    DenoVal = -DenoVal;
                    NueVal = -NueVal;
                }
                if (NueVal % DenoVal != 0)
                {
                    WritingSteps.Add("m = " + '%' + '$' + NueVal + '/' + DenoVal + '$' + '%' + '\n'); // m = slope
                    WritingSteps.Add("y = |mx |+ c" + '\n');
                    WritingSteps.Add("||y = " + '%' + NueVal + '/' + DenoVal + '%' + "x + c" + '\n'); // replace the value
                    WritingSteps.Add('$' + YAxis.text + '$' + " = " + '$' + XAxis.text + '$' + " X " + '%' + NueVal + '/' + DenoVal + '%' + " + c " + '\n'); //replace the value
                    FractionCalculator.MultiplyFractionByInteger(ref NueVal, ref DenoVal, (int)XVal);
                    WritingSteps.Add(YAxis.text + " = " + '%' + '$' + NueVal + '/' + DenoVal + '$' + '%' + " + c " + '\n'); // calc mx
                    WritingSteps.Add("|" + '%' + NueVal + '/' + DenoVal + '%' + " + c" + " = " + YAxis.text + " " + '\n');  // replace right and left
                    WritingSteps.Add("%" + '$' + NueVal + '/' + DenoVal + '$' + "%" + "    + c = " + YAxis.text + " " + '\n'); // animation
                    FractionCalculator.SubtractFractions((int)YVal, 1, ref NueVal, ref DenoVal);
                    WritingSteps.Add("c = " + '|' + '%' + '$' + NueVal + '/' + DenoVal + '$' + '%' + '\n');
                    WritingSteps.Add("y = " + '%' + '$' + int.Parse(nue) + '/' + int.Parse(deno) + '$' + '%' + "x" + '%' + '$' + NueVal + '/' + DenoVal + '$' + '%');
                }
                else
                {
                    WritingSteps.Add("m = " + '%' + '$' + NueVal + '$' + '%' + '\n'); // m = slope
                    WritingSteps.Add("y = |mx |+ c" + '\n');
                    WritingSteps.Add("||y = " + '%' + NueVal + '%' + "x + c" + '\n'); // replace the value
                    WritingSteps.Add('$' + YAxis.text + '$' + " = " + '$' + XAxis.text + '$' + " X " + '%' + NueVal + '%' + " + c " + '\n'); //replace the value
                    FractionCalculator.MultiplyFractionByInteger(ref NueVal, ref DenoVal, (int)XVal);
                    WritingSteps.Add(YAxis.text + " = " + '$' + NueVal / DenoVal + '$' + " + c " + '\n'); // calc mx
                    WritingSteps.Add("|" + NueVal / DenoVal + " + c" + " = " + YAxis.text + " " + '\n');  // replace right and left
                    WritingSteps.Add("$" + NueVal / DenoVal + "$    + c = " + YAxis.text + " " + '\n'); // animation
                    FractionCalculator.SubtractFractions((int)YVal, 1, ref NueVal, ref DenoVal);
                    WritingSteps.Add("c = $" + (YVal - MX) + '$' + '\n');
                    WritingSteps.Add("y = " + '%' + '$' + int.Parse(nue) + '/' + int.Parse(deno) + '$' + '%' + "x|+ " + "$" + (YVal - MX) + "$");
                }

                StartCoroutine(SpeakSteps(WritingSteps));
            }
        }

    }
    public IEnumerator SpeakSteps(List<string> writingSteps)
    {
        IsAnimatedPart = false;
        PauseBtn.gameObject.SetActive(true);
        for (int i = 0; i < writingSteps.Count; i++)
        {
            yield return StartCoroutine(ResumeCoroutine());
            yield return StartCoroutine(SpeakAndWait(writingSteps[i], i));
            if(i== writingSteps.Count - 1)
            {
                InExplain = false;

            }
        }
    }
    public IEnumerator SpeakAndWait(string written, int index)
    {
        String temp = "", TempCpy = "", Number = "", nue = "", deno = "";
        bool isEqualToZero = false, IsPositive = false;
        int CurrntAnswerLen = Answer.text.Length;
        int i = 0;
        while (i < written.Length)
        {
            if (!written[i].Equals('|') && !written[i].Equals('$') && i < written.Length - 1 && !written[i].Equals('%'))
            {
                temp += written[i];
                TempCpy += written[i];
                if (index == 5 && i == written.Length - 2)
                {
                    break;
                }
            }
            else if (written[i].Equals('%'))
            {
                i++;
                bool ISDash = false;
                bool WillBeSpeaked = false;
                if (written[i].Equals('$'))
                {
                    WillBeSpeaked = true;
                    i++;
                }
                if (WillBeSpeaked)
                {
                    IsAnimatedPart = false;
                    StartCoroutine(PlayVoiceClipAndWait(clipIndex));
                    clipIndex++;
                    while (!IsAnimatedPart)
                    {
                        yield return null;
                    }
                    Answer.text += temp;
                    temp = "";
                }
                while (!written[i].Equals('%'))
                {
                    if (!written[i].Equals('$'))
                    {
                        if (!ISDash && !written[i].Equals('/'))
                        {
                            nue += written[i];
                        }
                        else if (written[i].Equals('/'))
                        {
                            ISDash = true;
                        }
                        else
                        {
                            if (!written[i].Equals('$'))
                                deno += written[i];
                        }
                    }
                    i++;
                }
                i++;
                if (WillBeSpeaked)
                {
                    bool IsDenoNegative = false;
                    if (!deno.Equals(""))
                    {
                        if (deno[0].Equals('-'))
                        {
                            deno = deno.Remove(0, 1);
                            IsAnimatedPart2 = false;
                            StartCoroutine(VoiceSpeaker.PlayByAddress("AsemEng/minus"));
                            while (!IsAnimatedPart2)
                            {
                                yield return null;
                            }
                            IsDenoNegative = true;
                        }
                    }

                    IsAnimatedPart2 = false;
                    StartCoroutine(VoiceSpeaker.PlayVoiceNumberAndWait(nue));
                    while (!IsAnimatedPart2)
                    {
                        yield return null;
                    }
                    if (!deno.Equals(""))
                    {
                        IsAnimatedPart2 = false;
                        if (!deno.Equals(""))
                        {
                            if (int.Parse(nue) % int.Parse(deno) != 0)
                            {
                                StartCoroutine(VoiceSpeaker.PlayByAddress("AsemEng/Over"));
                                while (!IsAnimatedPart2)
                                {
                                    yield return null;
                                }
                                IsAnimatedPart2 = false;
                                StartCoroutine(VoiceSpeaker.PlayVoiceNumberAndWait(deno));
                                while (!IsAnimatedPart2)
                                {
                                    yield return null;
                                }
                                if (IsDenoNegative)
                                    deno = '-' + deno;
                            }
                        }
                    }

                }
                String AnswerTemp = Answer.text.Substring(0, CurrntAnswerLen);
                String WrittenSub = written.Substring(i);
                if (index != 8)
                {
                    if (!deno.Equals(""))
                    {
                        if(index!=3)
                        {
                            if (int.Parse(nue) % int.Parse(deno) != 0)
                                Answer.text = AnswerTemp + FractionCalculator.GetFrec(true, TempCpy, int.Parse(nue), int.Parse(deno), WrittenSub) + '\n';
                            else
                                Answer.text = AnswerTemp + TempCpy + nue + WrittenSub;

                        }
                        else
                        {
                            StringBuilder colored = new StringBuilder();
                            if (int.Parse(nue) % int.Parse(deno) != 0)
                            {
                                colored = new StringBuilder( FractionCalculator.GetFrec(true, TempCpy, int.Parse(nue), int.Parse(deno), WrittenSub) + '\n');
                                int NueIndex = colored.ToString().IndexOf('>');
                                for (int j = NueIndex+1; j <colored.Length ; j++)
                                {
                                    if (!colored[j].Equals(' '))
                                    {
                                        colored.Remove(j, nue.Length);
                                        colored.Insert(j, $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>{nue}</color>");
                                        break;
                                    }
                                }
                                int EqualIndex = colored.ToString().LastIndexOf('=') + 1;
                                colored.Remove(EqualIndex, XAxis.text.Length + 4);
                                colored.Insert(EqualIndex, $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>{XAxis.text + " X "}</color>");
                                int DenoIndex = colored.ToString().LastIndexOf("/line")-deno.Length-3;
                                colored.Remove(DenoIndex, deno.Length+2);
                                colored.Insert(DenoIndex, $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>{"  "+deno}</color>");

                            }
                            else
                            {
                                colored = new StringBuilder(TempCpy + nue + WrittenSub);
                            }
                            Answer.text =AnswerTemp +  colored;
                        }
                    }
                    else
                    {
                        if (index == 3)
                        {

                            string TempCpyColored = TempCpy.Substring(YAxis.text.Length+2);
                            TempCpy = TempCpy.Substring(0,YAxis.text.Length + 2);
                            TempCpyColored = $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>{TempCpyColored}</color>";
                            nue = $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>{nue}</color>";
                            Answer.text = AnswerTemp + TempCpy+TempCpyColored + nue + WrittenSub;

                        }
                        else
                            Answer.text = AnswerTemp + TempCpy + nue + WrittenSub;

                    }
                    i = written.Length;
                }
                else
                {
                    if (!written[i + 1].Equals('|'))
                    {
                        IsAnimatedPart = false;
                        StartCoroutine(PlayVoiceClipAndWait(clipIndex));
                        clipIndex++;
                        while (!IsAnimatedPart)
                        {
                            yield return null;
                        }
                        Answer.text = Answer.text.Substring(0, CurrntAnswerLen);
                        i += 2;
                        ISDash = false;
                        String NueTemp = "", DenoTemp = "";
                        while (!written[i].Equals('%'))
                        {
                            if (!written[i].Equals('$'))
                            {
                                if (!ISDash && !written[i].Equals('/'))
                                {
                                    NueTemp += written[i];
                                }
                                else if (written[i].Equals('/'))
                                {
                                    ISDash = true;
                                }
                                else
                                {
                                    if (!written[i].Equals('$'))
                                        DenoTemp += written[i];
                                }
                            }
                            i++;
                        }
                        if (int.Parse(NueTemp) / int.Parse(DenoTemp) >= 0)
                        {
                            Answer.text += FractionCalculator.GetFrec(true, TempCpy, int.Parse(nue), int.Parse(deno), "x + ");
                            IsAnimatedPart2 = false;
                            StartCoroutine(VoiceSpeaker.PlayByAddress("AsemEng/plus"));
                            while (!IsAnimatedPart2)
                            {
                                yield return null;
                            }
                        }
                        else
                        {
                            if (int.Parse(NueTemp) / int.Parse(DenoTemp) >= 0)
                            {
                                Answer.text += FractionCalculator.GetFrec(true, TempCpy, int.Parse(nue), int.Parse(deno), "x  ");
                            }
                        }
                        IsAnimatedPart2 = false;
                        StartCoroutine(VoiceSpeaker.PlayVoiceNumberAndWait(NueTemp));
                        while (!IsAnimatedPart2)
                        {
                            yield return null;
                        }
                        if (int.Parse(NueTemp) % int.Parse(DenoTemp) != 0)
                        {
                            IsAnimatedPart2 = false;
                            StartCoroutine(VoiceSpeaker.PlayByAddress("AsemEng/Over"));
                            while (!IsAnimatedPart2)
                            {
                                yield return null;
                            }

                            IsAnimatedPart2 = false;
                            StartCoroutine(VoiceSpeaker.PlayVoiceNumberAndWait(DenoTemp));
                            while (!IsAnimatedPart2)
                            {
                                yield return null;
                            }
                            Answer.text = Answer.text.Substring(0, CurrntAnswerLen);
                            Answer.text += FractionCalculator.GetFrec(true, TempCpy, int.Parse(nue), int.Parse(deno), "x + ", int.Parse(NueTemp), int.Parse(DenoTemp));
                        }
                        else
                        {
                            Answer.text = Answer.text.Substring(0, CurrntAnswerLen);
                            Answer.text += FractionCalculator.GetFrec(true, TempCpy, int.Parse(nue), int.Parse(deno), "x + " + NueTemp);
                        }
                    }
                    else
                    {
                        i += 5;
                        String num = "";
                        while (!written[i].Equals('$'))
                        {
                            num += written[i];
                            i++;
                        }
                        if (int.Parse(nue) % int.Parse(deno) != 0)
                        {
                            Debug.Log(nue + " سسس " + deno);

                            Answer.text = Answer.text.Substring(0, CurrntAnswerLen);
                            IsAnimatedPart = false;
                            StartCoroutine(PlayVoiceClipAndWait(clipIndex));
                            while (!IsAnimatedPart)
                            {
                                yield return null;
                            }
                            Answer.text = AnswerTemp + FractionCalculator.GetFrec(true, TempCpy, int.Parse(nue), int.Parse(deno), "x");


                        }
                        else
                        {
                            Answer.text = Answer.text.Substring(0, CurrntAnswerLen);
                            Answer.text += TempCpy + nue;
                        }
                        IsAnimatedPart = false;
                        StartCoroutine(PlayVoiceClipAndWait(clipIndex));
                        while (!IsAnimatedPart)
                        {
                            yield return null;
                        }
                        if (int.Parse(num) > 0)
                        {
                            IsAnimatedPart2 = false;
                            StartCoroutine(VoiceSpeaker.PlayByAddress("AsemEng/plus"));
                            while (!IsAnimatedPart2)
                            {
                                yield return null;
                            }
                            IsAnimatedPart2 = false;
                            StartCoroutine(VoiceSpeaker.PlayVoiceNumberAndWait(num));
                            while (!IsAnimatedPart2)
                            {
                                yield return null;
                            }
                            if (int.Parse(nue) % int.Parse(deno) != 0)
                            {
                                Answer.text = Answer.text.Substring(0, CurrntAnswerLen);
                                Answer.text = AnswerTemp + FractionCalculator.GetFrec(true, TempCpy, int.Parse(nue), int.Parse(deno), "x + " + num);
                            }
                            else
                            {
                                Answer.text += "x + " + num;
                            }
                        }
                        else
                        {
                            IsAnimatedPart2 = false;
                            StartCoroutine(VoiceSpeaker.PlayVoiceNumberAndWait(num));
                            while (!IsAnimatedPart2)
                            {
                                yield return null;
                            }
                            if (int.Parse(nue) % int.Parse(deno) != 0)
                            {
                                Answer.text = Answer.text.Substring(0, CurrntAnswerLen);
                                Answer.text = AnswerTemp + FractionCalculator.GetFrec(true, TempCpy, int.Parse(nue), int.Parse(deno), "x " + num);
                            }
                            else
                            {
                                Answer.text += "x " + num;
                            }
                        }
                    }
                }
            }
            else if (written[i].Equals('$'))
            {
                IsAnimatedPart = false;
                StartCoroutine(PlayVoiceClipAndWait(clipIndex));
                clipIndex++;
                while (!IsAnimatedPart)
                {
                    yield return null;
                }
                Answer.text += temp;
                temp = "";
                Number = "";
                int j = i + 1;
                for (; !written[j].Equals('$') && j < written.Length; j++)
                {
                    Number += written[j];
                }
                IsAnimatedPart2 = false;
                StartCoroutine(VoiceSpeaker.PlayVoiceNumberAndWait(Number));
                while (!IsAnimatedPart2)
                {
                    yield return null;
                }
                Answer.text += Number;
                TempCpy += Number;
                i = j;
            }
            else
            {
                if (i == written.Length - 1)
                {
                    if (!temp.Equals("") && written[written.Length - 1].Equals('\n') && index != 4)
                    {
                        IsAnimatedPart = false;
                        StartCoroutine(PlayVoiceClipAndWait(clipIndex));
                        clipIndex++;
                        while (!IsAnimatedPart)
                        {
                            yield return null;
                        }
                    }
                    temp += '\n';
                    Answer.text += temp;
                    break;
                }
                IsAnimatedPart = false;
                StartCoroutine(PlayVoiceClipAndWait(clipIndex));
                clipIndex++;
                while (!IsAnimatedPart)
                {
                    yield return null;
                }
                Answer.text += temp;
                temp = "";
                if (index == 4)
                    break;
            }
            i++;
        }
        if (index == 5 && deno.Equals(""))
        {
            Answer.text += temp;
            Answer.text += '\n';
        }
        if (index == 6)
        {
            if (!deno.Equals("") && !deno.Equals("1"))
            {
                Answer.text = Answer.text.Substring(0, CurrntAnswerLen);
                if (int.Parse(deno) < 0 || int.Parse(nue) < 0)
                {
                    Answer.text += FractionCalculator.GetFrec(true, "-  ", int.Parse(nue), int.Parse(deno), "  + c = " + YAxis.text, int.Parse(nue), int.Parse(deno), true);
                }
                else
                {
                    IsPositive = true;
                    Answer.text += FractionCalculator.GetFrec(true, "", int.Parse(nue), int.Parse(deno), "  + c = " + YAxis.text, int.Parse(nue), int.Parse(deno), true);
                }
                Answer.text += "\n";

            }
            else
            {
                isEqualToZero = true;
                String coloredText = "";
                if (Number.Equals(""))
                    Number = nue;
                if (int.Parse(Number) > 0)
                {
                    IsPositive = true;
                    coloredText = $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>{"-  " + Number}</color>";
                }
                else
                {
                    StringBuilder NumberTemp = new StringBuilder(Number);
                    NumberTemp[0] = '+';
                    Number = NumberTemp.ToString();
                    coloredText = $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>{" " + Number}</color>";
                }
                Answer.text = Answer.text.Substring(0, Answer.text.Length - 1);
                Answer.text += coloredText;
                Answer.text += "\n";
            }
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
                if (!isEqualToZero)
                {
                    worldPosition.y += 0.02f * Screen.height;
                    if (IsPositive)
                        worldPosition.x = 0.16f * Screen.width;
                    else
                        worldPosition.x = 0.2f * Screen.width;

                }

                Circle.transform.position = worldPosition;
                Circle.SetActive(true);

                // Adjust Arrow position using Circle's position
                float arrowSizeX = Circle.transform.position.x;
                float arrowSizeY =  Circle.transform.position.y;
                Vector3 ArrowSize = new Vector3(arrowSizeX, arrowSizeY, 0);
                Arrow.transform.position = new Vector3(ArrowSize.x, 1.07f * ArrowSize.y, worldPosition.z);

                Arrow.SetActive(true);
            }
            else
            {
                Debug.LogError("Failed to find the last word position. " +
                    "lastCharacterIndex: " + lastCharacterIndex +
                    ", textInfo: " + textInfo +
                    ", textInfo.characterInfo.Length: " + (textInfo != null ? textInfo.characterInfo.Length.ToString() : "null"));
                Circle.SetActive(false);
                Arrow.SetActive(false);
            }
        }
    }
    public int DetectTypeAndDeal()
    {
        if (type == 2)
        {
            return 0;
        }
        return 1;
    }

    void LoadAllAudioClips()
    {
        // Load all audio clips from the "AsemEng/q1" subdirectory
        voiceClips = Resources.LoadAll<AudioClip>("AsemEng/q1");
        Numbers = Resources.LoadAll<AudioClip>("AsemEng/Numbers");
    }
    public IEnumerator PlayVoiceClipAndWait(int index)
    {
        if (Numerator != null)
        {
            Numerator.GetComponent<Image>().color = Color.white;
            Denominator.GetComponent<Image>().color = Color.white;
        }

        YAxis.GetComponent<Image>().color = Color.white;
        XAxis.GetComponent<Image>().color = Color.white;
        audioSource.clip = voiceClips[index];
        if (audioSource.clip.name.Equals("4-AReplaceY"))
        {
            ResetColors(2);
            YAxis.GetComponent<Image>().color = Color.black;
        }
        else if (audioSource.clip.name.Equals("4-BXReplacement"))
        {
            ResetColors(1);
            XAxis.GetComponent<Image>().color = Color.black;

        }
        else if (audioSource.clip.name.Equals("1-FirstPutM") || audioSource.clip.name.Equals("3-BReplaceTheValueM"))
        {
            ResetColors(3);
            if(Numerator != null)
            {
                Numerator.GetComponent<Image>().color = Color.black;
                Denominator.GetComponent<Image>().color  = Color.black;
            }

        }
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        IsAnimatedPart = true;
    }
    public void ResetAns()
    {
        clipIndex = 0;
        Arrow.active = false;
        Circle.active = false;
        IsAnimatedPart = false;
        IsAnimatedPart2 = false;
    }
    public void ResetColors(int Num)
    {
        if(Num==1)
        {
            YAxis.GetComponent<Image>().color = Color.white;
            if (Numerator != null)
            {
                Numerator.GetComponent<Image>().color = Color.white;
                Denominator.GetComponent<Image>().color = Color.white;
            }

        }
        else if(Num==2)
        {
            XAxis.GetComponent<Image>().color = Color.white;
            if (Numerator != null)
            {
                Numerator.GetComponent<Image>().color = Color.white;
                Denominator.GetComponent<Image>().color = Color.white;
            }
        }
        else
        {
            YAxis.GetComponent<Image>().color = Color.white;
            XAxis.GetComponent<Image>().color = Color.white;
        }
    }
}