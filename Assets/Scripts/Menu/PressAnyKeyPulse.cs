using TMPro;
using UnityEngine;

public class PressAnyKeyPulse : MonoBehaviour
{
    [SerializeField]
    private TMP_Text pressAnyKeyText;

    [SerializeField]
    private float pulseSpeed = 2f;

    [SerializeField]
    private float minAlpha = 0.3f;

    [SerializeField]
    private float maxAlpha = 1f;

    private Color originalColor;

    /// <summary>
    ///
    /// /// </summary>
    void Start()
    {
        if (pressAnyKeyText == null)
            pressAnyKeyText = GetComponent<TMP_Text>();

        originalColor = pressAnyKeyText.color;
    }

    /// <summary>
    ///
    /// /// </summary>
    void Update()
    {
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f);
        pressAnyKeyText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
    }
}
