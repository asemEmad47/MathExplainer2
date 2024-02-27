using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class FractionCalculator
{
    // Function for multiplying a fraction by an integer
    public static bool IsNegative = false;
    public static void MultiplyFractionByInteger(ref int numerator, ref int denominator, int multiplier)
    {
        numerator *= multiplier;
        SimplifyFraction(ref numerator, ref denominator);
    }

    // Function for adding two fractions
    public static void AddFractions(ref int numerator1, ref int denominator1, int numerator2, int denominator2)
    {
        numerator1 = numerator1 * denominator2 + numerator2 * denominator1;
        denominator1 *= denominator2;
        SimplifyFraction(ref numerator1, ref denominator1);
    }

    // Function for subtracting one fraction from another
    public static void SubtractFractions(int numerator1, int denominator1, ref int numerator2, ref int denominator2)
    {
        numerator2 = numerator1 * denominator2 - numerator2 * denominator1;
        denominator2 *= denominator1;
        SimplifyFraction(ref numerator2, ref denominator2);
    }

    // Function for simplifying a fraction
    public static void SimplifyFraction(ref int numerator, ref int denominator)
    {
        int gcd = GCD(numerator, denominator);
        numerator /= gcd;
        denominator /= gcd;
    }

    // Helper function to calculate the greatest common divisor (GCD)
    private static int GCD(int a, int b)
    {
        while (b != 0)
        {
            int temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }
    public static String GetFrec(bool FromSolve, string PrevLine, int numeratorValue, int denominatorValue, String Extra = "", int anotherNumerator = 0, int AnotherDeno = 0, bool WillBeColored = false)
    {
        if (numeratorValue < 0)
        {
            numeratorValue = -numeratorValue;
            StringBuilder Negative = new StringBuilder(PrevLine);
            IsNegative = true;
            PrevLine = Negative.ToString();

        }
        if (denominatorValue < 0)
        {
            denominatorValue = -denominatorValue;
            StringBuilder Negative = new StringBuilder(PrevLine);
            IsNegative = true;
            PrevLine = Negative.ToString();
        }
        int MoreExstraSpaces = 0;
        if (WillBeColored)
            MoreExstraSpaces +=10;
        String DenoSpaces = "", NueSpaces = "", ExtraSpaces = "", coloredNue = anotherNumerator.ToString(), coloredDeno = AnotherDeno.ToString();
        for (int i = 0; i < PrevLine.Length + 3 + Math.Ceiling(0.5 * PrevLine.Length); i++)
        {
            if (denominatorValue < 10)
                DenoSpaces += " ";
            else
            {
                if (i < PrevLine.Length + 1 + Math.Ceiling(0.5 * PrevLine.Length))
                    DenoSpaces += " ";
                else
                    break;
            }
        }
        for (int i = 0; i < PrevLine.Length + 3 + Math.Ceiling(0.5 * PrevLine.Length); i++)
        {
            if (numeratorValue < 10)
                NueSpaces += " ";
            else
            {
                if (i < PrevLine.Length + 1 + Math.Ceiling(0.5 * PrevLine.Length))
                    NueSpaces += " ";
                else
                    break;
            }
        }
        if (denominatorValue < 10 && numeratorValue > 10)
            DenoSpaces = DenoSpaces.Substring(1, DenoSpaces.Length - 1);
        else if (denominatorValue > 10 && numeratorValue < 10)
            NueSpaces = NueSpaces.Substring(1, NueSpaces.Length - 1);

        for (int i = 0; i < Extra.Length + 3 + Math.Ceiling(0.5 * Extra.Length) + MoreExstraSpaces; i++)
        {
            ExtraSpaces += " ";
        }
        if (WillBeColored)
        {
            coloredNue = $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>{anotherNumerator}</color>";
            if (AnotherDeno < 0)
            {
                AnotherDeno = -AnotherDeno;
            }
            coloredDeno = $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>{AnotherDeno}</color>";
        }
        if (FromSolve)
        {
            if (anotherNumerator != 0)
            {
                if (!WillBeColored)
                {
                    ExtraSpaces += "  ";
                    return $"<size=75%>{NueSpaces + numeratorValue + ExtraSpaces + coloredNue}<line-height=35%>\n<size=100%>{PrevLine}\u2015 {Extra}\u2015</size>\n{DenoSpaces + denominatorValue + ExtraSpaces + coloredDeno}</line-height></size>";

                }
                else
                {
                    if (IsNegative)
                    {
                        return $"<size=75%>{" " + NueSpaces + numeratorValue + ExtraSpaces + coloredNue}<line-height=35%>\n<size=100%>{PrevLine}\u2015{Extra + " "} + \u2015</size>\n{" " + DenoSpaces + denominatorValue + ExtraSpaces + coloredDeno}</line-height></size>";

                    }
                    else
                    {
                        return $"<size=75%>{" "+NueSpaces + numeratorValue + ExtraSpaces + coloredNue} <line-height=35%>\n<size=100%> {PrevLine} \u2015 {Extra + " "} -\u2015</size>\n{" "+ DenoSpaces + denominatorValue + ExtraSpaces + coloredDeno}</line-height></size>";

                    }
                }

            }
            else
            {
                if (!IsNegative)
                    return $"<size=85%>{NueSpaces + numeratorValue}<line-height=30%>\n<size=100%>{PrevLine}―{Extra}</size>\n{DenoSpaces + denominatorValue}</line-height></size>";
                else
                    return $"<size=85%>{NueSpaces + "   " + numeratorValue}<line-height=30%>\n<size=100%>{PrevLine + "- "}―{Extra}</size>\n{DenoSpaces + "   " + denominatorValue}</line-height></size>";
            }
        }
        else
        {
            if (anotherNumerator != 0)
            {
                if (!WillBeColored)
                {
                    if (IsNegative)
                        return $"<size=75%>{NueSpaces + numeratorValue + ExtraSpaces + coloredNue}<line-height=35%>\n<size=100%>{PrevLine}\u2015 {Extra}+'-'+\u2015</size>\n{DenoSpaces + denominatorValue + ExtraSpaces + "    " + coloredDeno}</line-height></size>";
                    else
                        return $"<size=75%>{NueSpaces + numeratorValue + ExtraSpaces + coloredNue}<line-height=35%>\n<size=100%>{PrevLine}\u2015 {Extra}\u2015</size>\n{DenoSpaces + denominatorValue + ExtraSpaces + coloredDeno}</line-height></size>";

                }
                else
                {

                    if (IsNegative)
                    {
                        return $"<size=60%>{" " + NueSpaces + numeratorValue + ExtraSpaces + coloredNue}<line-height=35%>\n<size=100%>{PrevLine}\u2015{Extra + " "} + \u2015</size>\n{" " + DenoSpaces + denominatorValue + ExtraSpaces + coloredDeno}</line-height></size>";

                    }
                    else
                    {
                        return $"<size=75%>{" " + NueSpaces + numeratorValue + ExtraSpaces + coloredNue}<line-height=35%>\n<size=100%>{PrevLine}\u2015{Extra + " "}-\u2015</size>\n{" " + DenoSpaces + denominatorValue + ExtraSpaces + coloredDeno}</line-height></size>";

                    }
                }

            }
            else
                return $"<size=85%>{NueSpaces + numeratorValue}<line-height=40%>\n<size=100%>{PrevLine}―{Extra}</size>\n{DenoSpaces + denominatorValue}</line-height></size>";
        }
    }
    public static String GetStringFrec(string PrevLine, string numerator, string denominator)
    {
        String NumOFSpaces = "";
        for (int i = 0; i < PrevLine.Length + 1 + Math.Ceiling(0.5 * PrevLine.Length); i++)
        {
            NumOFSpaces += " ";
        }
        return $"<size=85%>{NumOFSpaces + numerator}<line-height=35%>\n<size=100%>{PrevLine}\u2015\u2015</size>\n{NumOFSpaces + denominator}</line-height></size>";
    }
}
