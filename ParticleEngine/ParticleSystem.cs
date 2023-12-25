using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ParticleEngine
{
    public class ParticleSystem
    {
        private Random random;
        public Vector2 EmitterLocation { get; set; }

        public float Direction { get; set; }
        private List<Particle> particles;
        private List<Texture2D> textures;
        private int amount { get; set; }

        public ParticleSystem(int amount, Vector2 emitterLocation, List<Texture2D> textures)
        {
            random = new Random();
            EmitterLocation = emitterLocation;
            this.textures = textures;
            particles = new List<Particle>();
            this.amount = amount;
        }
        Vector2 velocity = Vector2.Zero;
        public void setVelocity(Vector2 newVelocity)
        {
            velocity = newVelocity;
        }
        private Particle GenerateNewParticle()
        {
            Texture2D texture = textures[random.Next(textures.Count)];
            Vector2 Position = EmitterLocation;
            velocity = Direction * new Vector2((float)(random.NextDouble() * 2 - 1));
            float angle = 0;
            float angularVelocity = 0.1f * 1f * (float)(random.NextDouble() * 2 - 1);
            Color color = new Color(
                (float)(random.NextDouble() * .05) + 0.9f,
                (float)(random.NextDouble() * .05) + 0.85f,
                (float)(random.NextDouble() * .05) + 1f);

            float scale = (0.05f * random.Next(2));
            int lifetime = 40 + random.Next(40);
            return new Particle(texture, Position, velocity, angle, angularVelocity, color, scale, lifetime, 1f, 1f, color);
        }

        public void Update()
        {
            for(int i = 0; i < amount; i++)
            {
                particles.Add(GenerateNewParticle());
            }

            for(int particle = 0; particle < particles.Count; particle++)
            {
                particles[particle].Update();
                if (particles[particle].Lifetime <= 0)
                {
                    particles.RemoveAt(particle);
                    particle--;
                }
            }
        }

        public void Draw(SpriteBatch spritebatch)
        {
            
            for(int i = 0; i < particles.Count; i++)
            {
                particles[i].Draw(spritebatch);
            }
        }
    }
}
