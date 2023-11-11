using SpeechLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Solve : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Answer;
    [SerializeField] private GameObject XMessage;
    [SerializeField] private GameObject YMessage;
    [SerializeField] private GameObject SlopeMessage;
    [SerializeField] private TMP_InputField XAxis;
    [SerializeField] private TMP_InputField YAxis;
    [SerializeField] private TMP_InputField Slope;
    private List<string> ExtraSpokenSteps;
    private List<string> ExtraWrittenSteps;
    private string SlopeTxt="";
    public bool CalledFromAnotherScript = false;
    public int type =0;
    public void SetSolve(GameObject Xmsg, GameObject Ymsg, GameObject Slpmsg, TMP_InputField X, TMP_InputField y,string slope, TextMeshProUGUI Ans,bool Another,int ty)
    {
        this.XMessage = Xmsg;
        this.YMessage = Ymsg;
        this.SlopeMessage = Slpmsg;
        this.XAxis = X;
        this.YAxis = y;
        this.SlopeTxt = slope;
        this.Answer = Ans;
        this.CalledFromAnotherScript = Another;
        this.type = ty;
    }
    public void SolveQuestion()
    {
        if (SlopeTxt==null || SlopeTxt.Equals(""))
            SlopeTxt = Slope.text;
        if (ValidateInputs(XAxis.text, YAxis.text, SlopeTxt))
        {
            float SlopeVal = float.Parse(SlopeTxt),XVal = float.Parse(XAxis.text) , YVal = float.Parse(YAxis.text);
            SlopeVal = (float)Math.Round(SlopeVal, 2);
            Answer.text += "m = " + SlopeTxt + '\n'
                + "y = mx + c"+ '\n'
                + "y = " + SlopeTxt + "x + c" + "                (" + XVal + ',' + YVal + ")" + '\n'
                +YAxis.text + " = " + XVal*SlopeVal+" + "+" c "+'\n'
                +"c = " + YAxis.text + " - " + XVal * SlopeVal + '\n'
                + "c = " + (YVal - (XVal * SlopeVal))+'\n'
                + "y = "+ SlopeTxt + " X " +((YVal - (XVal * SlopeVal)) >= 0?" + ":"") +(YVal - (XVal * SlopeVal));
        }
    }
    public bool ValidateInputs(string X, string y, string z)
    {
        bool isXNumber = float.TryParse(X, out _);
        bool isYNumber = float.TryParse(y, out _);
        bool isZNumber = float.TryParse(z, out _);
        if (isXNumber && isYNumber && isZNumber)
        {
            XMessage.active = false;
            YMessage.active = false;
            SlopeMessage.active = false;
            if (!CalledFromAnotherScript)
                Answer.text = "";
            return true;
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
                YMessage.active=true;
            }
            else
                YMessage.active =false;
            if (!isZNumber)
            {
                SlopeMessage.active=true;
            }
            else
                SlopeMessage.active = false;
            if(!CalledFromAnotherScript) 
                Answer.text = "";
            return false;
        }
    }
    public void SolveStepByStep()
    {
        if (SlopeTxt == null || SlopeTxt.Equals(""))
            SlopeTxt = Slope.text;

        if (ValidateInputs(XAxis.text, YAxis.text, SlopeTxt))
        {
            float SlopeVal = float.Parse(SlopeTxt), XVal = float.Parse(XAxis.text), YVal = float.Parse(YAxis.text);
            SlopeVal = (float)Math.Round(SlopeVal, 2);
            if (DetectTypeAndDeal() == 0)
                YVal = 0;
            string SlopeFraction = SlopeVal.ToString();
            List<string> VoiceSteps = new List<string> {
            "first put m equals to slope which is " + SlopeVal,
            "second write the equation of the straight line which is y equals| mx| plus c",
            "then put the value of the slope in the equation which is " + SlopeVal,
            "then in the equation put the value of y which is " +  YVal + "| and the value of x which is " +  XVal+ " then put the constant " + "  C  ",
            "|move the value " + (XVal * SlopeVal) + " to the left side by a negative sign",
            "Subtract " + (XVal * SlopeVal) + " from " +  YVal + " so c equals |" + (YVal - (XVal * SlopeVal)),
            "so, the equation is " + "y equals |" + SlopeVal + " X |" +((YVal - (XVal * SlopeVal)) >= 0?"plus":"")  +'|'+ (YVal - (XVal * SlopeVal)),
};

            List<string> WritingSteps = new List<string> {
            "m = " + SlopeFraction + '\n',
            "y = |mx |+ c" + '\n',
            "y = " + SlopeFraction + "x + c"  +"                ("+XVal+','+YVal+")"+ '\n',
            YVal + "| = " + XVal * SlopeVal + " + c " + '\n',
            YVal + " = c +" + XVal * SlopeVal +'\n',
            "c = |" + (YVal - (XVal * SlopeVal)) + '\n',
            "y = |" + SlopeFraction + " X |" + ((YVal -(XVal * SlopeVal)) >= 0 ? " + " : "")  +'|'+ (YVal - (XVal * SlopeVal)),
            };
            if (ExtraSpokenSteps != null)
            {
                VoiceSteps[0] = "now put m equals to slope which is " + SlopeVal;
                VoiceSteps[1] = "then write the equation of the straight line which is y equals| mx| plus c";
                for (int i = 0; i < ExtraSpokenSteps.Count; i++)
                {
                    VoiceSteps.Insert(i, ExtraSpokenSteps[i]);
                    WritingSteps.Insert(i, ExtraWrittenSteps[i]);
                }
            }
            StartCoroutine(SpeakSteps(VoiceSteps, WritingSteps));
        }
    }
    public void SetExtraDetails(List<string> voiceSteps, List<string> writingSteps)
    {
        this.ExtraSpokenSteps = voiceSteps;
        this.ExtraWrittenSteps = writingSteps;
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
        voice.Rate = -3;
        if (SlopeTxt == null)
            SlopeTxt = Slope.text;
        if (spoken[0].Equals('|'))
        {
            voice.Speak(spoken, SpeechVoiceSpeakFlags.SVSFlagsAsync | SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak);
            // Wait for speech to finish
            while (voice.Status.RunningState != SpeechRunState.SRSEDone)
            {
                yield return null;
            }
            Answer.text += written;
            PrepareForAnimation(written);
        }
        else
        {
            String temp="";
            int WrittenCounter = 0;
            for (int i = 0; i < spoken.Length; i++)
            {
                if (!spoken[i].Equals('|') && i<spoken.Length-1)
                {
                    temp += spoken[i];
                }
                else
                {
                    if(i==spoken.Length-1) // handeling last latter case
                        temp += spoken[i];
                    voice.Speak(temp, SpeechVoiceSpeakFlags.SVSFlagsAsync | SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak);
                    // Wait for speech to finish
                    while (voice.Status.RunningState != SpeechRunState.SRSEDone)
                    {
                        yield return null;
                    }
                    while (!written[WrittenCounter].Equals('|'))
                    {
                        Answer.text += written[WrittenCounter++];
                        if(WrittenCounter == written.Length) // preventing falling in out of bounds
                            break;
                    }
                    WrittenCounter++;
                    temp = "";
                }
            }
        }
    }
    public void PrepareForAnimation(String written)
    {


            int characterIndex = Answer.text.Length - 1;
            if (characterIndex >= 0 && characterIndex < Answer.text.Length)
            {
                StringBuilder AnimatedLine = new StringBuilder(written);
                StringBuilder AnswerTemp = new StringBuilder(Answer.text);
                AnswerTemp.Remove(AnswerTemp.Length - 1, 1);
                for (int i = AnswerTemp.Length-1; i >0 && !AnswerTemp[i].Equals('\n'); i--)
                {
                    AnswerTemp.Remove(i, 1);
                }
                if (float.Parse(YAxis.text) > 0)
                {
                    AnimatedLine.Insert(0, '+');
                }
                AnswerTemp.Insert(AnswerTemp.Length,AnimatedLine);
                int index = AnswerTemp.Length;
                int counter = AnswerTemp.Length;
                for (; counter >0 &&!AnswerTemp[counter-2].Equals('+') && !AnswerTemp[counter-2].Equals('-'); counter--) // getting the last number to animate it
                    index--;

                if (AnswerTemp[counter - 2].Equals('-'))
                {
                    AnswerTemp.Remove(counter - 2, 1);
                    index--;
                    AnswerTemp.Remove(counter - 2, 1);// reomving built-in +
            }
                else
                {
                    AnswerTemp[counter - 2] = '-';
                    index--;
                }
            StartCoroutine(Animate(AnswerTemp.ToString(), index));
        }
    }
    public IEnumerator Animate(string adjustedText,int index)
    {
        float animationDuration = 0.7f; // Set the duration of the animation in seconds.
        float startTime = Time.time;
        float elapsedTime = 0;
        float SlopeVal = float.Parse(SlopeTxt), XVal = float.Parse(XAxis.text);
        float XSlope = XVal*SlopeVal;
        int modVal = (int)(MathF.Abs(XSlope) / (MathF.Pow(10,XSlope.ToString().Length-1)));
        int sourcePosition = 10;

        while (elapsedTime < animationDuration)
        {
            // Calculate the progress of the animation as a ratio of elapsed time to the animation duration
            float progress = elapsedTime / animationDuration;
            // Interpolate between the source and destination position tags
            Debug.Log(modVal);
             
            string currentPosTag = InterpolatePositionTags(sourcePosition, index-adjustedText.Length-2-modVal , progress);

            // Replace previous position tags with the currentPosTag
            string animatedText = adjustedText.Replace("<pos=", "<pos=0>");

            // Insert the current position tag at the second-to-last position in the text
            animatedText = animatedText.Insert(index-1, currentPosTag);

            // Update the text displayed on the screen with the animatedText
            Answer.text = animatedText;

            // Yield control for one frame, allowing Unity to update and render the text
            yield return null;

            // Update the elapsed time by subtracting the starting time from the current time
            elapsedTime = Time.time - startTime;
        }
    }
    public string InterpolatePositionTags(int sourcePosition, int destinationPosition, float progress)
    {
        int interpolatedPosition = (int)Mathf.Lerp(sourcePosition, destinationPosition, progress);

        return $"<pos={interpolatedPosition}%>";
    }
    public int DetectTypeAndDeal()
    {
        if (type == 2)
        {
            return 0;
        }
        return 1;
    }
}
