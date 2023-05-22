using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using AddressBook.Models;
using AddressBook.Service;

namespace AddressBook
{
    class ApplicationViewModel : INotifyPropertyChanged
    {
        private Address selectedAddress;

        private Address operationAddress { get; set; }

        private string operationName { get; set; }

        public bool TextActive { get; set; }

        public static ObservableCollection<Address> Addresses { get; set; }

        // (команда) Кнопка добавления нового адреса
        private RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                    (addCommand = new RelayCommand(obj =>
                    {
                        Address address = new Address(Address.FreeID(Addresses));

                        operationName = "addAddress";

                        operationAddress = address;

                        Addresses.Insert(0, address);

                        SelectedAddress = address;
                    }));
            }
        }

        // (команда) Кнопка изменения выбраного адреса
        private RelayCommand updateCommand;
        public RelayCommand UpdateCommand
        {
            get
            {
                return updateCommand ??
                  (updateCommand = new RelayCommand(obj =>
                  {
                      operationName = "updateAddress";

                      operationAddress = selectedAddress;
                  }));
            }
        }

        // (команда) Кнопка удаления выбранного адреса
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

        // (команда) Кнопка сохранения изменений
        private RelayCommand saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return saveCommand ??
                  (saveCommand = new RelayCommand(obj =>
                  {
                      operationName = "";

                      operationAddress = null;
                  }));
            }
        }

        // (команда) Кнопка отмены изменений
        private RelayCommand cancelCommand;
        public RelayCommand CancelCommand
        {
            get
            {
                return cancelCommand ??
                  (cancelCommand = new RelayCommand(obj =>
                  {
                      switch (operationName)
                      {
                          case "addAddress":
                              Addresses.Remove(Addresses.Single(a => a.Id == operationAddress.Id));

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
                              break;

                          case "updateAddress":
                              selectedAddress = operationAddress;

                              int index = Addresses.IndexOf(Addresses.Single(a => a.Id == operationAddress.Id));

                              Addresses[index] = operationAddress;

                              break;

                          default:
                              break;
                      }

                      operationName = "";

                      operationAddress = null;

                  }));
            }
        }

        // Обработка выбора адреса
        public Address SelectedAddress
        {
            get { return selectedAddress; }
            set
            {
                selectedAddress = value;

                OnPropertyChanged("SelectedAddress");
            }
        }


        public static void SaveInFile()
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
