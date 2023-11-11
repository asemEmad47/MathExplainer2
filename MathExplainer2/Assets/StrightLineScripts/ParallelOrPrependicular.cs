using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;

public class ParallelOrPrependicular : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Answer;
    [SerializeField] private TMP_InputField XAxis;
    [SerializeField] private TMP_InputField YAxis;
    [SerializeField] private GameObject AMessage;
    [SerializeField] private GameObject BMessage;
    [SerializeField] private GameObject CMessage;
    [SerializeField] private GameObject PointMessage;
    [SerializeField] private TMP_InputField APoint;
    [SerializeField] private TMP_InputField BPoint;
    [SerializeField] private TMP_InputField CPoint;
    [SerializeField] private int type;
    private float slope;
    public GameObject go;
    public bool ValidateInputs(string X1, string y1 , string a , string b , string c)
    {
        bool isX1Number = float.TryParse(X1, out _);
        bool isY1Number = float.TryParse(y1, out _);
        bool isANumber = float.TryParse(a, out _);
        bool isBNumber = float.TryParse(b, out _);
        bool isCNumber = float.TryParse(c, out _);

        if (isX1Number && isY1Number&& isANumber&& isBNumber&& isCNumber)
        {
            AMessage.active = false;
            BMessage.active = false;        
            CMessage.active = false;        
            PointMessage.active = false;
            Answer.text = "";
            return true;
        }
        else
        {
            if (!isX1Number || !isY1Number)
            {
                PointMessage.active = true;
            }
            else
                PointMessage.active = false;

            if (!isANumber)
            {
                AMessage.active =true;
            }
            else
                AMessage.active = false;      
            if (!isBNumber)
            {
                BMessage.active =true;
            }
            else
                BMessage.active = false;         
            if (!isCNumber)
            {
                CMessage.active =true;
            }
            else
                CMessage.active = false;

            Answer.text = "";
            return false;
        }
    }
    public void solveQuestion()
    {
        if (ValidateInputs(XAxis.text, YAxis.text,APoint.text,BPoint.text,CPoint.text))
        {
            string Equation = $"<sup>-a</sup>/<sub>b</sub>";
            Answer.text += "m1 = " + Equation + '\n';
            Answer.text += "m1 = " + $"<sup>{"-" + APoint.text}</sup>/<sub>{BPoint.text}</sub>" + '\n';
            slope = (-float.Parse(APoint.text)) / (float.Parse(BPoint.text));
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
            solve.SetSolve(AMessage, BMessage, PointMessage, XAxis, YAxis, slope.ToString(), Answer, true, 0);
            solve.SolveQuestion();
        }
    }
    public void ExplainQuestion()
    {
        if ((ValidateInputs(XAxis.text, YAxis.text, APoint.text, BPoint.text, CPoint.text)))
        {
            string Equation = $"<sup>-a</sup>/<sub>b</sub>";
            slope = (-float.Parse(APoint.text)) / (float.Parse(BPoint.text));
            slope = (float)Math.Round(slope, 2);
            Solve solve = go.GetComponent<Solve>();
            List<string> VoiceSteps = new List<string> {
                "first put the equation of the slope which is negative "+"A" +"over b",
                "m1 equals negative"+ APoint.text+"over"+BPoint.text,
                "Since the two straight lines are parallel, their slopes are the same which equals"+slope,  
            };
            List<string> WritingSteps = new List<string> {
                "m1 = " + Equation + '\n',
                "m1 = " + $"<sup>{"-"+APoint.text}</sup>/<sub>{BPoint.text}</sub>" + '\n',
                "m1 = m2 = "+slope +'\n',
            };
            if (type == 1)
            {
                VoiceSteps[2] = "Since the two straight lines are perpendicular, so m2 = negative 1 over m1 which equals" + slope;
                WritingSteps[2] = "since l1 perpendicular to l2 , m2 = " + $"<sup>-1</sup>/<sub>m1</sub>" + " = " + $"<sup>-1</sup>/<sub>{slope}</sub>" + '\n';
                slope = -1 / slope;
            }
            solve.SetSolve(AMessage, BMessage, PointMessage, XAxis, YAxis, slope.ToString(), Answer, true, 0);
            solve.SetExtraDetails(VoiceSteps, WritingSteps);
            solve.SolveStepByStep();
        }
    }
}
