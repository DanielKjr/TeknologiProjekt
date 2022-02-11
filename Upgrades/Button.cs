using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace TeknologiProjekt
{
    internal class Button : Component
    {

        private MouseState _currentMouse;
        private MouseState _previousMouse;

        private Texture2D _sprite;
        private SpriteFont font = GameWorld.standardFont;
        private bool isHovering;
        private float spriteScale = 1;


        public event EventHandler minerClick;
        public event EventHandler farmerClick;
        public event EventHandler lumberClick;


        public Vector2 Position { get; set; }
        public string Text { get; set; }
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, _sprite.Width, _sprite.Height);
            }
        }

        public Button(Texture2D sprite) 
        {
            _sprite = sprite;
            spriteScale = 0.5f;

        }


       
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var color = Color.White;

            if (isHovering == true)
            {
                color = Color.Gray;
            }
            

            spriteBatch.Draw(_sprite,Position, null, color, 0f, Vector2.Zero, spriteScale, SpriteEffects.None,0f);

            if (!string.IsNullOrEmpty(Text))
            {
                var x = (Rectangle.X + (Rectangle.Width / 2) + 20) - (font.MeasureString(Text).X / 2);
                var y = (Rectangle.Y + (Rectangle.Width / 2 + 30)) - (font.MeasureString(Text).Y / 2);

                spriteBatch.DrawString(GameWorld.standardFont, Text, new Vector2(x, y), Color.Black, 0, Vector2.Zero, 0.7f, 0, 0);
            }
        }

        /// <summary>
        /// Updates mouse rectangle for collision/checking if it intersects, if it does change color and invoke event if conditions are met.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();

            var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

            isHovering = false;

            if (mouseRectangle.Intersects(Rectangle))
            {
                isHovering = true;
                if (_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
                {
                    if (GameObject.Gold >= 20 && GameObject.Wood >= 50)
                    {
                        minerClick?.Invoke(this, new EventArgs());
                    }
                    if (GameObject.Gold >= 50 && GameObject.Food >= 20)
                    {
                        farmerClick?.Invoke(this, new EventArgs());
                    }
                    if (GameObject.Gold >= 20 && GameObject.Food >= 50)
                    {
                        lumberClick?.Invoke(this, new EventArgs());
                    }
                }
            }
        }
    }
}
