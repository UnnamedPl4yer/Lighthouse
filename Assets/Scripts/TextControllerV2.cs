using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TextControllerV2 : MonoBehaviour
{
    public Image black;
    public Image white;
    private readonly string[] texts = 
    {
        "",
        "",
        "*radio starts cracking*\n" +
            "Sailor: 'Hello? Can someone hear me? This is Lucky Lucy speaking!\nThe light of the light house has stopped! " +
            "I believe our ship is close to the island and we fear to sail right into it! If someone can hear me, please help us!'",
        "Father: '*Cough* Uuuurgh, I need to get up and help these poor sailors... Argh.... My head is spinning... *Cough*\n" +
            "I bet the storm damaged the lense... Where is that stupid replacement lense anyway...\n*Cough* Must...get...up...*snorr*'",
        "",
    };

    private int curImage = 0;

    private GUIStyle guistyle = new GUIStyle("HelpBox");

    private void Awake()
    {
        OutFade(2);
        guistyle.fontSize = 32;
    }

    public void IncreaseImageCounter()
    {
        curImage += 1;
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("MainGame");
    }

    void OnGUI()
    {
        if (texts[curImage] != "")
        {
            float topLeftX = 0;
            float topLeftY = 0;
            float w = 0;
            float h = 0;

            if (curImage == 3)
            {
                topLeftX = (int)Screen.width * 0.1f;
                topLeftY = (int)Screen.height * 0.05f;
                w = (int)Screen.width * 0.8f;
                h = (int)Screen.height * 0.2f;
            } 
            else
            {
                topLeftX = (int)Screen.width * 0.1f;
                topLeftY = (int)Screen.height * 0.75f;
                w = (int)Screen.width * 0.8f;
                h = (int)Screen.height * 0.2f;
            }
            GUI.Box(new Rect(topLeftX, topLeftY, w, h), texts[curImage], guistyle);
        }
    }

    public void OutFade(int seconds)
    {
        StartCoroutine(FadeOut(seconds));
    }

    public void InFade(int seconds)
    {
        StartCoroutine(FadeIn(seconds));
    }

    IEnumerator FadeOut(int seconds)
    {
        float target = seconds / 0.1f;
        float t = 0;
        for (int i = 0; i < target; i++)
        {
            var tempColor = black.color;
            tempColor.a = Mathf.Lerp(tempColor.a, 0, t);
            black.color = tempColor;
            t += Time.deltaTime;
            yield return new WaitForSeconds(1 / target);
        }
        var tempColor2 = black.color;
        tempColor2.a = 0;
        black.color = tempColor2;
        black.enabled = false;
    }

    IEnumerator FadeIn(int seconds)
    {
        float target = seconds / 0.1f;
        float t = 0;
        for (int i = 0; i < target; i++)
        {
            var tempColor = black.color;
            tempColor.a = Mathf.Lerp(tempColor.a, 1, t);
            black.color = tempColor;
            t += Time.deltaTime;
            yield return new WaitForSeconds(1 / target);
        }
        var tempColor2 = black.color;
        tempColor2.a = 1;
        black.color = tempColor2;
    }

}
