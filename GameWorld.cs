using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace TeknologiProjekt
{
    public class GameWorld : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public static SpriteFont standardFont;

        public static List<GameObject> gameObjects = new List<GameObject>();
        public static List<Vector2> resourceLocations = new List<Vector2>();
        public static Vector2 sceenSize;

        private List<Component> gameButtons = new List<Component>();

        public GameWorld()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 1680;
            _graphics.PreferredBackBufferHeight = 1050;
            _graphics.ApplyChanges();
            sceenSize = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

            gameObjects.Add(new Settlement(new Vector2(sceenSize.X / 2, sceenSize.Y / 2)));
            resourceLocations.Add(new Vector2(sceenSize.X / 2, sceenSize.Y / 2));

            gameObjects.Add(new Gold(new Vector2(0, 900), 5000));
            gameObjects.Add(new Gold(new Vector2(1500, 900), 5000));
            resourceLocations.Add(new Vector2(0, 900));
            resourceLocations.Add(new Vector2(1500, 900));



            gameObjects.Add(new Wood(new Vector2(0, 500), 5000));
            gameObjects.Add(new Wood(new Vector2(1500, 500), 5000));
            resourceLocations.Add(new Vector2(0, 500));
            resourceLocations.Add(new Vector2(1500, 500));

            gameObjects.Add(new Food(new Vector2(0, 100), 5000));
            gameObjects.Add(new Food(new Vector2(1500, 100), 5000));
            resourceLocations.Add(new Vector2(0, 100));
            resourceLocations.Add(new Vector2(1500, 100));


            gameObjects.Add(new Worker(new Vector2(sceenSize.X / 2 + 50, sceenSize.Y / 2 + 50), Task.Gold));
            gameObjects.Add(new Worker(new Vector2(sceenSize.X / 2 + 50, sceenSize.Y / 2 + 50), Task.Wood));
            gameObjects.Add(new Worker(new Vector2(sceenSize.X / 2 + 50, sceenSize.Y / 2 + 50), Task.Food));
            var minerButton = new Button(Content.Load<Texture2D>("Resources/Settlement"))
            {
                Position = new Vector2(500, 500),
                //Text = "Miner"
            };
            gameButtons.Add(minerButton);
            minerButton.Click += MinerButtonClick;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            standardFont = Content.Load<SpriteFont>("StandardFont");
            foreach (GameObject go in gameObjects)
            {
                go.LoadContent(Content);
               

            }



        





        }

        private void MinerButtonClick(object sender, EventArgs e)
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            foreach (GameObject go in gameObjects)
            {
                go.Update(gameTime);
            }

            foreach (Component b in gameButtons)
            {
                b.Update(gameTime);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            foreach (GameObject go in gameObjects)
            {
                go.Draw(_spriteBatch);
            }
            foreach (var component in gameButtons)
            {
                component.Draw(gameTime, _spriteBatch);
            }

            Hud.Draw(_spriteBatch);

            _spriteBatch.DrawString(standardFont, $"{gameObjects.Count}", new Vector2(100, 100), Color.Black);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
