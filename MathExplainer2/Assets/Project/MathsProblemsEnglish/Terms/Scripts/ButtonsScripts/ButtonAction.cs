using TMPro;
using UnityEngine;
using System.Text.RegularExpressions;

public class ButtonAction : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    public static bool IsPower = false;
    public static bool IsFraction = false;
    public static bool IsUpper = false;
    public static bool IsLower = false;
    public static int CurrentSpaces = 3;
    public static int CurrentSpacesCounter = 0;
    public static int Cursor = 0;
    public static int FakeCursor = 0;
    public static int NumOfFractions = 0;

    
    int counter = 0;
    public void ZeroCLick()
    {
        WriteOnInput('0');
    }  
    public void OneCLick()
    {
        WriteOnInput('1');
    }
    public void TwoCLick()
    {
        WriteOnInput('2');
    }
    public void ThreeCLick()
    {
        WriteOnInput('3');
    }
    public void FourCLick()
    {
        WriteOnInput('4');
    }
    public void FiveCLick()
    {
        WriteOnInput('5');
    }
    public void SixCLick()
    {
        WriteOnInput('6');
    }
    public void SevenCLick()
    {
        WriteOnInput('7');
    }
    public void EightCLick()
    {
        WriteOnInput('8');
    }
    public void NineCLick()
    {
        WriteOnInput('9');
    }
    public void EraserClick()
    {
        if (inputField.text.Length > 0)
        {
            if (inputField.text.Length > 0 && Cursor > 0 && FakeCursor > 0)
            {
                bool Isfraction = false;
                if (Cursor > 0 && !inputField.text[Cursor-1].Equals('>'))
                {
                    if (inputField.text[Cursor-1].Equals('―'))
                        Isfraction = true;
                    inputField.text = inputField.text.Remove(Cursor - 1, 1);
                    Cursor--;
                    FakeCursor--;
                }
                if (Isfraction) 
                {
                    string FractionTillDash = inputField.text.Substring(0, Cursor);
                    int NumOfFractionsBefore = CountSubstringOccurrences(FractionTillDash, "―");
                    int NueIndex = inputField.text.IndexOf("%>") + 2;
                    int Fractioncounter = NumOfFractionsBefore;

                    while (inputField.text[NueIndex].Equals(' '))
                        NueIndex++;

                    while (Fractioncounter > 0) // getting the pos of nue
                    {
                        if (inputField.text[NueIndex].Equals(' '))
                        {
                            Fractioncounter--;
                            while (inputField.text[NueIndex].Equals(' '))
                                NueIndex++;
                            NueIndex--;
                        }
                        NueIndex++;
                    }

                    int DeletingPointer = 0;
                    while (!inputField.text[NueIndex].Equals('<') && !inputField.text[NueIndex].Equals(' ')) // deleting nue
                    {
                        inputField.text = inputField.text.Remove(NueIndex, 1);
                        DeletingPointer++;
                    }

                    Fractioncounter = NumOfFractionsBefore;
                    int DenoIndex = inputField.text.IndexOf("%>",NueIndex) + 2;
                    while (inputField.text[DenoIndex].Equals(' '))
                        DenoIndex++;

                    while (Fractioncounter > 0) // getting the pos of deno
                    {
                        if (inputField.text[DenoIndex].Equals(' '))
                        {
                            Fractioncounter--;
                            while (inputField.text[DenoIndex].Equals(' '))
                                DenoIndex++;
                            DenoIndex--;

                        }
                        DenoIndex++;
                    }

                    DeletingPointer = 0;
                    while (!inputField.text[DenoIndex].Equals('<') && !inputField.text[DenoIndex].Equals(' ')) // deleting deno
                    {
                        inputField.text = inputField.text.Remove(DenoIndex, 1);
                        DeletingPointer++;
                    }
                    NumOfFractions--;
                    if(NumOfFractions == 0)
                    {
                        int NewLineIndex = inputField.text.IndexOf('\n');
                        inputField.text = inputField.text.Substring(NewLineIndex+1);
                        inputField.text = inputField.text.Substring(0, inputField.text.IndexOf('\n'));
                        Cursor = inputField.text.Length - 1;
                        FakeCursor = Cursor;

                    }
                    else
                    {
                        Cursor--;
                        FakeCursor--;
                    }
                }
                else if (Cursor <= inputField.text.Length&& inputField.text.Length>0)
                {

                    if (inputField.text[Cursor-1].Equals('>')&&!inputField.text[Cursor - 2].Equals('%'))
                    {

                        if ( Cursor - 13 > 0 && (inputField.text[Cursor-13].Equals('>')))
                        {
                            Cursor -= 12;
                            inputField.text = inputField.text.Remove(Cursor, 7);
                            inputField.text = inputField.text.Remove(Cursor - 5, 5);
                            FakeCursor--;
                        }
                        else
                        {
                            inputField.text = inputField.text.Remove(Cursor-12, 12);
                            Cursor -= 12;
                        }
                        counter++;
                    }
                }
            }
            inputField.selectionAnchorPosition = FakeCursor;
            inputField.selectionFocusPosition = FakeCursor;
            inputField.ActivateInputField();
        }
    }
    public void DotClick()
    {
        WriteOnInput('.');
    }
    public void GraterThanClick()
    {
        WriteOnInput('>');
    }
    public void LessThanClick()
    {
        WriteOnInput('<');
    }
    public void EqualClick()
    {
        WriteOnInput('=');
    }
    public void PlusClick()
    {
        WriteOnInput('+');
    }
    public void MinusClick()
    {
        WriteOnInput('-');
    }
    public void MultiplyClick()
    {
        WriteOnInput('*');
    }
    public void DevideClick()
    {
        WriteOnInput('/');
    }
    public void DecimalClick()
    {
        WriteOnInput(',');
    }
    public void OpenBracketClick()
    {
        WriteOnInput('(');
    }
    public void CloseBracketClick()
    {
        WriteOnInput(')');
    }
    public void XClick()
    {
        WriteOnInput('x');
    }
    public void YClick()
    {
        WriteOnInput('y');
    }
    public void ZClick()
    {
        WriteOnInput('z');
    }
    public void EClick()
    {
        WriteOnInput('e');
    }    
    public void Pow2Click()
    {
        int CurrentPos = inputField.text.Length;
        inputField.text+="<sup>2</sup>";
        Cursor += (inputField.text.Length-CurrentPos);
        FakeCursor += 1;
    }
    public void PowXClick()
    {
        IsPower = true;
    }
    public void RootClick()
    {
        WriteOnInput('√');
    }
    public void AbsClick()
    {
        inputField.text += "||";

    }
    public void AClick()
    {
        WriteOnInput('a');
    }
    public void BClick()
    {
        WriteOnInput('b');
    }  
    public void CClick()
    {
        WriteOnInput('c');
    }
    public void MakeABC()
    {

        GameObject.Find("LeftButtons/abc").SetActive(false);
        GameObject.Find("KeyBoard/LeftButtons/XYZ").SetActive(true);
        GameObject.Find("KeyBoard/LeftButtons/ForthRow/a").SetActive(true);
        GameObject.Find("KeyBoard/LeftButtons/ForthRow/b").SetActive(true);
        GameObject.Find("KeyBoard/LeftButtons/ForthRow/c").SetActive(true); 
        GameObject.Find("KeyBoard/LeftButtons/ForthRow/x").SetActive(false);
        GameObject.Find("KeyBoard/LeftButtons/ForthRow/y").SetActive(false);
        GameObject.Find("KeyBoard/LeftButtons/ForthRow/z").SetActive(false);


    }
    public void Makexyz()
    {
        GameObject.Find("KeyBoard/LeftButtons/abc").SetActive(true);
        GameObject.Find("KeyBoard/LeftButtons/XYZ").SetActive(false);
        GameObject.Find("KeyBoard/LeftButtons/ForthRow/a").SetActive(false);
        GameObject.Find("KeyBoard/LeftButtons/ForthRow/b").SetActive(false);
        GameObject.Find("KeyBoard/LeftButtons/ForthRow/c").SetActive(false);
        GameObject.Find("KeyBoard/LeftButtons/ForthRow/x").SetActive(true);
        GameObject.Find("KeyBoard/LeftButtons/ForthRow/y").SetActive(true);
        GameObject.Find("KeyBoard/LeftButtons/ForthRow/z").SetActive(true);
    }
    public void UpClick()
    {
        int previousNewlineIndex = inputField.text.IndexOf("</size>");
        if (previousNewlineIndex >= 0 &&!IsUpper)
        {
            IsUpper = true;
            IsLower = false;
            int NumOFDenoDigits = inputField.text.IndexOf("<size=60%>");
            while (!inputField.text[previousNewlineIndex].Equals('>'))
            {
                previousNewlineIndex--;
            }
            Cursor = previousNewlineIndex+1;
            FakeCursor = NumOFDenoDigits;

            while (inputField.text[Cursor].Equals(' '))
                Cursor++;

            if (inputField.text[FakeCursor+10].Equals(' '))
            {
                int counter = 0;
                while (inputField.text[FakeCursor + 10 + counter].Equals(' '))
                    FakeCursor++;
            }
        }
        inputField.selectionAnchorPosition = FakeCursor;
        inputField.selectionFocusPosition = FakeCursor;
        inputField.ActivateInputField();

    }
    public void DownClick()
    {
        UpClick();
        int nextNewlineIndex = inputField.text.LastIndexOf('\n');
        if (nextNewlineIndex >= 0 && FakeCursor<=inputField.text.Length&&!IsLower)
        {
            while (int.TryParse(inputField.text[Cursor].ToString(), out _))
            {
                Cursor++;
            }
            Cursor = nextNewlineIndex + 25;
            IsUpper = false;
            IsLower = true ;

            while (inputField.text[Cursor].Equals(' '))
            {
                Cursor++;
            }
            int Beforesign = (Mathf.Abs(inputField.text.IndexOf("<line-height=0.3em>") - inputField.text.IndexOf("</line-height>")) - 22);
            FakeCursor = Mathf.Abs(inputField.text.IndexOf("</") - 1 - inputField.text.IndexOf("%>") + 2) + Beforesign;
            int SpacesINdex = inputField.text.IndexOf("<size=60%>",3) + 10;
            while (inputField.text[SpacesINdex].Equals(' '))
            {
                SpacesINdex++;
                FakeCursor++;
            }

        }
        inputField.selectionAnchorPosition = FakeCursor;
        inputField.selectionFocusPosition = FakeCursor;
        inputField.ActivateInputField();
    }
    public void RightClick()
    {
        if (Cursor + 1 <= inputField.text.Length )
        {
            if(Cursor<  inputField.text.LastIndexOf("</size>") && inputField.text[Cursor].Equals(' '))
            {
                IsLower = false;
                IsUpper = false;
                while (inputField.text[Cursor].Equals(' '))
                {
                    Cursor++;
                    FakeCursor++;
                } 
            }
            else if (!inputField.text[Cursor].Equals('<') &&!inputField.text[Cursor].Equals('\n'))
            {
                Cursor += 1;
                FakeCursor += 1;
            }
            else if(inputField.text[Cursor].Equals('<') && inputField.text[Cursor + 1].Equals('/')) // deno or nue is finshed
            {
                IsLower = false;
                IsUpper = false;
                Cursor = inputField.text.IndexOf('\n' , Cursor)+1;
                FakeCursor +=1;
            }
        }
        if (Cursor < inputField.text.Length)
        {
            if (inputField.text[Cursor].Equals('<') && inputField.text[Cursor + 1].Equals('s')) 
            {
                IsLower = false;
                IsUpper = false;
                int lastIndex = inputField.text.IndexOf('>', Cursor);
                Cursor = lastIndex + 1;
            }
        }
        inputField.selectionAnchorPosition = FakeCursor;
        inputField.selectionFocusPosition = FakeCursor;
        inputField.ActivateInputField();

    }

    public void LeftClick()
    {
        if (Cursor > 0)
        {
            if (Cursor < inputField.text.IndexOf("</size>") && inputField.text[Cursor].Equals(' '))
            {
                IsLower = false;
                IsUpper = false;
                while (inputField.text[Cursor].Equals(' '))
                {
                    Cursor--;
                    FakeCursor--;
                }
            }
            else
            {
                Cursor -= 1;
                FakeCursor -= 1;
            }

        }
        if (Cursor > 0 && Cursor-1>=0)
        {
            if (inputField.text[Cursor].Equals('>') && !inputField.text[Cursor - 1].Equals('e'))
            {
                IsLower = false;
                IsUpper = false;
                int lastIndex = inputField.text.LastIndexOf('<', Cursor);
                Cursor = lastIndex - 1;
                if(Cursor < inputField.text.Length && Cursor >= 0) {
                    if (!int.TryParse(inputField.text[Cursor].ToString(), out _))
                        Cursor -= 6;
                }

            }
        }
        if (Cursor <= 0)
            Cursor = 0;
        inputField.selectionAnchorPosition = FakeCursor;
        inputField.selectionFocusPosition = FakeCursor;
        inputField.ActivateInputField();

    }

    public void FractionClick()
    {
        NumOfFractions++;
        string newFraction = "";
        if(inputField.text.Length==0)
            newFraction = $"<size=60%>#</size><line-height=0.3em>\n―\n</line-height><size=60%>#</size>";
        else
        {
            if (!IsFraction)
            {
                newFraction = $"<size=60%>#</size><line-height=0.3em>\n―\n</line-height><size=60%>#</size>";
                string InputFieldContent = inputField.text;
                inputField.text = newFraction;
                for (int i = 0; i < InputFieldContent.Length; i++)
                {
                    Cursor = inputField.text.IndexOf('―');
                    WriteOnInput(InputFieldContent[i]);
                }
                FakeCursor = (inputField.text.IndexOf("</")-1 - inputField.text.IndexOf("%>")+2)-2+InputFieldContent.Length;
                newFraction = "";
            }
            else
            {
                int CursorTemp = Cursor;
                int FakeCursorTemp = FakeCursor;
                int FirstNewLineIndex = inputField.text.IndexOf('\n');
                int LastNewLineIndex = inputField.text.LastIndexOf('\n');
                int LastDash = inputField.text.LastIndexOf('―', LastNewLineIndex,LastNewLineIndex-FirstNewLineIndex);
                int numOfSpaces = 0 , counter = LastDash+1;
                while (!inputField.text[counter].Equals('\n')) {
                    numOfSpaces++;
                    counter++;
                }
                int i = 0;
                inputField.text = inputField.text.Insert(counter , "―");
                UpClick();

                while (!inputField.text[Cursor].Equals('<'))
                    Cursor++;

                while (i<numOfSpaces+1)
                {
                    int j = 0;
                    while (j < CurrentSpaces)
                    {
                        inputField.text = inputField.text.Insert(Cursor," ");
                        j++;
                    }
                    ResetSpacesVars();
                    i++;
                }
                inputField.text = inputField.text.Insert(Cursor , "#");
                i = 0;
                DownClick();
                while (!inputField.text[Cursor].Equals('<'))
                    Cursor++;
                while (i < numOfSpaces+1)
                {
                    int j = 0;
                    while (j < CurrentSpaces)
                    {
                        inputField.text = inputField.text.Insert(Cursor, " ");
                        j++;
                    }
                    ResetSpacesVars();
                    i++;
                }
                inputField.text = inputField.text.Insert(Cursor, "#");
                LastDash = inputField.text.LastIndexOf('―');
                Cursor = LastDash ;
                FakeCursor = FakeCursorTemp+(LastDash-CursorTemp) ;
            }
        }
        inputField.text = inputField.text.Insert(Cursor, newFraction);

        if(!IsFraction)
            RightClick();

        IsFraction = true;
        inputField.selectionAnchorPosition = FakeCursor;
        inputField.selectionFocusPosition = FakeCursor;
        inputField.ActivateInputField();
    }
    public void WriteOnInput(char latter)
    {
        int CurrentPos = inputField.text.Length;
        if (IsPower)
        {
            inputField.text = inputField.text.Insert(Cursor, "<sup>");
            Cursor += 5;
        }
        else
        {
            if(Cursor+1< inputField.text.Length && Cursor-1>0)
            {
                if (inputField.text[Cursor].Equals('#'))
                {
                    inputField.text = inputField.text.Remove(Cursor , 1);
                }
            }
            inputField.text = inputField.text.Insert(Cursor, latter.ToString());
            AddSpaces();
        }
        if (IsPower)
        {
            inputField.text = inputField.text.Insert(1 + Cursor, "</sup>");
            Cursor += 6;
        }
        if (NumOfFractions >= 2 && inputField.text.IndexOf('―') != -1 && (Cursor < inputField.text.IndexOf("</size>") || (Cursor > inputField.text.IndexOf('\n') && Cursor > inputField.text[inputField.text.LastIndexOf("</line-height>")])))
        {
            int temp = Cursor;
            while (!inputField.text[temp].Equals(' ') && !inputField.text[temp].Equals('>')&&!inputField.text[temp].Equals('<'))
                temp++;
            if(inputField.text[temp].Equals(' '))
            {
                if (inputField.text[temp+1].Equals('<'))
                    inputField.text = inputField.text.Remove(temp, 1);
                else
                    inputField.text = inputField.text.Remove(temp, 2);
            }

        }
        Cursor += 1;
        FakeCursor += 1;
        inputField.selectionAnchorPosition = FakeCursor;
        inputField.selectionFocusPosition = FakeCursor;
        inputField.ActivateInputField();

    }
    public int CountSubstringOccurrences(string text, string substring)
    {
        return Regex.Matches(text, Regex.Escape(substring)).Count;
    }
    public void AddSpaces()
    {
        if (CountSubstringOccurrences(inputField.text.Substring(Cursor), "</size>") == 1 && Cursor < inputField.text.LastIndexOf('―'))
        {
            int NueIndex = inputField.text.IndexOf("%>") + 2;
            int DenoIndex = inputField.text.IndexOf("%>", NueIndex) + 2;
            int indexOfFirstDash = inputField.text.IndexOf('―');
            if(Cursor < indexOfFirstDash || inputField.text.IndexOf('―',Cursor)!=-1)
            {
                if (inputField.text.IndexOf('―', Cursor) != -1)
                {
                    string FractionTillDash = inputField.text.Substring(0, Cursor);
                    int NumOfFractionsBefore = CountSubstringOccurrences(FractionTillDash, "―");
                    int Fractioncounter = NumOfFractionsBefore;

                    while (inputField.text[NueIndex].Equals(' '))
                        NueIndex++;

                    while (Fractioncounter > 0) // getting the pos of nue
                    {
                        if (inputField.text[NueIndex].Equals(' '))
                        {
                            Fractioncounter--;
                            while (inputField.text[NueIndex].Equals(' '))
                                NueIndex++;
                            NueIndex--;
                        }
                        NueIndex++;
                    }

                    Fractioncounter = NumOfFractionsBefore;
                    while (inputField.text[DenoIndex].Equals(' '))
                        DenoIndex++;

                    while (Fractioncounter > 0) // getting the pos of deno
                    {
                        if (inputField.text[DenoIndex].Equals(' '))
                        {
                            Fractioncounter--;
                            while (inputField.text[DenoIndex].Equals(' '))
                                DenoIndex++;
                            DenoIndex--;

                        }
                        DenoIndex++;
                    }
                }
                int counter = 0;
                while (counter < CurrentSpaces)
                {
                    inputField.text = inputField.text.Insert(NueIndex + counter, " ");
                    inputField.text = inputField.text.Insert(DenoIndex + 1 + counter, " "); // plus nue space
                    counter++;
                }
                ResetSpacesVars();
            }
        }

    }
    public void ResetSpacesVars()
    {
        Cursor += CurrentSpaces;
        FakeCursor += CurrentSpaces;
        if (CurrentSpacesCounter == 0 && CurrentSpaces != 3)
        {
            CurrentSpaces--;
        }
        if (CurrentSpacesCounter < 1)
        {
            CurrentSpacesCounter++;
        }
        else
        {
            CurrentSpacesCounter = 0;
            CurrentSpaces += 1;
        }
    }
}
