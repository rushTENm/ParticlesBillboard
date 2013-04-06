using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Billboard
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class ParticleComponent : IGameComponent
    {
        public List<Emitter> particleEmitterList;

        Matrix invertY = Matrix.CreateScale(1, -1, 1);

        Vector3 particlePosition = new Vector3(-20, 60, -10);
        Vector3 viewSpacePosition;

        public ParticleComponent(BillboardGame game)
        {
            // TODO: Construct any child components here
            game.Components.Add(this);
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public void Initialize()
        {
            particleEmitterList = new List<Emitter>();
        }

        public void LoadContent(ContentManager content)
        {
            Emitter fireEmitter = new Emitter();
            fireEmitter.Active = true;
            fireEmitter.TextureList.Add(content.Load<Texture2D>("fire"));
            fireEmitter.RandomEmissionInterval = new RandomMinMax(330);
            fireEmitter.ParticleLifeTime = 1350;
            fireEmitter.ParticleDirection = new RandomMinMax(0);
            fireEmitter.ParticleSpeed = new RandomMinMax(1f);
            fireEmitter.ParticleRotation = new RandomMinMax(0);
            fireEmitter.RotationSpeed = new RandomMinMax(0);
            fireEmitter.ParticleFader = new ParticleFader(true, true, 0);
            fireEmitter.ParticleScaler = new ParticleScaler(0.4f, 0.6f, 0, 1000);
            fireEmitter.Position = new Vector2(0, 370);

            Emitter smokeEmitter = new Emitter();
            smokeEmitter.Active = true;
            smokeEmitter.TextureList.Add(content.Load<Texture2D>("smoke"));
            smokeEmitter.RandomEmissionInterval = new RandomMinMax(45);
            smokeEmitter.ParticleLifeTime = 1350;
            smokeEmitter.ParticleDirection = new RandomMinMax(-15, 15);
            smokeEmitter.ParticleSpeed = new RandomMinMax(4.5f);
            smokeEmitter.ParticleRotation = new RandomMinMax(0);
            smokeEmitter.RotationSpeed = new RandomMinMax(-0.008f, 0.008f);
            smokeEmitter.ParticleFader = new ParticleFader(true, true);
            smokeEmitter.ParticleScaler = new ParticleScaler(0.5f, 1f, 50, smokeEmitter.ParticleLifeTime);
            smokeEmitter.Position = new Vector2(0, 0);

            particleEmitterList.Add(smokeEmitter);
            particleEmitterList.Add(fireEmitter);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {
            foreach (Emitter emitter in particleEmitterList)
            {
                emitter.UpdateParticles(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch, BasicEffect basicEffect, Matrix projection, Matrix view)
        {
            basicEffect.World = invertY;
            basicEffect.View = Matrix.Identity;
            basicEffect.Projection = projection;

            spriteBatch.Begin(0, null, null, DepthStencilState.DepthRead, RasterizerState.CullNone, basicEffect);

            foreach (Emitter emitter in particleEmitterList)
            {
                viewSpacePosition = Vector3.Transform(particlePosition, view * invertY);
        
                foreach (Particle particle in emitter.ParticleList)
                {
                    spriteBatch.Draw(particle.Texture, (particle.Position * 0.05f) + new Vector2(viewSpacePosition.X, viewSpacePosition.Y), null, particle.Color, particle.Rotation, particle.Center, particle.Scale * 0.5f, 0, viewSpacePosition.Z);
                }
            }

            spriteBatch.End(); 
        }
    }
}
