using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Pract4.View;

namespace Pract4
{
    public partial class MainWindow : Window
    {
        private string fileName = "Clientes.xml";
        private List<Cliente> clientes = new List<Cliente>();

        public MainWindow()
        {
            InitializeComponent();
            getXMLData();
            showAll();
        }
        private void btnShowAllClick(object sender, RoutedEventArgs e)
        {
            showAll();
        }

        private void btnSearchClick(object sender, RoutedEventArgs e)
        {
            showByID(int.Parse(this.tbId.Text));
        }

        public void showByID(int id)
        {
            foreach (var cliente in clientes)
            {
                if (cliente.Id == id)
                {
                    lbShowData.Content = $"{cliente.Id} | {cliente.Name} - {cliente.Correo}";
                }
            }
        }
        public void showAll()
        {
            string data = "";
            foreach (var cliente in clientes)
            {
                data += $"{cliente.Id} | {cliente.Name} - {cliente.Correo}\n";
            }
            this.lbShowData.Content = data;
        }

        public void getXMLData()
        {
            if (File.Exists(fileName))
            {
                using (XmlReader reader = XmlReader.Create(fileName))
                {
                    string nombre = "", correo = "";
                    int id = 0;

                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name)
                            {
                                case "id":
                                    reader.Read();
                                    if (reader.NodeType == XmlNodeType.Text)
                                        if (!int.TryParse(reader.Value, out id)) Console.Error.WriteLine("ERROR! En la conversión de tipos.");
                                    break;

                                case "nombre":
                                    reader.Read();
                                    if (reader.NodeType == XmlNodeType.Text)
                                        nombre = reader.Value.Trim();
                                    break;

                                case "correo":
                                    reader.Read();
                                    if (reader.NodeType == XmlNodeType.Text)
                                        correo = reader.Value.Trim();

                                    clientes.Add(new Cliente(id, nombre, correo));
                                    break;
                            }
                        }
                    }
                }
            }
            else
            {
                Console.Error.WriteLine("ERROR! No se ha encontrado el archivo.");
            }
        }

        private void btnAddClick(object sender, RoutedEventArgs e)
        {
            AddUserWindow addUserWindow = new AddUserWindow(clientes, fileName);
            addUserWindow.ShowDialog();
        }

        private void btnDeleteClick(object sender, RoutedEventArgs e)
        {
            DelUserWindow delUserWindow = new DelUserWindow(clientes, fileName);
            delUserWindow.ShowDialog();
        }

        private void btnModifyClick(object sender, RoutedEventArgs e)
        {
            ModifyUserWindow modifyUserWindow = new ModifyUserWindow(clientes, fileName);
            modifyUserWindow.ShowDialog();
        }
    }

    public class Cliente
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Correo { get; set; }

        public Cliente (int id, string name, string correo)
        {
            Id = id;
            Name = name;
            Correo = correo;
        }
    }
}
