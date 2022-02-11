using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TeknologiProjekt
{
    public class Hud
    {
        /// <summary>
        /// Draws the player pool resources to the screen as well as a overall available ressource while in debug mode.
        /// Was intended to show more and have more options.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(GameWorld.standardFont, $"Food: {GameObject.Food}\nWood: {GameObject.Wood}\nGold: {GameObject.Gold}", new Vector2(500, GameWorld.sceenSize.Y -65), Color.Black);

#if DEBUG
            spriteBatch.DrawString(GameWorld.standardFont, $"Foodresource: {GameObject.foodResource}\nWoodresource: {GameObject.woodResource}\nGoldresource: {GameObject.goldResource}", new Vector2(300, GameWorld.sceenSize.Y -65), Color.Black,0,Vector2.Zero,1f,0,0);
#endif
        }
    }
}
