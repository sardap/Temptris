using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemptureBlockGameGame.src
{
	class ScoreInfoRow: TextInfoRow
	{
		public Score Score { get; set; }

		public override string Message
		{
			get
			{
				return string.Format("Score: {0}", Score.Value);
			}

			set
			{
				throw new InvalidOperationException();
			}
		}
	}
}
