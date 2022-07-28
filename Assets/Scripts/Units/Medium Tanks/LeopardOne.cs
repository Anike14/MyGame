public class LeopardOne : Tank
{
    void Start(){
        _unitType = Constants._unitType_Tank;

        _maximumMovement = 5;
        _landExtraCost = 0;
	    _waterExtraCost = 2;
        _seaExtraCost = 100000;
        _grassExtraCost = 0;
	    _forestExtraCost = 2;
        _rockExtraCost = 1;
        _mountainExtraCost = 4;

	    _landStepConsumption = 1f;
        _waterStepConsumption = 1f;
        _seaStepConsumption = 100000f;
	    _grassStepConsumption = 1f;
	    _forestStepConsumption = 3f;
	    _rockStepConsumption = 5f;
        _mountainConsumption = 8f;

        _armor = new []{70f, 50f, 30f};
        _armorWeakness = new []{40f, 50f, 10f};

        _penetration = 260f;

        _stealth = new []{95f,85f,75f,60f,45f,30f};
    }

    public override string GetModelName() {
        return "Leopard-I";
    }

}
