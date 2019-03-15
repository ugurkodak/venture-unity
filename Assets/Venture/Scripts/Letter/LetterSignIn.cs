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
            ButtonSignIn.onClick.AddListener(async () => { await Game.Instance.ManagerLetter.Submit(this); });
        }

        public override async Task Open()
        {
            // TODO: Animation
        }

        // This function is called by ButtonSignIn, not letter
        public override async Task Submit()
        {

            ButtonSignIn.GetComponentInChildren<Text>().text = "Signing In...";
            ButtonSignIn.interactable = false;

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
                Game.Instance.ManagerLetter.Open(Game.Instance.ManagerLetter.LetterRegister);
                Game.Instance.ManagerLetter.Discard(this);
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