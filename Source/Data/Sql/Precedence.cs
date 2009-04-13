using System;

namespace BLToolkit.Data.Sql
{
	public class Precedence
	{
		public const int Primary            = 90; // (x) x.y f(x) a[x] x++ x-- new typeof sizeof checked unchecked
		public const int Unary              = 80; // + - ! ++x --x (T)x
		public const int Multiplicative     = 70; // * / %
		public const int Additive           = 60; // + -
		public const int Comparison         = 50; // ANY ALL SOME EXISTS, IS [NOT], IN, BETWEEN, LIKE, < > <= >=, == !=
		public const int Bitwise            = 40; // ^
		public const int LogicalNegation    = 30; // NOT
		public const int LogicalConjunction = 20; // AND
		public const int LogicalDisjunction = 10; // OR
		public const int Unknown            =  0;
	}
}
