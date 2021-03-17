using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameProject3.Collisions
{
    public struct BoundingCircle
    {

        /// <summary>
        /// Center of the bounding circle
        /// </summary>
        public Vector2 Center;

        /// <summary>
        /// Radius of bounding circle
        /// </summary>
        public float Radius;

        public BoundingCircle(Vector2 center, float radius)
        {
            Center = center;
            Radius = radius;
        }

        public bool CollidesWith(BoundingRectangle other)
        {
            return CollisionHelper.Collides(this, other);
        }

        /// <summary>
        /// tests for collision between this and another bounding circle
        /// </summary>
        /// <param name="other"></param>
        /// <returns>true for collision, false otherwise</returns>
        public bool CollidesWith(BoundingCircle other)
        {
            return CollisionHelper.Collides(this, other);
        }
    }
}
