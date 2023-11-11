using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;
using System;
using static UnityEditor.IMGUI.Controls.PrimitiveBoundsHandle;
using UnityEngine.UIElements;
using System.Net.Http;

public class TwoPointsScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Answer;
    [SerializeField] private GameObject Point1Message;
    [SerializeField] private GameObject Point2Message;
    [SerializeField] private TMP_InputField X1Axis;
    [SerializeField] private TMP_InputField X2Axis;
    [SerializeField] private TMP_InputField Y1Axis;
    [SerializeField] private TMP_InputField Y2Axis;
    private string numeratorStr = "y2|-y1|";
    private string DenoStr = "x2|-x1";
    private float slope;
    public GameObject go;
    public bool ValidateInputs(string X1, string y1, string X2, string y2)
    {
        bool isX1Number = float.TryParse(X1, out _);
        bool isY1Number = float.TryParse(y1, out _);
        bool isX2Number = float.TryParse(X2, out _);
        bool isy2Number = float.TryParse(y2, out _);

        if (isX1Number&&isX2Number&&isY1Number&&isy2Number)
        {
            Point1Message.active = false;
            Point2Message.active = false;
            Answer.text = "";
            return true;
        }
        else
        {
            if (!isX1Number && ! isY1Number)
            {
                Point1Message.active = true;
            }
            else
                Point1Message.active = false;
            if (!isX2Number && !isy2Number)
            {
                Point2Message.active = true;
            }
            else
                Point2Message.active = false;    
            Answer.text = "";
            return false;
        }
    }
    public void solveQuestion()
    {
        if(ValidateInputs(X1Axis.text, Y1Axis.text,X2Axis.text,Y2Axis.text))
        {
            string Equation = $"<size=9><sup>{numeratorStr}</sup>/<sub>{DenoStr}</sub></size>";
            Answer.text = "m = " + Equation+'\n';
            Answer.text+= "m = "+$"<sup>{Y2Axis.text + " - "+Y1Axis.text}</sup>/<sub>{X2Axis.text + " - " + X1Axis.text}</sub>"+'\n';
            slope = (float.Parse(Y2Axis.text) - float.Parse(Y1Axis.text)) / (float.Parse(X2Axis.text) - float.Parse(X1Axis.text));
            slope = (float)Math.Round(slope, 2);
            Solve solve = go.GetComponent<Solve>();
            solve.SetSolve(Point2Message, Point2Message, Point2Message, X1Axis, Y1Axis, slope.ToString(), Answer, true,1);
            solve.SolveQuestion();
        }
    }
    public void ExplainQuestion()
    {
        if (ValidateInputs(X1Axis.text, Y1Axis.text, X2Axis.text, Y2Axis.text))
        {
            string Equation = $"<size=9><sup>{numeratorStr}</sup>/<sub>{DenoStr}</sub></size>";
            slope = (float.Parse(Y2Axis.text) - float.Parse(Y1Axis.text)) / (float.Parse(X2Axis.text) - float.Parse(X1Axis.text));
            slope = (float)Math.Round(slope, 2);
            Solve solve = go.GetComponent<Solve>();
            solve.SetSolve(Point2Message, Point2Message, Point2Message, X1Axis, Y1Axis, slope.ToString(), Answer, true,1);
            List<string> VoiceSteps = new List<string> {
                "first put the equation of the slope which is m equals| y2 |minus y1 |over x2 | minus x1",
                    "now m equals |" + float.Parse(Y2Axis.text) + "| minus " + float.Parse(Y1Axis.text) + "| then divide the result by " + float.Parse(X2Axis.text) + "| minus " + float.Parse(X1Axis.text),
            };
            List<string> WritingSteps = new List<string> {
                "m = |" + Equation + '\n',
                "m = |"+$"<size=9><sup>{float.Parse(Y2Axis.text)}|-{float.Parse(Y1Axis.text)}|</sup>/<sub>{ float.Parse(X2Axis.text)}|-{float.Parse(X1Axis.text)}</sub></size>" + '\n',
            };
            solve.SetExtraDetails(VoiceSteps, WritingSteps);
            solve.SolveStepByStep();
        }
    }
}
