﻿using ApressGame.Enum;
using ApressGame.Objects.Base;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApressGame.States.Base
{
	public abstract class BaseGameState
	{
		private const string FallbackTexture = "Empty";
		private ContentManager _contentManager;

		private readonly List<BaseGameObject> _gameObjects = new List<BaseGameObject>();
		public abstract void LoadContent();
		public void UnloadContent()
		{
			_contentManager.Unload();
		}
		public abstract void HandleInput();

		public event EventHandler<BaseGameState> OnStateSwitched;
		public event EventHandler<Events> OnEventNotification;

		public void Initialize(ContentManager contentManager)
		{
			_contentManager = contentManager;
		}

		protected Texture2D LoadTexture(string textureName)
		{
			var texture = _contentManager.Load<Texture2D>(textureName);

			return texture ?? _contentManager.Load<Texture2D>(FallbackTexture);
		}

		protected void NotifyEvent(Events eventType, object argument = null)
		{
			OnEventNotification?.Invoke(this, eventType);

			foreach (var gameObject in _gameObjects)
			{
				gameObject.OnNotify(eventType);
			}
		}

		protected void SwitchState(BaseGameState gameState)
		{
			OnStateSwitched?.Invoke(this, gameState);
		}

		protected void AddGameObject(BaseGameObject gameObject)
		{
			_gameObjects.Add(gameObject);
		}

		public void Render(SpriteBatch spriteBatch)
		{
			foreach (var gameObject in _gameObjects.OrderBy(a => a.zIndex))
			{
				gameObject.Render(spriteBatch);
			}
		}
	}
}
