using UnityEngine;

public class WardrobeManager : MonoBehaviour
{
    [Header("Sadaļu Paneļi")]
    [SerializeField] private GameObject[] categoryPanels;

    // Uzlabota metode ar drošības pārbaudēm
    public void SwitchCategory(int index)
    {
        // 1. Pārbaudām, vai indekss vispār eksistē sarakstā
        if (index < 0 || index >= categoryPanels.Length)
        {
            Debug.LogError($"Kļūda: Mēģina ieslēgt paneli ar indeksu {index}, bet sarakstā ir tikai {categoryPanels.Length} paneļi!");
            return;
        }

        // 2. Izslēdzam visus
        for (int i = 0; i < categoryPanels.Length; i++)
        {
            categoryPanels[i].SetActive(false);
        }

        // 3. Ieslēdzam vajadzīgo
        categoryPanels[index].SetActive(true);
    }
}