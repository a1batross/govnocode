using System;
using System.Collections.Generic;
using System.Linq;

namespace OOP_Lab9
{
	public class Register : Element, IComparable<Register>
	{
		public Register( string name ) : base( name, BITS+2, BITS )
		{
			m_Memory = new Memory[BITS];
			for (int i = 0; i < BITS; i++)
				m_Memory [i] = new Memory ();
		}
			
		public override void Run()
		{
			bool bSet = GetInputData (SET);
			bool bReset = GetInputData (RESET);

			if (bReset) {
				for (int i = 0; i < m_Memory.Length; i++) {
					m_Memory [i].SetInputData (SET, false);
					m_Memory [i].SetInputData (RESET, true);

					m_Memory [i].Run ();

					m_OutputData [i] = m_Memory [i].GetOutputData (0);
				}
				return;
			}

			if (bSet) {
				for (int i = 0; i < m_Memory.Length; i++) {
					m_Memory [i].SetInputData (SET, GetInputData(DATA+i));
					m_Memory [i].SetInputData (RESET, false);

					m_Memory [i].Run ();

					m_OutputData [i] = m_Memory [i].GetOutputData (0);
				}
				return;
			}

		}

		private Memory[] m_Memory;

		public static readonly int  SET   = 0;
		public static readonly int  RESET = 1;
		public static readonly int  DATA  = 2;
		public static readonly int  BITS  = 10;

        public int CompareTo(Register reg)
        {
            if (reg == null)
                return 1;


            // compare by name
            return Name.CompareTo(reg.Name);
        }

		// RS trigger
		public class Memory : Element, IComparable<Memory>
		{
			public Memory () : base( "RS Trigger", 2, 2 )
			{
			}
	
			public Memory( Memory mem ) 
			{
				Name = mem.Name;
				Inputs = mem.Inputs;
				Outputs = mem.Outputs;
				m_InputData = mem.m_InputData;
			}

			public static readonly int  SET   = 0;
			public static readonly int  RESET = 1;

			public bool Error { get; private set; }

			public bool GetDirectOutput() {
				return GetOutputData (0);
			}
			public bool GetReverseOutput() {
				return GetOutputData (1);
			}

			public override void Run()
			{
				bool bSet = GetInputData (SET);
				bool bReset = GetInputData (RESET);

				if (bSet && bReset) {
					Error = true;
					return;
				}

				if (!bSet && !bReset) {
					return; // save value
				}

				m_OutputData [0] = bSet;
				m_OutputData [1] = bReset;
			}

            public int CompareTo(Memory obj)
            {
                if (obj == null)
                    return 1;

                // errored Memory should be last
                if (this.Error && !obj.Error)
                    return -1;
                else if (!this.Error && obj.Error)
                    return 1;

                bool bReset = GetInputData(RESET);
                bool other_bReset = obj.GetInputData(RESET);

                // reset memory should be last, as it's not contain any real data
                /*if (bReset && !other_bReset)
                    return -1;
                else if (!bReset && other_bReset)
                    return 1;*/

                // equal
                return 0;
            }

    		public static bool operator ==( Memory a, Memory b )
			{
				return a.m_InputData.SequenceEqual (b.m_InputData);
			}

			public static bool operator !=( Memory a, Memory b )
			{
				return !a.m_InputData.SequenceEqual (b.m_InputData);
			}
		}
	}
}

