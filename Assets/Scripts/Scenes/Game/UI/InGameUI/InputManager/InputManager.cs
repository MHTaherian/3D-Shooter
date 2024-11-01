using System;
using UnityEngine;

namespace Scenes.Game.UI.InGameUI.InputManager
{
    public abstract class InputManager : MonoBehaviour
    {
        /// <summary>
        /// Invokes a normalized Vector2 used to multiply a factor
        /// </summary>
        public abstract event Action<Vector2> OnInputValueUpdated;
        public abstract event Action OnInputTapped;
    }
}