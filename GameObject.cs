using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Threading;

namespace TeknologiProjekt
{
    public abstract class GameObject
    {
        protected Vector2 position;
        protected Vector2 moveDir;
        protected Texture2D sprite;
        protected Color color = Color.White;
        protected float spriteScale = 1;

        protected Thread taskThread;
        protected Task taskState;

        private string spritePath;
        protected float moveSpeed;
        protected bool isAlive;

        protected int workerInventory = 0;
        public static int workerMaxInventory = 10;

        public static int goldResource = 0;
        public static int woodResource = 0;
        public static int foodResource = 0;
        public static int Wood = 50;
        public static int Food = 50;
        public static int Gold = 50;

        public Vector2 Position { get => position; set => position = value; }

        public GameObject(Vector2 _pos, string _spritePath)
        {
            position = _pos;
            spritePath = _spritePath;
        }
        public virtual void Update(GameTime gameTime)
        {
           
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
           
            spriteBatch.Draw(sprite, position, null, color, 0f, Vector2.Zero, spriteScale, SpriteEffects.None, 0f);

        }
        public virtual void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>(spritePath);
         
        }



        

    }

    public class Settlement : GameObject
    {
        public Settlement(Vector2 _pos) : base(_pos, "Resources/Settlement")
        {
            spriteScale = 0.5f;
        }
    }
}
