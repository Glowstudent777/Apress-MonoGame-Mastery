using ApressGame.Enum;
using ApressGame.Objects;
using ApressGame.States.Base;
using Microsoft.Xna.Framework.Input;

namespace ApressGame.States
{
	public class GameplayState : BaseGameState
	{
		private const string PlayerFighter = "Fighter";
		private const string BackgroundTexture = "Barren";
		public override void LoadContent()
		{
			AddGameObject(new SplashImage(LoadTexture(BackgroundTexture)));
			AddGameObject(new PlayerSprite(LoadTexture(PlayerFighter)));
		}

		public override void HandleInput()
		{
			var state = Keyboard.GetState();

			if (state.IsKeyDown(Keys.Escape))
			{
				NotifyEvent(Events.GAME_QUIT);
			}
		}
	}
}
