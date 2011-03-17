using System;

namespace BLToolkit.Data.Linq.Builder
{
	public interface ISequenceBuilder
	{
		int             BuildCounter { get; set; }
		bool            CanBuild     (ExpressionBuilder builder, BuildInfo buildInfo);
		IBuildContext BuildSequence(ExpressionBuilder builder, BuildInfo buildInfo);
	}
}
