using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TermsGenerator : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private GameObject Keyboard;
    [SerializeField] private Button EmptyButton;
    private TouchScreenKeyboard m_keyboard;
    void Start()
    {
        m_keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, false);
        inputField = GetComponent<TMP_InputField>();

        // Set up event listeners for when the input field is selected and deselected
        inputField.onSelect.AddListener(OnSelect);
        inputField.onDeselect.AddListener(OnDeselect);
        EmptyButton.onClick.AddListener(Hidekeyboard);
    }
    void LateUpdate()
    {
        m_keyboard.active = false;
    }
    public void OnSelect(string value)
    {
        // Disable default Unity keyboard when input field is selected
        TouchScreenKeyboard.hideInput = true;
        ButtonAction.IsPower = false;
        inputField.GetComponent<TMP_InputField>().ActivateInputField();
        Keyboard.SetActive(true);
    }

    public void OnDeselect(string value)
    {
        // Re-enable default Unity keyboard when input field is deselected
        TouchScreenKeyboard.hideInput = false;
    }
    public void Hidekeyboard()
    {
        Keyboard.SetActive(false);
    }
}
