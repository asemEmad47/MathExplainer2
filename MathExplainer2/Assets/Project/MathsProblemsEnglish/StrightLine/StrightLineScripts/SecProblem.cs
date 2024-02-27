using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TwoPointsScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Answer;
    [SerializeField] private TextMeshProUGUI X1;
    [SerializeField] private TextMeshProUGUI Y1;
    [SerializeField] private TextMeshProUGUI X2;
    [SerializeField] private TextMeshProUGUI Y2;
    [SerializeField] private GameObject Point1Message;
    [SerializeField] private GameObject Point2Message;
    [SerializeField] private TMP_InputField X1Axis;
    [SerializeField] private TMP_InputField X2Axis;
    [SerializeField] private TMP_InputField Y1Axis;
    [SerializeField] private TMP_InputField Y2Axis;
    [SerializeField] GameObject Circle;
    [SerializeField] GameObject Arrow;
    [SerializeField] private Button PauseBtn;
    [SerializeField] private Button ResumeBtn;
    [SerializeField] private Button SolveQ;
    [SerializeField] private Button Explain;
    private string numeratorStr = "Y<sub style='font-size: larger;'>2</sub> - Y<sub style='font-size: larger;'>1</sub>";
    private string denominatorStr = "X<sub style='font-size: larger;'>2</sub> - X<sub style='font-size: larger;'>1</sub>";
    private string nue = "";
    private string deno = "";
    public static bool IsAnimatedPart2 = false;
    private bool IsAnimatedPart = false;
    public int clipIndex = 0;
    private bool Pause = false;
    private bool InExplain = false;
    public static AudioClip[] voiceClips;
    public static AudioSource audioSource;
    public GameObject go;
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
        SolveQ.onClick.AddListener(solveQuestion);
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
    public bool ValidateInputs(string X1, string y1, string X2, string y2)
    {
        bool isX1Number = float.TryParse(X1, out _);
        bool isY1Number = float.TryParse(y1, out _);
        bool isX2Number = float.TryParse(X2, out _);
        bool isy2Number = float.TryParse(y2, out _);

        if (isX1Number && isX2Number && isY1Number && isy2Number)
        {
            Point1Message.active = false;
            Point2Message.active = false;
            Answer.text = "";
            return true;
        }
        else
        {
            if (!isX1Number && !isY1Number)
            {
                Point1Message.active = true;
            }
            else
                Point1Message.active = false;
            if (!isX2Number && !isy2Number)
            {
                Point2Message.active = true;
            }
            else
                Point2Message.active = false;
            Answer.text = "";
            return false;
        }
    }
    public void solveQuestion()
    {
        if (!InExplain)
        {
            CalcSlope();
            if (ValidateInputs(X1Axis.text, Y1Axis.text, X2Axis.text, Y2Axis.text))
            {
                int Nue = int.Parse(Y2Axis.text) - int.Parse(Y1Axis.text);
                int Deno = int.Parse(X2Axis.text) - int.Parse(X1Axis.text);
                Answer.text = "";
                Answer.text += $"<line-height=25%>{FractionCalculator.GetStringFrec("m = ", numeratorStr, denominatorStr)}</line-height>\n";
                Answer.text += $"<line-height=25%>{FractionCalculator.GetStringFrec("m = ", Y2Axis.text + "-" + Y1Axis.text, X2Axis.text + "-" + X1Axis.text)}</line-height>\n";
                FractionCalculator.SimplifyFraction(ref Nue, ref Deno);
                Solve solve = go.GetComponent<Solve>();
                solve.SetSolve(Point1Message, Point2Message, Point2Message, X1Axis, Y1Axis, Nue.ToString(), Deno.ToString(), Answer, true, 1, Circle, Arrow, PauseBtn, ResumeBtn);
                solve.SolveQuestion();
            }
        }

    }
    public void ExplainQuestion()
    {
        if (!InExplain)
        {
            InExplain = true;
            if (ValidateInputs(X1Axis.text, Y1Axis.text, X2Axis.text, Y2Axis.text))
            {
                PauseBtn.gameObject.SetActive(true);
                List<string> WritingSteps = new List<string> {
                "|m = " + $"#{numeratorStr}/{denominatorStr}#" + '\n',
                "|m = " + '#'+Y2Axis.text + '-' +Y1Axis.text + '/'+X2Axis.text + '-'+X2Axis.text +'#'+ '\n',
            };
                StartCoroutine(SpeakSteps(WritingSteps));

            }
        }
    }
    public IEnumerator SpeakSteps(List<string> writingSteps )
    {
        CalcSlope();

        for (int i = 0; i < writingSteps.Count; i++)
        {
            yield return StartCoroutine(ResumeCoroutine());

            yield return StartCoroutine(SpeakAndWait(writingSteps[i], i));
            if (i == writingSteps.Count - 1)
            {
                PauseBtn.gameObject.SetActive(false);
                ResumeBtn.gameObject.SetActive(false);
                InExplain = false;
                Solve solve = go.GetComponent<Solve>();
                solve.SetSolve(Point1Message, Point2Message, Point2Message, X1Axis, Y1Axis, nue, deno, Answer, true, 1, Circle, Arrow, PauseBtn, ResumeBtn);
                solve.SolveStepByStep();
            }
        }
    }
    public IEnumerator SpeakAndWait(string written, int index)
    {
        int i = 0;
        while (i < written.Length)
        {
            if (written[i].Equals('#'))
            {
                IsAnimatedPart = false;
                StartCoroutine(PlayVoiceClipAndWait(clipIndex));
                while (!IsAnimatedPart)
                    yield return null;

                i++;
                if (index == 0)
                {
                    clipIndex++;
                    Answer.text += "m = ";
                    IsAnimatedPart = false;
                    StartCoroutine(PlayVoiceClipAndWait(clipIndex));
                    while (!IsAnimatedPart)
                        yield return null;

                    clipIndex++;
                    Answer.text += "Y<sub style='font-size: larger;'>2</sub> ";
                    IsAnimatedPart2 = false;
                    StartCoroutine(VoiceSpeaker.PlayByAddress("AsemEng/minus"));
                    while (!IsAnimatedPart2)
                        yield return null;

                    Answer.text += "- ";
                    IsAnimatedPart = false;
                    StartCoroutine(PlayVoiceClipAndWait(clipIndex));
                    while (!IsAnimatedPart)
                        yield return null;

                    clipIndex++;
                    Answer.text += "Y<sub style='font-size: larger;'>1</sub>";
                    IsAnimatedPart2 = false;
                    StartCoroutine(VoiceSpeaker.PlayByAddress("AsemEng/Over"));
                    while (!IsAnimatedPart2)
                        yield return null;

                    Answer.text ="";
                    Answer.text += FractionCalculator.GetStringFrec("m = ", numeratorStr, "");
                    IsAnimatedPart = false;
                    StartCoroutine(PlayVoiceClipAndWait(clipIndex));
                    while (!IsAnimatedPart)
                        yield return null;

                    clipIndex++;
                    Answer.text = "";
                    Answer.text += FractionCalculator.GetStringFrec("m = ", numeratorStr, "X<sub style='font-size: larger;'>2</sub>");
                    StartCoroutine(VoiceSpeaker.PlayByAddress("AsemEng/minus"));
                    IsAnimatedPart2 = false;
                    while (!IsAnimatedPart2)
                        yield return null;

                    Answer.text = "";
                    Answer.text += FractionCalculator.GetStringFrec("m = ", numeratorStr, "X<sub style='font-size: larger;'>2</sub> - ");
                    IsAnimatedPart = false;
                    StartCoroutine(PlayVoiceClipAndWait(clipIndex));
                    while (!IsAnimatedPart)
                        yield return null;

                    clipIndex++;
                    Answer.text = "";
                    Answer.text += FractionCalculator.GetStringFrec("m = ", numeratorStr, "X<sub style='font-size: larger;'>2</sub> - X<sub>1</sub>");
                    i = written.Length;
                    X1.gameObject.SetActive(true);
                    X2.gameObject.SetActive(true);
                    Y1.gameObject.SetActive(true);
                    Y2.gameObject.SetActive(true);
                }
                else
                {
                    int CurrentLen = Answer.text.Length;
                    bool FirstSub=false,ScndSub=false;
                    while (!written[i].Equals('#'))
                    {
                        if (written[i].Equals('/'))
                        {
                            Answer.text += "m = ";
                            Y2Axis.GetComponent<Image>().color = Color.black;
                            IsAnimatedPart2 = false;
                            StartCoroutine(VoiceSpeaker.PlayVoiceNumberAndWait(Y2Axis.text));
                            while (!IsAnimatedPart2)
                                yield return null;
                            Y2Axis.GetComponent<Image>().color = Color.white;

                            Answer.text = Answer.text.Substring(0,CurrentLen);
                            Answer.text += "m = "+Y2Axis.text;
                            IsAnimatedPart2 = false;
                            StartCoroutine(VoiceSpeaker.PlayByAddress("AsemEng/minus"));
                            while (!IsAnimatedPart2)
                                yield return null;

                            Answer.text = Answer.text.Substring(0, CurrentLen);
                            Y1Axis.GetComponent<Image>().color= Color.black;
                            Answer.text += "m = " + Y2Axis.text + " - ";
                            IsAnimatedPart2 = false;
                            StartCoroutine(VoiceSpeaker.PlayVoiceNumberAndWait(Y1Axis.text));
                            while (!IsAnimatedPart2)
                                yield return null;
                            Y1Axis.GetComponent<Image>().color = Color.white;
                            Answer.text = Answer.text.Substring(0, CurrentLen);
                            Answer.text += "m = " + Y2Axis.text + " - " + Y1Axis.text;
                            IsAnimatedPart2 = false;
                            StartCoroutine(VoiceSpeaker.PlayByAddress("AsemEng/Over"));
                            while (!IsAnimatedPart2)
                                yield return null;

                            Answer.text = Answer.text.Substring(0, CurrentLen);
                            if(int.Parse(Y2Axis.text)>0)
                                Answer.text += FractionCalculator.GetStringFrec("m = ", "  "+Y2Axis.text + " - "+Y1Axis.text, "");
                            else
                                Answer.text += FractionCalculator.GetStringFrec("m = ",Y2Axis.text + " - " + Y1Axis.text, "");

                        }
                        else if (written[i].Equals('-'))
                        {

                            if (!FirstSub)
                                FirstSub = true;
                            else
                                ScndSub = true;
                        }
                        i++;
                    }
                    IsAnimatedPart2 = false;
                    X2Axis.GetComponent<Image>().color = Color.black;
                    StartCoroutine(VoiceSpeaker.PlayVoiceNumberAndWait(X2Axis.text));
                    while (!IsAnimatedPart2)
                        yield return null;
                    X2Axis.GetComponent<Image>().color = Color.white;

                    Answer.text = Answer.text.Substring(0, CurrentLen);
                    if (int.Parse(Y2Axis.text) > 0)
                        Answer.text += FractionCalculator.GetStringFrec("m = ", "  " + Y2Axis.text + " - " + Y1Axis.text, X2Axis.text);
                    else
                        Answer.text += FractionCalculator.GetStringFrec("m = ", Y2Axis.text + " - " + Y1Axis.text, X2Axis.text);
                    IsAnimatedPart2 = false;
                    StartCoroutine(VoiceSpeaker.PlayByAddress("AsemEng/minus"));
                    while (!IsAnimatedPart2)
                        yield return null;

                    Answer.text = Answer.text.Substring(0, CurrentLen);
                    X1Axis.GetComponent<Image>().color = Color.black;
                    if (int.Parse(Y2Axis.text) > 0)
                        Answer.text += FractionCalculator.GetStringFrec("m = ", "  " + Y2Axis.text + " - " + Y1Axis.text, X2Axis.text + " - ");
                    else
                        Answer.text += FractionCalculator.GetStringFrec("m = ", Y2Axis.text + " - " + Y1Axis.text, X2Axis.text + " - ");
                    IsAnimatedPart2 = false;
                    StartCoroutine(VoiceSpeaker.PlayVoiceNumberAndWait(X1Axis.text));
                    while (!IsAnimatedPart2)
                        yield return null;
                    X1Axis.GetComponent<Image>().color = Color.white;

                    Answer.text = Answer.text.Substring(0, CurrentLen);
                    if (int.Parse(Y2Axis.text) > 0)
                        Answer.text += FractionCalculator.GetStringFrec("m = ", "  " + Y2Axis.text + " - " + Y1Axis.text, X2Axis.text + " - " + X1Axis.text);
                    else
                        Answer.text += FractionCalculator.GetStringFrec("m = ", Y2Axis.text + " - " + Y1Axis.text, X2Axis.text + " - " + X1Axis.text);
                    i =written.Length;
                }
            }
            i++;
        }
        Answer.text += '\n';
    }
    public IEnumerator PlayVoiceClipAndWait(int index)
    {
        audioSource.clip = voiceClips[index];
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        IsAnimatedPart = true;
    }
    public void CalcSlope()
    {
        audioSource = GetComponent<AudioSource>();
        voiceClips = Resources.LoadAll<AudioClip>("AsemEng/q2");
        int nueInt = int.Parse(Y2Axis.text) - int.Parse(Y1Axis.text);
        int DenoInt = int.Parse(X2Axis.text) - int.Parse(X1Axis.text);
        FractionCalculator.SimplifyFraction(ref nueInt, ref DenoInt);
        nue = nueInt.ToString();
        deno = DenoInt.ToString();
        Circle.active = false;
        Arrow.active=false;
        Answer.text = "";
        clipIndex=0;
    }
}
