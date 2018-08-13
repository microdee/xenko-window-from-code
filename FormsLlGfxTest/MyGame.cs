using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xenko;
using Xenko.Core;
using Xenko.Core.IO;
using Xenko.Core.Mathematics;
using Xenko.Data;
using Xenko.Engine;
using Xenko.Engine.Design;
using Xenko.Games;
using Xenko.Graphics;
using Xenko.Rendering;
using Buffer = Xenko.Graphics.Buffer;

namespace FormsLlGfxTest
{
    public class MyGame : Game
    {
        public void InitVulkan()
        {
            var rendersettings = Settings.Configurations.Get<RenderingSettings>();
            rendersettings.DefaultGraphicsProfile = GraphicsProfile.Level_11_2;
            rendersettings.PreferredGraphicsPlatform = PreferredGraphicsPlatform.Vulkan;
        }

        protected override void PrepareContext()
        {
            Context.InitializeDatabase = true;
            base.PrepareContext();
            //InitVulkan();
        }

        private MutablePipelineState pipelineState;

        private EffectInstance simpleEffect;
        private const int QuadCount = 3;

        public static VertexDeclaration VertexDeclaration = VertexPositionNormalTexture.Layout;
        public static PrimitiveType PrimitiveType = PrimitiveType.TriangleList;

        private Buffer _tribuf;

        private void Init()
        {
            _tribuf = Buffer.New(GraphicsDevice, new[]
            {
                new Vector4(-0.5f, 0.5f, 0, 1),
                new Vector4(0.5f, 0.5f, 0, 1),
                new Vector4(0, -0.5f, 0, 1)
            }, BufferFlags.VertexBuffer, GraphicsResourceUsage.Immutable);

            simpleEffect = new EffectInstance(new Effect(GraphicsDevice, SpriteEffect.Bytecode));
            simpleEffect.Parameters.Set(SpriteBaseKeys.MatrixTransform, Matrix.Identity);
            simpleEffect.UpdateEffect(GraphicsDevice);

            pipelineState = new MutablePipelineState(GraphicsDevice);
            pipelineState.State.SetDefaults();
            pipelineState.State.InputElements = VertexDeclaration.CreateInputElements();
            pipelineState.State.PrimitiveType = PrimitiveType;
        }
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            var cl = GraphicsContext.CommandList;
            cl.ResetTargets();
            var gp = GraphicsDevice.Presenter;
            cl.Clear(gp.BackBuffer, Color.Blue);
            cl.Clear(gp.DepthStencilBuffer, DepthStencilClearOptions.DepthBuffer | DepthStencilClearOptions.Stencil);
            // Render to the backbuffer
            cl.SetRenderTargetAndViewport(gp.DepthStencilBuffer, gp.BackBuffer);

            if (_tribuf == null)
            {
                Init();
            }

            simpleEffect.UpdateEffect(GraphicsDevice);

            pipelineState.State.RootSignature = simpleEffect.RootSignature;
            pipelineState.State.EffectBytecode = simpleEffect.Effect.Bytecode;
            pipelineState.State.BlendState = BlendStates.Default;
            pipelineState.State.Output.CaptureState(cl);
            pipelineState.Update();

            cl.SetPipelineState(pipelineState.CurrentState);

            // Apply the effect
            simpleEffect.Apply(GraphicsContext);

            cl.SetVertexBuffer(0, _tribuf, 0, 16);
            cl.Draw(3, 0);
        }
    }
}
