
#region File Description

//-----------------------------------------------------------------------------
// InputState.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

#endregion

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace GameStateManagement
{
    /// <summary>
    /// Helper for reading input from keyboard, gamepad, and touch input. This class 
    /// tracks both the current and previous state of the input devices, and implements 
    /// query methods for high level input actions such as "move up through the menu"
    /// or "pause the game".
    /// </summary>
    public class InputState
    {
        public const int MAX_INPUTS = 4;

        public readonly KeyboardState[] CurrentKeyboardStates;
        public readonly GamePadState[] CurrentGamePadStates;

        public readonly KeyboardState[] LastKeyboardStates;
        public readonly GamePadState[] LastGamePadStates;
        
        public readonly bool[] GamePadWasConnected;
        
        public TouchCollection TouchState;
        
        public readonly List<GestureSample> Gestures = new List<GestureSample>();
        
        /// <summary>
        /// Constructs a new input state.
        /// </summary>
        public InputState()
        {
            this.CurrentKeyboardStates = new KeyboardState[MAX_INPUTS];
            this.CurrentGamePadStates = new GamePadState[MAX_INPUTS];

            this.LastKeyboardStates = new KeyboardState[MAX_INPUTS];
            this.LastGamePadStates = new GamePadState[MAX_INPUTS];

            this.GamePadWasConnected = new bool[MAX_INPUTS];
        }
        
        /// <summary>
        /// Reads the latest state user input.
        /// </summary>
        public void Update()
        {
            for (int i = 0; i < MAX_INPUTS; i++)
            {
                this.LastKeyboardStates[i] = this.CurrentKeyboardStates[i];
                this.LastGamePadStates[i] = this.CurrentGamePadStates[i];

                this.CurrentKeyboardStates[i] = Keyboard.GetState((PlayerIndex)i);
                this.CurrentGamePadStates[i] = GamePad.GetState((PlayerIndex)i);
                
                // Keep track of whether a gamepad has ever been
                // connected, so we can detect if it is unplugged.
                if (this.CurrentGamePadStates[i].IsConnected)
                {
                    this.GamePadWasConnected[i] = true;
                }
            }

            // Get the raw touch state from the TouchPanel
            this.TouchState = TouchPanel.GetState();
            
            // Read in any detected gestures into our list for the screens to later process
            this.Gestures.Clear();
            while (TouchPanel.IsGestureAvailable)
            {
                this.Gestures.Add(TouchPanel.ReadGesture());
            }

        }
        
        /// <summary>
        /// Helper for checking if a key was pressed during this update. The
        /// controllingPlayer parameter specifies which player to read input for.
        /// If this is null, it will accept input from any player. When a keypress
        /// is detected, the output playerIndex reports which player pressed it.
        /// </summary>
        public bool IsKeyPressed(Keys key, PlayerIndex? controllingPlayer, out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                // Read input from the specified player.
                playerIndex = controllingPlayer.Value;
                
                int i = (int)playerIndex;
                
                return this.CurrentKeyboardStates[i].IsKeyDown(key);
            }
            else
            {
                // Accept input from any player.
                return (this.IsKeyPressed(key, PlayerIndex.One, out playerIndex) ||
                        this.IsKeyPressed(key, PlayerIndex.Two, out playerIndex) ||
                        this.IsKeyPressed(key, PlayerIndex.Three, out playerIndex) ||
                        this.IsKeyPressed(key, PlayerIndex.Four, out playerIndex));
            }
        }
        
        /// <summary>
        /// Helper for checking if a button was pressed during this update.
        /// The controllingPlayer parameter specifies which player to read input for.
        /// If this is null, it will accept input from any player. When a button press
        /// is detected, the output playerIndex reports which player pressed it.
        /// </summary>
        public bool IsButtonPressed(Buttons button, PlayerIndex? controllingPlayer, out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                // Read input from the specified player.
                playerIndex = controllingPlayer.Value;
                
                int i = (int)playerIndex;
                
                return this.CurrentGamePadStates[i].IsButtonDown(button);
            }
            else
            {
                // Accept input from any player.
                return (this.IsButtonPressed(button, PlayerIndex.One, out playerIndex) ||
                        this.IsButtonPressed(button, PlayerIndex.Two, out playerIndex) ||
                        this.IsButtonPressed(button, PlayerIndex.Three, out playerIndex) ||
                        this.IsButtonPressed(button, PlayerIndex.Four, out playerIndex));
            }
        }
        
        /// <summary>
        /// Helper for checking if a key was newly pressed during this update. The
        /// controllingPlayer parameter specifies which player to read input for.
        /// If this is null, it will accept input from any player. When a keypress
        /// is detected, the output playerIndex reports which player pressed it.
        /// </summary>
        public bool IsNewKeyPress(Keys key, PlayerIndex? controllingPlayer, out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                // Read input from the specified player.
                playerIndex = controllingPlayer.Value;
                
                int i = (int)playerIndex;
                
                return (this.CurrentKeyboardStates[i].IsKeyDown(key) &&
                        this.LastKeyboardStates[i].IsKeyUp(key));
            }
            else
            {
                // Accept input from any player.
                return (this.IsNewKeyPress(key, PlayerIndex.One, out playerIndex) ||
                        this.IsNewKeyPress(key, PlayerIndex.Two, out playerIndex) ||
                        this.IsNewKeyPress(key, PlayerIndex.Three, out playerIndex) ||
                        this.IsNewKeyPress(key, PlayerIndex.Four, out playerIndex));
            }
        }
        
        /// <summary>
        /// Helper for checking if a button was newly pressed during this update.
        /// The controllingPlayer parameter specifies which player to read input for.
        /// If this is null, it will accept input from any player. When a button press
        /// is detected, the output playerIndex reports which player pressed it.
        /// </summary>
        public bool IsNewButtonPress(Buttons button, PlayerIndex? controllingPlayer, out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                // Read input from the specified player.
                playerIndex = controllingPlayer.Value;
                
                int i = (int)playerIndex;
                
                return (this.CurrentGamePadStates[i].IsButtonDown(button) &&
                        this.LastGamePadStates[i].IsButtonUp(button));
            }
            else
            {
                // Accept input from any player.
                return (this.IsNewButtonPress(button, PlayerIndex.One, out playerIndex) ||
                        this.IsNewButtonPress(button, PlayerIndex.Two, out playerIndex) ||
                        this.IsNewButtonPress(button, PlayerIndex.Three, out playerIndex) ||
                        this.IsNewButtonPress(button, PlayerIndex.Four, out playerIndex));
            }
        }
    }
}