using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public TextAsset simpleJSON;
    public TextAsset intermediateJSON;
    public TextAsset trickyJSON;

    public string currentOperation;

    public int difficulty = 1;

    [SerializeField] TMPro.TextMeshProUGUI firstQuestionPart;

    [SerializeField] TMPro.TextMeshProUGUI secondQuestionPart;

    public static GameLogic instance;

    public QuestionBank simpleQuestions = new QuestionBank();
    public QuestionBank intermediateQuestions = new QuestionBank();
    public QuestionBank trickyQuestions = new QuestionBank();


    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // Deserialize the JSON string into a C# object
        simpleQuestions = JsonUtility.FromJson<QuestionBank>(simpleJSON.text);
        intermediateQuestions = JsonUtility.FromJson<QuestionBank>(intermediateJSON.text);
        trickyQuestions = JsonUtility.FromJson<QuestionBank>(trickyJSON.text);

        GameLogic.instance.GetQuestion();
    }

    //we generate the question, a and b are operands
    public void GetQuestion()
    {
        switch (difficulty)
        {
            case 1:
                int randomQuestion = Random.Range(0, simpleQuestions.questions.Length);
                string[] questionText = ReplaceSymbols(simpleQuestions.questions[randomQuestion].question).Split("?");
                firstQuestionPart.text = questionText[0];
                secondQuestionPart.text = questionText[1];
                currentOperation = simpleQuestions.questions[randomQuestion].operation;
                return;
            case 2:
                randomQuestion = Random.Range(0, intermediateQuestions.questions.Length);
                questionText = ReplaceSymbols(intermediateQuestions.questions[randomQuestion].question).Split("?");
                firstQuestionPart.text = questionText[0];
                secondQuestionPart.text = questionText[1];
                currentOperation = intermediateQuestions.questions[randomQuestion].operation;
                return;
            case 3:
                randomQuestion = Random.Range(0, trickyQuestions.questions.Length);
                questionText = ReplaceSymbols(trickyQuestions.questions[randomQuestion].question).Split("?");
                firstQuestionPart.text = questionText[0];
                secondQuestionPart.text = questionText[1];
                currentOperation = trickyQuestions.questions[randomQuestion].operation;
                return;
        }
    }

    string ReplaceSymbols(string expression)
    {
        return expression.Replace("*", "x").Replace("/", "÷");
    }

}
