using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace GameProject3
{
    public class LilDragon : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
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

        //private EnemySprite enemy1;



        public LilDragon()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            dragon = new CharacterSprite();


            //rand = new System.Random();
            pk = new PenguinKing(new Vector2(GraphicsDevice.Viewport.Width - 102, GraphicsDevice.Viewport.Height - 176));
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            dragon.LoadContent(Content);
            spriteFont = Content.Load<SpriteFont>("bangers");
            spriteFont2 = Content.Load<SpriteFont>("bangers2");

            pk.LoadContent(Content);

            for (int i = 0; i < fireballs.Length; i++)
            {
                fireballs[i] = new BallProjectileSprite(Content, "Fireball");
            }
            for (int i = 0; i < feesh.Length; i++)
            {
                feesh[i] = new BallProjectileSprite(Content, "Feesh");
            }

            dragonSound = Content.Load<SoundEffect>("DragonShoot");
            dragonHit = Content.Load<SoundEffect>("hit");
            pkSound = Content.Load<SoundEffect>("PenguinShoot");
            pkHit = Content.Load<SoundEffect>("hit2");
            backgroundMusic = Content.Load<Song>("AIGeneratedBGM");
            MediaPlayer.Volume = 0.3f;
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(backgroundMusic);

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            dragon.Update(gameTime);

            if (dragon.position.Y > GraphicsDevice.Viewport.Height-64)
            {
                dragon.grounded = true;
            }
            else dragon.grounded = false;
            

            dragon.Color = Color.White;

            mouseState = Mouse.GetState();
            timer += gameTime.ElapsedGameTime.TotalSeconds;
            if (mouseState.LeftButton == ButtonState.Pressed && timer > .5f)
            {
                if (fbTicker > fireballs.Length-1) fbTicker = 0;
                fireballs[fbTicker].Shoot(dragon.position, new Vector2(mouseState.X,mouseState.Y), gameTime);
                dragonSound.Play(.2f, 0, 0);
                fbTicker++;
                timer = 0;
            }

            foreach(BallProjectileSprite fb in fireballs)
            {
                fb.Update(gameTime, dragon.position);
            }


            pkTimer += gameTime.ElapsedGameTime.TotalSeconds;

            
            pk.Update(dragon.position);
            
            if(pk.animationFrame == 0 && pkTimer > 1.2f)
            {
                if (feeshTicker + 4 > feesh.Length - 1) feeshTicker = 0; 
                feesh[feeshTicker].Shoot(new Vector2(pk.position.X - 96, pk.position.Y-16), new Vector2(dragon.position.X, dragon.position.Y - 80), gameTime, 2);
                feesh[feeshTicker + 1].Shoot(new Vector2(pk.position.X - 96, pk.position.Y-16), new Vector2(dragon.position.X, dragon.position.Y-20), gameTime, 2);
                feesh[feeshTicker + 2].Shoot(new Vector2(pk.position.X - 96, pk.position.Y-16), new Vector2(dragon.position.X, dragon.position.Y + 20), gameTime, 2);
                feesh[feeshTicker + 3].Shoot(new Vector2(pk.position.X - 96, pk.position.Y-16), new Vector2(dragon.position.X, dragon.position.Y + 80), gameTime, 2);
                pkSound.Play(.2f, 0, 0);
                feeshTicker += 4;
                pkTimer = 0;
            }

            foreach (BallProjectileSprite f in feesh) f.Update(gameTime, pk.position);

            foreach(BallProjectileSprite f in feesh)
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

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            
            dragon.Draw(gameTime, _spriteBatch);

            //_spriteBatch.DrawString(spriteFont, $"Angle?: {dragon.rotation}", new Vector2(2, 2), Color.Gold);

            pk.Draw(gameTime, _spriteBatch);

            if (health < 1 && !winFlag)
            {
                _spriteBatch.DrawString(spriteFont2, "You Lost!", new Vector2(300, 200), Color.Black, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
            } else if(pkHealth < 1)
            {
                winFlag = true;
                _spriteBatch.DrawString(spriteFont2, "You Won!", new Vector2(300, 200), Color.Gold, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
            }

            foreach (BallProjectileSprite fb in fireballs) fb.Draw(gameTime, _spriteBatch);

            foreach (BallProjectileSprite f in feesh) f.Draw(gameTime, _spriteBatch);

            _spriteBatch.DrawString(spriteFont, $"Your Health: {health}       Boss Health: {pkHealth}", new Vector2(2, 2), Color.Black);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
