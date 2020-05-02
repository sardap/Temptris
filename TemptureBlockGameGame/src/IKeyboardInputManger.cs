using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemptureBlockGameGame.src
{
	enum KeyStates
	{
		Up = 0,
		Down = 1,
		Clicked = 2
	}
	public interface IKeyboardInputManger
	{
		Dictionary<Keys, bool> KeyPressed { get; }

		Dictionary<Keys, bool> KeyClicked { get; }

		void Step();
	}
}
