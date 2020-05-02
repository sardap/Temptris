using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace TemptureBlockGameGame.src
{
	class InfoBar
	{
		private List<IInfoRow> _infoRows = new List<IInfoRow>();

		public void Add(IInfoRow row)
		{
			_infoRows.Add(row);
		}

		internal void Draw(Point point, SpriteBatch spriteBatch)
		{
			var width = _infoRows.Max(i => i.Width);

			foreach (var row in _infoRows)
			{
				ShapeExtensions.DrawRectangle(spriteBatch, new RectangleF(point.X, point.Y, width, row.Height), Color.Black, 5);
				row.Draw(point, spriteBatch);
				point.Y += row.Height;
			}
		}
	}
}
