using ApressGame.Objects.Base;
using Microsoft.Xna.Framework.Graphics;

namespace ApressGame.Objects
{
	public class PlayerSprite : BaseGameObject
	{
		public PlayerSprite(Texture2D texture)
		{
			_texture = texture;
		}
	}
}
