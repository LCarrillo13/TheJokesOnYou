using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScripts.MenuScripts
{
	public class CloseGame : MonoBehaviour
	{
		public void Close()
		{
			Application.Quit();
		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#endif
		}
	}
}