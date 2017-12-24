using System;
using System.Collections.Generic;
using System.Linq;

namespace OOP_Lab9
{
	public abstract class Element : IComparable<Element>
	{
		public Element() : this( "", 0, 0 ) { }
		public Element( string _name ) : this( _name, 1, 1 ) { }
		public Element( string _name, int input, int output )
		{
			Name = _name;
			Inputs = input;
			Outputs = output;
		}

		abstract public void Run();

		public string Name { get; protected set; }
		public int Inputs 
		{ 
			get { return m_iInputs; } 
			set	
			{ 
				m_iInputs = value;
				if( m_InputData == null )
					m_InputData = new List<bool> (m_iInputs);
				
				ResizeIOData (ref m_InputData, m_iInputs);
			}
		}
		public int Outputs
		{
			get { return m_iOutputs; }
			protected set // don't allow change size of outputs, because Run() can may bother about outputs count
			{ 
				m_iOutputs = value; 
				if( m_OutputData == null )
					m_OutputData = new List<bool> (m_iOutputs);

				ResizeIOData (ref m_OutputData, m_iOutputs);
			}
		}

		public bool GetInputData( int idx ) {
			return m_InputData [idx];
		}

		public void SetInputData( int idx, bool data ) {
			m_InputData [idx] = data;
		}

		public void SetInputData( int idx, int data ) {
			m_InputData [idx] = data != 0;
		}
			
		public bool GetOutputData( int idx ) {
			return m_OutputData [idx];
		}

		private int m_iInputs;
		protected List<bool> m_InputData;

		private int m_iOutputs;
		protected List<bool> m_OutputData;

		public static void ResizeIOData( ref List<bool> data, int targetSize )
		{
			if (targetSize > data.Count) {
				data.AddRange(Enumerable.Repeat(false, targetSize - data.Count));
			} else if (targetSize < data.Count) {
				data.RemoveRange (targetSize, data.Count - targetSize);
			}
		}

        public int CompareTo(Element obj)
        {
            if (obj == null)
                return 1;

            return m_iInputs.CompareTo(obj.Inputs);
        }
	}
}
