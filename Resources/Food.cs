using Microsoft.Xna.Framework;

namespace TeknologiProjekt
{
    public class Food : Resource
    {
        public Food(Vector2 _pos, int _resourceCapacity) : base(_pos, "Resources/Granary", _resourceCapacity)
        {
            foodResource += resourceCapacity;
        }
    }
}
