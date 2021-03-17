using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GameProject3.Collisions;

namespace GameProject3
{
    public class BallProjectileSprite
    {
        private double timer;

        public Vector2 position;

        private Texture2D texture;

        private BoundingCircle bounds;

        public bool isVisible = false;

        public double direction;

        private Random random;

        private double speedScale;


        public BoundingCircle Bounds => bounds;

        public BallProjectileSprite(ContentManager content, String str)
        {
            random = new Random();
            speedScale = 1;
            LoadContent(content, str);
        }

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content, String str)
        {
            texture = content.Load<Texture2D>(str);
        }

        /// <summary>
        /// directs the fireball to its location
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="playerP"> Unused, deprecated, remove at later date </param>
        public void Update(GameTime gameTime, Vector2 playerP)
        {
            position += new Vector2((float)Math.Cos(direction) / 2, (float)Math.Sin(direction) / 2) * (float)(speedScale + 6);
            bounds.Center = position;
        }

        /// <summary>
        /// "creation" of a bullet, sets direction relative to two objects
        /// </summary>
        /// <param name="sourceVector"></param>
        /// <param name="directedVector"></param>
        /// <param name="gameTime"></param>
        public void Shoot(Vector2 sourceVector, Vector2 directedVector, GameTime gameTime)
        {
            direction = Math.Atan2(directedVector.Y - (double)sourceVector.Y, directedVector.X - (double)sourceVector.X);
            position = new Vector2(sourceVector.X + 56, sourceVector.Y - 8);
            isVisible = true;
            timer = gameTime.ElapsedGameTime.TotalSeconds + 8;

        }

        /// <summary>
        /// "creation" of a bullet, sets direction relative to two objects and randomized speed of bullet
        /// expand on these two, fewer hardcoded speeds
        /// </summary>
        /// <param name="sourceVector"></param>
        /// <param name="directedVector"></param>
        /// <param name="gameTime"></param>
        /// <param name="speedScale"></param>
        public void Shoot(Vector2 sourceVector, Vector2 directedVector, GameTime gameTime, double speedScale)
        {
            direction = Math.Atan2(directedVector.Y - (double)sourceVector.Y, directedVector.X - (double)sourceVector.X);
            position = new Vector2(sourceVector.X+56, sourceVector.Y-8);
            isVisible = true;
            this.speedScale = speedScale * random.NextDouble(); 
            timer = gameTime.ElapsedGameTime.TotalSeconds + 8;

        }
        /// <summary>
        /// Draws the animated sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!isVisible) return;

            if (gameTime.ElapsedGameTime.TotalSeconds > timer)
            {
                isVisible = false;
            }
            
            spriteBatch.Draw(texture, position, null, Color.White, 0, new Vector2(8,8), .5f, SpriteEffects.None, 0);
        }
    }
}
