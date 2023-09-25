using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;

namespace CSC316Assignment2
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;


        Texture2D target_Sprite;
        Texture2D crosshair_Sprite;
        Texture2D background_Sprite;

        SpriteFont gameFont;

        Vector2 targetPosition = new Vector2(300, 300);
        Vector2 pos;

        MouseState mstate;

        float mouseTargetDist;
        const int TARGET_RADIUS = 45;

        int score = 0;
        bool mRelease = true;
        float timer = 10f;
        float countdown = 3f;

        SoundEffect quack;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            target_Sprite = Content.Load<Texture2D>("duck_target_white");
            crosshair_Sprite = Content.Load<Texture2D>("crosshair_blue_small");
            background_Sprite = Content.Load<Texture2D>("forest");

            gameFont = Content.Load<SpriteFont>("galleryFont");

            quack = Content.Load<SoundEffect>("quack");

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (countdown > 0)
            {
                countdown -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (timer > 0 && countdown <= 0)
            {
                timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            mstate = Mouse.GetState();
            mouseTargetDist = Vector2.Distance(targetPosition, new Vector2(mstate.X, mstate.Y));
            pos = new Vector2(mstate.X - 20, mstate.Y - 20);

            if (targetPosition.X > _graphics.PreferredBackBufferWidth || targetPosition.Y > _graphics.PreferredBackBufferHeight)
            {
                Random rand = new Random();
                targetPosition.X = rand.Next(0, _graphics.PreferredBackBufferWidth - TARGET_RADIUS + 1);
                targetPosition.Y = rand.Next(0, _graphics.PreferredBackBufferHeight - TARGET_RADIUS + 1);
            }

            targetPosition.X += 1;
            if (mstate.LeftButton == ButtonState.Pressed && mRelease == true)
            {
                if (mouseTargetDist < TARGET_RADIUS && timer > 0)
                {
                    score++;
                    quack.Play();
                    Random rand = new Random();
                    targetPosition.X = rand.Next(0, _graphics.PreferredBackBufferWidth - TARGET_RADIUS + 1);
                    targetPosition.Y = rand.Next(0, _graphics.PreferredBackBufferHeight - TARGET_RADIUS + 1);
                }
                mRelease = false;
            }
            if (mstate.LeftButton == ButtonState.Released)
            {
                mRelease = true;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _spriteBatch.Draw(background_Sprite, new Vector2(0, 0), Color.White);

            if (countdown > 0)
            {
                _spriteBatch.DrawString(gameFont, "Get Ready!".ToString(), new Vector2(300, 200), Color.Black);
                _spriteBatch.DrawString(gameFont, Math.Ceiling(countdown).ToString(), new Vector2(370, 250), Color.DarkBlue);
            }
            else if (timer > 0 && countdown < 0)
            {
                _spriteBatch.DrawString(gameFont, "Time Remaining: " + Math.Ceiling(timer).ToString(), new Vector2(0, 0), Color.Black);
                _spriteBatch.DrawString(gameFont, "Score: " + score.ToString(), new Vector2(0, 30), Color.Black);
                _spriteBatch.Draw(target_Sprite, new Vector2(targetPosition.X - TARGET_RADIUS, targetPosition.Y - TARGET_RADIUS), Color.White);
                _spriteBatch.Draw(crosshair_Sprite, pos, Color.White);
                IsMouseVisible = false;
            }
            else
            {
                _spriteBatch.DrawString(gameFont, "Times Up!", new Vector2(300, 200), Color.DarkRed);
                _spriteBatch.DrawString(gameFont, "Final Score: " + score.ToString(), new Vector2(270, 230), Color.DarkBlue);
                IsMouseVisible = true;
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}