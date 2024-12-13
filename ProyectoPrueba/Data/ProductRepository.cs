using Dapper;
using Microsoft.Data.SqlClient;
using ProyectoPrueba.Models;
using System;
using System.Collections.Generic;

namespace ProyectoPrueba.Data
{
    public class ProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<Product>("SELECT * FROM Products ORDER BY Id");
            }
        }

        public Product GetProductById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QuerySingleOrDefault<Product>("SELECT * FROM Products WHERE Id = @Id", new { Id = id });
            }
        }

        public void AddProduct(Product product)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    ValidateProduct(product);
                    connection.Execute(
                        "INSERT INTO Products (Name, Description, Price, Quantity) VALUES (@Name, @Description, @Price, @Quantity)",
                        product
                    );
                }
                catch (SqlException ex) when (ex.Number == 2627)
                {
                    throw new ArgumentException("El nombre del producto ya existe en la base de datos.");
                }
            }
        }

        public void UpdateProduct(Product product)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    ValidateProduct(product);
                    connection.Execute(
                        "UPDATE Products SET Name = @Name, Description = @Description, Price = @Price, Quantity = @Quantity WHERE Id = @Id",
                        product
                    );
                }
                catch (SqlException ex)
                {
                    throw new InvalidOperationException($"Error al actualizar el producto: {ex.Message}");
                }
            }
        }

        public void DeleteProduct(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Execute("DELETE FROM Products WHERE Id = @Id", new { Id = id });
                }
                catch (SqlException ex)
                {
                    throw new InvalidOperationException($"Error al eliminar el producto: {ex.Message}");
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

        public bool IsProductNameUnique(string productName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var count = connection.ExecuteScalar<int>(
                    "SELECT COUNT(*) FROM Products WHERE Name = @Name",
                    new { Name = productName }
                );

                return count == 0;
            }
        }
    }
}