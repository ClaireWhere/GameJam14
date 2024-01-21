using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam14.Game.GameSystem;
static class Input
{
    private static KeyboardState _keyboardState;
    private static KeyboardState _previousKeyboardState;
    private static MouseState _mouseState;
    private static MouseState _previousMouseState;

    public static Vector2 MousePosition
    {
        get
        {
            return new Vector2(_mouseState.X, _mouseState.Y);
        }
    }

    public static Vector2 MouseDelta
    {
        get
        {
            return new Vector2(_mouseState.X - _previousMouseState.X, _mouseState.Y - _previousMouseState.Y);
        }
    }

    public static Vector2 MouseAimDirection
    {
        get
        {
            return Vector2.Normalize(MousePosition);
        }
    }

    public static bool IsKeyDown(Keys key) => _keyboardState.IsKeyDown(key);
    public static bool IsKeyUp(Keys key) => _keyboardState.IsKeyUp(key);

    public static void Update()
    {
        _previousKeyboardState = _keyboardState;
        _previousMouseState = _mouseState;

        _keyboardState = Keyboard.GetState();
        _mouseState = Mouse.GetState();
    }

    public enum MouseButtonType
    {
        Left,
        Right,
        Middle
    }

    public static bool IsKeyPressed(Keys key)
    {
        return _keyboardState.IsKeyDown(key) && _previousKeyboardState.IsKeyUp(key);
    }

    public static bool IsKeyReleased(Keys key)
    {
        return _keyboardState.IsKeyUp(key) && _previousKeyboardState.IsKeyDown(key);
    }

    public static bool CheckMouseButtonState(MouseButtonType buttonType, ButtonState state)
    {
        ButtonState otherState = state == ButtonState.Pressed ? ButtonState.Released : ButtonState.Pressed;

        return buttonType switch
        {
            MouseButtonType.Left => _mouseState.LeftButton == state && _previousMouseState.LeftButton == otherState,
            MouseButtonType.Right => _mouseState.RightButton == state && _previousMouseState.RightButton == otherState,
            MouseButtonType.Middle => _mouseState.MiddleButton == state && _previousMouseState.MiddleButton == otherState,
            _ => false,
        };
    }
}
