using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class CharacterSelector : MonoBehaviour
{
    [Header("Tēlu saraksts")]
    [SerializeField] private List<GameObject> characterModels;

    public void OnCharacterChanged(int index)
    {
        for (int i = 0; i < characterModels.Count; i++)
        {
            characterModels[i].SetActive(false);
        }

        if (index >= 0 && index < characterModels.Count)
        {
            characterModels[index].SetActive(true);
        }
    }
}