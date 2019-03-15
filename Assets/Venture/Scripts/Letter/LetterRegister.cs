using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Venture.Data;

namespace Venture
{
    public class LetterRegister : Letter
    {
        public Dropdown DropdownPrefix;
        public Button ButtonFirstName;
        public Button ButtonLastName;
        public Button ButtonSignature;

        private List<string> firstNames;
        private string[] firstNameUndoBuffer;
        private List<string> lastNames;
        private string[] lastNameUndoBuffer;


        async void Start()
        {
            DropdownPrefix.ClearOptions();
            // TODO: This looks error prone. Not using enum or something like that
            DropdownPrefix.AddOptions(new List<string> { "Mx.", "Ms.", "Mr." });

            // Names are randomly selected from a premade list.
            firstNames = await Game.Instance.Data.GetCharacterFirstNames();
            lastNames = await Game.Instance.Data.GetCharacterLastNames();
            firstNameUndoBuffer = new string[2];
            lastNameUndoBuffer = new string[2];

            ButtonFirstName.onClick.AddListener(pickRandomFirstName);
            ButtonLastName.onClick.AddListener(pickRandomLastName);
            ButtonSignature.onClick.AddListener(async () => { await Game.Instance.ManagerLetter.Submit(this); });
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
            ButtonSignature.interactable = false;

            await Game.Instance.Data.Register(Game.Instance.Data.UserId, new CharacterMeta
            {
                firstName = ButtonFirstName.GetComponentInChildren<Text>().text,
                lastName = ButtonLastName.GetComponentInChildren<Text>().text,
                prefix = (CharacterPrefix)DropdownPrefix.value
            });

            if (Game.Instance.Data.User != null)
            {
                Game.Instance.Console.Print("Register succeeded.");
            }
        }

        public override async Task Discard()
        {
            // TODO: Animation
        }

        private void pickRandomFirstName()
        {
            string name = firstNames[Random.Range(0, firstNames.Count)];
            ButtonFirstName.GetComponentInChildren<Text>().text = name;

            if (firstNameUndoBuffer[0] != null)
            {
                firstNameUndoBuffer[1] = firstNameUndoBuffer[0];
                firstNameUndoBuffer[0] = name;
            }
            else firstNameUndoBuffer[0] = name;
        }

        private void pickRandomLastName()
        {
            string name = lastNames[Random.Range(0, lastNames.Count)];
            ButtonLastName.GetComponentInChildren<Text>().text = name;

            if (lastNameUndoBuffer[0] != null)
            {
                lastNameUndoBuffer[1] = lastNameUndoBuffer[0];
                lastNameUndoBuffer[0] = name;
            }
            else lastNameUndoBuffer[0] = name;
        }
    }
}