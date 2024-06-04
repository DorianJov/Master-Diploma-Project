using UnityEngine;

public class HideCursorOnStart : MonoBehaviour
{
    void Start()
    {
        // Hide the cursor and lock it in the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Optionally, you can add methods to show the cursor again when needed
    public void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
