using UnityEngine;

public class HungerBar : MonoBehaviour
{
    private Vector3 originalScale;
    private float maxHunger = 100f;
    private static float currentHunger = 0f;
    private float hungerIncreaseRate;

    void Start()
    {
        originalScale = transform.localScale;
        hungerIncreaseRate = maxHunger / (5 * 60); // Расчет скорости увеличения голода для достижения максимума за 5 минут
    }

    void Update()
    {
        IncreaseHunger();
        UpdateHungerBar();
        CheckDead();
    }

    public static void Feed(float value)
    {
        currentHunger -= value;
        currentHunger = Mathf.Max(currentHunger, 0f);
    }

    void CheckDead()
    {
        if (currentHunger == 0f)
        {
            PlayerBattleController.Die();
        }
    }

    void IncreaseHunger()
    {
        currentHunger += hungerIncreaseRate * Time.deltaTime;
        currentHunger = Mathf.Min(currentHunger, maxHunger);
    }

    void UpdateHungerBar()
    {
        float hungerScale = 1 - (currentHunger / maxHunger);
        transform.localScale = new Vector3(hungerScale * originalScale.x, originalScale.y, originalScale.z);
    }
}