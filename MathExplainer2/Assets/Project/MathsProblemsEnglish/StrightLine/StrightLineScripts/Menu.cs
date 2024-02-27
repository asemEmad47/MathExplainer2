using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private Button StrightLineBtn;
    [SerializeField] private GameObject MenuPanel;
    [SerializeField] private GameObject StrightLinePanel;
    [SerializeField] private TextMeshProUGUI AnswerText;

    private string originalAnswerText;
    private Color originalAnswerColor;

    public void StrightLineChk()
    {

        if (StrightLinePanel != null)
        {
            MenuPanel.SetActive(false);
            StrightLinePanel.SetActive(true);
        }
        else
        {
            originalAnswerText = AnswerText.text; // Store the original value
            originalAnswerColor = AnswerText.color; // Store the original color

            AnswerText.text = "Coming soon";
            AnswerText.color = Color.red; // Set the color to red

            StartCoroutine(ResetAnswerText());
        }
    }

    private System.Collections.IEnumerator ResetAnswerText()
    {
        yield return new WaitForSeconds(2f);
        AnswerText.text = originalAnswerText; // Restore the original value
        AnswerText.color = originalAnswerColor; // Restore the original color
    }
}
