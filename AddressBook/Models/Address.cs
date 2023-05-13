using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AddressBook.Models
{
    [Serializable]
    public class Address : IDataErrorInfo
    {
        public int Id { get; set; }

        public string Surname { get; set; }

        public string Name { get; set; }

        public string Patronymic { get; set; }

        public string Phone { get; set; }

        public string StringFIO
        {
            get
            {
                return Surname + " " + Name + " " + Patronymic;
            }
        }

        public Address()
        {
            Id = -1;

            Surname = string.Empty;

            Name = string.Empty;

            Patronymic = string.Empty;

            Phone = string.Empty;
        }

        public Address(int id)
        {
            Id = id;

            Surname = string.Empty;

            Name = string.Empty;

            Patronymic = string.Empty;

            Phone = string.Empty;
        }

        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;

                switch (columnName)
                {
                    case "Surname":
                        if (Surname.Any(c => char.IsNumber(c)))
                        {
                            return "Фамилия не должна содержать цифр";
                        }

                        if (Surname.Length <= 2)
                        {
                            error = "Фамилия должна содержать не менее 2 букв";
                        }
                        else if (Surname.Length > 50)
                        {
                            error = "Фамилия должна содержать не более 50 букв";
                        }
                        break;

                    case "Name":
                        if (Name.Any(c => char.IsNumber(c)))
                        {
                            return "Фамилия не должна содержать цифр";
                        }

                        if (Name.Length <= 2)
                        {
                            return "Имя должно содержать не менее 2 букв";
                        }
                        else if (Name.Length > 50)
                        {
                            return "Имя должно содержать не более 50 букв";
                        }
                        break;

                    case "Phone":
                        if (Phone.Any(c => c == ' '))
                        {
                            error = "Номер телефона введен не полностью";
                        }

                        break;
                }
                return error;
            }
        }
        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        public static int FreeID(ObservableCollection<Address> addresses)
        {
            if (addresses.Count != 0 && addresses.Count - 1 != addresses.Last().Id)
            {
                List<int> listId = addresses.Select(a => a.Id).ToList();

                for (int i = 0; i < listId.Count; i++)
                {
                    if (listId[i] + 1 != listId[i + 1])
                    {
                        return i + 2;
                    }
                }
            }

            return addresses.Count;
        }
    }
}
