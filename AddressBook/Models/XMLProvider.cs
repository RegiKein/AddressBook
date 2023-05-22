using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace AddressBook.Models
{
    class XMLProvider
    {
        static ObservableCollection<Address>? LastLoadAddresBook;

        private static string FileName = string.Empty;

        public XMLProvider(string fileName)
        {
            Logger.InitLogger();

            FileName = fileName;
        }

        static public ObservableCollection<Address> ReadFile()
        {
            if (String.IsNullOrEmpty(FileName))
            {
                Logger.Log.Error("XMLProvider.Readfile() Filename is Null or Empty");

                return new ObservableCollection<Address>();
            }

            Logger.Log.Info("XMLProvider.Readfile() from file " + FileName + " BEGIN");

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Address[]));

                Address[]? addressesArray = null;

                using (FileStream fs = new FileStream(FileName, FileMode.OpenOrCreate))
                {
                    addressesArray = (serializer.Deserialize(fs) as Address[]).OrderBy(a => a.Id).ToArray();
                }

                ObservableCollection<Address> addresses = new ObservableCollection<Address>(addressesArray.OrderBy(a => a.Id));

                LastLoadAddresBook = addresses;

                Logger.Log.Info("XMLProvider.Readfile() from file " + FileName + " END");

                return addresses;
            }
            catch (Exception ex)
            {
                Logger.Log.Error("XMLProvider.Readfile() " + ex.ToString());

                return new ObservableCollection<Address>();
            }
        }

        static public void SaveXML(ObservableCollection<Address> addresses)
        {
            if (String.IsNullOrEmpty(FileName))
            {
                Logger.Log.Error("XMLProvider.SaveXML() Filename is Null or Empty");

                return;
            }

            try
            {
                Logger.Log.Info("XMLProvider.SaveXML() from file " + FileName + " BEGIN");

                XmlSerializer formatter = new XmlSerializer(typeof(ObservableCollection<Address>));

                using (FileStream fs = new FileStream(FileName, FileMode.Create))
                {
                    formatter.Serialize(fs, addresses);
                }

                Logger.Log.Info("XMLProvider.SaveXML() from file " + FileName + " END");
            }
            catch (Exception ex)
            {
                Logger.Log.Error("XMLProvider.Readfile() " + ex.ToString());
            }
        }


        // TODO: Пока оставить тут. пригодится при реализации активного хранения изменений
        /*public void AddElement(Address address)
        {
            if (xRoot != null)
            {
                foreach (XmlElement xnode in xRoot)
                {
                    if (address.Id == Convert.ToInt32(xnode.Attributes.GetNamedItem("id").Value))
                    {
                        if (xnode.ChildNodes[0].InnerText == address.Surname &&
                            xnode.ChildNodes[1].InnerText == address.Name &&
                            xnode.ChildNodes[2].InnerText == address.Patronymic &&
                            xnode.ChildNodes[3].InnerText == address.Phone)
                        {
                            return;
                        }
                        else
                        {
                            address.Id++;
                        }
                    }
                }

                XmlElement addressElem = XmlDoc.CreateElement("address");

                XmlAttribute idAddressAttr = XmlDoc.CreateAttribute("id");
                idAddressAttr.AppendChild(XmlDoc.CreateTextNode(address.Id.ToString()));

                XmlElement surnameElem = XmlDoc.CreateElement("surname");
                surnameElem.AppendChild(XmlDoc.CreateTextNode(address.Surname.ToString()));

                XmlElement nameElem = XmlDoc.CreateElement("name");
                nameElem.AppendChild(XmlDoc.CreateTextNode(address.Name.ToString()));

                XmlElement patronymicElem = XmlDoc.CreateElement("patronymic");
                patronymicElem.AppendChild(XmlDoc.CreateTextNode(address.Patronymic.ToString()));

                XmlElement phoneElem = XmlDoc.CreateElement("phone");
                phoneElem.AppendChild(XmlDoc.CreateTextNode(address.Phone.ToString()));

                addressElem.Attributes.Append(idAddressAttr);

                addressElem.AppendChild(surnameElem);
                addressElem.AppendChild(nameElem);
                addressElem.AppendChild(patronymicElem);
                addressElem.AppendChild(phoneElem);

                xRoot?.AppendChild(addressElem);

                XmlDoc.Save(FileName);
            }
        }

        public void UpdateElement(Address address)
        {
            if (xRoot != null)
            {
                foreach (XmlElement xnode in xRoot)
                {
                    if (address.Id == Convert.ToInt32(xnode.Attributes.GetNamedItem("id").Value))
                    {
                        foreach (XmlNode childnode in xnode.ChildNodes)
                        {
                            switch (childnode.Name)
                            {
                                case "surname":
                                    childnode.InnerText = address.Surname;
                                    break;

                                case "name":
                                    childnode.InnerText = address.Name;
                                    break;

                                case "patronymic":
                                    childnode.InnerText = address.Patronymic;
                                    break;

                                case "phone":
                                    childnode.InnerText = address.Phone;
                                    break;
                            }
                        }
                        break;
                    }
                }
                XmlDoc.Save(FileName);
            }
        }

        public void RemoveElement(int elementId)
        {

        }*/
    }
}
