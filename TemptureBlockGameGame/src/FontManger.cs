using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemptureBlockGameGame.src
{
	class FontManger
	{
		private static FontManger _singleton = null;

		private FontManger()
		{
		}

		static public FontManger GetInstance()
		{
			if(_singleton == null)
			{
				_singleton = new FontManger();
			}

			return _singleton;
		}
	}
}
