using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TigerTwo : Tank
{
    void start() {
        _unitType = Constants._unitType_Tank;

        _maximumMovement = 4;
        _landExtraCost = 0;
	    _waterExtraCost = 2;
        _grassExtraCost = 0;
	    _forestExtraCost = 3;
        _rockExtraCost = 1;

	    _landStepConsumption = 1.5f;
        _waterStepConsumption = 2f;
	    _grassStepConsumption = 1.5f;
	    _forestStepConsumption = 6f;
	    _rockStepConsumption = 9f;

        _armor = 175f;
        _armorWeakness = 30f;

        _penetration = 180f;

        _stealth = new []{70f,50f,40f,30f,20f,10f};
    }
}
