using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameoverScreen : MonoBehaviour
{
    public ScreenState state;

    public Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if(anim.GetBool("isEnded") || anim.GetBool("isGameOver"))
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                SceneManager.LoadScene("Start Scene");
            }
        }

    }

    public void MakeGameOver(bool check)
    {
        anim.SetBool("isGameOver", check);
    }

    public void MakeEnd()
    {
        anim.SetBool("isEnded", true);
    }

    void ChangeState(ScreenState newState)
    {
        if (state == newState) return;

        if (state == ScreenState.GameOver)
            anim.SetBool("isGameOver",false);
        else if (state == ScreenState.GameOverEnd)
            anim.SetBool("isEnded", false);

        state = newState;
        if (state == ScreenState.GameOver)
            anim.SetBool("isGameOver", true);
        else if (state == ScreenState.GameOverEnd)
            anim.SetBool("isEnded", true);
    }

}

public enum ScreenState
{
    GameOverIdle,
    GameOver,
    GameOverEnd,
}
