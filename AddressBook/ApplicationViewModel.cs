using System;
using System.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using AddressBook.Models;

using System.Xml;
using AddressBook.Service;

namespace AddressBook
{
    class ApplicationViewModel : INotifyPropertyChanged
    {
        private Address selectedAddress;

        private Address operationAddress { get; set; }

        public bool TextActive { get; set; }

        public static ObservableCollection<Address> Addresses { get; set; }

        private RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                    (addCommand = new RelayCommand(obj =>
                    {
                        Address address = new Address(Address.FreeID(Addresses));

                        Addresses.Insert(0, address);

                        SelectedAddress = address;
                    }));
            }
        }

        private RelayCommand updateCommand;
        public RelayCommand UpdateCommand
        {
            get
            {
                return updateCommand ??
                  (updateCommand = new RelayCommand(obj =>
                  {

                  }));
            }
        }

        private RelayCommand removeCommand;
        public RelayCommand RemoveCommand
        {
            get
            {
                return removeCommand ??
                  (removeCommand = new RelayCommand(obj =>
                  {
                      if (Addresses.Count > 0)
                      {
                          Addresses.Remove(selectedAddress);

                          if (Addresses.Count > 0)
                          {
                              SelectedAddress = Addresses[0];
                          }
                          else
                          {
                              Address address = new Address();

                              Addresses.Insert(0, address);

                              selectedAddress = address;
                          }
                      }
                  }));
            }
        }


        private RelayCommand saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return saveCommand ??
                  (saveCommand = new RelayCommand(obj =>
                  {
                      TextActive = !TextActive;
                  }));
            }
        }

        private RelayCommand cancelCommand;
        public RelayCommand CancelCommand
        {
            get
            {
                return cancelCommand ??
                  (cancelCommand = new RelayCommand(obj =>
                  {
                      selectedAddress.Name = "cancel";
                  }));
            }
        }

        public Address SelectedAddress
        {
            get { return selectedAddress; }
            set
            {
                selectedAddress = value;

                OnPropertyChanged("SelectedAddress");
            }
        }

        public static void SaveKostil()
        {
            XMLProvider.SaveXML(Addresses);
        }

        public ApplicationViewModel()
        {
            XMLProvider xProvider = new XMLProvider("AddressBook.xml");

            Addresses = XMLProvider.ReadFile();

            selectedAddress = Addresses.First();

            operationAddress = selectedAddress;

            TextActive = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }


    }
}
