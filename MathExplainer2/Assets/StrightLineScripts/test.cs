using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class test : MonoBehaviour
{
    private string arabicText = "انقل العدد الي الناحية الثانية وأعكس إشارته"; // Replace with the Arabic text you want to read
    public AudioSource audioSource;

    public void Start()
    {
    }  
    public void PlayArabic()
    {
        StartCoroutine(SpeakArabicText());
    }

    public IEnumerator SpeakArabicText()
    {
        string url = "https://translate.google.com/translate_tts?ie=UTF-8&q=" + WWW.EscapeURL(arabicText) + "&tl=ar&client=tw-ob";

        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                AudioClip audioClip = DownloadHandlerAudioClip.GetContent(www);
                audioSource.clip = audioClip;
                audioSource.Play();
            }
        }
    }
}
