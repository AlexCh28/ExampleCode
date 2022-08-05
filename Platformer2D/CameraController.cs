using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject target; //цель камеры

    Vector3 offset; //смещение относительно цели
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - target.transform.position; //вычисляем смещение
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.transform.position + offset; //двигаем камеру за целью
    }
}
