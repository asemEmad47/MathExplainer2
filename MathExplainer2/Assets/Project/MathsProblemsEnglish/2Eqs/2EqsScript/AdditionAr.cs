using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class AdditionAr : MonoBehaviour, TwoEqsParent
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
    string Y1Sign = "+", Y2Sign = "+", ResSubRes = "+", X1Sign = "", X2Sign = "";
    float X1Val, X2Val, Y1Val, Y2Val;
    float X1Y2Val, X2Y1Val, Y2Y1Val, Y1Y2Val, Res1Y2Val, Res2Y1Val;
    float ResFloat1, ResFloat2;
    float XFinalResult = 0;
    float DivisionRes = 0;
    bool notEqual = true;
    [SerializeField] private TextMeshProUGUI lineRenderer;

    public AdditionAr(GameObject Circle, GameObject Arrow, TMP_InputField X1, TMP_InputField Y1, TMP_InputField Res1, TMP_InputField X2, TMP_InputField Y2, TMP_InputField Res2, ref TextMeshProUGUI Answer, ref TextMeshProUGUI lineRenderer)
    {
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
        Y1Val = float.Parse(Y1.text);
        Y2Val = float.Parse(Y2.text);
        ResFloat1 = float.Parse(Res1.text);
        ResFloat2 = float.Parse(Res2.text);
    }

    public void Solve()
    {
        ResetValues();
        Answer.text = "";
        Answer.isRightToLeftText = true; // Enable RTL for the entire text

        bool notEqual = true;

        if (MathF.Abs(Y1Val) != MathF.Abs(Y2Val))
        {
            if (Y1Val % Y2Val == 0 && Y1Val > Y2Val)
            {
                Answer.text += "  " + "<align=left>" +X1Sign.ToString()+ ReverseSubstring( X1Val) + "</align>" + " س " + Y1Sign.ToString()+ ReverseSubstring( Y1Val) + " " + " ص = " + ReverseSubstring(ResFloat1) + "<line-height=0.5em> \n\n</line-height>";
                Answer.text += "  " + "<align=left>" + X2Sign + ReverseSubstring(X2Val) + "</align>" + " س " +X2Sign.ToString()+ ReverseSubstring( Y2Val) + " "+ " ص = " + ReverseSubstring(ResFloat2) + " x )" + $"<color=#{ColorUtility.ToHtmlStringRGB(Color.green)}>{ReverseSubstring(-Y1Val / Y2Val)}</color>" + "(<line-height=1.5em> \n</line-height>";

                X2Val *= -(Y1Val / Y2Val);
                ResFloat2 *= -(Y1Val / Y2Val);
                Y2Val *= -(Y1Val / Y2Val);
            }
            else
            {
                Answer.text += "  " + "<align=left>" + X1Sign + ReverseSubstring(X1Val) + "</align>" + " س " + X1Sign.ToString() + ReverseSubstring(Y1Val) + " " + " ص = " + ReverseSubstring(ResFloat1) + "x )" + $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.green)}>{ReverseSubstring(-Y2Val / Y1Val)}</color>" + "(" + "<line-height=0.5em> \n\n</line-height>";
                Answer.text += "  " + "<align=left>" + X2Sign + ReverseSubstring(X2Val) + "</align>" + " س " + X2Sign.ToString() + ReverseSubstring(Y2Val) + " " + $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.green)}>{Y2Val}</color>" + " ص = " + ReverseSubstring(ResFloat2) + "--<(ع2)<line-height=1.5em> \n</line-height>";

                X1Val *= -(Y2Val / Y1Val);
                ResFloat1 *= -(Y2Val / Y1Val);
                Y1Val *= -(Y2Val / Y1Val);
            }
        }
        else
        {
            Answer.text += "  " + "<align=left>" + X1Sign + ReverseSubstring(X1Val) + "</align>" + " س " + X1Sign.ToString() + ReverseSubstring(Y1Val) + " " + " ص = " + ReverseSubstring(ResFloat1) + "<line-height=0.5em> \n\n</line-height>";
            Answer.text += "  " + "<align=left>" + X2Sign + ReverseSubstring(X2Val) + "</align>" + " س " + X2Sign.ToString() + ReverseSubstring(Y2Val) + " " + " ص = " + ReverseSubstring(ResFloat2) + "<line-height=1.5em> \n</line-height>";
        }

        if (Y1Val < 0)
            Y1Sign = "";
        else
            Y1Sign = "+";

        if (Y2Val < 0)
            Y2Sign = "";
        else
            Y2Sign = "+";

        if (X1Val > 0)
            X1Sign = " ";
        else
            X1Sign = "";

        if (X2Val > 0)
            X2Sign = " ";
        else
            X2Sign = "";

        if (!notEqual)
        {
            Answer.text += "  " + "<align=left>" + X1Sign + ReverseSubstring(X1Val) + "</align>" + " س " + X1Sign.ToString() + ReverseSubstring(Y1Val) + " " + " ص = " + ReverseSubstring(ResFloat1) + "<line-height=0.5em> \n\n</line-height>";
            Answer.text += "  " + "<align=left>" + X2Sign + ReverseSubstring(X2Val) + "</align>" + " س " + " " + Y2Sign.ToString()+ReverseSubstring( Y2Val) + " "  + " ص = " + ReverseSubstring(ResFloat2) + "<line-height=1.5em> \n</line-height>";
        }

        Answer.text += "  " + "<align=left>" + X1Sign + ReverseSubstring(X1Val) + "</align>" + " س " + " " + Y1Sign.ToString() + ReverseSubstring(Y1Val) + " " + " ص = " + ReverseSubstring(ResFloat1) + "<line-height=0.5em> \n</line-height>";
        Answer.text += "<line-height=0.5em>+\n</line-height>";
        SolvingTwoEqs.DisplayLineRender(lineRenderer, Answer);
        lineRenderer.transform.position = new Vector3(lineRenderer.transform.position.x - 600, lineRenderer.transform.position.y, 0f);
        Answer.text += "  " + "<align=left>" + X2Sign + ReverseSubstring(X2Val) + "</align>" + " س  " + " " + Y2Sign.ToString() + ReverseSubstring(Y2Val) + " " + " ص = " + ReverseSubstring(ResFloat2) + '\n';

        if ((ResFloat1 - ResFloat2) < 0)
            ResSubRes = "";

        Answer.text += "   " + ReverseSubstring(X1Val + X2Val) + "س = " + " " + ReverseSubstring(ResFloat1 + ResFloat2) + "<line-height=1.5em> \n</line-height>";
        Answer.text += "   س = " + ReverseSubstring(ResFloat1 + ResFloat2) + " ÷ " + ReverseSubstring(X1Val + X2Val) + "<line-height=1.5em> \n</line-height>";
        Answer.text += $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.gray)}>{"              س = " + ReverseSubstring((ResFloat1 + ResFloat2) / (X1Val + X2Val))}</color>" + "<line-height=1.5em> \n</line-height>";

        XFinalResult = MathF.Round((ResFloat1 + ResFloat2) / (X1Val + X2Val), 2);
        ResetValues();

        if (Y2Val < 0)
        {
            Answer.text += "   " + ReverseSubstring(X2Val) + " × " + ')' + " " + ReverseSubstring(XFinalResult) + "( " + ReverseSubstring(Y2Val) + " ص " + " = " + ReverseSubstring(ResFloat2) + " --<)ع2( <line-height=1.5em> \n</line-height>";
            Answer.text += "   " + ReverseSubstring(X2Val * XFinalResult) + " " + ReverseSubstring(Y2Val) + " ص " + " = " + ReverseSubstring(ResFloat2) + "<line-height=1.5em> \n</line-height>";
        }
        else
        {
            Answer.text += "   " + ReverseSubstring(X2Val) + " × " + ')' + ReverseSubstring(XFinalResult) + "( + " + ReverseSubstring(Y2Val) + " س " + " = " + ReverseSubstring(ResFloat2) + " --<)ع2( <line-height=1.5em> \n</line-height>";
            Answer.text += "   " + ReverseSubstring(X2Val * XFinalResult) + " + " + ReverseSubstring(Y2Val) + " س " + " = " + ReverseSubstring(ResFloat2) + "<line-height=1.5em> \n</line-height>";
        }

        if ((X2Val * X2Val) > 0)
            Answer.text += "   " + ReverseSubstring(Y2Val) + " ص " + " = " + ReverseSubstring(ResFloat2) + " - " + ReverseSubstring(X2Val * XFinalResult) + "<line-height=1.5em> \n</line-height>";
        else
            Answer.text += "   " + ReverseSubstring(Y2Val) + " ص " + " = " + ReverseSubstring(ResFloat2) + " + " + ReverseSubstring(Mathf.Abs(X2Val * XFinalResult)) + "<line-height=1.5em> \n</line-height>";

        Answer.text += "   " + ReverseSubstring(Y2Val) + " ص " + " = " + ReverseSubstring(ResFloat2 - (X2Val * XFinalResult)) + "\n";
        Answer.text += "   " + " ص " + " = " + ReverseSubstring(ResFloat2 - (X2Val * XFinalResult)) + " ÷ " + ReverseSubstring(Y2Val) + "\n";
        Answer.text += $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.gray)}>{"   " + " ص " + " = " + ReverseSubstring((ResFloat2 - (X2Val * XFinalResult)) / Y2Val)}</color>" + "<line-height=1.5em> \n</line-height>";
        Answer.text += "   " + "}" + ')' + ReverseSubstring(XFinalResult) + ',' + ReverseSubstring(MathF.Round(((ResFloat2 - (X2Val * XFinalResult)) / Y2Val), 2)) + '(' + '{';
    }

    // Function to reverse a substring while keeping the negative sign before the number
    string ReverseSubstring(float value)
    {
        string reversedValue = Math.Abs(value).ToString();
        reversedValue = new string (reversedValue.Reverse().ToArray());
        return (value < 0 ? "-" : "") + reversedValue;
    }

    public IEnumerator SpeakAndWait(string written, int index)
    {
        if (written.Length != 0 && written[0].Equals('!'))
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
            Answer.text += written[(written.LastIndexOf('!') + 1)..];
        }
        if (index == 1)
        {
            int counter = 0;
            while (counter < 4)
            {
                yield return (PlayVoiceClipAndWait(clipIndex));
                counter++;
            }
        }
        else if (index == 2)
        {
            if (MathF.Abs(Y1Val) != MathF.Abs(Y2Val))
            {
                if (Y2Val % Y1Val == 0 && Y2Val > Y1Val)
                {
                    DivisionRes = -float.Parse(Y2.text) / float.Parse(Y1.text);
                    yield return (PlayVoiceClipAndWait(clipIndex));
                    yield return (VoiceSpeaker.PlayVoiceNumberAndWait((-Y2Val / Y1Val).ToString()));
                    Answer.text = "  " + X1Sign + X1Val + "X " + Y1Sign.ToString() + " " + Y1Val + "Y = " + ResFloat1 + " x (" + $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.green)}>{-Y2Val / Y1Val}</color>" + ") " + "<line-height=0.5em> \n\n</line-height>";

                    Answer.text += "  " + X2Sign + X2Val + "X " + Y2Sign.ToString() + " " + $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.green)}>{Y2Val}</color>" + "Y = " + ResFloat2 + "\n";

                    yield return new WaitForSeconds(1f);
                    //---------------------------------------------------------------------------
                    yield return (VoiceSpeaker.PlayVoiceNumberAndWait(DivisionRes.ToString()));// starting multplication
                    yield return (VoiceSpeaker.PlayByAddress("SolvingTwoEqs/time_Sonya_Eng"));
                    yield return (VoiceSpeaker.PlayVoiceNumberAndWait(X1.text));
                    yield return (VoiceSpeaker.PlayByAddress("SolvingTwoEqs/equal_Sonya_Eng"));

                    float MultIRes = float.Parse(X1.text) * DivisionRes;
                    yield return (VoiceSpeaker.PlayVoiceNumberAndWait(MultIRes.ToString()));
                    yield return (VoiceSpeaker.PlayByAddress("SolvingTwoEqs/x_Sonya_Eng"));

                    int counter = written.IndexOf('|');
                    Answer.text += written.Substring(0, counter);
                    //---------------------------------------------------------------------------
                    yield return (VoiceSpeaker.PlayVoiceNumberAndWait(DivisionRes.ToString()));// starting multplication
                    yield return (VoiceSpeaker.PlayByAddress("SolvingTwoEqs/time_Sonya_Eng"));
                    yield return (VoiceSpeaker.PlayVoiceNumberAndWait(Y1.text));
                    yield return (VoiceSpeaker.PlayByAddress("SolvingTwoEqs/equal_Sonya_Eng"));

                    MultIRes = float.Parse(Y1.text) * DivisionRes;
                    yield return (VoiceSpeaker.PlayVoiceNumberAndWait(MultIRes.ToString()));
                    yield return (VoiceSpeaker.PlayByAddress("SolvingTwoEqs/y_Sonya_Eng"));

                    int oldCounter = counter;
                    counter = written.IndexOf('|', counter + 1);
                    Answer.text += written.Substring(oldCounter + 1, counter - oldCounter - 1);
                    //-----------------------------------------------------------------------------------------------
                    yield return (VoiceSpeaker.PlayVoiceNumberAndWait(DivisionRes.ToString()));// starting multplication
                    yield return (VoiceSpeaker.PlayByAddress("SolvingTwoEqs/time_Sonya_Eng"));
                    yield return (VoiceSpeaker.PlayVoiceNumberAndWait(Res1.text));
                    yield return (VoiceSpeaker.PlayByAddress("SolvingTwoEqs/equal_Sonya_Eng"));
                    yield return (VoiceSpeaker.PlayVoiceNumberAndWait((DivisionRes * ResFloat1).ToString()));
                    Answer.text += written.Substring(counter + 1, written.Length - counter - 1);
                    Answer.text += "  " + X2Sign + X2Val + "X " + Y2Sign.ToString() + " " + Y2Val + "y = " + ResFloat2 + "\n";
                    X1Y2Val = X1Val * (-Y2Val / Y1Val); Y1Y2Val = Y1Val * (-Y2Val / Y1Val); Res1Y2Val = ResFloat1 * (-Y2Val / Y1Val);
                    X2Y1Val = X2Val; X2Y1Val = X2Val; Y2Y1Val = Y2Val; Res2Y1Val = ResFloat2;
                    clipIndex++;
                }
                else
                {
                    DivisionRes = -float.Parse(Y1.text) / float.Parse(Y2.text);
                    clipIndex++;
                    yield return (PlayVoiceClipAndWait(clipIndex));
                    yield return (VoiceSpeaker.PlayVoiceNumberAndWait((DivisionRes).ToString()));
                    Answer.text = "  " + X1Sign + X1Val + "X " + Y1Sign.ToString() + " " + $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.green)}>{Y1Val}</color>" + "Y = " + ResFloat1 + "<line-height=0.5em> \n\n</line-height>";

                    Answer.text += "  " + X2Sign + X2Val + "X " + Y2Sign.ToString() + " " + Y2Val + "Y = " + ResFloat2 + "  x  (" + $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.green)}>{DivisionRes}</color>" + ")\n";
                    Answer.text += "  " + X1Sign + X1Val + "X " + Y1Sign.ToString() + " " + Y1Val + "Y = " + ResFloat1 + "<line-height=0.5em> \n\n</line-height>";
                    notEqual = false;
                    X1Y2Val = X1Val; X2Y1Val = X1Val; Y1Y2Val = Y1Val; Res1Y2Val = ResFloat1;
                    X2Y1Val = X2Val * (-Y1Val / Y2Val); Y2Y1Val = Y2Val * (-Y1Val / Y2Val); Res2Y1Val = ResFloat2 * (-Y1Val / Y2Val);
                }
            }
            else
            {
                X1Y2Val = X1Val; Y1Y2Val = Y1Val; Res1Y2Val = ResFloat1;
                X2Y1Val = X2Val; Y2Y1Val = Y2Val; Res2Y1Val = ResFloat2;

            }
        }
        else if (index == 3)
        {
            if (!notEqual)
            {
                yield return (VoiceSpeaker.PlayByAddress("SolvingTwoEqs/In the second equation_Sonya_Eng"));
                yield return (VoiceSpeaker.PlayVoiceNumberAndWait(DivisionRes.ToString()));
                yield return (VoiceSpeaker.PlayByAddress("SolvingTwoEqs/time_Sonya_Eng"));
                yield return (VoiceSpeaker.PlayVoiceNumberAndWait(X2.text));
                yield return (VoiceSpeaker.PlayByAddress("SolvingTwoEqs/equal_Sonya_Eng"));

                float MultIRes = DivisionRes * float.Parse(X2.text);
                yield return (VoiceSpeaker.PlayVoiceNumberAndWait(MultIRes.ToString()));
                yield return (VoiceSpeaker.PlayByAddress("SolvingTwoEqs/x_Sonya_Eng"));

                int counter = written.IndexOf('|');
                Answer.text += written.Substring(0, counter);
                //---------------------------------------------------------------------------
                yield return (VoiceSpeaker.PlayVoiceNumberAndWait(DivisionRes.ToString()));
                yield return (VoiceSpeaker.PlayByAddress("SolvingTwoEqs/time_Sonya_Eng"));
                yield return (VoiceSpeaker.PlayVoiceNumberAndWait(Y2.text));
                yield return (VoiceSpeaker.PlayByAddress("SolvingTwoEqs/equal_Sonya_Eng"));

                MultIRes = DivisionRes * float.Parse(Y2.text);
                yield return (VoiceSpeaker.PlayVoiceNumberAndWait(MultIRes.ToString()));

                yield return (VoiceSpeaker.PlayByAddress("SolvingTwoEqs/y_Sonya_Eng"));

                int oldCounter = counter;
                counter = written.IndexOf('|', counter + 1);
                Answer.text += written.Substring(oldCounter + 1, counter - oldCounter - 1);
                //-----------------------------------------------------------------------------------------------
                yield return (VoiceSpeaker.PlayVoiceNumberAndWait(DivisionRes.ToString()));
                yield return (VoiceSpeaker.PlayByAddress("SolvingTwoEqs/time_Sonya_Eng"));
                yield return (VoiceSpeaker.PlayVoiceNumberAndWait(Res2.text));
                yield return (VoiceSpeaker.PlayByAddress("SolvingTwoEqs/equal_Sonya_Eng"));
                yield return (VoiceSpeaker.PlayVoiceNumberAndWait((DivisionRes * float.Parse(Res2.text)).ToString()));

                Answer.text += written.Substring(counter + 1, written.Length - counter - 1);
            }

        }
        else if (index == 7)
        {
            yield return new WaitForSeconds(1f);
            yield return (VoiceSpeaker.PlayVoiceNumberAndWait((X1Y2Val).ToString()));
            yield return (VoiceSpeaker.PlayByAddress("SolvingTwoEqs/x_Sonya_Eng"));
            yield return (VoiceSpeaker.PlayByAddress("SolvingTwoEqs/plus_Sonya_Eng"));
            yield return (VoiceSpeaker.PlayVoiceNumberAndWait((X2Y1Val).ToString()));
            yield return (VoiceSpeaker.PlayByAddress("SolvingTwoEqs/x_Sonya_Eng"));
            yield return (VoiceSpeaker.PlayByAddress("SolvingTwoEqs/equal_Sonya_Eng"));
            yield return (VoiceSpeaker.PlayVoiceNumberAndWait(((X2Y1Val) + (X1Y2Val)).ToString()));
            yield return (VoiceSpeaker.PlayByAddress("SolvingTwoEqs/x_Sonya_Eng"));
            Answer.text += "   " + ((X2Y1Val) + (X1Y2Val)) + " x ";

            //------------------------------------------------------------------------------------------
            yield return new WaitForSeconds(0.5f);
            yield return (VoiceSpeaker.PlayVoiceNumberAndWait((Y1Y2Val).ToString()));
            yield return (VoiceSpeaker.PlayByAddress("SolvingTwoEqs/y_Sonya_Eng"));
            yield return (VoiceSpeaker.PlayByAddress("SolvingTwoEqs/plus_Sonya_Eng"));
            yield return (VoiceSpeaker.PlayVoiceNumberAndWait((Y2Y1Val).ToString()));
            yield return (VoiceSpeaker.PlayByAddress("SolvingTwoEqs/y_Sonya_Eng"));
            yield return (VoiceSpeaker.PlayByAddress("SolvingTwoEqs/equal_Sonya_Eng"));
            yield return (VoiceSpeaker.PlayVoiceNumberAndWait("0"));

            //------------------------------------------------------------------------------------------           
            yield return new WaitForSeconds(0.5f);
            yield return (VoiceSpeaker.PlayVoiceNumberAndWait((Res1Y2Val).ToString()));
            yield return (VoiceSpeaker.PlayByAddress("SolvingTwoEqs/plus_Sonya_Eng"));
            yield return (VoiceSpeaker.PlayVoiceNumberAndWait((Res2Y1Val).ToString()));
            yield return (VoiceSpeaker.PlayByAddress("SolvingTwoEqs/equal_Sonya_Eng"));
            yield return (VoiceSpeaker.PlayVoiceNumberAndWait((Res2Y1Val + Res1Y2Val).ToString()));
            Answer.text += " = " + (Res2Y1Val + Res1Y2Val) + '\n';


        }
        else if (index == 8)
        {
            yield return (VoiceSpeaker.PlayByAddress("SolvingTwoEqs/send_Sonya_Eng"));
            yield return (VoiceSpeaker.PlayVoiceNumberAndWait(((X1Y2Val) + (X2Y1Val)).ToString()));
            yield return (VoiceSpeaker.PlayByAddress("SolvingTwoEqs/To the other side_Sonya_Eng"));

            Answer.text += "   x = " + (Res1Y2Val + Res2Y1Val).ToString() + " ÷ " + ((X1Y2Val) + (X2Y1Val)).ToString() + '\n';

        }
        else if (index == 9)
        {
            yield return (VoiceSpeaker.PlayByAddress("SolvingTwoEqs/so_Sonya_Eng"));
            yield return (VoiceSpeaker.PlayByAddress("SolvingTwoEqs/x_Sonya_Eng"));
            yield return (VoiceSpeaker.PlayByAddress("SolvingTwoEqs/equal_Sonya_Eng"));

            X1Val = MathF.Round(((Res1Y2Val) + (Res2Y1Val)) / ((X1Y2Val) + (X2Y1Val)), 2);

            yield return (VoiceSpeaker.PlayVoiceNumberAndWait(X1Val.ToString()));

            Answer.text += $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.gray)}>{"              x = " + X1Val}</color>" + '\n';
            XFinalResult = X1Val;
        }
        else if (index == 10)
        {
            yield return (VoiceSpeaker.PlayByAddress("SolvingTwoEqs/by replacing_Sonya_Eng"));
            yield return (VoiceSpeaker.PlayByAddress("SolvingTwoEqs/x_Sonya_Eng"));
            yield return (VoiceSpeaker.PlayByAddress("SolvingTwoEqs/In the second equation_Sonya_Eng"));
            yield return (VoiceSpeaker.PlayByAddress("SolvingTwoEqs/so_Sonya_Eng"));
            yield return (VoiceSpeaker.PlayVoiceNumberAndWait(X2Val.ToString()));
            yield return (VoiceSpeaker.PlayByAddress("SolvingTwoEqs/time_Sonya_Eng"));
            yield return (VoiceSpeaker.PlayVoiceNumberAndWait(XFinalResult.ToString()));

            Answer.text += "   " + X2Val.ToString() + " × (" + XFinalResult + ')';

            yield return (VoiceSpeaker.PlayByAddress("SolvingTwoEqs/plus_Sonya_Eng"));
            Answer.text += " + ";

            yield return (VoiceSpeaker.PlayVoiceNumberAndWait(Y2Val.ToString()));
            yield return (VoiceSpeaker.PlayByAddress("SolvingTwoEqs/y_Sonya_Eng"));

            Answer.text += Y2Val.ToString() + "y ";

            yield return (VoiceSpeaker.PlayByAddress("SolvingTwoEqs/equal_Sonya_Eng"));
            Answer.text += " = ";

            yield return (VoiceSpeaker.PlayVoiceNumberAndWait(ResFloat2.ToString()));
            Answer.text += ResFloat2.ToString() + '\n';

        }
        else if (index == 11)
        {
            yield return (VoiceSpeaker.PlayByAddress("SolvingTwoEqs/so_Sonya_Eng"));

            yield return (VoiceSpeaker.PlayVoiceNumberAndWait((XFinalResult * X2Val).ToString()));
            Answer.text += "   " + (XFinalResult * X2Val).ToString();

            if (Y2Val > 0)
            {
                yield return (VoiceSpeaker.PlayByAddress("SolvingTwoEqs/plus_Sonya_Eng"));
                Answer.text += " + ";
            }

            yield return (VoiceSpeaker.PlayVoiceNumberAndWait(Y2Val.ToString()));
            Answer.text += Y2Val.ToString();

            yield return (VoiceSpeaker.PlayByAddress("SolvingTwoEqs/y_Sonya_Eng"));
            Answer.text += "y";





            yield return (VoiceSpeaker.PlayByAddress("SolvingTwoEqs/equal_Sonya_Eng"));
            Answer.text += " = ";

            yield return (VoiceSpeaker.PlayVoiceNumberAndWait(ResFloat2.ToString()));
            Answer.text += ResFloat2.ToString() + '\n';

        }
        else if (index == 12)
        {
            yield return new WaitForSeconds(0.5f);
            if ((X2Val * XFinalResult) < 0)
                Answer.text += "   " + Y2.text + "y " + (X2Val * XFinalResult) + " = " + Res2.text;
            else
                Answer.text += "   " + Y2.text + "y +" + (X2Val * XFinalResult) + " = " + Res2.text;

            yield return (VoiceSpeaker.PlayByAddress("SolvingTwoEqs/send_Sonya_Eng"));
            yield return (VoiceSpeaker.PlayVoiceNumberAndWait((X2Val * XFinalResult).ToString()));
            yield return (VoiceSpeaker.PlayByAddress("SolvingTwoEqs/To the other side_Sonya_Eng"));

            X2Val = (float)Math.Round((-X2Val * XFinalResult), 2);

            if ((X2Val * XFinalResult) < 0)
                Answer.text += $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.gray)}>{" + " + X2Val.ToString()}</color>" + '\n';
            else
                Answer.text += $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.gray)}>{" " + X2Val.ToString()}</color>" + '\n';

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
                    worldPosition.x += 40;
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
            yield return (VoiceSpeaker.PlayByAddress("SolvingTwoEqs/so_Sonya_Eng"));

            yield return (VoiceSpeaker.PlayVoiceNumberAndWait(Y2Val.ToString()));
            Answer.text += "   " + Y2Val.ToString();

            yield return (VoiceSpeaker.PlayByAddress("SolvingTwoEqs/y_Sonya_Eng"));
            Answer.text += "y";

            yield return (VoiceSpeaker.PlayByAddress("SolvingTwoEqs/equal_Sonya_Eng"));
            Answer.text += " = ";

            X2Val += ResFloat2;

            yield return (VoiceSpeaker.PlayVoiceNumberAndWait(X2Val.ToString()));
            Answer.text += X2Val.ToString() + '\n';
        }
        else if (index == 14)
        {
            yield return (VoiceSpeaker.PlayByAddress("SolvingTwoEqs/then_Sonya_Eng"));
            yield return (VoiceSpeaker.PlayByAddress("SolvingTwoEqs/send_Sonya_Eng"));
            yield return (VoiceSpeaker.PlayVoiceNumberAndWait((Y2Val).ToString()));
            yield return (VoiceSpeaker.PlayByAddress("SolvingTwoEqs/To the other side_Sonya_Eng"));
            Answer.text += "   y = " + X2Val + " ÷ " + Y2Val + '\n';
        }
        else if (index == 15)
        {
            yield return (VoiceSpeaker.PlayByAddress("SolvingTwoEqs/so_Sonya_Eng"));
            yield return (VoiceSpeaker.PlayByAddress("SolvingTwoEqs/y_Sonya_Eng"));
            yield return (VoiceSpeaker.PlayByAddress("SolvingTwoEqs/equal_Sonya_Eng"));
            Y2Val = MathF.Round((float)X2Val / Y2Val);
            yield return (VoiceSpeaker.PlayVoiceNumberAndWait((Y2Val).ToString()));
            Answer.text += $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.gray)}>{"              y " + "= " + (Y2Val)}</color>" + '\n';
        }
        else if (index == 16)
        {
            Answer.text += "   " + "{" + '(' + XFinalResult + ',' + Y2Val + ')' + '}' + '\n';
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
        voiceClips = Resources.LoadAll<AudioClip>("Solving2EqsSounds");
    }
    public IEnumerator PlayVoiceClipAndWait(int index)
    {
        audioSource.clip = voiceClips[index];
        clipIndex++;
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
    }
    public List<string> SolveStepByStep()
    {
        ResetValues();
        Answer.text = "";
        LoadAllAudioClips();
        GameObject audioObject = new GameObject("VoiceAudioSource");
        audioSource = audioObject.AddComponent<AudioSource>();
        VoiceSpeaker.NumbersName = "EngNums";
        VoiceSpeaker.voiceClipsName = "SolvingTwoEqs";
        VoiceSpeaker.PointPlace = "SolvingTwoEqs/POINT_Sonya_Eng";
        VoiceSpeaker.LoadAllAudioClips();
        // Assuming WritingSteps is a list or collection of strings
        List<string> WritingSteps = new List<string>
        {
            "!!  " + X1Sign + X1Val + "X " + Y1Sign.ToString() + " " + Y1Val + "Y = " + ResFloat1 + "<line-height=0.5em> \n\n</line-height>",
            "!!  " + X2Sign + X2Val + "X " + Y2Sign.ToString() + " " + Y2Val + "Y = " + ResFloat2 + "\n"
        };
        if (MathF.Abs(Y1Val) != MathF.Abs(Y2Val))
        {
            if (Y1Val % Y2Val == 0 && Y1Val > Y2Val)
            {

                WritingSteps.Add("  " + X1Sign + X1Val + "X |" + Y1Sign + Y1Val + " Y = |" + ResFloat1 + "<line-height=0.5em> \n\n</line-height>");
                if (Y2Val * (-Y1Val / Y2Val) < 0)
                    WritingSteps.Add("  " + X2Sign + X2Val * (-Y1Val / Y2Val) + "X |" + Y2Val * (-Y1Val / Y2Val) + " Y = |" + ResFloat2 * (-Y1Val / Y2Val) + "\n");
                else
                    WritingSteps.Add("  " + X2Sign + X2Val * (-Y1Val / Y2Val) + "X |+" + Y2Val * (-Y1Val / Y2Val) + " Y = |" + ResFloat2 * (-Y1Val / Y2Val) + "\n");
                X2Val *= -(Y1Val / Y2Val);
                ResFloat2 *= -(Y1Val / Y2Val);
                Y2Val *= -(Y1Val / Y2Val);
            }
            else
            {
                if (Y1Val * (-Y2Val / Y1Val) < 0)
                {
                    WritingSteps.Add("  " + X1Sign + X1Val * (-Y2Val / Y1Val) + "X |" + Y1Val * (-Y2Val / Y1Val) + " Y = |" + ResFloat1 * (-Y2Val / Y1Val) + "<line-height=0.5em> \n\n</line-height>");
                }
                else
                {
                    WritingSteps.Add("  " + X1Sign + X1Val * (-Y2Val / Y1Val) + "X |+" + Y1Val * (-Y2Val / Y1Val) + " Y = |" + ResFloat1 * (-Y2Val / Y1Val) + "<line-height=0.5em> \n\n</line-height>");
                }
                WritingSteps.Add("  " + X2Sign + X2Val + "X |" + Y2Sign + Y2Val + " Y = |" + ResFloat2 + "\n");
                X1Val *= -(Y2Val / Y1Val);
                ResFloat1 *= -(Y2Val / Y1Val);
                Y1Val *= -(Y2Val / Y1Val);
            }

        }
        else
        {
            WritingSteps.Add("");
            WritingSteps.Add("");
        }
        WritingSteps.Add("  " + X1Sign + X1Val + "X " + " " + Y1Sign.ToString() + " " + Y1Val + "Y = " + ResFloat1 + "<line-height=0.5em> \n</line-height>");
        WritingSteps.Add("<line-height=0.5em>+\n</line-height>");
        WritingSteps.Add("  " + X2Sign + X2Val + "X " + " " + Y2Sign.ToString() + " " + Y2Val + "Y = " + ResFloat2 + '\n');
        ResetValues();

        WritingSteps.Add("   " + (X1Val + X2Val) + "x = " + ResSubRes + " " + (ResFloat1 + ResFloat2) + '\n');
        WritingSteps.Add("   x = " + (ResFloat1 + ResFloat2) + " ÷ " + (X1Val + X2Val) + '\n');
        WritingSteps.Add($"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.gray)}>{"              x = " + (ResFloat1 + ResFloat2) / (X1Val + X2Val)}</color>" + '\n');
        if (Y2Val < 0)
        {
            WritingSteps.Add("   " + X2Val + " × " + '(' + " " + XFinalResult + ") " + Y2Val + "y " + " = " + ResFloat2 + " -->(Sec eq) \n");
            WritingSteps.Add("   " + (X2Val * XFinalResult) + " " + Y2Val.ToString() + "y " + " = " + ResFloat2 + "\n");
        }
        else
        {
            WritingSteps.Add("   " + X2Val + " × " + '(' + XFinalResult + ") + " + Y2Val + "y " + " = " + ResFloat2 + " -->(Sec eq) \n");
            WritingSteps.Add("   " + (X2Val * XFinalResult) + " + " + Y2Val.ToString() + "y " + " = " + ResFloat2 + "\n");
        }

        if ((X2Val * X2Val) > 0)
            WritingSteps.Add("   " + Y2Val.ToString() + "y " + " = " + ResFloat2 + " - " + (X2Val * XFinalResult) + "\n");
        else
            WritingSteps.Add("   " + Y2Val.ToString() + "y " + " = " + ResFloat2 + " + " + Mathf.Abs(X2Val * XFinalResult) + "\n");

        WritingSteps.Add("   " + Y2Val.ToString() + "y " + " = " + (ResFloat2 - (X2Val * XFinalResult)) + "\n");
        WritingSteps.Add("   " + "y " + " = " + (ResFloat2 - (X2Val * XFinalResult)) + " ÷ " + Y2Val + "\n");
        WritingSteps.Add($"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.gray)}>{"   " + "y " + " = " + ((ResFloat2 - (X2Val * XFinalResult)) / Y2Val)}</color>" + "\n");
        WritingSteps.Add("   " + "{" + '(' + XFinalResult + ',' + ((ResFloat2 - (X2Val * XFinalResult)) / Y2Val) + ')' + '}');
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
        Circle.SetActive(false);
        Arrow.SetActive(false);
        if (Y1Val < 0)
            Y1Sign = "";

        if (Y2Val < 0)
            Y2Sign = "";

        if (X1Val > 0)
            X1Sign = " ";

        if (X2Val > 0)
            X2Sign = " ";

        clipIndex = 0;
        ResSubRes = "+"; X1Sign = ""; X2Sign = "";

        DivisionRes = 0;
        notEqual = true;
    }
}

