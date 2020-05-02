using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemptureBlockGameGame.src
{
	interface IBlockBluePrint
	{
		Dictionary<Directions, IBlockBluePrint> Blocks { get; }

		Color? Color { get; set; } 

		void Add(Directions directions, IBlockBluePrint value);

		int MaxWidth();

		int MaxHeight();

		IBlockBluePrint GetBottom();

		IBlockBluePrint GetTop();
	}
}
