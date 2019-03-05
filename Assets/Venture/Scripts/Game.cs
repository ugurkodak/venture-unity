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
            //DontDestroyOnLoad(gameObject);

            Data = new GameData();
            Console = new Console();
        }

        private void Start()
        {
            LetterManager = transform.Find("LetterManager").GetComponent<Prefab.LetterManager>();
            LetterManager.Open(LetterManager.SignIn);
        }
    }
}