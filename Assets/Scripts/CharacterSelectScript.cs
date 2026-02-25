using UnityEngine;
using TMPro; // Nepieciešams TextMeshPro darbināšanai

public class CharacterSelector : MonoBehaviour
{
    [Header("Tēlu iestatījumi")]
    [SerializeField] private GameObject[] characters;

    [Header("7. Prasība: Apraksti")]
    [SerializeField] private TextMeshProUGUI descriptionDisplay; // Ievelc tekstu no Scroll View
    [SerializeField][TextArea(3, 10)] private string[] charDescriptions; // Ieraksti 3 aprakstus

    public void OnCharacterChanged(int index)
    {
        // 1. Pārslēdzam tēlu vizuāli
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].SetActive(i == index);
        }

        // 2. Nomainām tekstu Scroll View
        if (index >= 0 && index < charDescriptions.Length)
        {
            descriptionDisplay.text = charDescriptions[index];
        }
    }
}