using UnityEngine;

namespace Venture
{
	public class IO : MonoBehaviour
	{
		public static IO Instance = null;
		public Console Console = null;

		void Awake()
		{
			//Persistent singleton
			if (Instance == null)
				Instance = this;
			else if (Instance != this)
				Destroy(gameObject);
			DontDestroyOnLoad(gameObject);
		}
	}
}
