using UnityEngine;
using Venture.Data;

namespace Venture
{
    // Global cotainer for everthing
    public class Game : MonoBehaviour
    {
        public static Game Instance;
        public GameData Data;
        public Console Console;
        public ManagerLetter ManagerLetter;

        public bool UIFlowDisabled = false; // For trying out prefabs in play mode

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

        private async void Start()
        {
            if (!UIFlowDisabled)
                await ManagerLetter.Open(ManagerLetter.LetterSignIn);
        }
    }
}