using System;
using FinalGame.Screens;

namespace FinalGame.StateManagement
{
    /// <summary>
    /// Defines an object that can create a screen when given its type.
    /// </summary>
    public interface IScreenFactory
    {
        GameScreen CreateScreen(Type screenType);
    }
}
