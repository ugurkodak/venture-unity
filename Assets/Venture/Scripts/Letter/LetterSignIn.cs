using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Venture
{
    public class LetterSignIn : Letter
    {
        public Button ButtonSignIn;

        void Awake()
        {
            ButtonSignIn.onClick.AddListener(async () => { await manager.Submit(this); });
        }

        public override async Task Open()
        {
            // TODO: Animation
        }

        public override async Task Submit()
        {
            gameObject.GetComponentInChildren<Text>().text = "Signing In...";

            await Game.Instance.Data.Login();
            // Data is loaded
            if (Game.Instance.Data.User != null)
            {
                Game.Instance.Console.Print("Login succeeded.");
            }
            // New user
            else
            {
                Game.Instance.Console.Print("New user.");
                manager.Open(Game.Instance.ManagerLetter.LetterRegister);
                manager.Discard(this);
            }
        }

        public override async Task Postpone()
        {
            Debug.LogError("Can't postpone sign-in letter.");
        }

        public override async Task Discard()
        {
            // TODO: Animation
        }
    }
}