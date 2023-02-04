using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Game : MonoBehaviour
{
    public GameObject Cannibal;
    public GameObject HUD;
    public GameObject VictimManager;

    private CannibalScript _cannibalScript;
    private HUDScript _hudScript;
    private VictimManager _victimManagerScript;

    // some event handlers get called in `Start` so subscribing in `Awake` is important!
    private void Awake()
    {
        _cannibalScript = Cannibal.GetComponent<CannibalScript>();
        _hudScript = HUD.GetComponent<HUDScript>();
        _victimManagerScript = VictimManager.GetComponent<VictimManager>();

        _cannibalScript.HealthChanged += PlayerHealthChanged;
        _cannibalScript.BiteTaken += PlayerBiteTaken;
        _cannibalScript.InventoryChanged += PlayerInventoryChanged;
    }
    
    private void OnDestroy()
    {
        _cannibalScript.HealthChanged -= PlayerHealthChanged;
        _cannibalScript.BiteTaken -= PlayerBiteTaken;
        _cannibalScript.InventoryChanged -= PlayerInventoryChanged;
    }

    private void PlayerHealthChanged(float newPlayerHealthPercent) =>
        _hudScript.UpdatePlayerHealth(newPlayerHealthPercent);

    private void PlayerBiteTaken(float newOrganHealthPercent) =>
        _hudScript.UpdateOrganHealth(newOrganHealthPercent);

    private void PlayerInventoryChanged(BodyPart bodyPart) => 
        _hudScript.UpdateOrgan(bodyPart);
}

