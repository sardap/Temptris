using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemptureBlockGameGame.src
{
	class CompraerTable<T1, T2> where T1 : IComparable, IEquatable<T1>
	{
		public interface ICompareOpator
		{
			bool Compare(T1 a, T1 b);
		}

		public class LessThan : ICompareOpator
		{
			public bool Compare(T1 a, T1 b)
			{
				return a.CompareTo(b) <= 0;
			}
		}

		public class MoveThan : ICompareOpator
		{
			public bool Compare(T1 a, T1 b)
			{
				return a.CompareTo(b) >= 1;
			}
		}

		protected SortedList<T1, T2> _odds = new SortedList<T1, T2>();

		public int CompareValue { get; set; }

		public ICompareOpator CompareOpator { get; set; }

		public CompraerTable()
		{
			CompareValue = 0;
			CompareOpator = new LessThan();
		}

		public virtual void Add(T1 key, T2 value)
		{
			_odds.Add(key, value);
		}

		public T2 Get(T1 number)
		{
			return _odds.First(entry => CompareOpator.Compare(number, entry.Key)).Value;
		}

		public KeyValuePair<T1, T2> GetAt(int index)
		{
			return _odds.ToList()[index];
		}

		public IList<KeyValuePair<T1, T2>> GetFirstAndPrevous(T1 start)
		{
			var result = new List<KeyValuePair<T1, T2>>();
			KeyValuePair<T1, T2>? last = null;

			foreach (var i in _odds)
			{
				if (CompareOpator.Compare(start, i.Key))
				{
					if(last != null)
						result.Add((KeyValuePair<T1, T2>)last);

					result.Add(i);
					break;
				}

				last = i;
			}

			return result;
		}

		public IList<KeyValuePair<T1, T2>> GetNextN(T1 start, int n)
		{
			var result = new List<KeyValuePair<T1, T2>>();

			bool found = false;

			foreach(var i in _odds)
			{
				if(CompareOpator.Compare(start, i.Key))
				{
					found = true;
				}

				if(found)
				{
					result.Add(i);
				}

				if (result.Count > n)
					break;
			}

			return result;
		}

		public T1 Max()
		{
			return _odds.Last().Key;
		}
	}
}
