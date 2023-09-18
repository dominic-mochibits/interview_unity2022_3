using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class WTWUtility
{
	public static IEnumerator LoadSceneWithDelay(string sceneName, float delay = 0.0f)
	{
		yield return new WaitForSeconds(delay);
		SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
	}
}

public static class WTWStringExtension
{

	//	- (NSString *) decrypt:(NSString *)line {
	//		NSString *tmp = [NSString stringWithFormat:@"%@", line];  
	//
	//		return [[[[[[[[self reverseString:tmp]
	//			stringByReplacingOccurrencesOfString:@"t3yy4wq" withString:@"u"]
	//			stringByReplacingOccurrencesOfString:@"pl6qwb" withString:@"o"]
	//			stringByReplacingOccurrencesOfString:@"12pjz3b" withString:@"i"]
	//			stringByReplacingOccurrencesOfString:@"xc44vbj" withString:@"e"]
	//			stringByReplacingOccurrencesOfString:@"q7w9q0ry" withString:@"a"]
	//			stringByReplacingOccurrencesOfString:@"t3yy123wq" withString:@","]
	//			stringByReplacingOccurrencesOfString:@"n24vhj90rg" withString:@";"];
	//	}

	public static string DecryptWTWLevel(this string str)
	{
		string level = new string(str.ToCharArray().Reverse().ToArray());
		return level.Replace("t3yy4wq", "u")
			.Replace("pl6qwb", "o")
			.Replace("12pjz3b", "i")
			.Replace("xc44vbj", "e")
			.Replace("q7w9q0ry", "a")
			.Replace("t3yy123wq", ",")
			.Replace("n24vhj90rg", ";");
	}
}

public static class WTWTextMeshExtension
{
	public static void FormatTextWrapAround(this TextMesh textMObj, int characterLimit)
	{
		string newText = "";
		string[] words = textMObj.text.Split(' ');
		int i = 0;
		foreach (string word in words)
		{
			i += word.Length;
			if (i >= characterLimit)
			{
				newText += "\n";
				i = word.Length;
			}
			newText += word + " ";
			i++;//accounts for added space
		}
		textMObj.text = newText;
	}
}