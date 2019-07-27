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
                    msg.Append($"{item.Property}: {item.Message}");
                }
            }

            return msg.ToString();
        }
    }
}
