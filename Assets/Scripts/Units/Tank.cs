using UnityEngine;
using UnityEngine.Events;

public class Tank : UnitBase
{
    [SerializeField]
    private UnityEvent OnFiring;
    [SerializeField]
    private UnityEvent OnPenetrated;
    [SerializeField]
    private UnityEvent OnScratched;

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
	public float _gunStability;

	[SerializeField]
	public float[] _armor;

	[SerializeField]
	public float[] _armorWeakness;

	[SerializeField]
	public float[] _stealth = new float[6];

	private bool scoutPos = false;

	private bool holdPos = false;

	private bool hidePos = false;

	public void ResetPos() {
		if (this.IsDestroyed()) return;
		scoutPos = false;
		holdPos = false;
		hidePos = false;
	}

	public void Scouting() {
		scoutPos = true;
		holdPos = false;
		hidePos = false;
	}

	public void holding() {
		scoutPos = false;
		holdPos = true;
		hidePos = false;
	}

	public void hiding() {
		scoutPos = false;
		holdPos = true;
		hidePos = true;
	}

	public void FireAt(Tank enemy) {
		// play fire feedback
		this.fire();

		int randomNum = Random.Range(1, 100);
		float randomPenetrationRatio = 1 + Random.Range(-_gunStability, _gunStability);
		bool penetrationResult = false;
		for (int i = 2; i >= 0; i--) {
			if (randomNum < enemy._armorWeakness[i]) {
				penetrationResult = this._penetration * randomPenetrationRatio > 
					(holdPos ? enemy._armor[i] * 1.05 : enemy._armor[i]);
				break;
			}
		}
		
		if (penetrationResult) {
			// play enemy calculation feedback
			Debug.Log("penetration!");
			enemy.getPenetrated();
		} else {
			// play enemy calculation feedback
			Debug.Log("scratch!!!");
			enemy.getScratched();
		}
	}

	private void fire() {
		OnFiring?.Invoke();
	}

	private void getPenetrated() {
		OnPenetrated?.Invoke();
	}

	private void getScratched() {
		OnScratched?.Invoke();
	}
}
