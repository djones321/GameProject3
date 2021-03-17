using Microsoft.Xna.Framework;
using GameProject3.StateManagement;

namespace GameProject3.Screens
{
    // The main menu screen is the first thing displayed when the game starts up.
    public class EndScreen : MenuScreen
    {
        public EndScreen(string result) : base(result)
        {
            var returnToMenuEntry = new MenuEntry("Return to Main Menu");


            //playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            //optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            //exitMenuEntry.Selected += OnCancel;
            returnToMenuEntry.Selected += ReturnToMenuEntrySelected;
            MenuEntries.Add(returnToMenuEntry);

            //MenuEntries.Add(returnToMenuEntry);
            //MenuEntries.Add(optionsMenuEntry);
            //MenuEntries.Add(exitMenuEntry);
        }

        private void ReturnToMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new MainMenuScreen());
        }

        private void OptionsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen(), e.PlayerIndex);
        }

        protected override void OnCancel(PlayerIndex playerIndex)
        {
            const string message = "Are you sure you want to exit this sample?";
            var confirmExitMessageBox = new MessageBoxScreen(message);

            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmExitMessageBox, playerIndex);
        }

        private void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }
    }
}
