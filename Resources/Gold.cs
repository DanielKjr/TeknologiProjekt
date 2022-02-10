using Microsoft.Xna.Framework;

namespace TeknologiProjekt
{
    public class Gold : Resource
    {
        public Gold(Vector2 _pos, int _resourceCapacity) : base(_pos, "Resources/GoldOre", _resourceCapacity)
        {
            goldResource += resourceCapacity;
        }


    }
}
