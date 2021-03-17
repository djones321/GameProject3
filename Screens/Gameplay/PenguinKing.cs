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
    class PenguinKing
    {
        public Vector2 position;

        //private Vector2 playerPosition;

        private Texture2D texture;

        private BoundingRectangle headBounds;

        public int animationFrame = 1;

        private double rotation;

        public bool Killed;

        private double animationTimer;

        public Color color = Color.White;


        public BoundingRectangle HeadBounds => headBounds;

        /// <summary>
        /// Creates a new coin sprite
        /// </summary>
        /// <param name="position">The position of the sprite in the game</param>
        public PenguinKing(Vector2 position)
        {
            this.position = position;
            this.headBounds = new BoundingRectangle(new Vector2(position.X, position.Y-60), 48, 224);
        }

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("PenguinKing");
        }

        public void Update(Vector2 playerP)
        {
            //position.X = Lerp(position.X, playerP.X, .005f);
            //position.Y = Lerp(position.Y, playerP.Y-32, .005f);
            //position = Vector2.Lerp(position, playerP, 50);

            //rotation = (Math.Atan2((double)playerP.Y - position.Y, (double)playerP.X - position.X));
            rotation = Math.Atan2((double)playerP.Y - position.Y, (double)playerP.X - position.X) + Math.PI;
            color = Color.White;
            //position += new Vector2((float)Math.Cos(rotation) / 2, (float)Math.Sin(rotation) / 2);
            //headBounds.Center = position;
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
            //if (Killed) return;
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if(animationTimer > 1.1f && animationTimer < 1.3f)
            {
                animationFrame = 0;
            }
            else if (animationTimer > 1.5f)
            {
                animationFrame = 1;
                animationTimer -= 1.5f;

            }
            //var source = new Rectangle(animationFrame * 16, 0, 16, 16);


            spriteBatch.Draw(texture, new Vector2(position.X-16, position.Y), new Rectangle(0, 64, 128, 256), color, 0, new Vector2(32, 32), 1f, 0, 0);
            spriteBatch.Draw(texture, new Vector2(position.X, position.Y), new Rectangle(animationFrame * 64, 0, 64, 64), color, (float)rotation, new Vector2(32, 32), 1.5f, 0, 0);
            

        }
    }
}

