using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemptureBlockGameGame.src
{

	class KeyboardInputManger: IKeyboardInputManger
	{
		private Dictionary<Keys, bool> _lastStates = new Dictionary<Keys, bool>();

		public Dictionary<Keys, bool> KeyPressed { get; set; }

		public Dictionary<Keys, bool> KeyClicked { get; set; }

		public KeyboardInputManger()
		{
			KeyPressed = new Dictionary<Keys, bool>();

			KeyClicked = new Dictionary<Keys, bool>();

			_lastStates = new Dictionary<Keys, bool>();

			foreach (var key in Enum.GetValues(typeof(Keys)))
			{
				_lastStates.Add((Keys)key, false);
				KeyPressed.Add((Keys)key, false);
				KeyClicked.Add((Keys)key, false);
			}
		}

		public void Step()
		{
			var state = Keyboard.GetState();

			foreach (var key in KeyPressed.Keys.ToArray())
			{
				KeyPressed[key] = state.IsKeyDown(key);
			}

			foreach (var key in KeyClicked.Keys.ToArray())
			{
				KeyClicked[key] = _lastStates[key] == true && KeyPressed[key] == false;
				_lastStates[key] = KeyPressed[key];
			}
		}
	}
}
