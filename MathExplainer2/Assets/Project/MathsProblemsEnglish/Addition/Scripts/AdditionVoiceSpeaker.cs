using System;
using System.Collections;
using System.Text;
using UnityEngine;

public class AdditionVoiceSpeaker : MonoBehaviour
{
    public static AudioClip[] voiceClips;
    public static AudioClip[] Numbers;
    public static AudioSource audioSource;
    public static IEnumerator PlayByAddress(String address)
    {
        LoadAllAudioClips();
        GameObject audioObject = new GameObject("VoiceAudioSource");
        audioSource = audioObject.AddComponent<AudioSource>();
        AudioClip AnyClip = Resources.Load<AudioClip>(address);
        audioSource.clip = AnyClip;
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length + 1f);
        audioSource.Stop();
        audioSource.clip = null;
    }
    public static IEnumerator PlayVoiceNumberAndWait(String text)
    {
        if (text.Length >= 1)
        {
            LoadAllAudioClips();
            GameObject audioObject = new GameObject("VoiceAudioSource");
            audioSource = audioObject.AddComponent<AudioSource>();
            if (text[0].Equals('-'))
            {
                AudioClip MinusAudioClip = Resources.Load<AudioClip>("AdditionTerms/AdditionSound/minus_Sonya_Eng");
                audioSource.clip = MinusAudioClip;
                audioSource.Play();
                yield return new WaitForSeconds(audioSource.clip.length + 1f);
                StringBuilder MinusRemoval = new StringBuilder(text);
                MinusRemoval.Remove(0, 1);
                text = MinusRemoval.ToString();
            }
            bool IsComplex = false;
            if (text.Length == 1)
            {
                audioSource.clip = GetUnder10Numbers(text);
            }
            else if (text.Length == 2)
            {
                audioSource.clip = GetUnder20Numbers(text);
                if (audioSource.clip == null)
                {
                    string temp = "";
                    temp += text[0];
                    temp += "0";
                    audioSource.clip = GetUnder20Numbers(temp);
                    audioSource.Play();
                    yield return new WaitForSeconds(audioSource.clip.length);
                    audioSource.clip = GetUnder10Numbers(text[1].ToString());
                    audioSource.Play();
                    yield return new WaitForSeconds(audioSource.clip.length);
                    IsComplex = true;
                }
            }
            else
            {
                if (text.Equals("100"))
                {
                    audioSource.clip = Numbers[28];
                }
                else
                {
                    int charIndex = text.IndexOf('.');
                    if (charIndex == 2)
                    {
                        audioSource.clip = GetUnder20Numbers(text.Substring(0, 2));
                        if (audioSource.clip == null)
                        {
                            string temp = "";
                            temp += text[0];
                            temp += "0";
                            audioSource.clip = GetUnder20Numbers(temp);
                            audioSource.Play();
                            yield return new WaitForSeconds(audioSource.clip.length);
                            audioSource.clip = GetUnder10Numbers(text[1].ToString());
                        }
                    }
                    else
                    {
                        audioSource.clip = GetUnder10Numbers(text.Substring(0, 1));
                    }
                    audioSource.Play();
                    yield return new WaitForSeconds(audioSource.clip.length);
                    AudioClip PointClip = Resources.Load<AudioClip>("AdditionTerms/AdditionSound/POINT_Sonya_Eng");
                    audioSource.clip = PointClip;
                    audioSource.Play();
                    yield return new WaitForSeconds(audioSource.clip.length);
                    for (int i = charIndex + 1; i < text.Length && i < charIndex + 4; i++)
                    {
                        audioSource.clip = GetUnder10Numbers(text[i].ToString());
                        audioSource.Play();
                        yield return new WaitForSeconds(audioSource.clip.length);
                    }

                    IsComplex = true;
                }
            }
            if (!IsComplex)
            {
                audioSource.Play();
                yield return new WaitForSeconds(audioSource.clip.length);
            }
            audioSource.Stop();
            Destroy(audioObject);
            audioSource = null;
        }
    }
    public static AudioClip GetUnder10Numbers(String text)
    {
        switch (text)
        {
            case "0":
                return Numbers[0];
            case "1":
                return Numbers[1];
            case "2":
                return Numbers[2];
            case "3":
                return Numbers[3];
            case "4":
                return Numbers[4];
            case "5":
                return Numbers[5];
            case "6":
                return Numbers[6];
            case "7":
                return Numbers[7];
            case "8":
                return Numbers[8];
            case "9":
                return Numbers[9];
            default:
                return Numbers[0];
        }
    }
    public static AudioClip GetUnder20Numbers(String text)
    {
        switch (text)
        {
            case "10":
                return Numbers[10];
            case "11":
                return Numbers[11];
            case "12":
                return Numbers[12];
            case "13":
                return Numbers[13];
            case "14":
                return Numbers[14];
            case "15":
                return Numbers[15];
            case "16":
                return Numbers[16];
            case "17":
                return Numbers[17];
            case "18":
                return Numbers[18];
            case "19":
                return Numbers[19];
            case "20":
                return Numbers[20];
            case "30":
                return Numbers[21];
            case "40":
                return Numbers[22];
            case "50":
                return Numbers[23];
            case "60":
                return Numbers[24];
            case "70":
                return Numbers[25];
            case "80":
                return Numbers[26];
            case "90":
                return Numbers[27];
            default:
                return null;
        }
    }
    public static void LoadAllAudioClips()
    {
        voiceClips = Resources.LoadAll<AudioClip>("AdditionTerms/AdditionSound");
        Numbers = Resources.LoadAll<AudioClip>("AdditionTerms/EngNums");
    }
}
