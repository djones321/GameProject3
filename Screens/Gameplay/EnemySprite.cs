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
    class EnemySprite
    {
        private Vector2 position;

        //private Vector2 playerPosition;
        
        private Texture2D texture;

        private BoundingCircle bounds;

        private double direction;

        public bool Killed;

        


        public BoundingCircle Bounds => bounds;

        /// <summary>
        /// Creates a new coin sprite
        /// </summary>
        /// <param name="position">The position of the sprite in the game</param>
        public EnemySprite(Vector2 position)
        {
            this.position = position;
            this.bounds = new BoundingCircle(position - new Vector2(-8, -8), 18);
        }

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Mecheval");
        }

        public void Update(Vector2 playerP)
        {
            //position.X = Lerp(position.X, playerP.X, .005f);
            //position.Y = Lerp(position.Y, playerP.Y-32, .005f);
            //position = Vector2.Lerp(position, playerP, 50);

            direction = Math.Atan2((double)playerP.Y - position.Y, (double)playerP.X+32 - position.X-32);
            position += new Vector2((float)Math.Cos(direction)/2, (float)Math.Sin(direction)/2);
            bounds.Center = position;
        }


        /*private float Lerp(float firstFloat, float secondFloat, float by)
        {
            return firstFloat * (1 - by) + secondFloat * by;
        }*/


        /// <summary>
        /// Draws the animated sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Killed) return;
            

            //var source = new Rectangle(animationFrame * 16, 0, 16, 16);
            spriteBatch.Draw(texture, position, null, Color.White);
        }
    }
}
