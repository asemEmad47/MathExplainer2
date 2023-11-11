using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;

public class PointsScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Answer;
    [SerializeField] private GameObject X1Message;
    [SerializeField] private GameObject Y1Message;
    [SerializeField] private TMP_InputField X1Axis;
    [SerializeField] private TMP_InputField Y1Axis;
    private string numeratorStr = "y2-y1";
    private string DenoStr = "x2-x1";
    private float slope;
    public GameObject go;
    public bool ValidateInputs(string X1, string y1)
    {
        bool isX1Number = float.TryParse(X1, out _);
        bool isY1Number = float.TryParse(y1, out _);

        if (isX1Number&&isY1Number)
        {
            X1Message.active = false;
            Y1Message.active = false;
            Answer.text = "";
            return true;
        }
        else
        {
            if (!isX1Number)
            {
                X1Message.active = true;
            }
            else
                X1Message.active = false;

            if (!isY1Number)
            {
                Y1Message.active = true;
            }
            else
                Y1Message.active = false;    

            Answer.text = "";
            return false;
        }
    }
    public void solveQuestion()
    {
        if(ValidateInputs(X1Axis.text, Y1Axis.text))
        {
            string Equation = $"<sup>{numeratorStr}</sup>/<sub>{DenoStr}</sub>";
            Answer.text = "St. Line intersect two axes at" + "(" + X1Axis.text + "," + "0" + ")" + "," + "(" + "0" + "," + Y1Axis.text + ")"+'\n';
            Answer.text += "m = " + Equation+'\n';
            Answer.text+= "m = "+$"<sup>{Y1Axis.text + " - 0"}</sup>/<sub>{"0 - " + X1Axis.text}</sub>"+'\n';
            slope = (float.Parse(Y1Axis.text) - 0) / (0 - float.Parse(X1Axis.text));
            slope = (float)Math.Round(slope, 2);
            Solve solve = go.GetComponent<Solve>();
            solve.SetSolve(X1Message, Y1Message, X1Message, X1Axis, Y1Axis, slope.ToString(), Answer, true,2);
            solve.SolveQuestion();
        }
    }
    public void ExplainQuestion()
    {
        if (ValidateInputs(X1Axis.text, Y1Axis.text))
        {
            string Equation = $"<sup>{numeratorStr}</sup>/<sub>{DenoStr}</sub>";
            slope = (float.Parse(Y1Axis.text) ) /(0 - float.Parse(X1Axis.text));
            slope = (float)Math.Round(slope, 2);
            Solve solve = go.GetComponent<Solve>();
            solve.SetSolve(X1Message, Y1Message, X1Message, X1Axis, Y1Axis, slope.ToString(), Answer, true,2);
            List<string> VoiceSteps = new List<string> {
                "the point which intersects x axis is" + X1Axis.text + " and 0" + "the point which intersects y axis is 0 and" + Y1Axis.text ,
                "first put the equation of the slope which is y2 minus y1 over x2 minus x1",
                    "now subtract "  + float.Parse(Y1Axis.text) + " minus " + 0 + " then divide the result by " + 0 + " minus " + float.Parse(X1Axis.text),
            };
            List<string> WritingSteps = new List<string> {
                "St. Line intersect two axes at"+"("+X1Axis.text+ ","+"0"+")"+","+"("+"0"+"," + Y1Axis.text+")"+'\n',
                "m = " + Equation + '\n',
                "m = "+$"<sup>{float.Parse(Y1Axis.text) + " - " + 0}</sup>/<sub>{0 + " - " + float.Parse(X1Axis.text)}</sub>" + '\n',
            };
            solve.SetExtraDetails(VoiceSteps, WritingSteps);
            solve.SolveStepByStep();
        }
    }
}
