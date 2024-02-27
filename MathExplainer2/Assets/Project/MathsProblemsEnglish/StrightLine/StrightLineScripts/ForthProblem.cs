using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Parallel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Answer;
    [SerializeField] private TextMeshProUGUI A;
    [SerializeField] private TextMeshProUGUI B;
    [SerializeField] private TextMeshProUGUI C;
    [SerializeField] private GameObject Point1Message;
    [SerializeField] private GameObject Point2Message;
    [SerializeField] private TMP_InputField X1Axis;
    [SerializeField] private TMP_InputField Y1Axis;
    [SerializeField] private TMP_InputField X;
    [SerializeField] private TMP_InputField Y;
    [SerializeField] private TMP_InputField Z;
    [SerializeField] GameObject Circle;
    [SerializeField] GameObject Arrow;
    [SerializeField] private Button PauseBtn;
    [SerializeField] private Button ResumeBtn;
    [SerializeField] private Button SolveQ;
    [SerializeField] private Button Explain;
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
    public bool ValidateInputs(string X1, string y1, string X, string y , string z)
    {
        bool isX1Number = float.TryParse(X1, out _);
        bool isY1Number = float.TryParse(y1, out _);
        bool isXNumber = float.TryParse(X, out _);
        bool isYNumber = float.TryParse(y, out _);
        bool isZNumber = float.TryParse(z, out _);


        if (isX1Number && isY1Number)
        {
            Point1Message.active = false;
            Point2Message.active = false;
            Answer.text = "";
            return true;
        }
        else
        {
            if (!isX1Number || !isY1Number)
            {
                Point1Message.active = true;
            }
            else
                Point1Message.active = false;
            if (!isXNumber || !isYNumber||!isZNumber)
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
        if(!InExplain)
        {
            CalcSlope();
            if (ValidateInputs(X1Axis.text, Y1Axis.text, X.text, Y.text, Z.text))
            {
                int Nue = -int.Parse(Y1Axis.text);
                int Deno = int.Parse(X1Axis.text);
                Answer.text = "";
                Answer.text += $"<line-height=25%>{FractionCalculator.GetStringFrec("m = ", "   -a", "     b")}</line-height>\n";
                Answer.text += $"<line-height=25%>{FractionCalculator.GetStringFrec("m = ", "  -" + X1Axis.text, "  " + Y1Axis.text)}</line-height>\n";
                Answer.text += "since L1 // L2 , Then m1 = m<sub style='font-size: larger;'>2</sub> = ";
                FractionCalculator.SimplifyFraction(ref Nue, ref Deno);
                Solve solve = go.GetComponent<Solve>();
                solve.SetSolve(Point1Message, Point2Message, Point2Message, X1Axis, Y1Axis, nue, deno, Answer, true, 1, Circle, Arrow, PauseBtn, ResumeBtn);
                solve.SolveQuestion();
            }
        }

    }
    public void ExplainQuestion()
    {
        if (!InExplain)
        {
            InExplain = true;
            if (ValidateInputs(X1Axis.text, Y1Axis.text, X.text, Y.text, Z.text))
            {
                List<string> WritingSteps = new List<string> {
                "#m = " + "#-a/b##" + '\n',
                "#m = " + '#'+X1Axis.text +'/'+X1Axis.text+'#'+ '\n',
                "#since L1 // L2 , Then m1 = m<sub style='font-size: larger;'>2</sub>#",
            };
                PauseBtn.gameObject.SetActive(true);
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
                clipIndex++;
                i++;
                if (index == 0)
                {
                    Answer.text += "m = -a";
                    IsAnimatedPart2 = false;
                    StartCoroutine(VoiceSpeaker.PlayByAddress("AsemEng/Over"));
                    while (!IsAnimatedPart2)
                        yield return null;

                    IsAnimatedPart = false;
                    StartCoroutine(PlayVoiceClipAndWait(clipIndex));
                    while (!IsAnimatedPart)
                        yield return null;
                    clipIndex++;

                    Answer.text = FractionCalculator.GetStringFrec("m = ", "  -a  ", "   b  ");
                    i = written.Length;
                    A.gameObject.SetActive(true);
                    B.gameObject.SetActive(true);
                    C.gameObject.SetActive(true);
                }
                else if (index == 1)
                {
                    String temp = Answer.text;
                    IsAnimatedPart2 = false;
                    StartCoroutine(VoiceSpeaker.PlayByAddress("AsemEng/minus"));
                    while (!IsAnimatedPart2)
                        yield return null;

                    IsAnimatedPart2 = false;
                    X.GetComponent<Image>().color = Color.black;

                    StartCoroutine(VoiceSpeaker.PlayVoiceNumberAndWait(X.text));
                    while (!IsAnimatedPart2)
                        yield return null;
                    Answer.text += "m = -" + X1Axis.text;
                    X.GetComponent<Image>().color = Color.white;

                    IsAnimatedPart2 = false;
                    StartCoroutine(VoiceSpeaker.PlayByAddress("AsemEng/Over"));
                    while (!IsAnimatedPart2)
                        yield return null;

                    Y.GetComponent<Image>().color = Color.black;
                    IsAnimatedPart2 = false;
                    StartCoroutine(VoiceSpeaker.PlayVoiceNumberAndWait(Y.text));
                    while (!IsAnimatedPart2)
                        yield return null;
                    Answer.text = temp;
                    Answer.text += FractionCalculator.GetStringFrec("m = ", "-" + X.text, Y.text);
                    Y.GetComponent<Image>().color = Color.white;

                    i = written.Length;
                }
                else
                {
                    IsAnimatedPart = false;
                    StartCoroutine(PlayVoiceClipAndWait(clipIndex));
                    while (!IsAnimatedPart)
                        yield return null;
                    Answer.text +="Since L1 // L2, then m = m<sub style='font-size: larger;'>2</sub> ";
                    i = written.Length;
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
        voiceClips = Resources.LoadAll<AudioClip>("AsemEng/Q4");
        int nueInt = - int.Parse(X1Axis.text);
        int DenoInt = int.Parse(Y1Axis.text);
        FractionCalculator.SimplifyFraction(ref nueInt, ref DenoInt);
        if (DenoInt < 0)
        {
            nue = (-nueInt).ToString();
            deno = (-DenoInt).ToString();
        }
        else
        {
            nue = (nueInt).ToString();
            deno = (DenoInt).ToString();
        }
        Circle.active = false;
        Arrow.active = false;
        Answer.text = "";
        clipIndex = 0;
    }
}
