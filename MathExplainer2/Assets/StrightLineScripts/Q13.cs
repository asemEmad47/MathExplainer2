using SpeechLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class Q13 : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Answer;
    [SerializeField] private GameObject AMessage;
    [SerializeField] private GameObject BMessage;
    [SerializeField] private GameObject CMessage;
    [SerializeField] private TMP_InputField X1Point;
    [SerializeField] private TMP_InputField Y1Point;
    [SerializeField] private TMP_InputField X2Point;
    [SerializeField] private TMP_InputField Y2Point;
    [SerializeField] private TMP_InputField DPoint;
    public bool ValidateInputs(string X1, string y1, string X2, string Y2, string D)
    {
        bool isX1Number = float.TryParse(X1, out _);
        bool isY1Number = float.TryParse(y1, out _);
        bool isX2Number = float.TryParse(X2, out _);
        bool isY2Number = float.TryParse(Y2, out _);
        bool isDNumber = float.TryParse(D, out _);

        if (isX1Number && isY1Number && isX2Number && isY2Number && isDNumber)
        {
            AMessage.active = false;
            BMessage.active = false;
            CMessage.active = false;
            Answer.text = "";
            return true;
        }
        else
        {
            if (!isX1Number || !isY1Number)
            {
                AMessage.active = true;
            }
            else
                AMessage.active = false;

            if (!isX2Number || !isY2Number)
            {
                BMessage.active = true;
            }
            else
                BMessage.active = false;
            if (!isDNumber)
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
        float X1Val = float.Parse(X1Point.text), Y1Val = float.Parse(Y1Point.text), X2Val = float.Parse(X2Point.text),Y2Val = float.Parse(Y2Point.text),result = float.Parse(DPoint.text);
        if (ValidateInputs(X1Point.text, Y1Point.text, X2Point.text, Y2Point.text, DPoint.text))
        {

            Answer.text += $"<sup>{"("+X1Point.text+"x"+Y2Point.text +")"+ "X + ("+X2Point.text+"x" + Y1Point.text + ")" +"Y"}</sup>/<sub>{Y1Point.text+"x"+Y2Point.text}</sub>"+" = " +result+ '\n'; 
            Answer.text += $"<sup>{(Y2Val*X1Val)+ "X + " + (X2Val*Y1Val) +"Y"}</sup>/<sub>{(Y1Val*Y2Val)}</sub>" + " = " + result + '\n';
            Answer.text += (Y2Val * X1Val) + "x"+" + "+ (X2Val * Y1Val)+ "Y = "+ (Y1Val * Y2Val*result)+'\n';
            Answer.text += (Y2Val * X1Val) + "x + "+ (X2Val * Y1Val) + "Y -"+ (Y1Val * Y2Val*result) +" = 0" +'\n'; 
            Answer.text += "slope = "+ $"<sup>-a</sup>/<sub>b</sub>" + " = "+ $"<sup>{-(Y2Val * X1Val)}</sup>/<sub>{(X2Val * Y1Val)}</sub>"+" = "+((-(Y2Val * X1Val))/ (X2Val * Y1Val)) + '\n';       
            Answer.text += "Y-int = "+ $"<sup>-c</sup>/<sub>b</sub>" + " = "+ $"<sup>{-(-Y1Val * Y2Val * result)}</sup>/<sub>{(X2Val * Y1Val)}</sub>"+" = "+((-(-Y1Val * Y2Val * result)) / (X2Val * Y1Val)) + '\n'; 

        }
    }
    public void SolveStepByStep()
    {
        float X1Val = float.Parse(X1Point.text), Y1Val = float.Parse(Y1Point.text), X2Val = float.Parse(X2Point.text), Y2Val = float.Parse(Y2Point.text), result = float.Parse(DPoint.text);
        if (ValidateInputs(X1Point.text, Y1Point.text, X2Point.text, Y2Point.text, DPoint.text))
        {
            List<string> VoiceSteps = new List<string> {
                "first the denominator will be the multiplying the two denominators then numerator will be the multiplication of x factor by y denominator plus y factor times x denominator",
                "then the result is"+Y2Val*X1Val+" X "+" plus " +X2Val*Y1Val+"  Y  " + "over"+Y1Val*Y2Val+"equals to"+ result,
                "by multiplying the result by the denominator then the result will be" + Y2Val*X1Val+" X " +"plus"+ (X2Val*Y1Val)+"   Y   " +"equals to "+(Y1Val * Y2Val*result),
                "move the result to the other side by a negative sign then the result will be" +(-Y1Val * Y2Val * result >= 0 ? "plus" : "Negative")+((-1)*Y1Val * Y2Val*result)+" plus "+ Y2Val*X1Val+" X " +"plus"+ (X2Val*Y1Val)+"   Y   " +"equals to zero",
                "slope equals to negtive a over b equals to" + ( -Y1Val * Y2Val * result  >= 0?"plus":"Negative")+((-1)*Y2Val * X1Val) + " over " + X2Val * Y1Val+ " which equals to " + Y1Val * Y2Val * result / X2Val * Y1Val,
                "   Y   "+ "intercyption = negative c over b which equals to negative" +( -Y1Val * Y2Val * result  >= 0?"plus":"Negative")+ ((-1)* Y1Val * Y2Val * result) +"over" + X2Val * Y1Val+ " which equals to"+Y1Val * Y2Val * result / X2Val * Y1Val,
            };

            List<string> WritingSteps = new List<string> {
                $"<sup>{"("+X1Point.text+"x"+Y2Point.text +")"+ "X + ("+X2Point.text+"x" + Y1Point.text + ")" +"Y"}</sup>/<sub>{Y1Point.text+"x"+Y2Point.text}</sub>"+" = " +result+ '\n',
                $"<sup>{(Y2Val*X1Val)+ "X + " + (X2Val*Y1Val) +"Y"}</sup>/<sub>{(Y1Val*Y2Val)}</sub>" + " = " + result + '\n',
                (Y2Val * X1Val) + "x" + " + " + (X2Val * Y1Val) + "Y = " + (Y1Val * Y2Val * result) + '\n',
                (Y2Val * X1Val) + "x + " + (X2Val * Y1Val) + "Y" +" = 0+"+(Y1Val * Y2Val * result),
                "slope = " + $"<sup>-a</sup>/<sub>b</sub>" + " = " + $"<sup>{-(Y2Val * X1Val)}</sup>/<sub>{(X2Val * Y1Val)}</sub>" + " = " + ((-(Y2Val * X1Val)) / (X2Val * Y1Val)) + '\n',
                "Y-int = " + $"<sup>-c</sup>/<sub>b</sub>" + " = " + $"<sup>{-(-Y1Val * Y2Val * result)}</sup>/<sub>{(X2Val * Y1Val)}</sub>" + " = " + ((-(-Y1Val * Y2Val * result)) / (X2Val * Y1Val)) + '\n',

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
        float X1Val = float.Parse(X1Point.text), Y1Val = float.Parse(Y1Point.text), X2Val = float.Parse(X2Point.text), Y2Val = float.Parse(Y2Point.text), result = float.Parse(DPoint.text);
        SpVoice voice = new SpVoice();
        voice.Rate = -6;
        voice.Speak(spoken, SpeechVoiceSpeakFlags.SVSFlagsAsync | SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak);
        // Wait for speech to finish
        while (voice.Status.RunningState != SpeechRunState.SRSEDone)
        {
            yield return null;
        }
        int IndexBeforeAdding = Answer.text.Length;
        Answer.text += written;
        if (spoken.Contains("move"))
        {
            int characterIndex = Answer.text.Length - 1;

            if (characterIndex >= 0 && characterIndex < Answer.text.Length)
            {
                // Modify the character by embedding rich text tags
                string adjustedText = "";
                adjustedText += Answer.text;
                StringBuilder temp = new StringBuilder(adjustedText);
                if ((Y2Val * X1Val) > 0)
                {
                    temp.Insert(IndexBeforeAdding, '+');
                }
                int index = temp.Length;
                for (int i = temp.Length; char.IsNumber(temp[i - 1]) || temp[i - 1].Equals('.'); i--)
                {
                    index--;
                }
                if ((Y1Val * Y2Val * result) > 0)
                {
                    temp.Insert(index, '-');
                }
                else
                    temp.Insert(index, '+');
                temp.Remove(index - 1, 1);
                if ((Y1Val * Y2Val * result) < 0)
                {
                    temp.Remove(index - 2, 2);
                    index--;
                }
                adjustedText = temp.ToString();
                StartCoroutine(Animate(adjustedText, index));

            }
        }
    }
    public IEnumerator Animate(string adjustedText, int index)
    {
        float animationDuration = 0.5f; // Set the duration of the animation in seconds.
        float startTime = Time.time;
        float elapsedTime = 0;

        // Define the destination position tag

        // Calculate the source position based on the current text length
        int sourcePosition = 10;

        while (elapsedTime < animationDuration)
        {
            // Calculate the progress of the animation as a ratio of elapsed time to the animation duration
            float progress = elapsedTime / animationDuration;

            // Interpolate between the source and destination position tags
            string currentPosTag = InterpolatePositionTags(sourcePosition, -4 - (adjustedText.Length - index), progress);

            // Replace previous position tags with the currentPosTag
            string animatedText = adjustedText.Replace("<pos=", "<pos=0>");

            // Insert the current position tag at the second-to-last position in the text
            animatedText = animatedText.Insert(index - 1, currentPosTag);

            // Update the text displayed on the screen with the animatedText
            Answer.text = animatedText;

            // Yield control for one frame, allowing Unity to update and render the text
            yield return null;

            // Update the elapsed time by subtracting the starting time from the current time
            elapsedTime = Time.time - startTime;
        }

        // Finish the animation by setting the text to the destination position

        Answer.text += '\n';
    }
    public string InterpolatePositionTags(int sourcePosition, int destinationPosition, float progress)
    {
        int interpolatedPosition = (int)Mathf.Lerp(sourcePosition, destinationPosition, progress);

        return $"<pos={interpolatedPosition}%>";
    }
}
