using System.Collections;

using UnityEngine;
using UnityEngine.Events;

public class Tank : UnitBase
{
    [SerializeField]
    private UnityEvent OnFiring;

    [SerializeField]
    private AudioSource _fireAudio;

    [SerializeField]
    private AudioSource _fireRadio;

    [SerializeField]
    private AudioSource _targetDestroyedRadio;

    [SerializeField]
    private UnityEvent OnPenetrated;

    [SerializeField]
    private AudioSource _getPenetratedAudio;

    [SerializeField]
    private UnityEvent OnScratched;

    [SerializeField]
    private AudioSource _getScratchedAudio;

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

	public void FireAt(Tank enemy) {
		/**
			https://stackoverflow.com/questions/44246629/coroutine-and-waitforseconds-not-working-as-planned:

			Similar questions have been asked many times before, on StackOverflow and on Unity Answers.
			Coroutines do not pause the execution of the function they have been called into. You have to put your logic inside a big coroutine instead :
		**/
		StartCoroutine(Fire(enemy));
	}

	private IEnumerator Fire(Tank enemy) {
		// play fire feedback
		_fireRadio.Play();
		// let's have a 0.35s waiting for playing the radio
		yield return new WaitForSeconds(0.35f);
		_fireAudio.Play();
		
		if (!isMyTurn) {
			if (this.showingForEnemy == 0)
            	OnScouted?.Invoke();
			else this.Scouted();
		}
		// let's have a 0.35s waiting for playing the main gun sound
		yield return new WaitForSeconds(0.35f);
		OnFiring?.Invoke();

		double randomNum = Random.Range(1, 100);
		float randomPenetrationRatio = 1 + Random.Range(-_gunStability, _gunStability);
		Debug.Log("randomNum: " + randomNum);
		bool penetrationResult = false;
		for (int i = 0; i < 3; i++) {
			Debug.Log("enemy._armorWeakness[i]: " + enemy._armorWeakness[i]);
			if (randomNum < enemy._armorWeakness[i]) {
				penetrationResult = this._penetration * randomPenetrationRatio > 
					(IsHolding() ? enemy._armor[i] * 1.05 : enemy._armor[i]);
				Debug.Log("this._penetration: " + this._penetration);
				Debug.Log("enemy._armor[i]: " + enemy._armor[i]);
				break;
			} else randomNum = randomNum - enemy._armorWeakness[i];
		}
		
		// let's have a 0.8s waiting for the shell to arrive
		yield return new WaitForSeconds(0.8f);
		if (penetrationResult) {
			// play enemy calculation feedback
			enemy.getPenetrated();
			// let's have a 0.8s waiting for the penetration effect to play
			yield return new WaitForSeconds(0.8f);
			_targetDestroyedRadio.Play();
		} else {
			// play enemy calculation feedback
			enemy.getScratched();
		}
		this.PlayDeselectedEffect();
		this.DeactivateActable();
	}

	private void getPenetrated() {
		_getPenetratedAudio.Play();
		OnPenetrated?.Invoke();
	}

	private void getScratched() {
		_getScratchedAudio.Play();
		OnScratched?.Invoke();
	}
}
