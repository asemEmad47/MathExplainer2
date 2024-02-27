using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using UnityEngine.UI;

public class TwoPointsNegative : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Answer;
    [SerializeField] private GameObject Point1Message;
    [SerializeField] private GameObject Point2Message;
    [SerializeField] private TMP_InputField X1Axis;
    [SerializeField] private TMP_InputField X2Axis;
    [SerializeField] private TMP_InputField Y1Axis;
    [SerializeField] private TMP_InputField Y2Axis;
    [SerializeField] private Button PauseBtn;
    [SerializeField] private Button ResumeBtn;
    private bool Pause = false;
    private bool InExplain = false;
    public float screenTimeoutDuration = 300f;
    private string numeratorStr = "y2-y1";
    private string DenoStr = "x2-x1";
    private float slope;
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
        if (ValidateInputs(X1Axis.text, Y1Axis.text, X2Axis.text, Y2Axis.text))
        {
            slope = (float.Parse(Y2Axis.text) - float.Parse(Y1Axis.text)) / (float.Parse(X2Axis.text) - float.Parse(X1Axis.text));
            slope = (float)Math.Round(slope, 2);
            float angel = (Mathf.Rad2Deg * Mathf.Atan(slope));
            string Equation = $"<sup>{numeratorStr}</sup>/<sub>{DenoStr}</sub>";
            Answer.text = "m = " + Equation + '\n';
            Answer.text += "m = " + $"<sup>{Y2Axis.text + " - " + Y1Axis.text}</sup>/<sub>{X2Axis.text + " - " + X1Axis.text}</sub>" +" = "+slope + '\n';
            Answer.text += "slope = tan E" + '\n';
            Answer.text += "so, tan E = " +slope + '\n';
            Answer.text += "E = tan" + $"<sup>-1</sup>" +slope+'\n';
            Answer.text += "E = " + angel.ToString() + '°'  +'\n';
            Answer.text += "so the angel = 180  - " + angel.ToString()+" = "+(180-angel) + '\n';
        }
    }
    public void ExplainQuestion()
    {
        if (ValidateInputs(X1Axis.text, Y1Axis.text, X2Axis.text, Y2Axis.text))
        {
            string Equation = $"<sup>{numeratorStr}</sup>/<sub>{DenoStr}</sub>";
            slope = (float.Parse(Y2Axis.text) - float.Parse(Y1Axis.text)) / (float.Parse(X2Axis.text) - float.Parse(X1Axis.text));
            slope = (float)Math.Round(slope, 2);
            float angel = (Mathf.Rad2Deg * Mathf.Atan(slope));
            List<string> VoiceSteps = new List<string> {
                "first put the equation of the slope which is y2 minus y1 over x2 minus x1",
                    "now subtract " + float.Parse(Y2Axis.text) + " minus " + float.Parse(Y1Axis.text) + " then divide the result by " + float.Parse(X2Axis.text) + " minus " + float.Parse(X1Axis.text),
                "slope equals to Tan  "+ "E",
                "so slope equals to Tan"+" E equals to" + (slope),
                "so slope equals to Tan inverse of" + (slope) +" which equals to " + (angel)+ "degrees" ,
                "to get the negative direction subtract E from 180 ," + " = 180 minus "+angel+" ="+(180-angel) + "degrees" ,
            };
            List<string> WritingSteps = new List<string> {
                "m = " + Equation + '\n',
                "m = "+$"<sup>{float.Parse(Y2Axis.text) + " - " + float.Parse(Y1Axis.text)}</sup>/<sub>{float.Parse(X2Axis.text) + " - " + float.Parse(X1Axis.text)}</sub>"+" = "+slope + '\n',
                "slope = Tan E " + '\n',
                "then Tan E = " + slope + '\n',
                "then Tan E = " + (Mathf.Rad2Deg * Mathf.Atan(slope)).ToString() + '°' + '\n',
                "so the angel = 180  - " + angel.ToString()+" = "+(180-angel) + '\n',
            };
            StartCoroutine(SpeakSteps(VoiceSteps, WritingSteps));
        }
    }
    public IEnumerator SpeakSteps(List<string> voiceSteps, List<string> writingSteps)
    {
        for (int i = 0; i < voiceSteps.Count; i++)
        {
            yield return StartCoroutine(SpeakAndWait(voiceSteps[i], writingSteps[i]));
        }
    }

    public IEnumerator SpeakAndWait(string spoken, string written)
    {
        //SpVoice voice = new SpVoice();
        //voice.Rate = -6;
        //voice.Speak(spoken, SpeechVoiceSpeakFlags.SVSFlagsAsync | SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak);
        // Wait for speech to finish
        //while (voice.Status.RunningState != SpeechRunState.SRSEDone)
        //{
        //  yield return null;
        //}
        //Answer.text += written;
        yield return null;
    }
}
