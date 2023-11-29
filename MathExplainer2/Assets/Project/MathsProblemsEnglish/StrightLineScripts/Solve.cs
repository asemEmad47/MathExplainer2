using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Solve : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Answer;
    [SerializeField] private GameObject XMessage;
    [SerializeField] private GameObject YMessage;
    [SerializeField] private GameObject SlopeMessage;
    [SerializeField] private TMP_InputField XAxis;
    [SerializeField] private TMP_InputField YAxis;
    [SerializeField] private TMP_InputField Slope;
    private AudioClip[] voiceClips;
    private AudioClip[] Numbers;
    private AudioSource audioSource;
    private List<string> ExtraSpokenSteps;
    private List<string> ExtraWrittenSteps;
    private string SlopeTxt="";
    public bool CalledFromAnotherScript = false;
    public int type =0;
    private int clipIndex = 0;
    private bool IsAnimatedPart = false;

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
                + YAxis.text + " = " + XVal + " X " + SlopeVal + " + c " + '\n'
                +YAxis.text + " = " + XVal*SlopeVal+" + "+" c "+'\n'
                + YAxis.text + "-"+ XVal * SlopeVal + " = "+ " c " + '\n'
                + "c = " + YAxis.text + " - " + XVal * SlopeVal + '\n'
                + "c = " + (YVal - (XVal * SlopeVal))+'\n'
                + "y = "+ SlopeTxt + " X " +((YVal - (XVal * SlopeVal)) >= 0?" + ":"") +(YVal - (XVal * SlopeVal));
        }
    }
    public bool ValidateInputs(string X, string y, string z)
    {
        bool isXNumber = float.TryParse(X, out _);
        bool isYNumber = float.TryParse(y, out _);
        bool isZNumber = float.TryParse(z, out _);

        float XNum = float.Parse(X);
        float YNum = float.Parse(y);
        float ZNum = float.Parse(z);
        if (isXNumber && isYNumber && isZNumber)
        {
            XMessage.active = false;
            YMessage.active = false;
            SlopeMessage.active = false;
            if (!CalledFromAnotherScript)
                Answer.text = "";
            return true;
        }
        else if(XNum >100 || YNum>100 || ZNum > 100)
        {
            if (XNum > 100)
            {
                XMessage.active = true;
            }        
            if (YNum > 100)
            {
                YMessage.active = true;
            }   
            if (ZNum > 100)
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


        audioSource = GetComponent<AudioSource>();
        LoadAllAudioClips();
        if (ValidateInputs(XAxis.text, YAxis.text, SlopeTxt))
        {
            float SlopeVal = float.Parse(SlopeTxt), XVal = float.Parse(XAxis.text), YVal = float.Parse(YAxis.text);
            SlopeVal = (float)Math.Round(SlopeVal, 2);
            if (DetectTypeAndDeal() == 0)
                YVal = 0;
            string SlopeFraction = SlopeVal.ToString();

            List<string> WritingSteps = new List<string> {
            "m = " +'$'+ SlopeFraction +'$'+ '\n',
            "y = |mx |+ c" + '\n',
            "|y = " + SlopeFraction + "x + c"  +"                ("+XVal+','+YVal+")"+ '\n',
            '$'+YAxis.text+'$' + " = " +'$'+ XAxis.text+'$' +" X "+SlopeVal + " + c " ,
            YAxis.text + " = " + '$'+(XVal * SlopeVal).ToString() +'$'+ " + c ",
            YAxis.text + " = c +" + '$'+(XVal * SlopeVal).ToString()+'$' +'\n', //animated part
            "|c = " + YAxis.text +" - " +(XVal * SlopeVal).ToString() + '\n',
            "|c = " + ((YVal - (XVal * SlopeVal)) < 0?" - ":"") +'$'+(YVal - (XVal * SlopeVal)).ToString()+'$' + '\n',
            "y = " +'$'+ SlopeFraction.ToString() + "$ X |" + ((YVal -(XVal * SlopeVal)) < 0 ? " - " : " + ") +'$'+ (YVal - (XVal * SlopeVal)).ToString() +'$',
            };
            //if (ExtraSpokenSteps != null)
            //{
                //VoiceSteps[0] = "now put m equals to slope which is " + SlopeVal;
                //VoiceSteps[1] = "then write the equation of the straight line which is y equals| mx| plus c";
                //for (int i = 0; i < ExtraSpokenSteps.Count; i++)
                //{
                    //VoiceSteps.Insert(i, ExtraSpokenSteps[i]);
                    //WritingSteps.Insert(i, ExtraWrittenSteps[i]);
                //}
            //}
            StartCoroutine(SpeakSteps(WritingSteps));
        }
    }
    public void SetExtraDetails(List<string> voiceSteps, List<string> writingSteps)
    {
        this.ExtraSpokenSteps = voiceSteps;
        this.ExtraWrittenSteps = writingSteps;
    }
    public IEnumerator SpeakSteps(List<string> writingSteps)
    {
        IsAnimatedPart = false;
        for (int i = 0; i < writingSteps.Count; i++)
        {
            yield return StartCoroutine(SpeakAndWait(writingSteps[i] , i));
        }
    }

    public IEnumerator SpeakAndWait(string written , int index)
    {
        if (SlopeTxt == null)
            SlopeTxt = Slope.text;
        String temp = "";
        int i = 0;
        while (i < written.Length)
        {
            if (!written[i].Equals('|') && !written[i].Equals('$')&&i<written.Length-1)
            {
                temp += written[i];
            }
            else if (written[i].Equals('$'))
            {
                IsAnimatedPart = false;
                StartCoroutine(PlayVoiceClipAndWait(clipIndex));//"y = |mx |+ c" + '\n',
                clipIndex++;
                while (!IsAnimatedPart)
                {
                    yield return null;
                }
                Answer.text += temp;
                temp = "";
                String Number = "";
                int j = i + 1;
                for (; !written[j].Equals('$') && j<written.Length; j++)
                {
                    Number += written[j];
                }
                IsAnimatedPart = false;
                StartCoroutine(PlayVoiceNumberAndWait(Number));
                while (!IsAnimatedPart)
                {
                    yield return null;
                }
                Answer.text += Number;
                i = j;
            }
            else
            {
                if(i== written.Length - 1) // problem is here
                {
                    if (!temp.Equals("") && written[written.Length-1].Equals('\n'))
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
            }
            i++;
        }
        if (index == 5)
        {
            PrepareForAnimation(written);
        }
    }
    public void PrepareForAnimation(String written)
    {
        int characterIndex = Answer.text.Length - 1;
        if (characterIndex >= 0 && characterIndex < Answer.text.Length)
        {
            StringBuilder AnimatedLine = new StringBuilder(written);
            for (int i = 0; i < AnimatedLine.Length; i++)
            {
                if (AnimatedLine[i].Equals('$'))
                {
                    AnimatedLine.Remove(i,1);
                }
            }
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

    void LoadAllAudioClips()
    {
        // Load all audio clips from the "AsemEng/q1" subdirectory
        voiceClips = Resources.LoadAll<AudioClip>("AsemEng/q1");
        Numbers = Resources.LoadAll<AudioClip>("AsemEng/Numbers");
    }
    IEnumerator PlayVoiceClipAndWait(int index)
    {
        audioSource.clip = voiceClips[index];
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        IsAnimatedPart = true;
    }
    IEnumerator PlayVoiceNumberAndWait(String text)
    {
        if (text.Length == 1)
        {
            switch (text)
            {
                case "0":
                    audioSource.clip = Numbers[0];
                    break;      
                case "1":
                    audioSource.clip = Numbers[1];
                    break;             
                case "2":
                    audioSource.clip = Numbers[2];
                    break;      
                case "3":
                    audioSource.clip = Numbers[3];
                    break;                
                case "4":
                    audioSource.clip = Numbers[4];
                    break;               
                case "5":
                    audioSource.clip = Numbers[5];
                    break;                
                case "6":
                    audioSource.clip = Numbers[6];
                    break;               
                case "7":
                    audioSource.clip = Numbers[7];
                    break;                
                case "8":
                    audioSource.clip = Numbers[8];
                    break;            
                case "9":
                    audioSource.clip = Numbers[9];
                    break;
                default:
                    audioSource.clip = Numbers[0];
                    break;
            }
        }
        else
        {
            switch (text)
            {
                case "10":
                    audioSource.clip = Numbers[10];
                    break;
                case "11":
                    audioSource.clip = Numbers[11];
                    break;
                case "12":
                    audioSource.clip = Numbers[12];
                    break;
                case "13":
                    audioSource.clip = Numbers[13];
                    break;
                case "14":
                    audioSource.clip = Numbers[14];
                    break;
                case "15":
                    audioSource.clip = Numbers[15];
                    break;
                case "16":
                    audioSource.clip = Numbers[16];
                    break;
                case "17":
                    audioSource.clip = Numbers[17];
                    break;
                case "18":
                    audioSource.clip = Numbers[18];
                    break;
                case "19":
                    audioSource.clip = Numbers[19];
                    break;
                case "20":
                    audioSource.clip = Numbers[20];
                    break;              
                case "30":
                    audioSource.clip = Numbers[21];
                    break;                
                case "40":
                    audioSource.clip = Numbers[22];
                    break;              
                case "50":
                    audioSource.clip = Numbers[23];
                    break;              
                case "60":
                    audioSource.clip = Numbers[24];
                    break;           
                case "70":
                    audioSource.clip = Numbers[25];
                    break;             
                case "80":
                    audioSource.clip = Numbers[26];
                    break;             
                case "90":
                    audioSource.clip = Numbers[27];
                    break;
                default:
                    String Temp = text[0].ToString();
                    Temp += "0";
                    IsAnimatedPart = false;
                    //action
                    Temp = text[1].ToString();
                    break;
            }
        }
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        IsAnimatedPart = true;
    }
}