using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private GameObject player;
    private GameObject enemy;
    private float lightPower;
    private bool hallucinating;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        hallucinating = false;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerController pc = player.GetComponent<PlayerController>();
        lightPower = pc.snailStrength / pc.maxSnailStrength;
        if (lightPower < 0.5f && hallucinating == false)
        {
            hallucinating = true;
            enemy.GetComponent<Animator>().SetBool("hallucinating", true);
        }
        else if (lightPower >= 0.5f && hallucinating == true)
        {
            hallucinating = false;
            enemy.GetComponent<Animator>().SetBool("hallucinating", false);
        }
    }
}
