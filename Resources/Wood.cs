using Microsoft.Xna.Framework;

namespace TeknologiProjekt
{
    public class Wood : Resource
    {
        public Wood(Vector2 _pos, int _resourceCapacity) : base(_pos, "Resources/Shed", _resourceCapacity)
        {
            woodResource += resourceCapacity;
        }
    }
}
