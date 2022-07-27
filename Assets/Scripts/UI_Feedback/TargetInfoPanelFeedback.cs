using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class TargetInfoPanelFeedback : MonoBehaviour
{
    public void PlayFeedback(UnitBase unit, GameObject targetInfoPanel) {
        List<Text> texts = new List<Text>();
        targetInfoPanel.GetComponentsInChildren<Text>(texts);
        foreach (Text text in texts) {
            switch (text.name) {
                case "Model": 
                    text.text = "Model: " + unit.GetModelName();
                    break;
                case "ArmorStrongest": 
                    text.text = "The Strongest Armor: " + unit._armor[0];
                    break;
                case "ArmorStrongestCover": 
                    text.text = "The Strongest Armor Cover Rate: " + unit._armorWeakness[0] + "%";
                    break;
                case "ArmorMed": 
                    text.text = "The Generic Armor: " + unit._armor[1];
                    break;
                case "ArmorMedCover": 
                    text.text = "The Generic Armor Cover Rate: " + unit._armorWeakness[1] + "%";
                    break;
                case "ArmorWeak": 
                    text.text = "The Weak Point of Armor: " + unit._armor[2];
                    break;
                case "ArmorWeakCover": 
                    text.text = "The Weak Point Armor Rate: " + unit._armorWeakness[2] + "%";
                    break;
                case "GunPenetration": 
                    text.text = "Gunfire Penetration: " + unit._penetration;
                    break;
            }
        }
    }
}
