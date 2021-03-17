using System;
using System.Threading;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using GameProject3.StateManagement;


namespace GameProject3.Screens
{
    // This screen implements the actual game logic. It is just a
    // placeholder to get the idea across: you'll probably want to
    // put some more interesting gameplay in here!
    public class GameplayScreen : GameScreen
    {        
        private ContentManager _content;
        private SpriteFont _gameFont;

        private Vector2 _playerPosition = new Vector2(100, 100);
        private Vector2 _enemyPosition = new Vector2(100, 100);

        private readonly Random _random = new Random();

        private float _pauseAlpha;
        private readonly InputAction _pauseAction;


        //private GraphicsDeviceManager _graphics;
        //private SpriteBatch _spriteBatch;
        private SpriteFont spriteFont;
        private SpriteFont spriteFont2;
        private CharacterSprite dragon;
        //private EnemySprite[] mecheval;
        private PenguinKing pk;
        private Random rand;
        private MouseState mouseState;
        private double timer;
        private double pkTimer;
        private int fbTicker;
        private int feeshTicker = 0;

        private BallProjectileSprite[] fireballs = new BallProjectileSprite[64];
        private BallProjectileSprite[] feesh = new BallProjectileSprite[40];

        private int health = 5;
        private int pkHealth = 100;
        private bool winFlag = false;

        private SoundEffect dragonSound;
        private SoundEffect dragonHit;
        private SoundEffect pkSound;
        private SoundEffect pkHit;
        private Song backgroundMusic;


        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            _pauseAction = new InputAction(
                new[] { Buttons.Start, Buttons.Back },
                new[] { Keys.Back, Keys.Escape }, true);

            

        }

        // Load graphics content for the game
        public override void Activate()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            _gameFont = _content.Load<SpriteFont>("gamefont");

            // A real game would probably have more content than this sample, so
            // it would take longer to load. We simulate that by delaying for a
            // while, giving you a chance to admire the beautiful loading screen.
            Thread.Sleep(2500);

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();

            dragon = new CharacterSprite();


            //rand = new System.Random();
            pk = new PenguinKing(new Vector2(ScreenManager.GraphicsDevice.Viewport.Width - 102, ScreenManager.GraphicsDevice.Viewport.Height - 176));
            //base.Initialize();

            dragon.LoadContent(_content);
            spriteFont = _content.Load<SpriteFont>("bangers");
            spriteFont2 = _content.Load<SpriteFont>("bangers2");

            pk.LoadContent(_content);

            for (int i = 0; i < fireballs.Length; i++)
            {
                fireballs[i] = new BallProjectileSprite(_content, "Fireball");
            }
            for (int i = 0; i < feesh.Length; i++)
            {
                feesh[i] = new BallProjectileSprite(_content, "Feesh");
            }

