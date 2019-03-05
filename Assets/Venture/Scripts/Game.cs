using UnityEngine;
using Venture.Data;
using Google;

namespace Venture
{
    // Global cotainer for everthing
    public class Game : MonoBehaviour
    {
        public static Game Instance;
        public GameData Data;
        public Console Console;
        public LetterManager LetterManager;

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);
            DontDestroyOnLoad(gameObject);

            Data = new GameData();
            Console = new Console();
        }

        private void Start()
        {
            LetterManager.Open(LetterManager.SignIn);
        }
    }
}