using UnityEngine;
using System;
using System.Runtime.InteropServices;
using UnityEngine.UI;

public class MoveWindow : MonoBehaviour
{
    public Toggle movement;
    public Toggle movementX;
    public Toggle movementY;
    public InputField inputSpeedX;
    public InputField inputSpeedY;
    public Button apply;
    public int speedX;
    public int speedY;
    public bool isMovement;
    public bool isMovementX;
    public bool isMovementY;
    int monitorX = Win32API.GetSystemMetrics(0);
    int monitorY = Win32API.GetSystemMetrics(1);
    bool isLeft = false;
    bool isTop = true;
    IntPtr hWnd = new IntPtr(Convert.ToInt32("0006065A", 16));

    public void OnSpeedXEndEdit()
    {
        string value = inputSpeedX.text;
        if (!int.TryParse(value, out int result)) inputSpeedX.text = "";
    }
    public void OnSpeedYEndEdit()
    {
        string value = inputSpeedY.text;
        if (!int.TryParse(value, out int result)) inputSpeedX.text = "";
    }

    public void OnClick()
    {
        speedX = int.Parse(inputSpeedX.text);
        speedY = int.Parse(inputSpeedY.text);
    }

    void Start()
    {
        int screenWidth = Win32API.GetSystemMetrics(0);
        int screenHeight = Win32API.GetSystemMetrics(1);

        Win32API.GetWindowRect(hWnd, out Win32API.RECT windowRect);
        int windowWidth = windowRect.Right - windowRect.Left;
        int windowHeight = windowRect.Bottom - windowRect.Top;

        int centerX = (screenWidth - windowWidth) / 2;
        int centerY = (screenHeight - windowHeight) / 2;

        Win32API.MoveWindow(hWnd, centerX, centerY, windowWidth, windowHeight, false);
    }

    void Update()
    {
        if (isMovement)
        {
            Win32API.GetWindowRect(hWnd, out Win32API.RECT rect);
            if (isMovementX)
            {
                if (isLeft)
                {
                    if (speedX == -1) speedX *= -1;
                    if (rect.Left <= 0)
                    {
                        isLeft = false;
                    }
                }
                else
                {
                    if (speedX == 1) speedX *= -1;
                    if (rect.Right >= monitorX)
                    {
                        isLeft = true;
                    }
                }
            }

            if (isMovementY)
            {
                if (isTop)
                {
                    if (speedY == -1) speedY *= -1;
                    if (rect.Top <= 0)
                    {
                        isTop = false;
                    }
                }
                else
                {
                    if (speedY == 1) speedY *= -1;
                    if (rect.Bottom >= monitorY)
                    {
                        isTop = true;
                    }
                }
            }
            Win32API.MoveWindow(hWnd, rect.Left + speedX, rect.Top + speedY, rect.Right - rect.Left, rect.Bottom - rect.Top, false);
        }
    }
}