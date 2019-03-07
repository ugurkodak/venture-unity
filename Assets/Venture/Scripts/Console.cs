using System.Collections.Generic;
using UnityEngine;
using System;

namespace Venture
{
    public class ConsoleEventArgs : EventArgs
    {
        public List<string> messages;
    }

    public class Console
    {
        const int MAX_MESSAGE_COUNT = 50;
        List<string> messages;

        public event EventHandler<ConsoleEventArgs> MessageRecieved;

        public Console()
        {
            messages = new List<string>();
        }

        public void Print(string message)
        {
            Debug.Log(message);

            // Update messages buffer (Max 50)
            if (messages.Count > MAX_MESSAGE_COUNT)
                messages.RemoveAt(MAX_MESSAGE_COUNT);
            messages.Insert(0, message);

            OnMessageRecieved(messages);
        }

        protected virtual void OnMessageRecieved(List<string> messages)
        {
            MessageRecieved?.Invoke(this, new ConsoleEventArgs() { messages = messages });
        }
    }
}