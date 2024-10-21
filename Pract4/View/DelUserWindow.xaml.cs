using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Xml;

namespace Pract4.View
{
    /// <summary>
    /// Lógica de interacción para DelUserWindow.xaml
    /// </summary>
    public partial class DelUserWindow : Window
    {
        List<Cliente> clientes;
        string fileName;
        public DelUserWindow(List<Cliente> clientes, string fileName)
        {
            InitializeComponent();
            this.clientes = clientes;
            this.fileName = fileName;
        }

        private void delClik(object sender, RoutedEventArgs e)
        {
            delFromList();

        }

        public bool delFromList()
        {
            if (int.TryParse(tBoxId.Text, out int id) && id >= 1 && id <= clientes.Count)
            {
                Cliente clienteEliminado = clientes.FirstOrDefault(cliente => cliente.Id == id);
                if (clienteEliminado != null)
                {
                    int index = clientes.IndexOf(clienteEliminado);
                    clientes.RemoveAt(index);

                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(fileName);

                    XmlNode clienteToRemove = xmlDoc.SelectSingleNode($"/clientes/cliente[id='{id}']");
                    if (clienteToRemove != null)
                    {
                        XmlNode root = xmlDoc.DocumentElement;
                        root.RemoveChild(clienteToRemove);
                    } else MessageBox.Show("No se encontró un cliente con ese ID en el archivo XML.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    for (int i = index; i < clientes.Count; i++)
                    {
                        clientes[i].Id -= 1;

                        XmlNode idModificado = xmlDoc.SelectSingleNode($"/clientes/cliente[nombre='{clientes[i].Name}']/id");
                        idModificado.InnerText = Convert.ToString(i + 1);
                    }

                    xmlDoc.Save(fileName);

                    this.lbInfo.Content = $"{clienteEliminado.Name} ha sido eliminado.";
                    return true;
                }else return false;
            }
            else
            {
                MessageBox.Show("No se encontró un cliente con ese ID.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private void cancelClick(object sender, RoutedEventArgs e)
        {
            this.Close();  
        }
    }
}
