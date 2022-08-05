using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindAmbience : MonoBehaviour
{
    void Awake()
    {
        if (FindObjectsOfType<WindAmbience>().Length > 1) Destroy(gameObject);
        else DontDestroyOnLoad(gameObject);
    }
}
