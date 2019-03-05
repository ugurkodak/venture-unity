using System.Collections.Generic;
using UnityEngine;

namespace Venture
{
    public class LetterManager : MonoBehaviour
    {
        public List<Letter> List;
        public Letter Active;

        // References set in editor
        public Canvas Canvas;
        public Letter SignIn;
        public Letter Register;
        //public Notification Settings;
        //public Notification Character;

        public void Open(Letter notification)
        {
            Active = Instantiate(notification, Canvas.transform);
            Active.Open();
            List.Add(Active);
        }
    }
}