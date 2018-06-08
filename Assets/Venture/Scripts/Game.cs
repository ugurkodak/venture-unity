using UnityEngine;

namespace Venture
{
	public class Game : MonoBehaviour
	{
		public static Game Instance = null;
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

		void Start()
		{
			Document.Instance.Open(Document.Instance.SignIn);
		}
	}
}