            dragonSound = _content.Load<SoundEffect>("DragonShoot");
            dragonHit = _content.Load<SoundEffect>("hit");
            pkSound = _content.Load<SoundEffect>("PenguinShoot");
            pkHit = _content.Load<SoundEffect>("hit2");
            backgroundMusic = _content.Load<Song>("AIGeneratedBGM");
            MediaPlayer.Volume = 0.3f;
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(backgroundMusic);
        }


        public override void Deactivate()
        {
            base.Deactivate();
        }

        public override void Unload()
        {
            _content.Unload();
        }

        // This method checks the GameScreen.IsActive property, so the game will
        // stop updating when the pause menu is active, or if you tab away to a different application.
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (health < 1 && !winFlag)
            {
                LoadingScreen.Load(ScreenManager, true, ControllingPlayer, new EndScreen("You Lost! Try again!\nPress Enter To Return to the Menu"));
                MediaPlayer.Stop();
            }
            else if (pkHealth < 1)
            {
                winFlag = true;
                LoadingScreen.Load(ScreenManager, true, ControllingPlayer, new EndScreen("You Won!\nPress Enter To Return to the Menu"));
                MediaPlayer.Stop();
            }

            base.Update(gameTime, otherScreenHasFocus, false);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                _pauseAlpha = Math.Min(_pauseAlpha + 1f / 32, 1);
            else
                _pauseAlpha = Math.Max(_pauseAlpha - 1f / 32, 0);

            if (IsActive)
            {
                // Apply some random jitter to make the enemy move around.
                //const float randomization = 10;

                //_enemyPosition.X += (float)(_random.NextDouble() - 0.5) * randomization;
                //_enemyPosition.Y += (float)(_random.NextDouble() - 0.5) * randomization;

                // Apply a stabilizing force to stop the enemy moving off the screen.
                /*var targetPosition = new Vector2(
                    ScreenManager.GraphicsDevice.Viewport.Width / 2 - _gameFont.MeasureString("Insert Gameplay Here").X / 2,
                    200);

                _enemyPosition = Vector2.Lerp(_enemyPosition, targetPosition, 0.05f);
                */
                // This game isn't very fun! You could probably improve
                // it by inserting something more interesting in this space :-)
                dragon.Update(gameTime);

                if (dragon.position.Y > ScreenManager.GraphicsDevice.Viewport.Height - 64)
                {
                    dragon.grounded = true;
                }
                else dragon.grounded = false;


                dragon.Color = Color.White;

                mouseState = Mouse.GetState();
                timer += gameTime.ElapsedGameTime.TotalSeconds;
                if (mouseState.LeftButton == ButtonState.Pressed && timer > .5f)
                {
                    if (fbTicker > fireballs.Length - 1) fbTicker = 0;
                    fireballs[fbTicker].Shoot(dragon.position, new Vector2(mouseState.X, mouseState.Y), gameTime);
                    dragonSound.Play(.2f, 0, 0);
                    fbTicker++;
                    timer = 0;
                }

                foreach (BallProjectileSprite fb in fireballs)
                {
                    fb.Update(gameTime, dragon.position);
                }


                pkTimer += gameTime.ElapsedGameTime.TotalSeconds;


                pk.Update(dragon.position);

                if (pk.animationFrame == 0 && pkTimer > 1.2f)
                {
                    if (feeshTicker + 4 > feesh.Length - 1) feeshTicker = 0;
                    feesh[feeshTicker].Shoot(new Vector2(pk.position.X - 96, pk.position.Y - 16), new Vector2(dragon.position.X, dragon.position.Y - 80), gameTime, 2);
                    feesh[feeshTicker + 1].Shoot(new Vector2(pk.position.X - 96, pk.position.Y - 16), new Vector2(dragon.position.X, dragon.position.Y - 20), gameTime, 2);
                    feesh[feeshTicker + 2].Shoot(new Vector2(pk.position.X - 96, pk.position.Y - 16), new Vector2(dragon.position.X, dragon.position.Y + 20), gameTime, 2);
                    feesh[feeshTicker + 3].Shoot(new Vector2(pk.position.X - 96, pk.position.Y - 16), new Vector2(dragon.position.X, dragon.position.Y + 80), gameTime, 2);
                    pkSound.Play(.2f, 0, 0);
                    feeshTicker += 4;
                    pkTimer = 0;
                }

                foreach (BallProjectileSprite f in feesh) f.Update(gameTime, pk.position);

                foreach (BallProjectileSprite f in feesh)
                {
                    if (f.Bounds.CollidesWith(dragon.Bounds))
                    {
                        dragon.Color = Color.Red;
                        f.isVisible = false;
                        f.position = new Vector2(-1000, -1000);
                        health--;
                        dragonHit.Play(.4f, 0, 0);
                    }
                }

                foreach (BallProjectileSprite fb in fireballs)
                {
                    if (fb.Bounds.CollidesWith(pk.HeadBounds))
                    {
                        pk.color = Color.Red;
                        fb.isVisible = false;
                        fb.position = new Vector2(-1000, -1000);
                        pkHealth--;
                        pkHit.Play(.1f, 0, 0);
                    }
                }




            }
        }

        // Unlike the Update method, this will only be called when the gameplay screen is active.
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            var keyboardState = input.CurrentKeyboardStates[playerIndex];
            var gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected && input.GamePadWasConnected[playerIndex];

            PlayerIndex player;
            if (_pauseAction.Occurred(input, ControllingPlayer, out player) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            else
            {
                // Otherwise move the player position.
                var movement = Vector2.Zero;

                if (keyboardState.IsKeyDown(Keys.Left))
                    movement.X--;

                if (keyboardState.IsKeyDown(Keys.Right))
                    movement.X++;

                if (keyboardState.IsKeyDown(Keys.Up))
                    movement.Y--;

                if (keyboardState.IsKeyDown(Keys.Down))
                    movement.Y++;

                var thumbstick = gamePadState.ThumbSticks.Left;

                movement.X += thumbstick.X;
                movement.Y -= thumbstick.Y;

                if (movement.Length() > 1)
                    movement.Normalize();

                _playerPosition += movement * 8f;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background. Why? Because!
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);

            // Our player and enemy are both actually just text strings.
            var spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            //spriteBatch.DrawString(_gameFont, "// TODO", _playerPosition, Color.Green);
            //spriteBatch.DrawString(_gameFont, "Insert Gameplay Here",
            //                       _enemyPosition, Color.DarkRed);

            dragon.Draw(gameTime, spriteBatch);

            //_spriteBatch.DrawString(spriteFont, $"Angle?: {dragon.rotation}", new Vector2(2, 2), Color.Gold);

            pk.Draw(gameTime, spriteBatch);

            

            foreach (BallProjectileSprite fb in fireballs) fb.Draw(gameTime, spriteBatch);

            foreach (BallProjectileSprite f in feesh) f.Draw(gameTime, spriteBatch);

            spriteBatch.DrawString(spriteFont, $"Your Health: {health}       Boss Health: {pkHealth}", new Vector2(2, 2), Color.Black);

            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || _pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, _pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }
    }
}
