using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroScript : MonoBehaviour
{
    [SerializeField] private GameObject overlayImage;
    [SerializeField] private TextMeshProUGUI textMesh;
    public float transitionDuration = 3f;
    public float textMovementDuration = 2f; // Duration for text movement
    public float startDelay = 2f;
    private float elapsedTime = 0f;
    private float textElapsedTime = 0f;
    private Vector3 startPos;
    private Vector3 endPos;

    void Start()
    {
        // Set initial position and target position for the text
        startPos = new Vector3(Screen.width, textMesh.transform.position.y, textMesh.transform.position.z);
        endPos = new Vector3(Screen.width / 2f, textMesh.transform.position.y, textMesh.transform.position.z);

        // Hide the text at the beginning
        textMesh.gameObject.SetActive(false);

        StartCoroutine(Transition());
    }

    public IEnumerator Transition()
    {
        yield return new WaitForSeconds(startDelay);

        // Show the text
        textMesh.gameObject.SetActive(true);

        // Gradually move the text from the right to the center of the screen
        while (textElapsedTime < textMovementDuration)
        {
            float t = textElapsedTime / textMovementDuration;
            textMesh.transform.position = Vector3.Lerp(startPos, endPos, t);
            textElapsedTime += Time.deltaTime;
            yield return null;
        }

        // Gradually increase opacity over transitionDuration seconds
        while (elapsedTime < transitionDuration)
        {
            float alpha = Mathf.Lerp(0.0001f, 6f, elapsedTime / (transitionDuration * 3));
            overlayImage.GetComponent<Image>().color = new Color(overlayImage.GetComponent<Image>().color.r, overlayImage.GetComponent<Image>().color.g, overlayImage.GetComponent<Image>().color.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        LoadNextScene();
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene("Project/MathsProblemsEnglish/PageDitector/DirectorScene");
    }
}
