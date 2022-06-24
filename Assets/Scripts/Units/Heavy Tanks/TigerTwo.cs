using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TigerTwo : Tank
{
    void start() {
        _unitType = "Tank";
        _maximumMovement = 4;
        _stepConsumption = 1.5f;
        _armor = 175f;
        _armorWeakness = 30f;
        _penetration = 180f;
        _stealth = new float[]{70,50,40,30,20,10};
    }
		
}
