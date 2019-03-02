using System.Collections.Generic;
using UnityEngine;

namespace Venture.Prefab
{
    // Letter Manager is a GameObject to assign prefabs in editor
    public class LetterManager : MonoBehaviour
    {
        public List<Letter> List;
        public Letter Active;

        // Prefab references set in editor
        public Canvas Canvas; // TODO: Fix canvas doesn't have a prefab
        public Letter SignIn;
        public Letter Register;
        //public Notification Settings;
        //public Notification Character;

        public void Open(Letter notification)
        {
            this.Active = Instantiate(notification, this.Canvas.transform);
            Active.Open();
            List.Add(Active);
        }
    }
}