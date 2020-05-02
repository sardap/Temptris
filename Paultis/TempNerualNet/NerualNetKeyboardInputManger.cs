using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemptureBlockGameGame.src;

namespace TempNerualNet
{
	class NerualNetKeyboardInputManger : IKeyboardInputManger
	{
		public Dictionary<Keys, bool> KeyPressed { get; set; }

		public Dictionary<Keys, bool> KeyClicked { get; set; }

		public NerualNetKeyboardInputManger()
		{
			KeyPressed = new Dictionary<Keys, bool>();

			KeyClicked = new Dictionary<Keys, bool>();

			foreach (var key in Enum.GetValues(typeof(Keys)))
			{
				KeyPressed.Add((Keys)key, false);
				KeyClicked.Add((Keys)key, false);
			}
		}

		public void MoveDirection(Directions direction)
		{
			switch(direction)
			{
				case Directions.Left:
					KeyPressed[Keys.Left] = true;
					break;
				case Directions.Right:
					KeyPressed[Keys.Right] = true;
					break;
				default:
					throw new NotImplementedException();
			}
		}

		public void Step()
		{
			foreach (var key in Enum.GetValues(typeof(Keys)))
			{
				KeyPressed[(Keys)key] = false;
				KeyClicked[(Keys)key] = false;
			}
		}
	}
}
