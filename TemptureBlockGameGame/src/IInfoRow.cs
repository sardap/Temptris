using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemptureBlockGameGame.src
{
	interface IInfoRow
	{
		int Height { get; }

		int Width { get; }

		void Draw(Point point, SpriteBatch spriteBatch);
	}
}
