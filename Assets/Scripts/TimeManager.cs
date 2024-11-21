using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    [SerializeField] GameObject TimeUI;
    [SerializeField] Sprite[] numericImages; 
    [SerializeField] Image hourImage;
    [SerializeField] Image minuteImage;

    public enum Season
    {
        Summer,
        Winter,
        Spring,
        Fall
    }

    public Season currentSeason = Season.Summer;
    public int dayNumber = 0;
    public int hour;
    public float minute;
    public float timeSpeed;

    private float plantGrowthTimer = 0f; // Temporizador para las plantas
    private float growthInterval = 5f;  // Intervalo de crecimiento (en segundos)

    private readonly Dictionary<Season, int> daysInSeason = new()
    {
        //My game, I set the rules how long a season is ;D
        {Season.Summer, 30},
        {Season.Winter, 20},
        {Season.Spring, 30},
        {Season.Fall, 35}
    };

    void Start()
    {
        /*hour = 6; minute = 0;*/
    }

    void Update()
    {
        CalculateGameTime();
        HandlePlantGrowth();
    }

    private void CalculateGameTime()
    {
        if (minute >= 60f)
        {
            hour += 1;
            minute = 0;
        }
        if (hour > 23)
        {
            dayNumber += 1;
            // =========== GROW PLANTS! ============
            GameManager.instance.tileManager.UpdateGrowthStages();
            hour = 0;
        }
        if (dayNumber > daysInSeason[currentSeason])
        {
            dayNumber = 0;
            currentSeason = GetNextSeason();
        }
        minute += Time.deltaTime * timeSpeed;
        hourImage.sprite = numericImages[hour];
        hourImage.SetNativeSize();
        minuteImage.sprite = numericImages[(int)minute];
        minuteImage.SetNativeSize();
    }

    private void HandlePlantGrowth()
    {
        // Incrementar el temporizador de crecimiento
        plantGrowthTimer += Time.deltaTime;

        // Verificar si ha pasado el intervalo
        if (plantGrowthTimer >= growthInterval)
        {
            plantGrowthTimer = 0f; // Reiniciar el temporizador
            GameManager.instance.tileManager.UpdateGrowthStages(); // Llamar al método de actualización
        }
    }

    private Season GetNextSeason()
    {
        
        switch (currentSeason)
        {
            case Season.Summer:
                return Season.Fall;
            case Season.Fall:
                return Season.Winter;
            case Season.Winter:
                return Season.Spring;
            case Season.Spring:
                return Season.Summer;
            default:
                return Season.Summer;
        }
    }

}
