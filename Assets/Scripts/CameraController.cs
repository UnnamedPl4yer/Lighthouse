using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private int yThreshold;
    private int yOffset;
    private readonly int zOffset = 6;

    // Start is called before the first frame update
    void Start()
    {
        yThreshold = 5;
        yOffset = 1;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (player.transform.position.y > yOffset)
        {
            Vector3 newPos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - zOffset);
            transform.position = Vector3.Lerp(transform.position, newPos, 0.1f);
        }
        else
        {
        */
            Vector3 newPos = new Vector3(player.transform.position.x, player.transform.position.y + yOffset, player.transform.position.z - zOffset);
            transform.position = Vector3.Lerp(transform.position, newPos, 0.05f);
        //}
    }
}
