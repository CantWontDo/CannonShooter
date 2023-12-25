using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ParticleEngine
{
    public class Particle
    {
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set;}
        public float Angle { get; set; }
        public float AngularVelocity { get; set; }
        public Color PColor { get; set; }
        public Color SecondColor { get; set; }
        public float Scale { get; set; }

        public float Speed { get; set; }
         
        public float Lifetime { get; set; }

        public float Gravity { get; set; }

        private int MaxLifeTime;

        private float CurrentScale;

        private Color CurrentColor;


        public Particle(Texture2D texture, Vector2 position, Vector2 velocity, float angle, float angularVelocity, Color color, float scale, int lifetime, float speed, float gravity, Color secondColor)
        {
            Texture = texture;
            Position = position;
            Velocity = velocity;
            Angle = angle;
            AngularVelocity = angularVelocity;
            PColor = color;
            CurrentColor = color;
            Scale = scale;
            Lifetime = lifetime;
            MaxLifeTime = lifetime;
            Speed = speed;
            Gravity = gravity;
            SecondColor = secondColor;
        }

        public void Update()
        {
            Lifetime -= (1 * Speed);

            CurrentScale = (float)(Scale * (Lifetime / MaxLifeTime));
            Position += Velocity;
            Angle += AngularVelocity;
        }

        public void Draw(SpriteBatch spritebatch)
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
            Vector2 origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
            
            spritebatch.Draw(Texture, Position, sourceRectangle, CurrentColor, Angle, origin, CurrentScale,
                SpriteEffects.None, 0f);
        }
    }
}
