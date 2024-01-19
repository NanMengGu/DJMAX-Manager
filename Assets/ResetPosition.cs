using System;
using UnityEngine;

public class ResetPosition : MonoBehaviour
{
    void Start()
    {
        IntPtr hWnd = Win32API.GetActiveWindow();
        if (hWnd == IntPtr.Zero) Debug.Log("호출");
        Win32API.MoveWindow(hWnd, 0, 0, 350, 200, false);
    }
}
