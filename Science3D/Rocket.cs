using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] float boostPower = 1000f, rotationSpeed = 300f;
    [SerializeField] AudioClip successAudio, deathAudio;
    [SerializeField] ParticleSystem engineVFX, successVFX, deathVFX;
    AudioSource audioSource;
    Rigidbody rb;
    GameManager gm;
    bool inTransition;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gm = FindObjectOfType<GameManager>();
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0;
    }

    void Update()
    {
        if (!inTransition)
        {
            Thrust();
            Rotate();
        }
    }

    void Rotate()
    {
        float deltaZ = -Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
        if (deltaZ != 0)
        {
            rb.freezeRotation = true;
            transform.Rotate(new Vector3(0, 0, deltaZ));
            rb.freezeRotation = false;
        }
    }

    void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddRelativeForce(Vector3.up * boostPower * Time.deltaTime);
            audioSource.volume = 1;
            if (!engineVFX.isPlaying) engineVFX.Play();
        }
        else
        {
            audioSource.volume = 0;
            engineVFX.Stop();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (inTransition) return;
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                Transition();
                break;
            default:
                Die();
                break;
        }
    }

    void Transition()
    {
        inTransition = true;
        audioSource.volume = 0;
        AudioSource.PlayClipAtPoint(successAudio, Camera.main.transform.position, 0.3f);
        successVFX.Play();
        StartCoroutine(gm.LoadNextLevel());
    }

    void Die()
    {
        inTransition = true;
        audioSource.volume = 0f;
        AudioSource.PlayClipAtPoint(deathAudio, Camera.main.transform.position, 0.3f);
        deathVFX.Play();
        StartCoroutine(gm.ReloadLevel());
    }
}
