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
    public class CharacterSprite
    {
        private GamePadState gamePadState;
        private KeyboardState keyboardState;
        private GamePadState prevGamePadState;
        private KeyboardState prevKeyboardState;

        private Texture2D texture;
        private short animationFrame = 0;
        private double animationTimer;

        public Vector2 position = new Vector2(200, 200);

        public Vector2 velocity;
        public Vector2 direction;
        public Vector2 nextPosition = new Vector2(200,200);
        public Vector2 gravity = new Vector2(0, 0.1f);
        public Vector2 friction = new Vector2(0.09f, 0);
        public bool grounded = false;
        public bool jumpFlag = false;
        public bool keyFlag = false;

        private bool flipped;
        private MouseState mouseState;
        public double rotation;

        /*private BoundingCircle bounds = new BoundingCircle(new Vector2(200, 200), 16);

        public BoundingCircle Bounds => bounds; //equivalent to { get; set; }*/
        private BoundingRectangle bounds; // = new BoundingRectangle(new Vector2(16, 0), 0, 0);




        public BoundingRectangle Bounds => bounds;

        public Color Color { get; set; } = Color.White;

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("DragonSprite2");
            bounds = new BoundingRectangle(new Vector2(position.X, position.Y), 48, 84);
        }

        /// <summary>
        /// Updates the sprite's position based on user input
        /// </summary>
        /// <param name="gameTime">The GameTime</param>
        public void Update(GameTime gameTime)
        {
            keyFlag = false;
            if (!grounded && !jumpFlag)
            {
                velocity += gravity;
                position.Y += velocity.Y;
            }
            else if(grounded && jumpFlag)
            {
                velocity.Y = 0;
                position -= new Vector2(0, 1);
            }
            else
            {
                jumpFlag = true;
            }

            prevGamePadState = gamePadState;
            prevKeyboardState = keyboardState;
            gamePadState = GamePad.GetState(0);
            keyboardState = Keyboard.GetState();

            mouseState = Mouse.GetState();

            rotation = Math.Atan2((double)mouseState.Y - position.Y, (double)mouseState.X - position.X);

            if (rotation > 1.59 || rotation < -1.59) flipped = true;
            else flipped = false;
            // Apply the gamepad movement with inverted Y axis
            position += gamePadState.ThumbSticks.Left * new Vector2(1, -1);
            if (gamePadState.ThumbSticks.Left.X < 0) flipped = true;
            if (gamePadState.ThumbSticks.Left.X > 0) flipped = false;

            // Apply keyboard movement
            //if ((keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W)) && (prevKeyboardState.IsKeyUp(Keys.Up) || prevKeyboardState.IsKeyUp(Keys.W))) position += new Vector2(0, -10);
            if (keyboardState.IsKeyDown(Keys.W) ||//&& prevKeyboardState.IsKeyUp(Keys.W)) || 
                keyboardState.IsKeyDown(Keys.Space)|| //&& prevKeyboardState.IsKeyUp(Keys.Space)) ||
                gamePadState.IsButtonDown(Buttons.A)) //&& prevGamePadState.IsButtonDown(Buttons.A)))
            {
                grounded = false;
                velocity = new Vector2(velocity.X, -1f);
                position.Y += velocity.Y;
                jumpFlag = false;
                //animationFrame = 1;
                keyFlag = true;
            }   //position -= gravity;



            //if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S)) position += new Vector2(0, 1);
            if (keyboardState.IsKeyDown(Keys.A))
            {
                velocity.X = -2;
                keyFlag = true;
                //position += new Vector2(-2, 0);
                //flipped = true;
            }
            
            
            if (keyboardState.IsKeyDown(Keys.D))
            {
                velocity.X = 2;
                keyFlag = true;
                //position += new Vector2(2, 0);
                //flipped = false;
            }


            if (velocity.X > 0.12f)
            {
                velocity -= friction;
            }
            else if (velocity.X < -.12f)
            {
                velocity += friction;
            }
            else velocity.X = 0;

            position.X += velocity.X;
            //update sprite's collision bounds
            //bounds.Center = position;



            bounds.X = position.X+16;
            bounds.Y = position.Y-44;
        }

        /// <summary>
        /// Draws the sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (animationTimer > 0.3 && jumpFlag && keyFlag)
            {
                if (animationFrame == 0) animationFrame = 2;
                else animationFrame = 0;
                animationTimer -= 0.3;
            }
            else if (animationTimer > 0.3 && !grounded && keyFlag)
            {
                animationFrame++;
                if (animationFrame > 1)
                {
                    animationFrame = 0;
                }
                animationTimer -= 0.3;

            }


            //SpriteEffects spriteEffects = (flipped) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            SpriteEffects headFlip = (flipped) ? SpriteEffects.FlipVertically : SpriteEffects.None;
            SpriteEffects bodyFlip = (flipped) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(texture, new Vector2(position.X+62, position.Y-4), new Rectangle(192, 0, 64, 64), Color, (float)rotation, new Vector2(8, 34), 1f, headFlip, 0);
            spriteBatch.Draw(texture, new Vector2(position.X + 32, position.Y), new Rectangle(animationFrame * 64, 0, 64, 64), Color.White, 0, new Vector2(0,0), 1f, bodyFlip, 0);
        }
    }
}
