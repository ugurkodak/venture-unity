using UnityEngine;
using UnityEngine.UI;
using Venture.Data;

namespace Venture
{
    public class Register : Letter
    {
        Button SubmitButton;
        InputField FirstNameField;
        InputField LastNameField;

        void Awake()
        {
            FirstNameField = transform.Find("Content").Find("Form").Find("FieldFirstName").GetComponent<InputField>();
            LastNameField = transform.Find("Content").Find("Form").Find("FieldLastName").GetComponent<InputField>();
            SubmitButton = transform.Find("Content").Find("ButtonSubmit").GetComponent<Button>();
            SubmitButton.onClick.AddListener(Submit);
        }

        public override void Open()
        {
            Game.Instance.Console.Print("Register Letter: Open");
        }

        public override async void Submit()
        {
            Game.Instance.Console.Print("HEEEEEY");
            // await Game.Instance.Data.Register(Game.Instance.Data.User.Id, new Character.CharacterMeta
            // {
            //     firstName = FirstNameField.text,
            //     lastName = LastNameField.text,
            //     prefix = Character.CharacterPrefix.MX
            // });
        }

        public override void Postpone()
        {
            Game.Instance.Console.Print("Register Letter: Postpone");
        }

        public override void Discard()
        {
            Game.Instance.Console.Print("Register Letter: Discard");
        }
    }
}