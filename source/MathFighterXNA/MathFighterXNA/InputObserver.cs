using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


namespace ClownSchool {

    public static class InputObserver {

        public static TouchLocation OldTouchState { get; set; }
        public static TouchLocation TouchState { get; set; }

        public static MouseState OldMouseState { get; set; }
        public static MouseState MouseState { get; set; }

        public static bool IsUsingTouchScreen { get; private set; }

        public static void Update() {
            OldTouchState = TouchState;
            TouchState = TouchPanel.GetState().FirstOrDefault();
            //Debug.WriteLine(TouchPanel.GetState().Count);

            OldMouseState = MouseState;
            MouseState = Mouse.GetState();

            if (OldMouseState.X != MouseState.X || OldMouseState.Y != MouseState.Y || MouseState.LeftButton == ButtonState.Pressed) {
                IsUsingTouchScreen = false;
            }

            //Debug.WriteLine(TouchState.State == TouchLocationState.Invalid);
            if (TouchState != null && TouchState.State != TouchLocationState.Invalid) {
                IsUsingTouchScreen = true;
            }
        }
    }
}
