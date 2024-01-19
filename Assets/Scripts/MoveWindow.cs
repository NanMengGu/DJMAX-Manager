using UnityEngine;
using System;
using System.Runtime.InteropServices;
using UnityEngine.UI;

public class MoveWindow : MonoBehaviour
{
    public Toggle movement;
    public Toggle movementX;
    public Toggle movementY;
    public InputField inputHandle;
    public InputField inputSpeedX;
    public InputField inputSpeedY;
    public Button apply;
    public int speedX;
    public int speedY;
    int finalSpeedX;
    int finalSpeedY;
    public bool isMovement;
    public bool isMovementX;
    public bool isMovementY;
    int monitorX = Win32API.GetSystemMetrics(0);
    int monitorY = Win32API.GetSystemMetrics(1);
    int aaa = Win32API.GetSystemMetrics(0) / 10;
    int bbb = Win32API.GetSystemMetrics(0) / 10;
    bool isLeft = false;
    bool isTop = true;
    string handle;
    IntPtr hWnd;

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
    public void OnHandleEndEdit()
    {
        handle = inputHandle.text;
    }

    public void OnClick()
    {
        InitializationValue();
        speedX = int.Parse(inputSpeedX.text);
        speedY = int.Parse(inputSpeedY.text);
    }

    public void OnToggleMovement()
    {
        isMovement = movement.isOn;
    }

    public void OnToggleMovementX()
    {
        isMovementX = movementX.isOn;
    }

    public void OnToggleMovementY()
    {
        isMovementY = movementY.isOn;
    }

    void InitializationValue()
    {
        hWnd = new IntPtr(Convert.ToInt32(handle, 16));
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
            finalSpeedX = isMovementX ? speedX : 0;
            finalSpeedY = isMovementY ? speedY : 0;
            if (isMovementX)
            {
                if (isLeft)
                {
                    if (finalSpeedX > 0) finalSpeedX *= -1;
                    if (rect.Left <= 0) isLeft = false;
                }
                else
                {
                    if (finalSpeedX < 0) finalSpeedX *= -1;
                    if (rect.Right >= monitorX) isLeft = true;
                }
            }

            if (isMovementY)
            {
                if (isTop)
                {
                    if (finalSpeedY > 0) finalSpeedY *= -1;
                    if (rect.Top <= 0) isTop = false;
                }
                else
                {
                    if (finalSpeedY < 0) finalSpeedY *= -1;
                    if (rect.Bottom >= monitorY) isTop = true;
                }
            }
            Win32API.MoveWindow(hWnd, rect.Left + (int)(finalSpeedX * Time.deltaTime), rect.Top + (int)(finalSpeedY * Time.deltaTime), rect.Right - rect.Left, rect.Bottom - rect.Top, false);
        }
    }
}