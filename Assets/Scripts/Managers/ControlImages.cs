using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ControlImages : MonoBehaviour
{
    [Header("UI Component")]
    [SerializeField] private Image targetImage; // Drag your Unity Image component here

    [Header("Sprites")]
    [SerializeField] private Sprite imageA; // Controller Sprite
    [SerializeField] private Sprite imageB; // Keyboard/Mouse Sprite

    [Header("Input Setup")]
    [SerializeField] private PlayerInput playerInput; // Drag your PlayerInput component here

    private void OnEnable()
    {
        if (playerInput != null)
        {
            playerInput.onControlsChanged += OnControlsChanged;
            // Initial check on spawn
            UpdateImage(playerInput.currentControlScheme);
        }
    }

    private void OnDisable()
    {
        if (playerInput != null)
        {
            playerInput.onControlsChanged -= OnControlsChanged;
        }
    }

    private void OnControlsChanged(PlayerInput input)
    {
        UpdateImage(input.currentControlScheme);
    }

    private void UpdateImage(string controlScheme)
    {
        // Checks if the active control scheme name contains "Gamepad" or "Joystick"
        if (controlScheme.Contains("Gamepad") || controlScheme.Contains("Controller"))
        {
            targetImage.sprite = imageA;
        }
        else
        {
            targetImage.sprite = imageB;
        }
    }
}