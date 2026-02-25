using UnityEngine;
using UnityEngine.UI;

public class ClothingResizer : MonoBehaviour
{
    [Header("UI Slideri drēbēm")]
    [SerializeField] private Slider clothingWidthSlider;
    [SerializeField] private Slider clothingHeightSlider;

    void Start()
    {
        // Iestatām robežas
        clothingWidthSlider.minValue = 0.5f;
        clothingWidthSlider.maxValue = 2.0f;
        clothingHeightSlider.minValue = 0.5f;
        clothingHeightSlider.maxValue = 2.0f;

        // Sākam ar MAKSIMĀLO izmēru (kā prasīts)
        clothingWidthSlider.value = 2.0f;
        clothingHeightSlider.value = 2.0f;

        // Pievienojam klausītājus
        clothingWidthSlider.onValueChanged.AddListener(delegate { ResizeEquipped(); });
        clothingHeightSlider.onValueChanged.AddListener(delegate { ResizeEquipped(); });
    }

    public void ResizeEquipped()
    {
        // Atrodam visus drēbju gabalus
        DraggableItem[] allItems = Object.FindObjectsOfType<DraggableItem>();

        foreach (DraggableItem item in allItems)
        {
            // Pārbaudām, vai drēbe ir uzvilkta (ir tēla bērns)
            if (item.transform.parent != null && item.transform.parent.CompareTag("Character"))
            {
                ApplyCurrentScaleToItem(item);
            }
        }
    }

    // ŠĪ IR FUNKCIJA, KURAS TRŪKA (tā ir publiska, lai citi skripti to redzētu)
    public void ApplyCurrentScaleToItem(DraggableItem item)
    {
        if (item != null)
        {
            item.ApplyRescale(clothingWidthSlider.value, clothingHeightSlider.value);
        }
    }
}