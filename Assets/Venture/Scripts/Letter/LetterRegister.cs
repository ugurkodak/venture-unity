using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Venture.Data;

namespace Venture
{
    public class LetterRegister : Letter
    {
        public Button ButtonSignature;
        public Dropdown DropdownPrefix;
        public Button ButtonFirstName;
        public Button ButtonLastName;

        void Awake()
        {
            ButtonSignature.onClick.AddListener(async () => {
                Game.Instance.Console.Print("asdasdasdasdasd");
                //await manager.Submit(this);
            });
        }

        public override async Task Open()
        {
            // TODO: Animation
        }

        public override async Task Postpone()
        {
            Debug.LogError("Can't postpone register letter.");
        }

        public override async Task Submit()
        {
            await Game.Instance.Data.Register(Game.Instance.Data.UserId, new CharacterMeta
            {
                firstName = ButtonFirstName.GetComponentInChildren<Text>().text,
                lastName = ButtonLastName.GetComponentInChildren<Text>().text,
                prefix = CharacterPrefix.MX
            });

            if (Game.Instance.Data.User != null)
            {
                Game.Instance.Console.Print("Register succeeded.");
            }
        }

        public override async Task Discard()
        {
            Debug.LogError("Error: Can't discard register letter");
        }
    }
}