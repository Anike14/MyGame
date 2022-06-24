using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
	[SerializeField]
	int _maximumMovement = 5;

	[SerializeField]
	float _stepConsumption = 1.5f;

	[SerializeField]
	float _armor = 175.0f;

	[SerializeField]
    // percentage: 30%
	float _armorWeakness = 30.0f;

	[SerializeField]
	float _penetration = 180.0f;

	[SerializeField]
	float[] _stealth = new float[]{70,50,40,30,20,10};
}
