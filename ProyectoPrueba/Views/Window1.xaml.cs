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
using ProyectoPrueba.Data;
using ProyectoPrueba.Models;


namespace Proyecto
{
    public partial class ProductFormWindow : Window
    {
        public Product Product { get; private set; }

        public ProductFormWindow(Product product = null)
        {
            InitializeComponent();

            if (product != null)
            {
                // Carga datos si se edita un producto existente
                Product = product;
                NameTextBox.Text = product.Name;
                DescriptionTextBox.Text = product.Description;
                PriceTextBox.Text = product.Price.ToString();
                QuantityTextBox.Text = product.Quantity.ToString();
            }
            else
            {
                // Crea un nuevo producto
                Product = new Product();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text) ||
                string.IsNullOrWhiteSpace(DescriptionTextBox.Text) ||
                !decimal.TryParse(PriceTextBox.Text, out var price) ||
                !int.TryParse(QuantityTextBox.Text, out var quantity) ||
                price <= 0 || quantity <= 0)
            {
                MessageBox.Show("Por favor, completa todos los campos correctamente.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Asigna los valores al producto
            Product.Name = NameTextBox.Text;
            Product.Description = DescriptionTextBox.Text;
            Product.Price = price;
            Product.Quantity = quantity;

            DialogResult = true; // Cierra la ventana y confirma


        }
    }
}