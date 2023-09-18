/*
 * DEBUG with timestamps
 *
 * For details, visit the Rombos blog:
 *  http://rombosblog.wordpress.com/2014/02/01/unity-debug-log-console-messages-with-timestamp/ 
 *
 * Copyright (c) 2014 Hans-Juergen Richstein, Rombos
 * http://www.rombos.de
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 *
 */

using UnityEngine;

public static class CustomDebug
{
	public static readonly bool isDebugBuild;
	static CustomDebug()
	{
		isDebugBuild = Debug.isDebugBuild;
		Debug.LogFormat("CustomDebug.isDebugBuild:{0} <-- should be true when testing, debug build or Editor", isDebugBuild);
		Debug.LogFormat("CustomDebug.killSwitch:{0}  <-- should be false", killSwitch);
	}

	// DO NOT CHANGE KILLSWITCH //
	static bool killSwitch = false;

#if UNITY_EDITOR
	static readonly string prefix = "";
#else
	static readonly string prefix = string.Empty;
#endif

	static string Timestamp()
	{
		return System.DateTime.UtcNow.ToString("HH:mm:ss.fff: ");

		// or try this if you need a date in front:
		//return System.DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff: ");
	}

	static string Prefix()
	{
		return prefix + Timestamp();
	}

	public static void Log(object obj)
	{
		if (!isDebuggerEnabled) { return; }
		Debug.Log(Prefix() + obj);
	}

	public static void LogFormat(string format, params object[] args)
	{
		if (!isDebuggerEnabled) { return; }
		Debug.LogFormat(Prefix() + format, args);
	}

	public static void LogStart(object obj)
	{
		if (!isDebuggerEnabled) { return; }
		Debug.Log(Timestamp() + "**  " + obj + "  **");
	}

	public static void LogStartFormat(string format, params object[] args)
	{
		if (!isDebuggerEnabled) { return; }
		Debug.LogFormat(Timestamp() + "**  " + format + "  **", args);
	}

	public static void LogInfo(object obj)
	{
		if (!isDebuggerEnabled) { return; }
		Debug.Log(Prefix() + "*** " + obj + " ***");
	}

	public static void LogInfoFormat(string format, params object[] args)
	{
		if (!isDebuggerEnabled) { return; }
		Debug.LogFormat(Prefix() + "*** " + format + " ***", args);
	}

	public static void LogEnd(object obj)
	{
		if (!isDebuggerEnabled) { return; }
		Debug.Log(Timestamp() + "****  " + obj + "  ****");
	}

	public static void LogEndFormat(string format, params object[] args)
	{
		if (!isDebuggerEnabled) { return; }
		Debug.LogFormat(Timestamp() + "****  " + format + "  ****", args);
	}


	#region Errors and Warnings

	public static void LogError(object obj)
	{
		Debug.LogError(Timestamp() + obj);
	}

	public static void LogErrorFormat(string format, params object[] args)
	{
		Debug.LogErrorFormat(Prefix() + format, args);
	}

	public static void LogWarning(object obj)
	{
		Debug.LogWarning(Timestamp() + obj);
	}

	public static void LogWarningFormat(string format, params object[] args)
	{
		Debug.LogWarningFormat(Prefix() + format, args);
	}

	public static void LogExceptionFormat(System.Exception ex, string operation)
	{
		LogErrorFormat("EXCEPTION caught while executing - {0}\nException: {1}", operation, ex);
	}

	public static void LogExceptionFormat(System.Exception ex, string operation, string format, params object[] args)
	{
		var message = string.Format(format, args);
		LogErrorFormat("EXCEPTION caught while executing - {0}\nMessage: {1}\nException: {2}", operation, message, ex);
	}

	#endregion


	public static bool developerConsoleVisible
	{
		get { return Debug.developerConsoleVisible; }
		set { Debug.developerConsoleVisible = value; }
	}


	#region SRDebugger

	public static bool isDebuggerEnabled
	{
		get
		{
			// if kill switch is turned on return false right away
			if (killSwitch) { return false; }

			// flip kill switch for briefly to get the value of the result
			killSwitch = true;

			// NOTE: anything within the kill swich Debug.Log will be turned off completely
			var result = isDebugBuild;// || Global.SystemSettings.DebuggerEnabled;

			// reset kill switch
			killSwitch = false;

			return result;
		}
	}

	/*
	 * Disable / Enable Debugger
	 */
	public static void EnableDebugger(bool enable = true)
	{
		//if (enable)
		//{
		//	SRDebug.Init();
		//}

		//SRDebug.Instance.IsTriggerEnabled = enable;

		//Global.SystemSettings.model.DebuggerEnabled = enable;
		//Global.SystemSettings.Save();
	}

	#endregion


	public static void DrawLine(Vector3 start, Vector3 end, Color? color = null, float duration = 0.0f, bool depthTest = true)
	{
		Color col = color.HasValue ? color.Value : Color.white; // workaround for problem with color constant as default value 
		Debug.DrawLine(start, end, col, duration, depthTest);
	}

	public static void DrawRay(Vector3 start, Vector3 dir, Color? color = null, float duration = 0.0f, bool depthTest = true)
	{
		Color col = color.HasValue ? color.Value : Color.white; // workaround for problem with color constant as default value 
		Debug.DrawRay(start, dir, col, duration, depthTest);
	}

	public static void Break()
	{
		Debug.Break();
	}

	public static void DebugBreak()
	{
		Debug.DebugBreak();
	}

	public static void ClearDeveloperConsole()
	{
		Debug.ClearDeveloperConsole();
	}
}
