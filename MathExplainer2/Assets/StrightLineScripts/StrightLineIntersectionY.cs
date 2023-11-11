using SpeechLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class StrightLineIntersectionY : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Answer;
    [SerializeField] private GameObject YMessage;
    [SerializeField] private GameObject SlopeMessage;
    [SerializeField] private TMP_InputField YAxis;
    [SerializeField] private TMP_InputField Slope;
    public void SolveQuestion()
    {
        if (ValidateInputs(YAxis.text, Slope.text))
        {
            float SlopeVal = float.Parse(Slope.text), YVal = float.Parse(YAxis.text);
            SlopeVal = (float)Math.Round(SlopeVal, 2);
            Answer.text += "m = " + SlopeVal +'\n';
            Answer.text += "y =  mx + c" + '\n';
            Answer.text += "y-intercepts = " + YAxis.text + '\n';
            Answer.text += "c = " + YAxis.text + '\n';
            Answer.text += "y = " + SlopeVal + "x +" + YAxis.text + "    -> the equation"+ '\n';
            Answer.text += "the stight line intersects with Y-axis , then x = 0" + '\n';
            Answer.text += "Y = "+SlopeVal +YAxis.text+'\n';
            Answer.text += "then point = (0," + YAxis.text + ")" + '\n';
        }
    }
    public bool ValidateInputs(string y, string slope)
    {
        bool isYNumber = float.TryParse(y, out _);
        bool isSlope = float.TryParse(slope, out _);
        Answer.text = "";
        if (!isYNumber)
        {
            YMessage.active = true;
            return false;
        }
        else
            YMessage.active = false;
        if (!isSlope)
        {
            SlopeMessage.active = true;
            return false;   
        }
        else
            SlopeMessage.active = false;
        return true;
    }
    public void SolveStepByStep()
    {
        if (ValidateInputs(YAxis.text, Slope.text))
        {
            float SlopeVal = float.Parse(Slope.text), YVal = float.Parse(YAxis.text);
            SlopeVal = (float)Math.Round(SlopeVal, 2);
            string SlopeFraction = SlopeVal.ToString();
            List<string> VoiceSteps = new List<string> {
                "first put m equals to slope which is " + Slope.text,
                "second write the equation of the straight line which is y equals mx plus c",
                "since y intercept y axis at " + YVal.ToString(),
                "then c = "+ YVal.ToString(),
                "so the equation is y equals to" + SlopeVal.ToString()+"x + "+YVal.ToString(),
                "since the straight line intersects with Y-axis , then x equals to 0" ,
                "so the y equals to" + SlopeVal.ToString()+"by zero+ "+YVal.ToString(),
                "then point = (0," + YAxis.text + ")",
        };

            List<string> WritingSteps = new List<string> {
                "m = " + SlopeVal +'\n',
                "y =  mx + c" + '\n',
                "y-intercepts = " + YAxis.text + '\n',
                "c = " + YAxis.text + '\n',
                "y = " + SlopeVal + "x +" + YAxis.text + "    -> the equation"+ '\n',
                "the stight line intersects with Y-axis , then x = 0"+ '\n',
                "Y = " + SlopeVal + YAxis.text + '\n',
                "then point = (0," + YAxis.text + ")" + '\n',
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
        SpVoice voice = new SpVoice();
        voice.Rate = -6;
        voice.Speak(spoken, SpeechVoiceSpeakFlags.SVSFlagsAsync | SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak);
        // Wait for speech to finish
        while (voice.Status.RunningState != SpeechRunState.SRSEDone)
        {
            yield return null;
        }
        Answer.text += written;
    }
}
