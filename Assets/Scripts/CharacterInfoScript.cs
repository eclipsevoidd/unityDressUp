using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class CharacterInfoHandler : MonoBehaviour
{
    [Header("Input Fields")]
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private TMP_InputField yearInput;

    [Header("Output Text")]
    [SerializeField] private TextMeshProUGUI resultText;

    public void DisplayCharacterInfo()
    {
        string charName = nameInput.text;
        string yearString = yearInput.text;

        // pārbaudām, vai lauki nav tukši
        if (string.IsNullOrEmpty(charName) || string.IsNullOrEmpty(yearString))
        {
            resultText.text = "Lūdzu, aizpildi visus laukus!";
            return;
        }

        // pārvērš ievadīto gadu par skaitli
        if (int.TryParse(yearString, out int birthYear))
        {
            int currentYear = DateTime.Now.Year;
            int age = currentYear - birthYear;

            // nevar būt vecāks par 150
            if (age < 0 || age > 150)
            {
                resultText.text = "Ievadīts nederīgs dzimšanas gads!";
            }
            else
            {
                resultText.text = $"Prezidents {charName} ir {age} gadus vecs!";
            }
        }
        else
        {
            resultText.text = "Gada laukā drīkst būt tikai skaitļi!";
        }
    }
}