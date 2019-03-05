using UnityEngine;
using UnityEngine.UI;

namespace Venture
{
    public class SignIn : Letter
    {
        private Button SignInButton;

        void Awake()
        {
            SignInButton = transform.Find("Content").Find("SignInButton").GetComponent<Button>();
            SignInButton.onClick.AddListener(Submit);
        }

        // void Start()
        // {
        //     SignInButton.onClick.Invoke();
        // }

        public override void Open()
        {
            Game.Instance.Console.Print("SigninLetter: Open");
        }

        public override async void Submit()
        {
            gameObject.GetComponentInChildren<Text>().text = "Signing In...";
            gameObject.GetComponentInChildren<Text>().fontStyle = FontStyle.Normal;

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
                Game.Instance.LetterManager.Open(Game.Instance.LetterManager.Register);
                Game.Instance.LetterManager.List.Remove(this);
                Destroy(this); // TODO animation
            }
        }

        public override void Postpone()
        {
            Game.Instance.Console.Print("SigninLetter: Postpone");
        }

        public override void Discard()
        {
            Game.Instance.Console.Print("SigninLetter: Discard");
        }
    }
}