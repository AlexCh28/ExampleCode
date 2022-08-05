using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaceTicker : MonoBehaviour
{
    [SerializeField] int speed = 2; //скорость
    Vector2 startPosition; //начальная точка шара
    Rigidbody2D rb; //физическое тело для ускорения
    int direct = 1; //направление движения    

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position; //получаем начальную точку
        rb = GetComponent<Rigidbody2D>(); //получаем физическое тело
        rb.AddForce(Vector2.right * direct * speed * 10, ForceMode2D.Impulse); //толкаем шар
    }

    // Update is called once per frame
    void Update()
    {
        //если близко к точке старта
        if (Vector2.Distance(startPosition, transform.position) < 0.01f)
        {
            direct *= -1; //переключем направление
            rb.AddRelativeForce(Vector2.right * direct * speed, ForceMode2D.Impulse); //даем толчок
        }
    }
}
