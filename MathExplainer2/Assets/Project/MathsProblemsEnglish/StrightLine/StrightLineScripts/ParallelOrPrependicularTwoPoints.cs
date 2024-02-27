using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Answer;
    [SerializeField] private TMP_InputField XAxis;
    [SerializeField] private TMP_InputField YAxis;
    [SerializeField] private GameObject AMessage;
    [SerializeField] private GameObject BMessage;
    [SerializeField] private GameObject CMessage;
    [SerializeField] private TMP_InputField APoint;
    [SerializeField] private TMP_InputField BPoint;
    [SerializeField] private TMP_InputField CPoint;
    [SerializeField] private TMP_InputField DPoint;
    private string numeratorStr = "y2-y1";
    private string DenoStr = "x2-x1";
    [SerializeField] private int type;
    private float slope;
    public GameObject go;
    public bool ValidateInputs(string X1, string y1, string a, string b, string c , string d)
    {
        bool isX1Number = float.TryParse(X1, out _);
        bool isY1Number = float.TryParse(y1, out _);
        bool isANumber = float.TryParse(a, out _);
        bool isBNumber = float.TryParse(b, out _);
        bool isCNumber = float.TryParse(c, out _);
        bool isDNumber = float.TryParse(d, out _);

        if (isX1Number && isY1Number && isANumber && isBNumber && isCNumber)
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

            if (!isANumber||isBNumber)
            {
                BMessage.active = true;
            }
            else
                BMessage.active = false;
            if (!isCNumber||!isDNumber)
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
        if (ValidateInputs(XAxis.text, YAxis.text, APoint.text, BPoint.text, CPoint.text,DPoint.text))
        {
            string Equation = $"<sup>{numeratorStr}</sup>/<sub>{DenoStr}</sub>";
            Answer.text += "m1 = " + Equation + '\n';
            Answer.text += "m = " + $"<sup>{DPoint.text + " - " + BPoint.text}</sup>/<sub>{CPoint.text +"-"+ APoint.text}</sub>" + '\n';
            slope = (float.Parse(DPoint.text)- float.Parse(BPoint.text)) / (float.Parse(CPoint.text) - float.Parse(APoint.text));
            slope = (float)Math.Round(slope, 2);
            if (type == 1)
            {
                Answer.text += "since l1 perpendicular to l2 , m2 = " + $"<sup>-1</sup>/<sub>m1</sub>" + " = " + $"<sup>-1</sup>/<sub>{slope}</sub>" + '\n';
                slope = -1 / slope;
            }
            else
            {
                Answer.text += "since l1 // l2 , m1 = m2 = " + slope + '\n';
            }
            Solve solve = go.GetComponent<Solve>();
            //solve.SetSolve(AMessage, BMessage, CMessage, XAxis, YAxis, slope.ToString(), Answer, true, 0);
            solve.SolveQuestion();
        }
    }
    public void ExplainQuestion()
    {
        if ((ValidateInputs(XAxis.text, YAxis.text, APoint.text, BPoint.text, CPoint.text, DPoint.text)))
        {
            string Equation = $"<sup>{numeratorStr}</sup>/<sub>{DenoStr}</sub>";
            slope = (float.Parse(DPoint.text) - float.Parse(BPoint.text)) / (float.Parse(CPoint.text) - float.Parse(APoint.text));
            slope = (float)Math.Round(slope, 2);
            Solve solve = go.GetComponent<Solve>();
            List<string> VoiceSteps = new List<string> {
                "first put the equation of the slope which is y2 minus y1 over x2 minus x1",
                "now subtract " + float.Parse(DPoint.text) + " minus " + float.Parse(BPoint.text) + " then divide the result by " + float.Parse(CPoint.text) + " minus " + float.Parse(APoint.text),
                "Since the two straight lines are parallel, their slopes are the same which equals"+slope,
            };
            List<string> WritingSteps = new List<string> {
                "m1 = " + Equation + '\n',
                "m = " + $"<sup>{DPoint.text + " - " + BPoint.text}</sup>/<sub>{CPoint.text +"-"+ APoint.text}</sub>" + '\n',
                "since l1 // l2 , m1 = m2 = " +slope +'\n',
            };
            if (type == 1)
            {
                VoiceSteps[2] = "Since the two straight lines are perpendicular, so m2 = negative 1 over m1 which equals" + slope;
                WritingSteps[2] = "since l1 perpendicular to l2 , m2 = " + $"<sup>-1</sup>/<sub>m1</sub>" + " = " + $"<sup>-1</sup>/<sub>{slope}</sub>" + '\n';
                slope = -1 / slope;
            }
            //solve.SetSolve(AMessage, BMessage, CMessage, XAxis, YAxis, slope.ToString(), Answer, true, 0);
            //solve.SetExtraDetails(VoiceSteps, WritingSteps);
            //solve.SolveStepByStep();
        }
    }
}
