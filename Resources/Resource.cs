using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TeknologiProjekt
{
    public class Resource : GameObject
    {
        protected int resourceCapacity;
        public Resource(Vector2 _pos, string _spritePath, int _resourceCapacity) : base(_pos, _spritePath)
        {
            resourceCapacity = _resourceCapacity;
            spriteScale = 0.5f;
        }
    }
}
