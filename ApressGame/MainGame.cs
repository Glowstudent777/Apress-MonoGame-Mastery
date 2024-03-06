using ApressGame.Enum;
using ApressGame.States;
using ApressGame.States.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace ApressGame
{
	public class MainGame : Game
	{
		private BaseGameState _currentGameState;

		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;

		private RenderTarget2D _renderTarget;
		private Rectangle _renderScaleRectangle;

		private const int DesignedResolutionWidth = 1280;
		private const int DesignedResolutionHeight = 720;

		private const float DesignedResolutionAspectRatio = DesignedResolutionWidth / (float)DesignedResolutionHeight;

		public MainGame()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
		}

		protected override void Initialize()
		{
			_graphics.PreferredBackBufferWidth = 1024;
			_graphics.PreferredBackBufferHeight = 768;
			_graphics.IsFullScreen = false;
			_graphics.ApplyChanges();

			_renderTarget = new RenderTarget2D(GraphicsDevice, DesignedResolutionWidth, DesignedResolutionHeight, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.DiscardContents);

			_renderScaleRectangle = GetScaleRectangle();

			base.Initialize();
		}

		private Rectangle GetScaleRectangle()
		{
			var variance = 0.5;
			var actualAspectRatio = Window.ClientBounds.Width / (float)Window.ClientBounds.Height;

			Rectangle scaleRectangle;

			if (actualAspectRatio <= DesignedResolutionAspectRatio)
			{
				var presentHeight = (int)(Window.ClientBounds.Width / DesignedResolutionAspectRatio + variance);
				var barHeight = (Window.ClientBounds.Height - presentHeight) / 2;

				scaleRectangle = new Rectangle(0, barHeight, Window.ClientBounds.Width, presentHeight);
			}
			else
			{
				var presentWidth = (int)(Window.ClientBounds.Height * DesignedResolutionAspectRatio + variance);
				var barWidth = (Window.ClientBounds.Width - presentWidth) / 2;

				scaleRectangle = new Rectangle(barWidth, 0, presentWidth, Window.ClientBounds.Height);
			}

			return scaleRectangle;
		}

		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);

			SwitchGameState(new SplashState());
		}

		private void CurrentGameState_OnStateSwitched(object sender, BaseGameState e)
		{
			SwitchGameState(e);
		}

		private void SwitchGameState(BaseGameState gameState)
		{
			_currentGameState?.UnloadContent();
			_currentGameState = gameState;
			_currentGameState.Initialize(Content);
			_currentGameState.LoadContent();

			_currentGameState.OnStateSwitched += CurrentGameState_OnStateSwitched;
			_currentGameState.OnEventNotification += _currentGameState_OnEventNotification;
		}

		private void _currentGameState_OnEventNotification(object sender, Events e)
		{
			switch (e)
			{
				case Events.GAME_QUIT:
					Exit();
					break;
			}
		}

		protected override void Update(GameTime gameTime)
		{
			_currentGameState.HandleInput();

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.SetRenderTarget(_renderTarget);

			GraphicsDevice.Clear(Color.CornflowerBlue);

			_spriteBatch.Begin();
			_currentGameState.Render(_spriteBatch);
			_spriteBatch.End();

			_graphics.GraphicsDevice.SetRenderTarget(null);
			_graphics.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 1.0f, 0);

			_spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
			_spriteBatch.Draw(_renderTarget, _renderScaleRectangle, Color.White);
			_spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
