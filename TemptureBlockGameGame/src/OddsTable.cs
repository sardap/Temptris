using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemptureBlockGameGame.src
{
	class OddsAlreadyAdded : Exception { }

	class InvaildOddsAdded : Exception { }

	class KeyClassMustSupportAddtion : Exception { }

	class OddsTable<T1, T2>: CompraerTable<T1, T2> where T1 : IComparable, IEquatable<T1>
	{
		public override void Add(T1 odds, T2 result)
		{
			if(_odds.Count == 0)
			{
				base.Add(odds, result);
				return;
			}
			
			dynamic dOdds = odds;
			dynamic dLast = _odds.Last().Key;

			var newOdds = dOdds + dLast;
			if (!_odds.ContainsKey(newOdds))
				base.Add((T1)newOdds, result);
		}
	}
}
