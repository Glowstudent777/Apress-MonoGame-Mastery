using ApressGame.Objects;
using ApressGame.States.Base;
using Microsoft.Xna.Framework.Input;

namespace ApressGame.States
{
	public class SplashState : BaseGameState
	{
		public override void LoadContent()
		{
			AddGameObject(new SplashImage(LoadTexture("splash")));
		}

		public override void HandleInput()
		{
			var state = Keyboard.GetState();

			if (state.IsKeyDown(Keys.Enter))
			{
				SwitchState(new GameplayState());
			}
		}
	}
}
