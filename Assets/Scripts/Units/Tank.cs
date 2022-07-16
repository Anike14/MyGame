using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : UnitBase
{
    [SerializeField]
	public int _fireRange;
    [SerializeField]
	public int _maximumMovement;
	[SerializeField]
	public int _landExtraCost;
	[SerializeField]
	public int _waterExtraCost;
	[SerializeField]
	public int _seaExtraCost;
	[SerializeField]
	public int _grassExtraCost;
	[SerializeField]
	public int _forestExtraCost;
	[SerializeField]
	public int _rockExtraCost;
	[SerializeField]
	public int _mountainExtraCost;

	[SerializeField]
	public float _landStepConsumption;
	[SerializeField]
	public float _waterStepConsumption;
	[SerializeField]
	public float _seaStepConsumption;
	[SerializeField]
	public float _grassStepConsumption;
	[SerializeField]
	public float _forestStepConsumption;
	[SerializeField]
	public float _rockStepConsumption;
	[SerializeField]
	public float _mountainConsumption;

	[SerializeField]
	public float _penetration;

	[SerializeField]
	public float[] _armor;

	[SerializeField]
	public float[] _armorWeakness;

	[SerializeField]
	public float[] _stealth = new float[6];

	public void FireAt(Tank enemy) {

	}
}
