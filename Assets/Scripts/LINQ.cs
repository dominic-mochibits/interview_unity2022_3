using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LINQ
{
	public class GameInfo
	{
		public class Panel
		{
			public bool Shown;
			public string Desc;
		}


		public void Process()
		{
			Panel[] panels = new Panel[]
			{
				new Panel { Shown = true, Desc = "A" },
				new Panel { Shown = true, Desc = "A" },
				new Panel { Shown = true, Desc = "A" },
				new Panel { Shown = true, Desc = "A" },
				new Panel { Shown = true, Desc = "A" },
				new Panel { Shown = true, Desc = "A" },
				new Panel { Shown = true, Desc = "A" },
			};


			List<Panel> readyPanels;
			// 1. Use Loop to find Panels with Shown = false and put the results "readyPanels"










			// 2. Use LINQ to find Panels with Shown = false and put the results "readyPanels"









			Panel[] readyPanelArray;
			// 3. Use LINQ to find Panels with Shown = false and put the results "readyPanels"










		}
	}
}



















public interface ITest
{
	int value1 { get; }

	void Test1();

	bool Test2();
}


public abstract class ATest : ITest
{
	public int value1 => 10;

	public virtual void Test1()
	{ }

	public abstract bool Test2();
}


public class Base
{
	protected void Test() { }
}

public class Derived: Base
{
	public void DerivedTest()
	{

	}
}

public class Test<T>
{

}
