using System.Collections;

using UnityEngine;
using UnityEngine.Events;

public class UnitBase : MonoBehaviour
{
    [SerializeField]
    public GameObject _me;

    [SerializeField]
    private UnityEvent OnActivateActable;

    [SerializeField]
    private UnityEvent OnDeactivateActable;

    [SerializeField]
    protected UnityEvent<bool> OnScouted;

    [SerializeField]
    protected UnityEvent OnConcealed;

    [SerializeField]
    public string _unitType;
    
    [SerializeField]
    protected AudioSource _selectedAudio;

    protected bool _idled = false;

    [SerializeField]
    protected AudioSource _idleAudio;

    [SerializeField]
    protected AudioSource _movingAudio;

    [SerializeField]
    protected AudioSource _deselectedAudio;
    
	[SerializeField]
	public int _viewRange;
    
	[SerializeField]
	public float[] _stealth = new float[6];

	protected bool scoutPos = false;

	protected bool holdPos = false;

	protected bool hidePos = false;

    protected bool isMyTurn = false;

    public void PlayerUpdate(LayerMask myLayer, LayerMask enemyLayer) {
        if (this._unitType == Constants._unitType_Tank) {
            MapManager_Land.ScoutFromMyPosition((Tank)this, 
                (Vector2Int)MapManager_Land._tilemap.WorldToCell(this.gameObject.transform.position), enemyLayer);
        }
    }

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

	public bool IsScouting() {
		return scoutPos;
	}

	public void holding() {
		scoutPos = false;
		holdPos = true;
		hidePos = false;
	}

	public bool IsHolding() {
		return holdPos;
	}

	public void hiding() {
		scoutPos = false;
		holdPos = true;
		hidePos = true;
	}
	
	public bool IsHiding() {
		return hidePos;
	}

	protected int showingForEnemy = 0;

    protected bool _canMove = true;
    protected bool _canAct = true;
    protected bool _destroyed = false;

    public void PlaySelectedEffect() {
        if (_selectedAudio.isPlaying) return;
        _selectedAudio.Play();
        _idled = true;
        StartCoroutine(PlayIdleEffect());
    }

    private IEnumerator PlayIdleEffect() {
        yield return new WaitForSeconds(1.2f);
        if (_idled) _idleAudio.Play();
    }

    public void PlayMovingEffect() {
        if (_movingAudio.isPlaying) return;
        if (_canMove) {
            _movingAudio.Play();
        }
    }

    public void StopMovingEffect() {
        _movingAudio.Stop();
    }

    public void PlayDeselectedEffect() {
        _idled = false;
        if (_selectedAudio.isPlaying || _idleAudio.isPlaying) {
            _selectedAudio.Stop();
            _idleAudio.Stop();
            _deselectedAudio.Play();
        } else {
            _selectedAudio.Stop();
            _idleAudio.Stop();
        }
    }

    public bool IsMoving() {
        return _movingAudio.isPlaying;
    }

    public bool IsMovable() {
        return !_destroyed && _canMove;
    }

    public bool IsActable() {
        return !_destroyed && _canAct;
    }

    public bool IsScouted() {
        return showingForEnemy > 0;
    }

    public bool IsDestroyed() {
        return _destroyed;
    }

    public void ActivateDestroyed() {
        _destroyed = true;
    }

    public void myTurn() {
        this.isMyTurn = true;
        OnScouted?.Invoke(true);
        ActivateMovable();
    }

    public void enemyTurn() {
        this.isMyTurn = false;
        if (showingForEnemy > 0) {
            showingForEnemy--;
        }
        if (showingForEnemy == 0) {
            OnConcealed?.Invoke();
        }

        //Just paint the unit
        OnActivateActable?.Invoke();
    }

    public void Scouted() {
        if (showingForEnemy == 0) {
            OnScouted?.Invoke(false);
        }
        showingForEnemy = 2;
    }

    public bool IsConcealed() {
        return showingForEnemy == 0;
    }

    public void ActivateMovable() {
        if (_destroyed) return;
        _canMove = true;
        _canAct = true;
        OnActivateActable?.Invoke();
    }

    public void DeactivateMovable() {
        StopMovingEffect();
        _canMove = false;
    }

    public void DeactivateActable() {
        DeactivateMovable();
        _canAct = false;
        OnDeactivateActable?.Invoke();
    }
}
