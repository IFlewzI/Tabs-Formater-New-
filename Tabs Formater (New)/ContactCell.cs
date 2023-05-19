using System;
using System.Collections.Generic;
using System.Text;

namespace Tabs_Formater
{
    class ContactCell
    {
        public string FullName { get; private set; }
        public string PhoneNumber { get; private set; }

        public ContactCell()
        {
            FullName = "Default";
            PhoneNumber = "Default";
        }

        public ContactCell(string name, string phoneNumber)
        {
            FullName = name;
            PhoneNumber = phoneNumber;
        }

        public void SetFullName(string newFullName) => FullName = newFullName;

        public void SetPhoneNumber(string newPhoneNumber) => PhoneNumber = newPhoneNumber;
    }
}