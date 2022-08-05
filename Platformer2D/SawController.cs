using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawController : MonoBehaviour
{
    public float speed = -200; //скорость вращения пилы по часовой стрелке    

    // Update is called once per frame
    void Update()
    {
        float rotation = speed * Time.deltaTime; //угол вращения за 1 кадр
        transform.Rotate(Vector3.forward, rotation); //поворот по оси Z, т.к. Vector3.forward x=0, y=0, z=1
    }
}
