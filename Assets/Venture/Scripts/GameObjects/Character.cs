using UnityEngine;
using System.Threading.Tasks;

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

		public async Task SetupInstance(string key)
		{
			await Data.Load(key);
		}
	}
}