using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb; //физическое тело
    Animator animator; //контроллер анимации
    SpriteRenderer spriteRenderer; //для разворота спрайта

    [Header("Движение")] //заголовок в инспекторе
    [SerializeField] float speed = 5f; //скорость движения
    [SerializeField] float jumpDistance = 1.5f; //множитель высоты прыжка
    [SerializeField] Transform legs; //ноги персонажа, для определения касания земли
    [SerializeField] LayerMask maskGround; //название слоя

    AudioSource audioSource; //звуковой компонент
    [Header("Звук")]
    [SerializeField] AudioClip soundCoin; //звук подбора монетки
    [SerializeField] AudioClip soundJump; //звук прыжка
    [SerializeField] AudioClip soundStepGrass; //звук шагов по траве
    [SerializeField] AudioClip soundStepSnow; //звук шагов по снегу
    
    AudioClip soundStep; //звук шагов текущего уровня
    bool isSoundPlay = false; //флаг звука шагов

    bool isDead = false; //флаг потери жизни, для запрета управления
    public bool isJump = false; //флаг для определения прыжка
    public float radiusLegs = 0.04f; //радиус для обнаружения земли
    Vector3 startPoint; //начальная точка персонажа
    bool isInPortal = false; //флаг что находимся в портале
    float blinkTime = 5; //время простоя перед морганием

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>(); //получаем доступк к физике
        animator = gameObject.GetComponent<Animator>(); //получаем доступ к аниматору
        spriteRenderer = GetComponent<SpriteRenderer>(); //получаем доступ к спрайтрендереру
        startPoint = transform.position; //записываем точку старта        
        audioSource = GetComponent<AudioSource>(); //находим звуковой компонент

        if (SceneManager.GetActiveScene().buildIndex == 2) //проверяем номер уровня, если снежный
        {            
            soundStep = soundStepSnow; //ставим на шаги звук по снегу
        }
        else
        {            
            soundStep = soundStepGrass; //ставим на шаги звук по траве
        }
    }

    private void FixedUpdate()
    {
        //определяем, стоит ли персонаж на земле и передаем результат в метод
        OnGround(!Physics2D.OverlapCircle(legs.position, radiusLegs, maskGround));
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead) //управляем пока не потеряли жизнь
        {
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) //движение влево
            {
                animator.SetBool("isRun", true); //включаем анимацию бега
                rb.velocity = new Vector2(-speed, rb.velocity.y); //задаем ускорение физическому телу
                spriteRenderer.flipX = true; //отобразить по оси X
                PlaySteps(true);
                blinkTime = 5; //сбрасываем время
            }
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) //движенеи вправо
            {
                animator.SetBool("isRun", true); //включаем анимацию бега
                rb.velocity = new Vector2(speed, rb.velocity.y); //задаем ускорение физическому телу
                spriteRenderer.flipX = false; //отобразить по оси X - выключить
                PlaySteps(true);
                blinkTime = 5; //сбрасываем время
            }
            if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D)) //влево-вправо не двигаемся
            {
                PlaySteps(false);
                animator.SetBool("isRun", false); //выключаем анимацию бега
                rb.velocity = Vector2.zero; //убираем ускорение физического тела
            }
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) //прыжки
            {
                PlaySteps(false);
                Jump(jumpDistance); //вызываем метод прыжка и передаем рсстояние
                blinkTime = 5; //сбрасываем время
            }

            if (isInPortal && Input.GetKey(KeyCode.E)) //кнопка 'Е'
            {
                GameManager.instance.OpenPortal(); //запускаем метод для переключения уровней
            }

            blinkTime -= Time.deltaTime; //уменьшаем время простоя каждый кадр
            if(blinkTime <= 0)
            {
                animator.SetTrigger("blink"); //запускаем анимациюморгания
                blinkTime = 5; //сбрасываем время
            }
        }
    }

    //метод запуска проигрывания звука шагов
    void PlaySteps(bool isRun)
    {
        if (isRun && !isSoundPlay) //бежим и звук не играет
        {
            isSoundPlay = isRun; //ставим флаг что звук играет
            audioSource.clip = soundStep; //ставим звук шагов
            audioSource.Play(); //запускаем проигрывание
        }
        else if (!isRun) //ели не бежим
        {
            isSoundPlay = isRun; //ставим флаг что звук не играет
            audioSource.clip = null; //очищаем звук шагов
        }
    }

    //метод обработки состояния прыжка
    void OnGround(bool onGround)
    {
        animator.SetBool("isJump", onGround);//запускаем или останавливаем анимацию прыжка
        isJump = onGround; //меняем состояние флага
    }
    
    //метод обработки прыжка
    void Jump(float distance)
    {
        if (!isJump) //если не в прыжке
        {
            audioSource.PlayOneShot(soundJump); //воспроизводим звук
            rb.velocity = new Vector2(0, 0);//останавливаем движение
            rb.AddForce(Vector2.up * speed * distance, ForceMode2D.Impulse); //прикладывем импульс вверх
        }
    }

    //определение столкновений с триггерами
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin")) //если столкнулись с монеткой
        {
            audioSource.PlayOneShot(soundCoin); //воспроизводим звук
            GameManager.instance.RemoveCoin(); //добавляем в геймменеджер монетку
            Destroy(collision.gameObject); //удаляем монетку
        }

        if (collision.CompareTag("Enemy")) //столкновение с врагом
        {
            GetDamage(); //запускаем получение урона
        }

        if (collision.CompareTag("Portal"))
        {
            isInPortal = true;
            GameManager.instance.ShowTextInfo("Нажмите \"E\" для перехода на следующий уровень");
        }
    }

    //обработка выхода из триггера
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Portal")) //если покинули портал
        {
            isInPortal = false; //выключаем флаг
            GameManager.instance.HideTextInfo(); //прячем текст
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) //столкновение с врагом
        {
            GetDamage(); //запускаем получение урона
        }
    }

    //метод возврата в начальную точку после получения урона
    void GetDamage()
    {
        isDead = true;
        transform.position = startPoint; //возврат в точку старта
        GameManager.instance.RemoveLive(); //вызов метода удаление жизни
        animator.SetBool("isRun", false); //останавливаем бег
        animator.SetBool("isJump", false); //останавливаем прыжок
        rb.velocity = new Vector2(0, 0); //останавливаем физику скорости

        StartCoroutine(DeadPause()); //запускаем корутину мигания
    }

    //корутина мигания при получении урона
    IEnumerator DeadPause()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>(); //получаем доступ к спрайтрендереру
        Color startColor = Color.white; //начальный цвет
        Color blinkColor = new Color(0.3f, 0.4f, 0.6f, 0.3f); //цвет мигания

        for (int i = 0; i < 3; i++) //повторяем 3 раза
        {
            sprite.color = blinkColor; //меняем цвет
            yield return new WaitForSeconds(0.2f); //ждем
            sprite.color = startColor;
            yield return new WaitForSeconds(0.2f);
        }

        isDead = false; //восстанавливаем управление
        GameManager.instance.HideTextInfo(); //скрываем текст об уроне
    }
}
