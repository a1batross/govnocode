using System;
using System.Collections.Generic;

namespace OOP_Lab9
{
	public class CombinationalElement : Element
	{
		public CombinationalElement ( string _name, int input ) : base( _name, input, 1 )
		{
			m_InputData.Capacity = Inputs;
		}

		public override void Run()
		{
			m_OutputData[0] = GetInputData( 0 );
			for( int i = 1; i < Inputs; i++ )
			{
				m_OutputData[0] &= GetInputData( i );
			}
			m_OutputData[0] = !m_OutputData[0];
		}
	}
}

