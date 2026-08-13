using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject _melee;

    bool _isAttacking = false;
    float _attackDuration = 0.3f;
    float _attackTimer = 0f;

    void OnEnable()
    {
        InputManager.Instance.AttackInput += OnAttack;
    }

    void OnDisable()
    {
        InputManager.Instance.AttackInput -= OnAttack;
    }

    void Awake()
    {
        _melee.SetActive(false);
    }

    void Update()
    {
        CheckMeleeTimer();
    }

    private void OnAttack()
    {
        if (!_isAttacking)
        {
            _melee.SetActive(true);
            _isAttacking = true;
        }
    }

    void CheckMeleeTimer()
    {
        if (_isAttacking)
        {
            _attackTimer += Time.deltaTime;

            if (_attackTimer >= _attackDuration)
            {
                _attackTimer = 0;
                _isAttacking = false;
                _melee.SetActive(false);
            }
        }
    }
}
