using Microsoft.Xna.Framework;
using FinalGame.StateManagement;
using System.Windows.Forms;

namespace FinalGame.Screens
{
    public class MainMenuScreen : MenuScreen
    {
        public MainMenuScreen() : base("Exceed (demo)")
        {
            var playGameMenuEntry = new MenuEntry("Play Game");
            var howToPlayMenuEntry = new MenuEntry("How To Play");
            var exitMenuEntry = new MenuEntry("Exit");

            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            howToPlayMenuEntry.Selected += HowToPlayMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            MenuEntries.Add(playGameMenuEntry);
            MenuEntries.Add(howToPlayMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }

        private void HowToPlayMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("WASD/Arrows to move\n" +
                "Left click to attack\n" +
                "Mouse to aim\n" +
                "Right click to throw teleport grenade, right click again to teleport to it\n" +
                "Be warned, you can only teleport where you fit!\n" +
                "Space to skip levels\n" +
                "Escape to quit" +
                "Destroy all the red enemies before they shoot you three times to win.\n" +
                "Good Luck!", "Tutorial", MessageBoxButtons.OK,  MessageBoxIcon.None);

        }

        private void PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new GameplayScreen());
        }

        protected override void OnCancel(PlayerIndex playerIndex)
        {
            DialogResult dialogResult = System.Windows.Forms.MessageBox.Show("Are you sure you want to exit?", "Message", MessageBoxButtons.OKCancel, MessageBoxIcon.None);//end game;
            if (dialogResult == DialogResult.OK)
            {
                ScreenManager.Game.Exit();
            }

        }

    }
}
