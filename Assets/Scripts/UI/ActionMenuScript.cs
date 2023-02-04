using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public delegate void ItemSelected(BodyPart bodyPart);

public class ActionMenuScript : MonoBehaviour
{
    public event ItemSelected ItemSelected;

    public Button ButtonPrefab;

    public void Show(List<BodyPart> bodyParts)
    {
        Button firstButton = null;
        for(var i = 0; i < bodyParts.Count; i++)
        {
            var bodyPart = bodyParts[i];
            var button = Instantiate(ButtonPrefab);
            button.transform.SetParent(transform);
            var textComponent = button.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            textComponent.text = bodyPart.Name;
            textComponent.fontSize = 0.2f;
            button.onClick.AddListener(() => ButtonPressed(bodyPart));

            if(i == 0)
            {
                firstButton = button;
            }
        }

        gameObject.SetActive(true);
        firstButton.Select();
    }

    private void ButtonPressed(BodyPart bodyPart) => ItemSelected?.Invoke(bodyPart);

    public void Hide()
    {
        foreach (Transform child in gameObject.transform)
        {
            Destroy(child.gameObject);
        }

        gameObject.SetActive(false);
    }
}
