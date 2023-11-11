using SpeechLib;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class SlopeFromEquationXAxis : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Answer;
    [SerializeField] private GameObject AMessage;
    [SerializeField] private GameObject BMessage;
    [SerializeField] private GameObject CMessage;
    [SerializeField] private TMP_InputField APoint;
    [SerializeField] private TMP_InputField BPoint;
    [SerializeField] private TMP_InputField CPoint;
    public bool ValidateInputs( string a, string b, string c)
    {
        bool isANumber = float.TryParse(a, out _);
        bool isBNumber = float.TryParse(b, out _);
        bool isCNumber = float.TryParse(c, out _);

        if (isANumber && isBNumber && isCNumber)
        {
            AMessage.active = false;
            BMessage.active = false;
            CMessage.active = false;
            Answer.text = "";
            return true;
        }
        else
        {

            if (!isANumber)
            {
                AMessage.active = true;
            }
            else
                AMessage.active = false;
            if (!isBNumber)
            {
                BMessage.active = true;
            }
            else
                BMessage.active = false;
            if (!isCNumber)
            {
                CMessage.active = true;
            }
            else
                CMessage.active = false;

            Answer.text = "";
            return false;
        }
    }
    public void solveQuestion()
    {
        if (ValidateInputs(APoint.text, BPoint.text, CPoint.text))
        {
            string Equation = $"<sup>-c</sup>/<sub>b</sub>";
            string SlopeEq = $"<sup>-c</sup>/<sub>b</sub>";
            float Y_int = (-float.Parse(CPoint.text)) / (float.Parse(BPoint.text));
            float slope = (-float.Parse(APoint.text)) / (float.Parse(BPoint.text));
            slope = (float)Math.Round(slope, 2);
            Answer.text += "y intercept = " + Equation+" = " + $"<sup>{"-" + CPoint.text}</sup>/<sub>{BPoint.text}</sub>" +"  = "+ Y_int + '\n';
            Answer.text += "slope = " + SlopeEq+" = " + $"<sup>{"-" + APoint.text}</sup>/<sub>{BPoint.text}</sub>" +"  = "+ slope + '\n';
            Answer.text += "slope = tan E = " + slope+ '\n';
            Answer.text += "E = " + (Mathf.Rad2Deg * Mathf.Atan(slope)).ToString() + '°' + '\n';

        }
    }
    public void ExplainQuestion()
    {
        if ((ValidateInputs( APoint.text, BPoint.text, CPoint.text)))
        {
            string Equation = $"<sup>-c</sup>/<sub>b</sub>";
            string SlopeEq = $"<sup>-c</sup>/<sub>b</sub>";
            float Y_int = (-float.Parse(CPoint.text)) / (float.Parse(BPoint.text));
            float slope = (-float.Parse(APoint.text)) / (float.Parse(BPoint.text));
            slope = (float)Math.Round(slope, 2);
            List<string> VoiceSteps = new List<string> {
                "first get y intersection which equals to negative c over b which equals to negative"+CPoint.text+"over"+BPoint.text+"which equals to"+Y_int,
                "second get the slope which equals to negative"+" a"+"over b which equals to negative"+APoint.text+"over"+BPoint.text+"which equals to"+Y_int,
                "Since the slope equals to tan e equals to"+slope,
                "so e equals to tan inverse of the slope which equals to "+(Mathf.Rad2Deg * Mathf.Atan(slope)).ToString() + "degrees",
            };
            List<string> WritingSteps = new List<string> {
            "y intercept = " + Equation+" = " + $"<sup>{"-" + CPoint.text}</sup>/<sub>{BPoint.text}</sub>" +" = "+ Y_int + '\n',
            "slope = " + SlopeEq + " = " + $"<sup>{"-" + APoint.text}</sup>/<sub>{BPoint.text}</sub>" + " = " + slope + '\n',
            "slope = tan E = " + slope + '\n',
            "E = " + (Mathf.Rad2Deg * Mathf.Atan(slope)).ToString() + '°' + '\n',
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
