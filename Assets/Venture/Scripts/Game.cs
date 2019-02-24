using UnityEngine;
using Venture.Data;

namespace Venture
{
	public class Game : MonoBehaviour
	{
		public static Game Instance;
		public GameData Data;

		void Awake()
		{
			if (Instance == null)
				Instance = this;
			else if (Instance != this)
				Destroy(gameObject);
			DontDestroyOnLoad(gameObject);
			Data = new GameData();
		}

		async void Start()
		{
            await Data.Register("UNITY-EDITOR", new Character.CharacterMeta
            {
                firstName = "Ugur",
                lastName = "Kodak",
                prefix = Character.CharacterPrefix.MR
            });
            Debug.Log(Data.Character.Meta);
        }
	}
}