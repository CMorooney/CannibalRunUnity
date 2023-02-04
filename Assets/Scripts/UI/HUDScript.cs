using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDScript : MonoBehaviour
{
    public Image PlayerHealthImage;
    public Image OrganHealthImage;
    public TextMeshProUGUI OrganNameLabel;

    public void UpdatePlayerHealth(float percent) => PlayerHealthImage.fillAmount = percent;
    public void UpdateOrganHealth(float percent) => OrganHealthImage.fillAmount = percent;

    public void UpdateOrgan(BodyPart bodyPart)
    { 
        if(bodyPart == null)
        {
            OrganNameLabel.text = string.Empty;
            OrganNameLabel.enabled = false;
            OrganHealthImage.enabled = false;
        }
        else
        {
            OrganNameLabel.text = bodyPart.Name;
            OrganNameLabel.enabled = true;
            OrganHealthImage.enabled = true;
        }
    }
}
