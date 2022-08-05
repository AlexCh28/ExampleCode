using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Интерфейс")]
    [SerializeField] private Text textCoins; //поле для монеток
    [SerializeField] private Text textLives; //поле для жизней
    [SerializeField] private Text textInfo; //информационная надпись
    [SerializeField] private GameObject panelNextLevel; //панель перехода между уровнями
    [SerializeField] private Text textPanelNextLevel; //текстовое поле панели перехода между уровнями
    
    [Header("Объекты")]
    [SerializeField] private GameObject portal; //переменная для портала


    int coins = 0; //количество монет
    int lives = 3; //количество жизней

    List<GameObject> listCoins = new List<GameObject>(); //список найденных монет
    internal int currentLevel; //текущий уровень
    
    void Start()
    {
        instance = GetComponent<GameManager>(); //получаем компонент геймменеджер
        //получаем номер текущего уровня
        currentLevel = SceneManager.GetActiveScene().buildIndex;        
        //находим все монеты и помещаем в список
        listCoins.AddRange(GameObject.FindGameObjectsWithTag("Coin"));
        coins = listCoins.Count; //получаем количество монет в списке
        listCoins.Clear(); //очищаем список

        portal.SetActive(false); //отключаем портал в начале

        textCoins.text = coins.ToString(); //выводим на экран количество монет
        textLives.text = lives.ToString(); //выводим на экран количество жизней
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //метод для изменения количества монет
    public void RemoveCoin()
    {
        coins--; //прибавляем монетку
        textCoins.text = coins.ToString(); //выводим на экран новое количество монет

        if (coins==0)
        {
            portal.SetActive(true); //включаем портал                     
        }
    }

    //метод для перехода в портал
    public void OpenPortal()
    {
        //если номер следующей сцены меньше количество сцен в игре
        if (currentLevel + 1 < SceneManager.sceneCountInBuildSettings)
        {
            panelNextLevel.SetActive(true); //включаем отображение панели
            Invoke("NextLevel", 3f); //запускаем метод перехода через 3 секунды
        }
        else
        {
            textPanelNextLevel.text = "Конец игры!"; //меняем надпись
            panelNextLevel.SetActive(true); //включаем отображение панели
            Invoke("LoadExit", 3f); //запускаем метод выхода через 3 секунды
        }
    }

    //метод для вычитания жизней
    public void RemoveLive()
    {
        lives--; //уменьшаем жизни        

        if (lives == 0) //если жизней не осталось
        {
            textPanelNextLevel.text = "Когда-нибудь повезет..."; //меняем надпись
            panelNextLevel.SetActive(true); //включаем отображение панели
            Invoke("LoadExit", 3f); //запускаем метод выхода через 3 секунды
        }
        else
        {
            textLives.text = lives.ToString(); //выводим на экран новое значение
            ShowTextInfo("Упс.. Нужно больше заниматься )"); //установка текста            
        }
    }

    //метод скрывает информационный текст
    public void HideTextInfo()
    {
        textInfo.gameObject.SetActive(false); //спрятать текст
    }

    //метод для отображения информационного текста
    public void ShowTextInfo(string text)
    {
        textInfo.text = text; //установка текста
        textInfo.gameObject.SetActive(true); //показать текст
    }

    //метод выхода из игры
    public void LoadExit()
    {
        //работает только в редакторе
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; //остановка

        //работает только в скомпилированном приложении
#elif UNITY_STANDALONE  
        Application.Quit(); //выход из игры
#endif
    }

    //загрузка следующей сцены
    void NextLevel()
    {
        SceneManager.LoadScene(currentLevel+1);
    }
}
