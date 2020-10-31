using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlugJarController : MonoBehaviour
{
    private float juiceLevel;
    private readonly float maxJuiceLevel = 20f;
    private GameObject juice;
    private readonly float fullLocalHeight = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        juiceLevel = 0;
        juice = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        juiceLevel = gameObject.GetComponentInParent<PlayerController>().snailStrength;
        float p = juiceLevel / maxJuiceLevel;
        float scaleDiff = juice.transform.localScale.y;
        juice.transform.localScale = new Vector3(0.1f, p * fullLocalHeight, 0.1f);
        scaleDiff -= juice.transform.localScale.y;
        juice.transform.position -= new Vector3(0, scaleDiff, 0);
    }
}
