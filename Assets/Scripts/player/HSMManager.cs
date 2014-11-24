﻿using UnityEngine;
using System.Collections;
using gameData;

public class HSMManager : MonoBehaviour
{

    //Heatlh
    //Stamina
    //Mana

    const float staminaRegenrationRate = 4;
    const float manaRegenrationRate = 10f;

    util.DropdownMenu.outputData selfSpellData = new util.DropdownMenu.outputData();
    util.DropdownMenu.outputData targetSpellData = new util.DropdownMenu.outputData();

    [SerializeField]
    Rect selfSpellPos = new Rect(125, 50, 125, 300);
    [SerializeField]
    Rect targetSpellPos = new Rect(125, 50, 125, 300);
    [SerializeField]
    string[] selfSpells;
    [SerializeField]
    string[] targetSpells;

    [SerializeField]
    Texture staminaTexture;
    [SerializeField]
    Texture healthTexture;
    [SerializeField]
    Texture manaTexture;

    Rect ScreenCenter;
    [SerializeField]
    Texture CrossHair;

    [SerializeField]
    gameData.Stats.microData resistances;
    [SerializeField]
    gameData.Stats.microData weaknesses;

    void Start()
    {
        HSM.Player = transform;
        ScreenCenter = new Rect(Screen.width / 2f - 32, Screen.height / 2f - 32, 64, 64);
    }

    void Update()
    {
        stamina();
        magic();
    }

    void OnGUI()
    {
        GUI.DrawTexture(new Rect(5, 5, (HSM.Stamina / HSM.maxStamina * 120), 20), staminaTexture);
        GUI.TextArea(new Rect(5, 5, 120, 20), "stamina: " + Mathf.FloorToInt(HSM.Stamina) + "/" + HSM.maxStamina);

        GUI.DrawTexture(new Rect(5, 25, (HSM.Health / HSM.maxHealth * 120), 20), healthTexture);
        GUI.TextArea(new Rect(5, 25, 120, 20), "health: " + Mathf.FloorToInt(HSM.Health) + "/" + HSM.maxHealth);

        GUI.DrawTexture(new Rect(5, 45, (HSM.Mana / HSM.maxMana * 120), 20), manaTexture);
        GUI.TextArea(new Rect(5, 45, 120, 20), "mana: " + Mathf.FloorToInt(HSM.Mana) + "/" + HSM.maxMana);

        selfSpellData = util.DropdownMenu.dropDown(selfSpells, selfSpellPos, selfSpellData);
        targetSpellData = util.DropdownMenu.dropDown(targetSpells, targetSpellPos, targetSpellData);

        //crossHair
        GUI.DrawTexture(ScreenCenter, CrossHair);
    }

    void magic()
    {
        //regen
        if (HSM.Mana < HSM.maxMana)
        {
            HSM.Mana += manaRegenrationRate * Time.deltaTime;
            if (HSM.Mana > HSM.maxMana)
                HSM.Mana = HSM.maxMana;
        }
        //Spell Part
            if (Input.GetKeyDown(KeyCode.C))
                MagicSpells.CastSelf((MagicSpells.SelfSpells)selfSpellData.indexNumber);

            if (Input.GetKeyDown(KeyCode.V))
                MagicSpells.CastAoE((MagicSpells.AoESpells)targetSpellData.indexNumber);
    }

    void stamina()
    {
        if (!Input.GetButton(Axis.Sprint) && HSM.Stamina < HSM.maxStamina)
        {

            if (Input.GetAxis(Axis.Horizontal) == 0 && Input.GetAxis(Axis.Vertical) == 0)
            {
                HSM.Stamina += staminaRegenrationRate * Time.deltaTime;
            }
            else
            {
                HSM.Stamina += (staminaRegenrationRate / 2f) * Time.deltaTime;
            }
            if (HSM.Stamina > HSM.maxStamina)
                HSM.Stamina = HSM.maxStamina;
        }
        else if (Input.GetButton(Axis.Sprint))
            if (HSM.Stamina > 1 && Input.GetButton(Axis.Sprint))
            {
                if (Input.GetAxis(Axis.Horizontal) != 0 || Input.GetAxis(Axis.Vertical) != 0)
                    HSM.Stamina -= 5f * Time.deltaTime;
            }
    }

    void TakeDMG(gameData.Stats.dmgData data)
    {
        foreach (gameData.Stats.DMGTypes d in data.dmgTypes)
        {
            if (resistances.weak == d)
                data.dmg /= 1.25f;
            else if (resistances.mid == d)
                data.dmg /= 1.5f;
            else if (resistances.strong == d)
                data.dmg /= 2f;
            else if (weaknesses.weak == d)
                data.dmg *= 1.25f;
            else if (weaknesses.mid == d)
                data.dmg *= 1.5f;
            else if (weaknesses.strong == d)
                data.dmg *= 2f;
        }

        gameData.HSM.Health -= data.dmg;
    }
}
