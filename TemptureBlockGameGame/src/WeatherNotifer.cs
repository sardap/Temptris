using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemptureBlockGameGame.src
{
	class WeatherNotifer
	{
		private Stack<IWeatherNotifcation> _messagesStack = new Stack<IWeatherNotifcation>();

		private IWeatherNotifcation _activeMessage = null;
		private double _timeSinceMessageShown;
		private SpriteFont _font = null;

		public void Initlise(ContentManager contentManager)
		{
			_font = contentManager.Load<SpriteFont>(@"Font/message");
		}

		public void PushNotifacation(IWeatherNotifcation notifcation)
		{
			_messagesStack.Push(notifcation);
		}

		public void Step(double deltaTime)
		{
			if(_activeMessage != null)
			{
				_timeSinceMessageShown += deltaTime;
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			if (_activeMessage == null)
			{
				if (_messagesStack.Count > 0)
				{
					_activeMessage = _messagesStack.Pop();
					Console.WriteLine("NEW MESSAGE");
				}
				else
				{
					return;
				}
			}

			if(_timeSinceMessageShown > _activeMessage.MessageShownFor)
			{
				_activeMessage = null;
				_timeSinceMessageShown = 0;
			}
			else
			{
				var messageSize = _font.MeasureString(_activeMessage.Message);

				var pos = new Vector2
				(
					(Const.WINDOW_WIDTH / 2) - (messageSize.X / 2),
					(Const.WINDOW_HEIGHT / 2) - (messageSize.Y / 2)
				);

				spriteBatch.DrawString
				(
					_font, 
					_activeMessage.Message, 
					pos, 
					_activeMessage.FontColor
				);
			}
		}
	}
}
