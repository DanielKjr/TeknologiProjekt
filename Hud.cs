using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TeknologiProjekt
{
    public class Hud
    {
        
        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(GameWorld.standardFont, $"Food: {GameObject.Food}\nWood: {GameObject.Wood}\nGold: {GameObject.Gold}", new Vector2(0, GameWorld.sceenSize.Y -65), Color.Black);

#if DEBUG
            spriteBatch.DrawString(GameWorld.standardFont, $"Foodresource: {GameObject.foodResource}\nWoodresource: {GameObject.woodResource}\nGoldresource: {GameObject.goldResource}", new Vector2(300, GameWorld.sceenSize.Y -65), Color.Black);
#endif
        }
    }
}
