using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : UnitBase
{
    [SerializeField]
	public int _maximumMovement;

	[SerializeField]
	public float _stepConsumption;

	[SerializeField]
	public float _armor;

	[SerializeField]
	public float _armorWeakness;

	[SerializeField]
	public float _penetration;

	[SerializeField]
	public float[] _stealth = new float[6];
}
