using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SubtractionArabic : MonoBehaviour, TwoEqsParent
{
    [SerializeField] GameObject Circle;
    [SerializeField] GameObject Arrow;
    [SerializeField] private TextMeshProUGUI Answer;

    [SerializeField] private TMP_InputField X2;
    [SerializeField] private TMP_InputField Y2;
    [SerializeField] private TMP_InputField Res2;

    [SerializeField] private TMP_InputField X1;
    [SerializeField] private TMP_InputField Y1;
    [SerializeField] private TMP_InputField Res1;

    private AudioClip[] voiceClips;
    private AudioSource audioSource;
    private int clipIndex = 0;
    string Y1Sign = "+", Y2Sign = "+", ResSubRes = "+";
    float X1Val, X2Val, XRes, Y1Val, Y2Val;
    float Y1AfterMult, Y2AfterMult;
    float ResFloat1, ResFloat2;
    float YFinalResult = 0;
    [SerializeField] private TextMeshProUGUI lineRenderer;

    public SubtractionArabic(GameObject Circle, GameObject Arrow, TMP_InputField X1, TMP_InputField Y1, TMP_InputField Res1, TMP_InputField X2, TMP_InputField Y2, TMP_InputField Res2, ref TextMeshProUGUI Answer, ref TextMeshProUGUI lineRenderer) {
        this.Circle = Circle;
        this.Arrow = Arrow;
        this.X1 = X1;
        this.X2 = X2;
        this.Y1 = Y1;
        this.Y2 = Y2;
        this.Res1 = Res1;
        this.Res2 = Res2;
        this.Answer = Answer;
        this.lineRenderer = lineRenderer;
        Answer.lineSpacing = 0.1f;
        X1Val = float.Parse(X1.text);
        X2Val = float.Parse(X2.text);
        XRes = X1Val * X2Val; Y1Val = float.Parse(Y1.text);
        Y2Val = float.Parse(Y2.text);
        Y1AfterMult = float.Parse(Y1.text) * X2Val;
        Y2AfterMult = float.Parse(Y2.text) * X1Val;
        ResFloat1 = float.Parse(Res1.text);
        ResFloat2 = float.Parse(Res2.text);

        if (Y1Val < 0)
            Y1Sign = "";

        if (Y2Val < 0)
            Y2Sign = "";
    }

    public void Solve()
    {
        Answer.text = "";
        ResetValues();
        Answer.text += "   " + $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.gray)}>{new string(X1Val.ToString().Reverse().ToArray())}</color>" + " س " + Y1Sign.ToString() + " " + new string (Y1Val.ToString().Reverse().ToArray()) + " ص "+" = " + new string (ResFloat1.ToString().Reverse().ToArray()) + " × )" + $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.green)}>{new string (X2Val.ToString().Reverse().ToArray())}</color>" + "(<line-height=0.5em> \n\n</line-height>";

        Answer.text += "   " + $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.green)}>{new string (X2Val.ToString().Reverse().ToArray())}</color>" + " س " + Y2Sign.ToString() + " " + new string(Y2Val.ToString().Reverse().ToArray()) + " ص = " + new string (ResFloat2.ToString().Reverse().ToArray()) + " × )" + $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.gray)}>{new string (X1Val.ToString().Reverse().ToArray())}</color>" + "( --< )ع2(<line-height=1.5em> \n</line-height>";

        Answer.text += "   " + new string (XRes.ToString().Reverse().ToArray()) + " س " + Y1Sign.ToString() + " " + new string(Y1AfterMult.ToString().Reverse().ToArray()) + " ص = " + new string((X2Val * ResFloat1).ToString().Reverse().ToArray()) + "<line-height=0.5em> \n\n</line-height>";
        Answer.text += "   " + new string (XRes.ToString().Reverse().ToArray()) + " س " + " " + Y2Sign.ToString() + " " + new string (Y2AfterMult.ToString().Reverse().ToArray()) + " ص = " + new string((X1Val * ResFloat2 ).ToString().Reverse().ToArray()) + "<line-height=1.5em> \n</line-height>";

        Answer.text += "   " + new string (XRes.ToString().Reverse().ToArray()) + " س " + " " + Y2Sign.ToString() + " " + new string (Y1AfterMult.ToString().Reverse().ToArray()) + " ص = " + new string ((X2Val * ResFloat1).ToString().Reverse().ToArray()) + "<line-height=0.5em> \n</line-height>";

        Answer.text += "<line-height=0.7em>-\n</line-height>";
        SolvingTwoEqs.DisplayLineRender(lineRenderer, Answer);
        lineRenderer.transform.position = new Vector3 (lineRenderer.transform.position.x-600, lineRenderer.transform.position.y, 0f);
        Answer.text += "   " + new string (XRes.ToString().Reverse().ToArray()) + " س " + " " + Y1Sign.ToString() + " " + new string (Y2AfterMult.ToString().Reverse().ToArray()) + "ص = " + new string  ((X1Val * ResFloat2).ToString().Reverse().ToArray()) + "<line-height=1.3em> \n</line-height>";

        if (((X2Val * ResFloat1) - (X1Val * ResFloat2)) < 0)
            ResSubRes = "";

        Answer.text += "   " + new string((Y1AfterMult - Y2AfterMult).ToString().Reverse().ToArray()) + " ص = " + new string((ResSubRes + ((X2Val * ResFloat1) - (X1Val * ResFloat2))).ToString().Reverse().ToArray()) + "<line-height=1.3em> \n</line-height>";
        Answer.text += "   س = " + new string((ResSubRes + ((X2Val * ResFloat1) - (X1Val * ResFloat2))).ToString().Reverse().ToArray()) + " ÷ " + new string((Y1AfterMult - Y2AfterMult).ToString().Reverse().ToArray()) + "<line-height=1.3em> \n</line-height>";
        Answer.text += $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.gray)}>{"              ص = " + new string((((X2Val * ResFloat1) - (X1Val * ResFloat2)) / (Y1AfterMult - Y2AfterMult)).ToString().Reverse().ToArray())}</color>" + "<line-height=1.3em> \n</line-height>";

        YFinalResult = (((X2Val * ResFloat1) - (X1Val * ResFloat2)) / (Y1AfterMult - Y2AfterMult));

        Answer.text += "   " + new string (X2Val.ToString().Reverse().ToArray()) + " س +" + new string(Y2Val.ToString().Reverse().ToArray()) + "× )" + new string(YFinalResult.ToString().Reverse().ToArray()) + "( = " + new string (ResFloat2.ToString().Reverse().ToArray()) + " --<)ع2( <line-height=1.3em> \n</line-height>";
        Answer.text += "   " + new string (X2Val.ToString().Reverse().ToArray()) + " س " + new string((Y2Val * YFinalResult).ToString().Reverse().ToArray()) + " = " + new string (ResFloat2.ToString().Reverse().ToArray()) + "<line-height=1.3em> \n</line-height>";

        if ((Y2Val * YFinalResult) > 0)
            Answer.text += "   " + new string (X2Val.ToString().Reverse().ToArray()) + " س " + "= " + new string (ResFloat2.ToString().Reverse().ToArray()) + " - " + new string((Y2Val * YFinalResult).ToString().Reverse().ToArray()) + "<line-height=1.3em> \n</line-height>";
        else
            Answer.text += "   " + new string (X2Val.ToString().Reverse().ToArray()) + " س " + "= " + new string (ResFloat2.ToString().Reverse().ToArray()) + " + " + new string(Mathf.Abs(Y2Val * YFinalResult).ToString().Reverse().ToArray()) + "<line-height=1.3em> \n</line-height>";

        Answer.text += "   " + new string (X2Val.ToString().Reverse().ToArray()) + " س " + "= " + new string((ResFloat2 - (Y2Val * YFinalResult)).ToString().Reverse().ToArray()) + "<line-height=1.3em> \n</line-height>";
        Answer.text += "   " + " س " + "= " + new string(((ResFloat2 - (Y2Val * YFinalResult)) + " ÷ " + X2Val).ToString().Reverse().ToArray()) + "<line-height=1.3em> \n</line-height>";
        Answer.text += $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.gray)}>{"              " + " س  " + "= " + new string(((ResFloat2 - (Y2Val * YFinalResult)) / X2Val).ToString().Reverse().ToArray())}</color>" + "<line-height=1.3em> \n</line-height>";
        Answer.text += "   م . ح" + "}" + ')' + new string(((ResFloat2 - (Y2Val * YFinalResult)) / X2Val).ToString().Reverse().ToArray()) + ',' + new string(YFinalResult.ToString().Reverse().ToArray()) + '(' + '{';
    }
    public IEnumerator SpeakAndWait(string written, int index)
    {
        if (written[0].Equals('!'))
        {
            foreach (char latter in written)
            {
                if (latter.Equals('!'))
                {
                    yield return (PlayVoiceClipAndWait(clipIndex));
                }
                else
                    break;
            }
            Answer.text += written.Substring(written.LastIndexOf('!') + 1);
        }
        else if (index == 2)
        {
            yield return (PlayVoiceClipAndWait(clipIndex));

            yield return (VoiceSpeaker.PlayVoiceNumberAndWait(X2.text));

            Answer.text = "   " + $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.gray)}>{new string(X1.text.Reverse().ToArray())}</color>" + " س " + Y1Sign.ToString() + " " + new string(Y1.text.Reverse().ToArray()) + " ص = " + new string(Res1.text.Reverse().ToArray()) + " × )" + $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.green)}>{new string(X2.text.Reverse().ToArray())}</color>" + "(<line-height=0.5em> \n\n</line-height>" + Answer.text.Substring(Answer.text.LastIndexOf("</line-height>") + 14);

            yield return (PlayVoiceClipAndWait(clipIndex));
            yield return (VoiceSpeaker.PlayVoiceNumberAndWait(X1.text));

            Answer.text = Answer.text.Substring(0, Answer.text.LastIndexOf("</line-height>") + 14) + "   " + $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.green)}>{new string(X2.text.Reverse().ToArray())}</color>" + " س " + Y2Sign.ToString() + " " + new string(Y2.text.Reverse().ToArray()) + " ص = " +new string(Res2.text.Reverse().ToArray())+ " × )" + $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.gray)}>{new string(X1.text.Reverse().ToArray())}</color>" + "(\n";

            yield return (PlayVoiceClipAndWait(clipIndex));

            //---------------------------------------------------------------------------

            yield return (VoiceSpeaker.PlayVoiceNumberAndWait(X2.text));// starting multplication
            yield return (VoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/time_Heba_Egy"));
            yield return (VoiceSpeaker.PlayVoiceNumberAndWait(X1.text));
            yield return (VoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/equal_Heba_Egy"));

            float MultIRes = float.Parse(X1.text) * float.Parse(X2.text);
            yield return (VoiceSpeaker.PlayVoiceNumberAndWait(MultIRes.ToString()));
            yield return (VoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/x_Heba_Egy"));

            int counter = written.IndexOf('|');
            Answer.text += written.Substring(0, counter);
            //---------------------------------------------------------------------------
            yield return (VoiceSpeaker.PlayVoiceNumberAndWait(X2.text));
            yield return (VoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/time_Heba_Egy"));
            yield return (VoiceSpeaker.PlayVoiceNumberAndWait(Y1.text));
            yield return (VoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/equal_Heba_Egy"));

            MultIRes = float.Parse(Y1.text) * float.Parse(X2.text);
            yield return (VoiceSpeaker.PlayVoiceNumberAndWait(MultIRes.ToString()));
            yield return (VoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/y_Heba_Egy"));

            int oldCounter = counter;
            counter = written.IndexOf('|', counter + 1);
            Answer.text += written.Substring(oldCounter + 1, counter - oldCounter - 1);
            //-----------------------------------------------------------------------------------------------
            yield return (VoiceSpeaker.PlayVoiceNumberAndWait(X2.text));
            yield return (VoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/time_Heba_Egy"));
            yield return (VoiceSpeaker.PlayVoiceNumberAndWait(Res1.text));
            yield return (VoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/equal_Heba_Egy"));
            yield return (VoiceSpeaker.PlayVoiceNumberAndWait((float.Parse(X2.text) * float.Parse(Res1.text)).ToString()));

            Answer.text += written.Substring(counter + 1, written.Length - counter - 1);
        }
        else if (index == 3)
        {
            yield return (VoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/In the second equation_Heba_Egy"));
            yield return (VoiceSpeaker.PlayVoiceNumberAndWait(X1.text));// starting multplication
            yield return (VoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/time_Heba_Egy"));
            yield return (VoiceSpeaker.PlayVoiceNumberAndWait(X2.text));
            yield return (VoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/equal_Heba_Egy"));

            float MultIRes = float.Parse(X1.text) * float.Parse(X2.text);
            yield return (VoiceSpeaker.PlayVoiceNumberAndWait(MultIRes.ToString()));
            yield return (VoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/x_Heba_Egy"));

            int counter = written.IndexOf('|');
            Answer.text += written.Substring(0, counter);
            //---------------------------------------------------------------------------
            yield return (VoiceSpeaker.PlayVoiceNumberAndWait(X1.text));
            yield return (VoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/time_Heba_Egy"));
            yield return (VoiceSpeaker.PlayVoiceNumberAndWait(Y2.text));
            yield return (VoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/equal_Heba_Egy"));

            MultIRes = float.Parse(Y2.text) * float.Parse(X1.text);
            yield return (VoiceSpeaker.PlayVoiceNumberAndWait(MultIRes.ToString()));

            yield return (VoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/y_Heba_Egy"));

            int oldCounter = counter;
            counter = written.IndexOf('|', counter + 1);
            Answer.text += written.Substring(oldCounter + 1, counter - oldCounter - 1);
            //-----------------------------------------------------------------------------------------------
            yield return (VoiceSpeaker.PlayVoiceNumberAndWait(X1.text));
            yield return (VoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/time_Heba_Egy"));
            yield return (VoiceSpeaker.PlayVoiceNumberAndWait(Res2.text));
            yield return (VoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/equal_Heba_Egy"));
            yield return (VoiceSpeaker.PlayVoiceNumberAndWait((float.Parse(X1.text) * float.Parse(Res2.text)).ToString()));

            Answer.text += written.Substring(counter + 1, written.Length - counter - 1);
        }
        else if (index == 7)
        {

            yield return (VoiceSpeaker.PlayVoiceNumberAndWait(XRes.ToString()));
            yield return (VoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/x_Heba_Egy"));
            yield return (VoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/minus_Heba_Egy"));
            yield return (VoiceSpeaker.PlayVoiceNumberAndWait(XRes.ToString()));
            yield return (VoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/x_Heba_Egy"));
            yield return (VoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/equal_Heba_Egy"));
            yield return (VoiceSpeaker.PlayVoiceNumberAndWait("0"));
            //------------------------------------------------------------------------------------------           
            yield return (VoiceSpeaker.PlayVoiceNumberAndWait(Y1AfterMult.ToString()));
            yield return (VoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/y_Heba_Egy"));
            yield return (VoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/minus_Heba_Egy"));
            yield return (VoiceSpeaker.PlayVoiceNumberAndWait(Y2AfterMult.ToString()));
            yield return (VoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/y_Heba_Egy"));
            yield return (VoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/equal_Heba_Egy"));
            yield return (VoiceSpeaker.PlayVoiceNumberAndWait((Y1AfterMult - Y2AfterMult).ToString()));
            yield return (VoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/y_Heba_Egy"));

            Answer.text += "   " + (Y1AfterMult - Y2AfterMult) + " ص ";
            //------------------------------------------------------------------------------------------           

            yield return (VoiceSpeaker.PlayVoiceNumberAndWait((ResFloat1 * X2Val).ToString()));
            yield return (VoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/minus_Heba_Egy"));
            yield return (VoiceSpeaker.PlayVoiceNumberAndWait((ResFloat2 * X1Val).ToString()));
            yield return (VoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/equal_Heba_Egy"));
            yield return (VoiceSpeaker.PlayVoiceNumberAndWait((ResFloat1 * X2Val - ResFloat2 * X1Val).ToString()));
            Answer.text += " = " + (ResFloat1 * X2Val - ResFloat2 * X1Val) + '\n';


        }
        else if (index == 8)
        {
            yield return (VoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/send_Heba_Egy"));
            yield return (VoiceSpeaker.PlayVoiceNumberAndWait((Y1AfterMult - Y2AfterMult).ToString()));
            yield return (VoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/To the other side_Heba_Egy"));

            Answer.text += "   ص = " + (ResFloat1 * X2Val - ResFloat2 * X1Val).ToString() + " ÷ " + (Y1AfterMult - Y2AfterMult).ToString() + '\n';

        }
        else if (index == 9)
        {
            yield return (VoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/so_Heba_Egy"));
            yield return (VoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/y_Heba_Egy"));
            yield return (VoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/equal_Heba_Egy"));

            Y1Val = (((X2Val * ResFloat1) - (X1Val * ResFloat2)) / (Y1AfterMult - Y2AfterMult));

            yield return (VoiceSpeaker.PlayVoiceNumberAndWait(Y1Val.ToString()));

            Answer.text += $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.gray)}>{"              ص = " + Y1Val}</color>" + '\n';
            YFinalResult = Y1Val;
        }
        else if (index == 10)
        {
            yield return (VoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/by replacing_Heba_Egy"));
            yield return (VoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/y_Heba_Egy"));
            yield return (VoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/In the second equation_Heba_Egy"));
            yield return (VoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/so_Heba_Egy"));
            yield return (VoiceSpeaker.PlayVoiceNumberAndWait(X2Val.ToString()));

            yield return (VoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/x_Heba_Egy"));
            Answer.text += "   " + X2Val.ToString() + " س ";

            yield return (VoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/plus_Heba_Egy"));
            Answer.text += " + ";

            yield return (VoiceSpeaker.PlayVoiceNumberAndWait(Y2Val.ToString()));
            Answer.text += Y2Val.ToString();

            yield return (VoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/time_Heba_Egy"));
            Answer.text += " × ";

            yield return (VoiceSpeaker.PlayVoiceNumberAndWait(Y1Val.ToString()));
            Answer.text += ')' + Y1Val.ToString() + '(';

            yield return (VoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/equal_Heba_Egy"));
            Answer.text += " = ";

            yield return (VoiceSpeaker.PlayVoiceNumberAndWait(ResFloat2.ToString()));
            Answer.text += ResFloat2.ToString() + '\n';

        }
        else if (index == 11)
        {
            yield return (VoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/so_Heba_Egy"));
            yield return (VoiceSpeaker.PlayVoiceNumberAndWait(X2Val.ToString()));
            Answer.text += "   " + X2Val.ToString();

            yield return (VoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/x_Heba_Egy"));
            Answer.text += "x";

            if (Y1Val * Y2Val > 0)
            {
                yield return (VoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/plus_Heba_Egy"));
                Answer.text += " + ";
            }
            yield return (VoiceSpeaker.PlayVoiceNumberAndWait((Y2Val * Y1Val).ToString()));
            Answer.text += (Y2Val * Y1Val).ToString();


            yield return (VoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/equal_Heba_Egy"));
            Answer.text += " = ";

            yield return (VoiceSpeaker.PlayVoiceNumberAndWait(ResFloat2.ToString()));
            Answer.text += ResFloat2.ToString() + '\n';

        }
        else if (index == 12)
        {
            yield return 0.6f;
            if ((Y2Val * Y1Val) < 0)
                Answer.text += "   " + X2.text + " س " + (Y2Val * Y1Val) + " = " + Res2.text;
            else
                Answer.text += "   " + X2.text + " س +" + (Y2Val * Y1Val) + " = " + Res2.text;

            yield return (VoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/send_Heba_Egy"));
            yield return (VoiceSpeaker.PlayVoiceNumberAndWait((Y2Val * Y1Val).ToString()));
            yield return (VoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/To the other side_Heba_Egy"));

            Y2Val = (-(Y2Val * Y1Val));

            if ((Y2Val * Y1Val) < 0)
                Answer.text += $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.gray)}>{"   + " + new string(Y2Val.ToString().Reverse().ToArray())}</color>" + '\n';
            else
                Answer.text += $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.gray)}>{"   " + new string(Y2Val.ToString().Reverse().ToArray())}</color>" + '\n';

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
                worldPosition.x += 200;
                worldPosition.y += 50;
                if ((Y2Val * Y1Val).ToString().Length >= 3)
                {
                    worldPosition.x += 30;
                    Circle.transform.localScale = new Vector3(Circle.transform.localScale.x * 1.4f, Circle.transform.localScale.y * 1.4f, Circle.transform.localScale.z);

                }
                Circle.transform.position = worldPosition;
                Circle.SetActive(true);

                // Adjust Arrow position using Circle's position
                float arrowSizeX = Circle.transform.position.x;
                float arrowSizeY = Circle.transform.position.y;
                Vector3 ArrowSize = new Vector3(arrowSizeX, arrowSizeY, 0);
                Arrow.transform.position = new Vector3(ArrowSize.x, 1.07f * ArrowSize.y, worldPosition.z);

                Arrow.SetActive(true);
            }
        }
        else if (index == 13)
        {
            yield return (VoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/so_Heba_Egy"));

            yield return (VoiceSpeaker.PlayVoiceNumberAndWait(X2Val.ToString()));
            Answer.text += "   " +new string(X2Val.ToString().Reverse().ToArray());

            yield return (VoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/x_Heba_Egy"));
            Answer.text += " س ";

            yield return (VoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/equal_Heba_Egy"));
            Answer.text += " = ";

            Y2Val += ResFloat2;

            yield return (VoiceSpeaker.PlayVoiceNumberAndWait(Y2Val.ToString()));
            Answer.text += Y2Val.ToString() + '\n';
        }
        else if (index == 14)
        {
            yield return (VoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/then_Heba_Egy"));
            yield return (VoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/send_Heba_Egy"));
            yield return (VoiceSpeaker.PlayVoiceNumberAndWait((X2Val).ToString()));
            yield return (VoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/To the other side_Heba_Egy"));
            Answer.text += "   س = " + new string(Y2Val.ToString().Reverse().ToArray()) + " ÷ " + X2Val + '\n';
        }
        else if (index == 15)
        {
            yield return (VoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/so_Heba_Egy"));
            yield return (VoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/x_Heba_Egy"));
            yield return (VoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/equal_Heba_Egy"));
            X2Val = (float)Y2Val / X2Val;
            yield return (VoiceSpeaker.PlayVoiceNumberAndWait((X2Val).ToString()));
            Answer.text += $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.gray)}>{"              س " + "= " + (new string(X2Val.ToString().Reverse().ToArray()))}</color>" + '\n';
        }
        else if (index == 16)
        {
            Answer.text += "   " + "}" + ')' + new string(X2Val.ToString().Reverse().ToArray()) + ',' + new string(YFinalResult.ToString().Reverse().ToArray()) + '(' + '{' + '\n';
        }
        else
        {
            if (!written[0].Equals('!'))
                Answer.text += written;
        }
        if (index == 5)
        {
            SolvingTwoEqs.DisplayLineRender(lineRenderer, Answer);
        }
    }
    public void LoadAllAudioClips()
    {
        voiceClips = Resources.LoadAll<AudioClip>("Solving2EqsSoundsAr");
    }
    public IEnumerator PlayVoiceClipAndWait(int index)
    {
        audioSource.clip = voiceClips[index];
        clipIndex++;
        audioSource.Play();
        Debug.Log(audioSource.clip.name);
        yield return new WaitForSeconds(audioSource.clip.length);
    }
    public List<string> SolveStepByStep()
    {
        Answer.text = "";
        ResetValues();
        LoadAllAudioClips();
        GameObject audioObject = new GameObject("VoiceAudioSource");
        audioSource = audioObject.AddComponent<AudioSource>();
        VoiceSpeaker.NumbersName = "EgyNums";
        VoiceSpeaker.voiceClipsName = "AdditionTerms/AdditionSound";
        VoiceSpeaker.PointPlace = "AdditionTerms/AdditionSound/POINT_Heba_Egy";
        VoiceSpeaker.LoadAllAudioClips();
        List<string> WritingSteps = new List<string>
        {
            "!!   " + new string (X1Val.ToString().Reverse().ToArray()) + " س " + Y1Sign.ToString() + " " + new string (Y1Val.ToString().Reverse().ToArray()) + " ص = " + new string(ResFloat1.ToString().Reverse().ToArray()) + "<line-height=0.5em> \n\n</line-height>",
            "!!   " + new string(X1Val.ToString().Reverse().ToArray())+ " س " + Y2Sign.ToString() + " " + new string(Y2Val.ToString().Reverse().ToArray()) + " ص = " + new string(ResFloat2.ToString().Reverse().ToArray()) + "\n",
            "   " + new string(XRes.ToString().Reverse().ToArray()) + "X |" + Y1Sign.ToString() + " " + new string(Y1AfterMult.ToString().Reverse().ToArray()) + " Y = |" + X2Val * ResFloat1 + "<line-height=0.5em> \n\n</line-height>",
            "   " + new string(XRes.ToString().Reverse().ToArray()) + "X |" + Y2Sign.ToString() + " " + new string(Y2AfterMult.ToString().Reverse().ToArray()) + " Y = |" + X1Val * ResFloat2 + "\n",
            "!!   " + new string(XRes.ToString().Reverse().ToArray()) + "X " + " " + Y2Sign.ToString() + " " + new string(Y1AfterMult.ToString().Reverse().ToArray()) + "Y = " +        new string((X2Val * ResFloat1).ToString().Reverse().ToArray()) + "<line-height=0.5em>\n</line-height>",
            "<line-height=0.5em>-\n</line-height>",
            "   " + new string(XRes.ToString().Reverse().ToArray()) + "X " + " " + Y1Sign.ToString() + " " + new string(Y2AfterMult.ToString().Reverse().ToArray()) + "Y = " + new string ((X1Val * ResFloat2).ToString().Reverse().ToArray()) + '\n'
        };


        if (((X2Val * ResFloat1) - (X1Val * ResFloat2)) < 0)
            ResSubRes = "";

        WritingSteps.Add("        " + new string((Y1AfterMult - Y2AfterMult).ToString().Reverse().ToArray()) + "Y = " + new string(ResSubRes + ((X2Val * ResFloat1) - (X1Val * ResFloat2))).ToString().Reverse().Reverse().ToArray() + '\n');
        WritingSteps.Add("   Y = " + ResSubRes + ((X2Val * ResFloat1) - (X1Val * ResFloat2)) + " ÷ " + (Y1AfterMult - Y2AfterMult) + '\n');
        WritingSteps.Add($"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.gray)}>{"              Y = " + (((X2Val * ResFloat1) - (X1Val * ResFloat2)) / (Y1AfterMult - Y2AfterMult))}</color>" + '\n');

        YFinalResult = (((X2Val * ResFloat1) - (X1Val * ResFloat2)) / (Y1AfterMult - Y2AfterMult));
        WritingSteps.Add("   " + X2Val + "X +" + Y2Val + "× (" + YFinalResult + ") = " + ResFloat2 + " -->(Sec eq) \n");

        WritingSteps.Add("   " + X2Val + "X " + (Y2Val * YFinalResult) + " = " + ResFloat2 + "\n");

        if ((Y2Val * YFinalResult) > 0)
            WritingSteps.Add("   " + X2Val + "X " + "= " + ResFloat2 + " - " + (Y2Val * YFinalResult) + "\n");
        else
            WritingSteps.Add("   " + X2Val + "X " + "= " + ResFloat2 + " + " + Mathf.Abs(Y2Val * YFinalResult) + "\n");

        WritingSteps.Add("   " + X2Val + "X " + "= " + (ResFloat2 - (Y2Val * YFinalResult)) + "\n");
        WritingSteps.Add("   " + "X " + "= " + (ResFloat2 - (Y2Val * YFinalResult)) + " ÷ " + X2Val + "\n");

        WritingSteps.Add($"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.gray)}>{"              " + "X " + "= " + ((ResFloat2 - (Y2Val * YFinalResult)) / X2Val)}</color>" + "\n");

        WritingSteps.Add("   " + "{" + '(' + ((ResFloat2 - (Y2Val * YFinalResult)) / X2Val) + ',' + YFinalResult + ')' + '}');
        return WritingSteps;
    }
    public void ResetValues()
    {
        X1Val = float.Parse(X1.text);
        X2Val = float.Parse(X2.text);
        Y1Val = float.Parse(Y1.text);
        Y2Val = float.Parse(Y2.text);
        ResFloat1 = float.Parse(Res1.text);
        ResFloat2 = float.Parse(Res2.text);
        clipIndex = 0;
        Circle.SetActive(false);
        Arrow.SetActive(false);
        if (Y1Val < 0)
            Y1Sign = "";

        if (Y2Val < 0)
            Y2Sign = "";
    }
}
