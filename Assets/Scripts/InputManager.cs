using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public float HorizontalInput { get; private set; }

    public event Action JumpInputPressed;
    public event Action JumpInputReleased;
    public event Action AttackInput;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Debug.Log(name);
        Instance = this;
    }

    void Update()
    {
        HorizontalInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump")) JumpInputPressed?.Invoke();
        if (Input.GetButtonUp("Jump")) JumpInputReleased?.Invoke();
        if (Input.GetButton("Fire1")) AttackInput?.Invoke();
    }
}