using System.Collections;

using UnityEngine;
using UnityEngine.Events;

public class UnitBase : MonoBehaviour
{
    [SerializeField]
    private UnityEvent OnActivateActable;

    [SerializeField]
    private UnityEvent OnDeactivateActable;

    [SerializeField]
    public string _unitType;
    
    [SerializeField]
    private AudioSource _selectedAudio;

    private bool _idled = false;

    [SerializeField]
    private AudioSource _idleAudio;

    [SerializeField]
    private AudioSource _movingAudio;

    [SerializeField]
    private AudioSource _deselectedAudio;

    private bool _canMove = true;
    private bool _canAct = true;
    private bool _destroyed = false;

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

    public bool IsDestroyed() {
        return _destroyed;
    }

    public void ActivateDestroyed() {
        _destroyed = true;
    }

    public void ActivateMovable() {
        if (_destroyed) return;
        Debug.Log("activating actable....");
        _canMove = true;
        _canAct = true;
        OnActivateActable?.Invoke();
    }

    public void DeactivateMovable() {
        Debug.Log("deactivating movable....");
        StopMovingEffect();
        _canMove = false;
    }

    public void DeactivateActable() {
        Debug.Log("deactivating actable....");
        DeactivateMovable();
        _canAct = false;
        OnDeactivateActable?.Invoke();
    }
}
