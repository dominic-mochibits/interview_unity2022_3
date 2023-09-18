namespace Mochibits.Utility
{
	public static class SystemRandom
	{
		static System.Random random = new System.Random();

		public static int Range(int maxValue)
		{
			return random.Next(maxValue);
		}

		public static float Range(float maxValue)
		{
			return Range(0f, maxValue);
		}

		public static int Range(int minValue, int maxValue)
		{
			return random.Next(minValue, maxValue);
		}

		public static float Range(float minValue, float maxValue)
		{
			// using the system random to get a value between 0 to 1
			double randomPercent = random.NextDouble();

			// get the range of the given parameter
			float range = maxValue - minValue;

			// calculate the value in Range
			float valueInRange = range * (float)randomPercent;

			// add the valueInRange to the minValue
			return minValue + valueInRange;
		}

		// use this one
		public static bool RandomBoolean()
		{
			return random.Next(2) == 1;
		}
	}

	public static class UnityRandom
	{
		public static void InitState(int seed)
		{
			UnityEngine.Random.InitState(seed);
		}

		public static int Range(int min, int max)
		{
			return UnityEngine.Random.Range(min, max);
		}

		public static float Range(float min, float max)
		{
			return UnityEngine.Random.Range(min, max);
		}

		public static UnityEngine.Vector2 InsideUnitCircle(UnityEngine.Vector2 magnitude)
		{
			var unitCircle = UnityEngine.Random.insideUnitCircle;
			return new UnityEngine.Vector2(unitCircle.x * magnitude.x, unitCircle.y * magnitude.y);
		}
	}
}