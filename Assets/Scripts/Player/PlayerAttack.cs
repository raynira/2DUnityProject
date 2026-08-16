using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject _melee;
    public Animator animator;

    public float _attackDuration = 0.3f;

    private bool _isAttacking = false;
    private float _attackTimer = 0f;

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

        animator.SetBool("IsAttacking", _isAttacking);
    }

    private void OnAttack()
    {
        if (!_isAttacking)
        {
            _isAttacking = true;
            _melee.SetActive(true);
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
