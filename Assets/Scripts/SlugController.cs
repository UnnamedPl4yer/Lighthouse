using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlugController : MonoBehaviour
{
    public GameObject player;
    private Light light;
    private ParticleSystem particleSystem;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        light = gameObject.GetComponentInChildren<Light>();
        particleSystem = gameObject.GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        light.range = 20 * Mathf.Exp(-distanceToPlayer);
        //particleSystem.emitRate setzen!
        //Debug.Log("DISTANCE TO PLAYER " + distanceToPlayer + " RANGE " + light.range);
    }
}
