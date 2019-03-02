using UnityEngine;
using UnityEngine.UI;

namespace Venture.Prefab
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

        // TODO
        public override void Open()
        {
            Game.Instance.Console.Print("SigninLetter: Open");
        }

        public override async void Submit()
        {
            Game.Instance.Console.Print("Signing in...");
            gameObject.GetComponentInChildren<Text>().text = "Signing In...";
            gameObject.GetComponentInChildren<Text>().fontStyle = FontStyle.Normal;

            Data.LoadState state = await Game.Instance.Data.Login();

            switch (state)
            {
                case Data.LoadState.NONE:
                    {
                        Game.Instance.Console.Print("Login failed.");
                        break;
                    }
                case Data.LoadState.NEW_USER:
                    {
                        Game.Instance.Console.Print("New User.");
                        Game.Instance.LetterManager.Open(Game.Instance.LetterManager.Register);
                        Game.Instance.LetterManager.List.Remove(this);
                        Destroy(this); //TODO animation
                        break;
                    }
                case Data.LoadState.CHARACTER:
                    {
                        Game.Instance.Console.Print("Login succedeed.");
                        break;
                    }
                default:
                    {
                        Game.Instance.Console.Print("Error: Unexpected login result");
                        break;
                    }
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