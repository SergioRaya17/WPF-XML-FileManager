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
    
    public partial class ModifyUserWindow : Window
    {
        List<Cliente> clientes;
        string fileName;
        public ModifyUserWindow(List<Cliente> clientes, string fileName)
        {
            InitializeComponent();
            this.clientes = clientes;
            this.fileName = fileName;
        }

        private void actualizarClick(object sender, RoutedEventArgs e)
        {

            if (int.Parse(this.lbIdentificador.Text) > 0 && int.Parse(this.lbIdentificador.Text) <= clientes.Count)
            {
                Cliente clienteModificado = clientes.FirstOrDefault(cliente => cliente.Id == int.Parse(this.lbIdentificador.Text));
                if (this.lbNombre.Text != "")
                    clienteModificado.Name = this.lbNombre.Text;

                if (this.lbCorreo.Text != "" && Regex.IsMatch(this.lbCorreo.Text, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                    clienteModificado.Correo = this.lbCorreo.Text;

                MessageBox.Show("Cliente actualizado con exito!", "Actulizado", MessageBoxButton.OK, MessageBoxImage.Information);
                updateXml();

            } else MessageBox.Show("ERROR! No se ha encontrado ese id.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public void updateXml()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(fileName);

            XmlNode nameToModify = xmlDoc.SelectSingleNode($"/clientes/cliente[id='{this.lbIdentificador.Text}']/nombre");
            XmlNode correoToModify = xmlDoc.SelectSingleNode($"/clientes/cliente[id='{this.lbIdentificador.Text}']/correo");

            if (this.lbNombre.Text != "")
                nameToModify.InnerText = this.lbNombre.Text;
                
            if (this.lbCorreo.Text != "" && Regex.IsMatch(this.lbCorreo.Text, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                correoToModify.InnerText = this.lbCorreo.Text;

            xmlDoc.Save(fileName);
            clear();
        }

        public void clear()
        {
            this.lbIdentificador.Text = "";
            this.lbNombre.Text = "";
            this.lbCorreo.Text = "";
        }

        private void cancelarClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
