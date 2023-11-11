using SpeechLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class Q14 : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Answer;
    [SerializeField] private GameObject AMessage;
    [SerializeField] private GameObject BMessage;
    [SerializeField] private GameObject CMessage;
    [SerializeField] private TMP_InputField X1Point;
    [SerializeField] private TMP_InputField Y1Point;
    [SerializeField] private TMP_InputField X2Point;
    [SerializeField] private TMP_InputField Y2Point;    
    [SerializeField] private TMP_InputField X3Point;
    [SerializeField] private TMP_InputField Y3Point;

    private string numeratorStr = "y2-y1";
    private string DenoStr = "x2-x1";
    private float slope;
    public GameObject go;
    public bool ValidateInputs(string X1, string y1, string X2, string Y2, string X3, string Y3)
    {
        bool isX1Number = float.TryParse(X1, out _);
        bool isY1Number = float.TryParse(y1, out _);
        bool isX2Number = float.TryParse(X2, out _);
        bool isY2Number = float.TryParse(Y2, out _);  
        bool isX3Number = float.TryParse(X3, out _);
        bool isY3Number = float.TryParse(Y3, out _);

        if (isX1Number && isY1Number && isX2Number && isY2Number && isX3Number && isY3Number)
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
            if (!isX3Number || !isY3Number)
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
        if (ValidateInputs(X1Point.text, Y1Point.text, X2Point.text, Y2Point.text, X3Point.text, Y3Point.text))
        {
            float neo = (float.Parse(Y2Point.text) + float.Parse(Y3Point.text)) / 2;
            float Deno = (float.Parse(X2Point.text) + float.Parse(X3Point.text)) / 2;
            string Equation = $"<sup>{numeratorStr}</sup>/<sub>{DenoStr}</sub>";
            Answer.text = "Mid BC = ("+$"<sup>{Y2Point.text + " + " + Y3Point.text}</sup>/<sub>2</sub>"+ $"<sup>{X2Point.text + " + " + X3Point.text}</sup>/<sub>2</sub>" + ")"+"= "+ "("+neo +","+Deno + ")" +'\n';
            Answer.text += "m = " + Equation + '\n';
            Answer.text += "m = " + $"<sup>{neo + " - " + Y1Point.text}</sup>/<sub>{Deno + " - " + X1Point.text}</sub>" + '\n';
            slope = (neo - float.Parse(Y1Point.text)) / (Deno - float.Parse(X1Point.text));
            slope = (float)Math.Round(slope, 2);
            String X1Temp = X1Point.text , Y1Temp = Y1Point.text ;
            X1Point.text = Deno.ToString();
            Y1Point.text = neo.ToString();
            Solve solve = go.GetComponent<Solve>();
            solve.SetSolve(AMessage, BMessage, CMessage, X1Point, Y1Point, slope.ToString(), Answer, true, 1);
            solve.SolveQuestion();
            X1Point.text =X1Temp;
            Y1Point.text = Y1Temp;
        }
    }
    public void SolveStepByStep()
    {
        if (ValidateInputs(X1Point.text, Y1Point.text, X2Point.text, Y2Point.text, X3Point.text, Y3Point.text))
        {
            float neo = (float.Parse(Y2Point.text) + float.Parse(Y3Point.text)) / 2;
            float Deno = (float.Parse(X2Point.text) + float.Parse(X3Point.text)) / 2;
            string Equation = $"<sup>{numeratorStr}</sup>/<sub>{DenoStr}</sub>";

            List<string> VoiceSteps = new List<string> {
                "First git the mid point of "+"   B   " + "   c   "+"which equals to "+ "   y   "+1 + "plus" + "    y   "+2+"   over 2"  +"  comma  "+ "   x    "+1+"plus"+"   x   "+"2",
                "Second put the equation of the slope which is y2 minus y1 over x2 minus x1",
                    "now subtract " + neo + " minus " + float.Parse(Y1Point.text) + " then divide the result by " + Deno + " minus " + float.Parse(X1Point.text),
            };
            List<string> WritingSteps = new List<string> {
                "Mid BC = (" + $"<sup>{Y2Point.text + " + " + Y3Point.text}</sup>/<sub>2</sub>" + $"<sup>{X2Point.text + " + " + X3Point.text}</sup>/<sub>2</sub>" + ")" + "= " + "(" + neo + "," + Deno + ")" + '\n',
                "m = " + Equation + '\n',
                "m = " + $"<sup>{neo + " - " + Y1Point.text}</sup>/<sub>{Deno + " - " + X1Point.text}</sub>" + '\n',
            };
            slope = (neo - float.Parse(Y1Point.text)) / (Deno - float.Parse(X1Point.text));
            slope = (float)Math.Round(slope, 2);
            String.Format("{0:0.00}", slope);
            String X1Temp = X1Point.text, Y1Temp = Y1Point.text;
            X1Point.text = Deno.ToString();
            Y1Point.text = neo.ToString();

            Solve solve = go.GetComponent<Solve>();
            solve.SetSolve(AMessage, BMessage, CMessage, X1Point, Y1Point, slope.ToString(), Answer, true, 1);
            solve.SetExtraDetails(VoiceSteps, WritingSteps);
            solve.SolveStepByStep();
        }
    }

}
