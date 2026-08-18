using UnityEngine;

public class FinishFlag : MonoBehaviour, IInteractable
{
    public InteractableObject[] _levers;
    public GameController game;
    public LevelsSurvivedSO survivedSO;

    [Header("Flag Position Parameters")]
    public Transform _revealPosition;
    public float _moveSpeed = 6f;

    private int _leversPulledCount;
    private bool _isFinished = false;
    private Vector3 _nextPosition;

    void Start()
    {
        _nextPosition = _revealPosition.position;
    }

    private void Update()
    {
        if (!_isFinished) return;

        transform.position = Vector3.MoveTowards(transform.position, _nextPosition, _moveSpeed * Time.deltaTime);
    }

    public bool CanInteract()
    {
        if (_isFinished) return true;

        return false;
    }

    public void Interact()
    {
        if (!CanInteract()) return;

        survivedSO.ResetState();
        game.VictoryScreen();
    }

    public void LeverPulled()
    {
        _leversPulledCount++;

        if (_leversPulledCount >= _levers.Length && !_isFinished)
        {
            _isFinished = true;
        }
    }
}
