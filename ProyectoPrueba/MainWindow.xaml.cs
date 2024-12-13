using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Configuration;
using ProyectoPrueba.Data;
using ProyectoPrueba.Models;

namespace Proyecto
{
    public partial class MainWindow : Window
    {
        private readonly ProductRepository _repository;
        public ObservableCollection<Product> Products { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["ProductInventoryConnectionString"].ConnectionString;

            _repository = new ProductRepository(connectionString);

            Products = new ObservableCollection<Product>(_repository.GetAllProducts().ToList());
            ProductsDataGrid.ItemsSource = Products;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var form = new ProductFormWindow();
            if (form.ShowDialog() == true)
            {
                try
                {
                    ValidateProduct(form.Product);
                    if (_repository.IsProductNameUnique(form.Product.Name))
                    {
                        _repository.AddProduct(form.Product);
                        Products.Add(form.Product);
                    }
                    else
                    {
                        MessageBox.Show("El nombre del producto ya existe en la base de datos.", "Error", MessageBoxButton.OK);
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorMessage(ex.Message);
                }
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsDataGrid.SelectedItem is Product selectedProduct)
            {
                var form = new ProductFormWindow(selectedProduct);
                if (form.ShowDialog() == true)
                {
                    try
                    {
                        ValidateProduct(form.Product);
                        _repository.UpdateProduct(form.Product);
                        // Actualizar el objeto seleccionado en la colección
                        foreach (var product in Products)
                        {
                            if (product.Id == form.Product.Id)
                            {
                                Products[Products.IndexOf(product)] = form.Product;
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowErrorMessage(ex.Message);
                    }
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsDataGrid.SelectedItem is Product selectedProduct)
            {
                if (MessageBox.Show($"¿Eliminar '{selectedProduct.Name}'?", "Confirmar", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        _repository.DeleteProduct(selectedProduct.Id);
                        Products.Remove(selectedProduct);
                    }
                    catch (Exception ex)
                    {
                        ShowErrorMessage(ex.Message);
                    }
                }
            }
        }

        private void ValidateProduct(Product product)
        {
            if (string.IsNullOrEmpty(product.Name))
            {
                throw new ArgumentException("El campo 'Nombre' es obligatorio.");
            }
            if (product.Price <= 0)
            {
                throw new ArgumentException("El precio debe ser un número positivo.");
            }
            if (product.Quantity <= 0)
            {
                throw new ArgumentException("La cantidad debe ser mayor que cero.");
            }
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK);
        }
    }
}
