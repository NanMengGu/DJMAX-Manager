using UnityEngine;
using System;
using UnityEngine.UI;

public class MoveWindow : MonoBehaviour
{
    public enum AnimationType
    {
        Bounce,
        Slide
    }
    [Header("Dropdown")]
    public Dropdown dropdownAnimationTypeX;
    [Header("Dropdown")]
    public Dropdown dropdownAnimationTypeY;

    public AnimationType animationTypeX;
    public AnimationType animationTypeY;

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

    bool isLeft = false;
    bool isTop = true;

    string handle;
    IntPtr hWnd;

    int windowWidth;
    int windowHeight;

    public void OnSpeedXEndEdit()
    {
        string value = inputSpeedX.text;
        if (!int.TryParse(value, out int result)) inputSpeedX.text = "";
    }

    public void OnSpeedYEndEdit()
    {
        string value = inputSpeedY.text;
        if (!int.TryParse(value, out int result)) inputSpeedY.text = "";
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
    public void OnValueChangedX()
    {
        animationTypeX = (AnimationType)dropdownAnimationTypeX.value;
    }

    public void OnValueChangedY()
    {
        animationTypeY = (AnimationType)dropdownAnimationTypeY.value;
        print(animationTypeY);
    }

    void InitializationValue()
    {
        hWnd = new IntPtr(Convert.ToInt32(handle, 16));
        Win32API.GetWindowRect(hWnd, out Win32API.RECT rect);

        windowWidth = rect.Right - rect.Left;
        windowHeight = rect.Bottom - rect.Top;

        int screenWidth = Win32API.GetSystemMetrics(0);
        int screenHeight = Win32API.GetSystemMetrics(1);

        int centerX = (screenWidth - windowWidth) / 2;
        int centerY = (screenHeight - windowHeight) / 2;

        Win32API.MoveWindow(hWnd, centerX, centerY, windowWidth, windowHeight, false);
    }

    void Update()
    {
        if (!isMovement) return;

        Win32API.GetWindowRect(hWnd, out Win32API.RECT rect);

        finalSpeedX = isMovementX ? speedX : 0;
        finalSpeedY = isMovementY ? speedY : 0;

        int newX = rect.Left;
        int newY = rect.Top;

        if (isMovementX)
        {
            if (animationTypeX == AnimationType.Bounce) newX = BounceX(rect);
            else newX = SlideX(rect);
        }

        if (isMovementY)
        {
            if (animationTypeY == AnimationType.Bounce) newY = BounceY(rect);
            else newY = SlideY(rect);
        }

        Win32API.MoveWindow(hWnd, newX, newY, windowWidth, windowHeight, true);
    }

    int BounceX(Win32API.RECT rect)
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

        return rect.Left + (int)(finalSpeedX * Time.deltaTime);
    }

    int BounceY(Win32API.RECT rect)
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

        return rect.Top + (int)(finalSpeedY * Time.deltaTime);
    }

    int SlideX(Win32API.RECT rect)
    {
        int newX = rect.Left + (int)(finalSpeedX * Time.deltaTime);

        if (finalSpeedX > 0)
        {
            if (rect.Left >= monitorX) newX = -windowWidth;
        }
        else if (finalSpeedX < 0)
        {
            if (rect.Right <= 0) newX = monitorX;
        }

        return newX;
    }

    int SlideY(Win32API.RECT rect)
    {
        int newY = rect.Top + (int)(finalSpeedY * Time.deltaTime);

        if (finalSpeedY > 0)
        {
            if (rect.Top >= monitorY) newY = -windowHeight;
        }
        else if (finalSpeedY < 0)
        {
            if (rect.Bottom <= 0) newY = monitorY;
        }

        return newY;
    }
}
