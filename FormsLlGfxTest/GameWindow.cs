using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpDX.DXGI;
using Xenko;
using Xenko.Data;
using Xenko.Engine;
using Xenko.Engine.Design;
using Xenko.Games;
using Xenko.Graphics;

namespace FormsLlGfxTest
{
    public partial class GameWindow : Form
    {
        public GameContextWinforms GameContext;
        public MyGame GameInstance;
        public GameWindow()
        {
            InitializeComponent();
        }

        private void OnLoaded(object sender, EventArgs e)
        {
            GameContext = new GameContextWinforms(this);
            GameInstance = new MyGame
            {
                GraphicsDeviceManager =
                {
                    PreferredGraphicsProfile = new[] {GraphicsProfile.Level_11_2}
                },
                AutoLoadDefaultSettings = true
            };

            GameInstance.WindowCreated += (o, args) =>
            {
                //GameInstance.InitVulkan();
                GameInstance.Window.AllowUserResizing = true;
                GameInstance.GraphicsDeviceManager.DeviceCreated += OnGameGraphicsCreated;
            };
            GameInstance.Run(GameContext);
        }

        private void OnGameGraphicsCreated(object sender, EventArgs e)
        {
        }
    }
}
