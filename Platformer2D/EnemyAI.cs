using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{    
    [SerializeField] private Sprite[] sprites; //массив спрайтов морды
    [SerializeField] private float speed = 100f; //скорость передвижения
    [SerializeField] private float targetDistance; //расстояние для действия

    [SerializeField] private PlayerController target; //цель
    Rigidbody2D rb; //физическое тело
    SpriteRenderer spriteRenderer; //для смены спрайтов
    [SerializeField] private Vector3 startPoint; //точка старта


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); //получаем компонент физического тела
        target = GameObject.FindObjectOfType<PlayerController>(); //находим цель
        spriteRenderer = GetComponent<SpriteRenderer>(); //компонент спрайтрендерер

        spriteRenderer.sprite = sprites[0]; //устанавливаем спрайт морды
    }

    // Update is called once per frame
    void Update()
    {
        //если расстояние до цели меньше нужной
        if (System.Math.Abs(transform.position.y - target.transform.position.y) < 2 && Vector2.Distance(transform.position, target.transform.position) < targetDistance)
        {            
            spriteRenderer.sprite = sprites[1]; //открываем глаза
            int direct = 1; //направление движения
            if (transform.position.x > target.transform.position.x) direct = -1; //определяем направление
            rb.velocity = new Vector2(direct * speed * Time.deltaTime, rb.velocity.y); //движение в сторону цели
        }
        //иначе если расстояние до точки старта менее 0,05
        else if (Vector2.Distance(transform.position, startPoint) < 0.05f)
        {            
            rb.velocity = new Vector2(0, 0); //останавливаемся
            spriteRenderer.sprite = sprites[0]; //закрываем глаза
        }
        //иначе двигаемся к точке старта
        else
        {
            int direct = 1;//направление движения
            if (transform.position.x > startPoint.x) direct = -1;//определяем направление
            rb.velocity = new Vector2(direct * speed * Time.deltaTime, rb.velocity.y);//движение в сторону стартовой точки
        }
    }

    //при касании земли получим точку старта
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (startPoint==Vector3.zero && collision.collider.name.Equals("Ground"))
        {
            startPoint = transform.position;//получаем точку старта
        }
    }

    //проверяем вхождение в триггер
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) //если столкнулись с врагом
        {
            transform.position = startPoint; //возвращаемся в точку старта
        }
    }
}
