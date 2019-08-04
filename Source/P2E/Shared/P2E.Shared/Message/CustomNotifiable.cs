using FluentValidator;
using System;
using System.Collections.Generic;
using System.Text;

namespace P2E.Shared.Message
{
    public class CustomNotifiable : Notifiable
    {
        public string Messages { get { return ParseMessages(); }}

        private string ParseMessages()
        {
            StringBuilder msg = new StringBuilder();
            if (Notifications != null)
            {
                foreach (var item in Notifications)
                {
                    if(!msg.ToString().Contains(item.Message))
                    msg.AppendLine($"{item.Message}");
                }
            }

            return msg.ToString();
        }

        public void RemoveNotifications()
        {

        }
    }
}
