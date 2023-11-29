using SpeechLib;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class SlopeAnd : MonoBehaviour
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
            Answer.text += "y = mx + c" + '\n';
            Answer.text += "y = "+ SlopeVal+"x + "+YVal + '\n';
            Answer.text += "slope = "+ SlopeVal+ ",    c = y-intercepts = " + YVal + '\n';
            Answer.text += "slope = Tan E "+ '\n';
            Answer.text += "then Tan E = "+SlopeVal+ '\n';
            Answer.text += "then Tan E = " + (Mathf.Rad2Deg * Mathf.Atan(SlopeVal)).ToString() + '°' + '\n';
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
            string SlopeFraction = SlopeVal.ToString();
            List<string> VoiceSteps = new List<string> {
                "first write the equation of the straight line which is y equals mx plus c",
                "then put the straight line equation which u enterd which is y equals to "+ (SlopeVal) +" x plus"+ (YVal),
                "by comparing coefficients slope = "+ (SlopeVal) + "  and c = y intercepts = " + (YVal) ,
                "slope equals to Tan  "+ "E",
                "so slope equals to Tan"+" E equals to" + (SlopeVal),
                "so slope equals to Tan inverse of" + (SlopeVal) +" which equals to " + (Mathf.Rad2Deg * Mathf.Atan(SlopeVal)).ToString() + "degrees" ,
            };

            List<string> WritingSteps = new List<string> {
                "y = mx + c" + '\n',
                "y = "+ SlopeVal+"x + "+YVal + '\n',
                "slope = " + SlopeVal + ",    c = y-intercepts = " + YVal + '\n',
                "slope = Tan E " + '\n',
                "then Tan E = " + SlopeVal + '\n',
                "then Tan E = " + (Mathf.Rad2Deg * Mathf.Atan(SlopeVal)).ToString() + '°' + '\n',
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
