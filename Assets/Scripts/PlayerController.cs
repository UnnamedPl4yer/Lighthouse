using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    private string dir;
    public int jumpSpeed;
    public float snailStrength;

    private bool grounded;
    private bool pushing;
    private Rigidbody rb;
    private Light flaskLight;
    private Light directionalLight;
    public readonly float maxSnailStrength = 20f;
    private GameObject postProcessObject;
    private Vignette vignette;

    public GameObject fluidDrop;
    private readonly float dropRate = 5;
    private bool waiting = false;
    private bool hasJar = false;
    private bool pickingUp = false;

    private readonly float trapLoss = 10f;
    private float moveMod = 1f;

    private GameObject flask;

    private Animator playerAnimator;

    private AudioSource source;
    public AudioClip walk;
    public AudioClip push;
    public AudioClip squish;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 1f;
        jumpSpeed = 300;
        snailStrength = 20;
        grounded = true;
        pushing = false;
        dir = "r";
        
        rb = gameObject.GetComponent<Rigidbody>();
        flaskLight = transform.GetChild(0).gameObject.GetComponentInChildren<Light>();
        directionalLight = transform.GetChild(1).gameObject.GetComponent<Light>();
        postProcessObject = transform.GetChild(2).gameObject;
        playerAnimator = gameObject.GetComponent<Animator>();
        source = gameObject.GetComponent<AudioSource>();
        flask = transform.GetChild(6).gameObject;
        flask.SetActive(false);
        flaskLight.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (pushing == true)
            moveMod = 1.25f;
        else if (pickingUp == true)
            moveMod = 0f;
        else
            moveMod = 1.5f;

        // CONTROLS
        // If spacebar is pressed and player is standing on floor
        if (Input.GetKeyDown(KeyCode.Space) && grounded == true)
        {
            StartCoroutine("Jump");
        }

        if (Input.GetKey(KeyCode.D))
        {
            if (dir == "l")
            {
                Quaternion targetRot = transform.rotation;
                targetRot.y *= (-1);
                transform.rotation = targetRot;
                dir = "r";
            }
            playerAnimator.SetBool("walk", true);
            rb.velocity = new Vector3(moveSpeed * moveMod, rb.velocity.y, 0);
            source.clip = walk;
            source.volume = 1f;
            source.Play();
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            playerAnimator.SetBool("walk", false);
        }

        if (Input.GetKey(KeyCode.A))
        {
            if (dir == "r")
            {
                Quaternion targetRot = transform.rotation;
                targetRot.y *= (-1);
                transform.rotation = targetRot;
                dir = "l";
            }
            playerAnimator.SetBool("walk", true);
            rb.velocity = new Vector3((-1)*moveSpeed * moveMod, rb.velocity.y, 0);
            if (pushing == false) 
                source.clip = walk;
            source.volume = 1f;
            source.Play();
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            playerAnimator.SetBool("walk", false);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            directionalLight.enabled = !directionalLight.enabled;
        }
        // END CONTROLS


        // UPDATE FLASK LIGHT STRENGTH IF PLAYER HAS JAR
        if (hasJar == true)
        {
            float modifier;
            if (pushing == true)
            {
                modifier = 0.25f;
            }
            else
            {
                modifier = 1f;
            }
            snailStrength = Mathf.Max(snailStrength - (1 / (dropRate * 8) * modifier), 0);
            flaskLight.range = snailStrength;
        }
        // END 


        // POSTPROCESSING
        // Check if PostProcessing Volumes Vignette Settings can be found/loaded
        if (postProcessObject.GetComponent<PostProcessVolume>().profile.TryGetSettings<Vignette>(out vignette))
        {
            //change vignette radius and smoothness according to snail-juice-level
            vignette.intensity.value = Mathf.Max(0.4f, 1f - snailStrength / maxSnailStrength);
            vignette.smoothness.value = Mathf.Max(0.5f, 1f - snailStrength / maxSnailStrength);
        }
        // END POSTPROCESSING


        // FLASHLIGHT CONTROL
        if (directionalLight.enabled == true && hasJar == true)
        {
            var v3 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f);
            v3 = Camera.main.ScreenToWorldPoint(v3);
            Vector3 diffVector = v3 - directionalLight.transform.position;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, diffVector, 1f, 0f);
            newDirection.z = 0;
            directionalLight.transform.rotation = Quaternion.LookRotation(newDirection);
        }
        // END FLASHLIGHT
            
        /*
        if (waiting == false)
        {
            StartCoroutine(LoseDrop(6f, 27f, false));
        }
        */
    }


    // COLLISIONS
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            grounded = true;
            playerAnimator.SetBool("jump", false);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Movable")
        {
            Rigidbody otherRB = collision.gameObject.GetComponent<Rigidbody>();
            if (transform.position.y < otherRB.gameObject.transform.position.y)
            {
                otherRB.velocity = new Vector3(rb.velocity.x, 0, 0);
                playerAnimator.SetBool("push", true);
                otherRB.constraints &= ~RigidbodyConstraints.FreezePositionX;
                //Vector3 newVelocity = new Vector3(rb.velocity.x, 0, 0);
                pushing = true;
                source.clip = push;
                source.volume = 1f;
                source.Play();
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            grounded = false;
        }

        
        if (collision.gameObject.tag == "Movable")
        {
            playerAnimator.SetBool("push", false);
            pushing = false;
        }
        
    }
    // END COLLISIONS

    // TRIGGERS
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Snail")
        { 
            //ActivatePickUpIndicator();
        }

        if (other.gameObject.tag == "Stolperfalle")
        {
            StartCoroutine(SteppedInTrap(other.gameObject));
        }

        if (other.gameObject.tag == "Finish")
        {
            SceneManager.LoadScene("EndOfGame");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Pick up snail
        if (other.gameObject.tag == "Snail" && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(PickUp(other.gameObject));
        }

        if (other.gameObject.tag == "Jar" && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(PickUp(other.gameObject));
            hasJar = true;
            flask.SetActive(true);
            flaskLight.enabled = true;
        }

        // Activate lever and play animations for lever and door
        if (other.gameObject.tag == "Lever" && Input.GetKeyDown(KeyCode.E))
        {
            other.GetComponent<Animator>().SetBool("leverActivated", true);
            other.GetComponent<LeverController>().myDoor.GetComponent<Animator>().SetBool("leverActivated", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Snail")
        {
            //DeactivatePickUpIndicator();
        }
    }
    // END TRIGGERS


    // COROUTINES
    IEnumerator PickUp(GameObject other)
    {
        pickingUp = true;
        playerAnimator.SetBool("pickUp", true);
        yield return new WaitForSeconds(1.5f);
        Destroy(other);
        yield return new WaitForSeconds(2f);
        source.clip = squish;
        source.volume = 0.2f;
        source.Play();
        snailStrength = maxSnailStrength;
        playerAnimator.SetBool("pickUp", false);
        pickingUp = false;
    }

    IEnumerator LoseDrop(float minLifetime, float maxLifetime, bool trap)
    {
        waiting = true;
        Vector3 spawn = new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z - 0.5f);
        float randomLifetime = UnityEngine.Random.Range(minLifetime, maxLifetime);
        if (trap == false)
        {
            Destroy(Instantiate(fluidDrop, spawn, transform.rotation), randomLifetime);
            yield return new WaitForSeconds(dropRate);
        }
        else
        {
            Quaternion randomDirection = UnityEngine.Random.rotation;
            GameObject d = Instantiate(fluidDrop, transform.position, randomDirection);
            d.GetComponent<Rigidbody>().velocity = Vector3.up * 0.001f;
            Destroy(d, randomLifetime);
        }
        waiting = false;
    }

    IEnumerator ShowStartleSign()
    {
        yield return null;
    }

    IEnumerator SteppedInTrap(GameObject trap)
    {
        Destroy(trap);

        StartCoroutine(ShowStartleSign());

        float t = 0f;
        for (int i = 0; i < 5; i++)
        {
            StartCoroutine(LoseDrop(0f, 1f, true));
        }

        for (int i = 0; i < 5; i++)
        {
            snailStrength = Mathf.Lerp(snailStrength, snailStrength - trapLoss, t);
            t += Time.deltaTime;
            yield return new WaitForSeconds(1 / 3);
        }

        yield return null;
    }

    IEnumerator Jump()
    {
        grounded = false;
        playerAnimator.SetBool("jump", true);
        rb.AddForce(0, jumpSpeed, 0);
        yield return new WaitForSeconds(1);
        playerAnimator.SetBool("jump", false);
        grounded = true;
    }
    // END COROUTINES
}