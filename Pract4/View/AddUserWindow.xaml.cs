using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;

namespace Pract4.View
{
    /// <summary>
    /// Lógica de interacción para AddUserWindow.xaml
    /// </summary>
    public partial class AddUserWindow : Window
    {
        List<Cliente> clientes;
        string fileName;
        public AddUserWindow(List<Cliente> clientes, string fileName)
        {
            InitializeComponent();
            this.clientes = clientes;
            this.fileName = fileName;
        }

        private void aceptarClick(object sender, RoutedEventArgs e)
        {
            if (tBoxName.Text != "" && Regex.IsMatch(tBoxMail.Text, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
            {
                clientes.Add(new Cliente(clientes.Last().Id + 1, tBoxName.Text, tBoxMail.Text));
                MessageBox.Show("Cliente creado con exito!");

                clearAll();
                updateXML();
            } else MessageBox.Show("El formato de los datos es incorrecto.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            this.Close();
        }

        public void updateXML()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(fileName);

            XmlElement parentElement = xmlDoc.CreateElement("cliente");
            
            XmlElement id = xmlDoc.CreateElement("id");
            XmlElement nombre = xmlDoc.CreateElement("nombre");
            XmlElement correo = xmlDoc.CreateElement("correo");

            id.InnerText = Convert.ToString(clientes.Last().Id);
            nombre.InnerText = clientes.Last().Name;
            correo.InnerText = clientes.Last().Correo;

            parentElement.AppendChild(id);
            parentElement.AppendChild(nombre);
            parentElement.AppendChild(correo);

            XmlNode root = xmlDoc.DocumentElement;
            root.AppendChild(parentElement);

            xmlDoc.Save(fileName);
        }

        private void cancelarClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void clearAll()
        {
            tBoxName.Text = "";
            tBoxMail.Text = "";
        }
    }
}
