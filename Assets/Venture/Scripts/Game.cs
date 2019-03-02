using UnityEngine;
using Venture.Data;

namespace Venture
{
    public class Game : MonoBehaviour
    {
        public static Game Instance;
        public GameData Data;
        public Console Console;
        public Prefab.LetterManager LetterManager;

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);
            DontDestroyOnLoad(gameObject);

            Data = new GameData();
            Console = new Console();
            LetterManager = transform.Find("LetterManager").GetComponent<Prefab.LetterManager>();
        }

        void Start()
        {
            LetterManager.Open(LetterManager.SignIn);
            //await Data.Register("UNITY-EDITOR", new Character.CharacterMeta
            //{
            //    firstName = "Ugur",
            //    lastName = "Kodak",
            //    prefix = Character.CharacterPrefix.MR
            //});
            //Debug.Log(Data.Character.Meta);
        }
    }
}