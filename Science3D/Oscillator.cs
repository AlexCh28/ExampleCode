using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 3f;
    const float tau = Mathf.PI * 2;
    float cycles, rawSinWave, movementFactor;
    Vector3 startPos, offset;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (period <= 0) return;
        cycles = Time.time / period;
        rawSinWave = Mathf.Sin(cycles * tau);
        movementFactor = rawSinWave / 2f + 0.5f;
        offset = movementFactor * movementVector;
        transform.position = startPos + offset;
    }
}
