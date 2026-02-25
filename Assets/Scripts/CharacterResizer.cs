using UnityEngine;
using UnityEngine.UI;

public class CharacterResizer : MonoBehaviour
{
    [Header("Tēla iestatījumi")]
    [SerializeField] private Transform characterTransform; // Ievelc tēlu grupu

    [Header("UI Slideri")]
    [SerializeField] private Slider widthSlider;
    [SerializeField] private Slider heightSlider;

    [Header("Mēroga robežas")]
    [SerializeField] private float minScale = 0.5f;
    [SerializeField] private float maxScale = 1.5f;

    void Start()
    {
        // Konfigurējam sliderus
        widthSlider.minValue = minScale;
        widthSlider.maxValue = maxScale;
        heightSlider.minValue = minScale;
        heightSlider.maxValue = maxScale;

        // Iestatām sākuma vērtību uz MAKSIMĀLO (kā prasīts)
        widthSlider.value = maxScale;
        heightSlider.value = maxScale;

        // Uzreiz piemērojam izmēru
        UpdateScale();

        // Pievienojam klausītājus
        widthSlider.onValueChanged.AddListener(delegate { UpdateScale(); });
        heightSlider.onValueChanged.AddListener(delegate { UpdateScale(); });
    }

    private void UpdateScale()
    {
        if (characterTransform != null)
        {
            characterTransform.localScale = new Vector3(widthSlider.value, heightSlider.value, 1f);
        }
    }
}