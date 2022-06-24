using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : UnitBase
{
	[SerializeField]
	public int _maximumMovement = 5;

	[SerializeField]
	public float _stepConsumption = 1.5f;

	[SerializeField]
	public float _armor = 175.0f;

	[SerializeField]
    // percentage: 30%
	public float _armorWeakness = 30.0f;

	[SerializeField]
	public float _penetration = 180.0f;

	[SerializeField]
	public float[] _stealth = new float[]{70,50,40,30,20,10};
}
