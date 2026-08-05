using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    public float HorizontalInput { get; private set; }
    public event Action JumpInputPressed;
    public event Action JumpInputReleased;

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

    // Update is called once per frame
    void Update()
    {
        HorizontalInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump")) JumpInputPressed?.Invoke();
        if (Input.GetButtonUp("Jump")) JumpInputReleased?.Invoke();
    }
}