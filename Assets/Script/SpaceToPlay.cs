using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SpaceToPlay : MonoBehaviour
{
    

    // Update is called once per frame
    void Update()
    {
       if (Keyboard.current.spaceKey.isPressed || Mouse.current.leftButton.wasPressedThisFrame)
        {
            SceneManager.LoadScene("Game");
            Time.timeScale = 1.0f;
        }
    }
}
