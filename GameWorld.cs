using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Threading;

namespace TeknologiProjekt
{
    public class GameWorld : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public static SpriteFont standardFont;
        protected Thread createWorkerThread;

        public static List<GameObject> gameObjects = new List<GameObject>();
        public static List<GameObject> newObjects = new List<GameObject>();
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
            _graphics.PreferredBackBufferHeight = 1000;
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
            gameObjects.Add(new Worker(new Vector2(sceenSize.X / 2 + 50, sceenSize.Y / 2 + 50), Task.Wood, 0));
            gameObjects.Add(new Worker(new Vector2(sceenSize.X / 2 + 50, sceenSize.Y / 2 + 50), Task.Food, 0, 0));

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

            //creates buttons for click events
            var minerButton = new Button(Content.Load<Texture2D>("Workers/MinerGirth"))
            {
                Position = new Vector2(1100, GameWorld.sceenSize.Y - 60),
                Text = "20 Gold & 50 Wood"
            };
            gameButtons.Add(minerButton);
            minerButton.minerClick += MinerButtonClick;

            var farmerButton = new Button(Content.Load<Texture2D>("Workers/FarmerGirth"))
            {
                Position = new Vector2(700, GameWorld.sceenSize.Y - 60),
                Text = "50 Gold & 20 Food"
            };
            gameButtons.Add(farmerButton);
            farmerButton.farmerClick += FarmerButtonClick;

            var lumberButton = new Button(Content.Load<Texture2D>("Workers/LumberGirth"))
            {
                Position = new Vector2(900, GameWorld.sceenSize.Y - 60),
                Text = "20 Gold & 50 Food"
            };
            gameButtons.Add(lumberButton);
            lumberButton.lumberClick += LumberButtonClick;

        }

        #region clickevent triggers
        /// <summary>
        /// Method is run on a click event, withdraws the ressource from the player pool, if they can afford to buy a worker
        /// and then starts a thread that instantiates the worker after a set time.
        /// Ideally this would be in a different class with parameter overloads instead of one for each, but we didn't have the time.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MinerButtonClick(object sender, EventArgs e)
        {
            createWorkerThread = new Thread(CreateMiner);
            createWorkerThread.IsBackground = true;
            GameObject.Gold -= 20;
            GameObject.Wood -= 50;
            createWorkerThread.Start();
        }

        /// <summary>
        /// Instantiates worker after 2000milisec
        /// </summary>
        private static void CreateMiner()
        {
            Thread.Sleep(2000);
            newObjects.Add(new Worker(new Vector2(sceenSize.X / 2 + 70, sceenSize.Y / 2 + 70), Task.Gold));
        }

        //creates Farmer
        private void FarmerButtonClick(object sender, EventArgs e)
        {
            createWorkerThread = new Thread(CreateFarmer);
            createWorkerThread.IsBackground = true;
            GameObject.Gold -= 50;
            GameObject.Food -= 20;
            createWorkerThread.Start();
        }

        private static void CreateFarmer()
        {
            Thread.Sleep(2000);
            newObjects.Add(new Worker(new Vector2(sceenSize.X / 2 + 70, sceenSize.Y / 2 + 70), Task.Food,0,0));
        }

        //creates Lumber
        private void LumberButtonClick(object sender, EventArgs e)
        {
            createWorkerThread = new Thread(CreateLumber);
            createWorkerThread.IsBackground = true;
            GameObject.Gold -= 20;
            GameObject.Food -= 50;
            createWorkerThread.Start();
        }

        private static void CreateLumber()
        {
            Thread.Sleep(2000);
            newObjects.Add(new Worker(new Vector2(sceenSize.X / 2 + 70, sceenSize.Y / 2 + 70), Task.Wood,0));
        }

        #endregion

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            foreach (var go in newObjects)
            {
                go.LoadContent(this.Content);
            }
            gameObjects.AddRange(newObjects);
            newObjects.Clear();

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

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
