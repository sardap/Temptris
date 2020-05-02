using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace TemptureBlockGameGame.src
{
	class TextInfoRow: IInfoRow
	{
		public int Height
		{
			get
			{
				var messageSize = Font.MeasureString(Message);
				return (int)Math.Round(messageSize.Y + Margin.Y);
			}
		}

		public int Width
		{
			get
			{
				var messageSize = Font.MeasureString(Message);
				return (int)Math.Round(messageSize.X + Margin.X);
			}
		}

		public SpriteFont Font { get; set; }

		public Point Margin { get; set; }

		public virtual string Message { get; set; }

		public Color Color { get; set; }

		public TextInfoRow()
		{
		}

		public void Draw(Point point, SpriteBatch spriteBatch)
		{
			var pos = (point + Margin).ToVector2();

			spriteBatch.DrawString
			(
				Font,
				Message,
				pos,
				Color,
				0,
				new Vector2(),
				0.5f,
				SpriteEffects.None,
				0
			);
		}
	}
}
