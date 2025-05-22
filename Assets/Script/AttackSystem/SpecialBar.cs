using UnityEngine;
using UnityEngine.UI;

public class SpecialBar : MonoBehaviour
{
    [SerializeField] private Slider barSlider;
    [SerializeField] private int maxCandy = 10;

    private int currentCandy;

    public int  CandyAmount => currentCandy;
    public int  MaxCandy    => maxCandy;
    public bool IsFull      => currentCandy >= maxCandy;  // ← new

    private void Awake()
    {
        if (barSlider == null) barSlider = GetComponent<Slider>();
        barSlider.minValue = 0;
        barSlider.maxValue = maxCandy;
        barSlider.value    = 0;
        currentCandy       = 0;
    }

    public void AddCandy(int amount = 1)
    {
        currentCandy = Mathf.Clamp(currentCandy + amount, 0, maxCandy);
        barSlider.value = currentCandy;
        Debug.Log($"[SpecialBar] currentCandy = {currentCandy}/{maxCandy}");
    }

    /// <summary>
    /// Returns true once when the bar hits max, then resets.
    /// </summary>
    public bool ConsumeFullBar()
    {
        if (currentCandy < maxCandy) return false;
        currentCandy = 0;
        barSlider.value = 0;
        return true;
    }
}