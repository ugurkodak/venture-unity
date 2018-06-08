using UnityEngine;

namespace Venture
{
	public class Character : MonoBehaviour
	{
		public static Character Instance;
		public Data.Character Data;

		void Awake()
		{
			if (Instance == null)
				Instance = this;
			else if (Instance != this)
				Destroy(gameObject);
			DontDestroyOnLoad(gameObject);
			Data = new Data.Character();
		}

		public void Initialize()
		{

		}
	}
}