using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
    
public class TextController : MonoBehaviour
{
    public bool showBox = true;

    private int textNum = 0;
    private string buttonText = "Next";

    private GUIStyle guistyle = new GUIStyle("HelpBox");

    private readonly string[] texts = {
        "*radio starts cracking*",
        "Sailor: 'Hello? Can someone hear me? This is Lucky Lucy speaking!\nThe light of the light house has stopped! " + 
            "I believe our ship is close to the island and we fear to sail right into it! If someone can hear me, please help us!'",
        "Father: '*Cough* *Cough* Uuuurgh, I need to get up and help these poor sailors... Argh.... My head is spinning... *Cough* *Cough*\n" + 
            "I bet the storm damaged the lense...\nWhere is that stupid replacement lense anyway... *Cough* *Cough* Must...get...up...*snorr*'"
    };

    private void Awake()
    {
        guistyle.fontSize = 24;
        guistyle.wordWrap = true;
    }

    void OnGUI()
    {
        if (showBox)
        {
            var topLeftX = (int) Screen.width * 0.1f;
            var topLeftY = (int) Screen.height * 0.75f;
            var w = (int) Screen.width * 0.8f;
            var h = (int) Screen.height * 0.2f;

            GUI.Box(new Rect(topLeftX, topLeftY, w, h), texts[textNum], guistyle);

            if (textNum == texts.Length - 1)
            {
                buttonText = "Start Game";
            }

            // The close button is 100x40 and centered at the bottom of the screen:
            if (GUI.Button(new Rect(Screen.width * 0.9f - 100, Screen.height - Screen.height * 0.1f, 100, 40), buttonText))
            {
                textNum += 1;

                if (textNum == texts.Length)
                {
                    SceneManager.LoadScene("MainGame");
                }
            }
        }
    }
}
