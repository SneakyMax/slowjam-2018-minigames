using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XInputDotNetPure;

namespace Game
{
    public static class InputManager
    {
        private static float KeyboardOrControllerAxis(IEnumerable<KeyCode> negative, IEnumerable<KeyCode> positive, Func<GamePadThumbSticks, float> getStick)
        {
            if (negative.Any(Input.GetKey))
                return -1;
            if (positive.Any(Input.GetKey))
                return 1;
            return getStick(GamePad.GetState(PlayerIndex.One).ThumbSticks);
        }

        private static bool KeyboardOrControllerButtonDown(IEnumerable<KeyCode> key, Func<GamePadButtons, ButtonState> getButton)
        {
            if (key.Any(Input.GetKeyDown))
                return true;

            return getButton(GamePad.GetState(PlayerIndex.One).Buttons) == ButtonState.Pressed;
        }

        private static bool KeyboardOrControllerButtonDown(IEnumerable<KeyCode> key, Func<GamePadButtons, bool> getButton)
        {
            if (key.Any(Input.GetKeyDown))
                return true;

            return getButton(GamePad.GetState(PlayerIndex.One).Buttons);
        }

        public static float Horizontal
        {
            get { return KeyboardOrControllerAxis(new [] { KeyCode.A, KeyCode.LeftArrow }, new [] { KeyCode.D, KeyCode.RightArrow }, x => x.Left.X); }
        }

        public static float Vertical
        {
            get { return KeyboardOrControllerAxis(new [] { KeyCode.W, KeyCode.UpArrow }, new [] { KeyCode.S, KeyCode.DownArrow }, x => x.Left.Y); }
        }

        public static bool Interact
        {
            get { return KeyboardOrControllerButtonDown(new[] { KeyCode.E, KeyCode.Return }, buttons => buttons.X); }
        }

        public static bool SecondaryInteract
        {
            get { return KeyboardOrControllerButtonDown(new[] { KeyCode.Q, KeyCode.RightShift }, buttons => buttons.X); }
        }

        public static bool Jump
        {
            get { return KeyboardOrControllerButtonDown(new[] { KeyCode.Space }, buttons => buttons.A); }
        }

        public static bool Ok
        {
            get { return KeyboardOrControllerButtonDown(new[] { KeyCode.Space, KeyCode.Return, KeyCode.E }, buttons => buttons.A == ButtonState.Pressed || buttons.X == ButtonState.Pressed); }
        }
    }
}