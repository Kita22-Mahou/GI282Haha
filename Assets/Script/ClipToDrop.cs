using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClipToDrop : MonoBehaviour
{
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Destroy(gameObject);
        }
    }
}
