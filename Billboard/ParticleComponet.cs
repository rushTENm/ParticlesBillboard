using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Billboard
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class ParticleComponent : DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        public List<Emitter> particleEmitterList;

        public ParticleComponent(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            particleEmitterList = new List<Emitter>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            foreach (Emitter emitter in particleEmitterList)
            {
                emitter.UpdateParticles(gameTime);
            }
            base.Update(gameTime);
        }

        public void Draw(GameTime gameTime, BasicEffect basicEffect, Matrix projection, Matrix view)
        {
            Matrix invertY = Matrix.CreateScale(1, -1, 1);

            basicEffect.World = invertY;
            basicEffect.View = Matrix.Identity;
            basicEffect.Projection = projection;

            spriteBatch.Begin(0, null, null, DepthStencilState.DepthRead, RasterizerState.CullNone, basicEffect);

            foreach (Emitter emitter in particleEmitterList)
            {
                Vector3 position = new Vector3(40, 50, 50);

                Vector3 viewSpacePosition = Vector3.Transform(position, view * invertY);

                emitter.DrawParticles(gameTime, spriteBatch, viewSpacePosition);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
