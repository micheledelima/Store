﻿using MongoDB.Driver;
using Store.Product.Interfaces.Repositories;
using Store.Product.Models;

namespace Store.DataBase.Product.Repositories
{
    public class ProductRepository : IProductRepository
    {
        // String de conexão 
        private readonly string _connectionString;
        private readonly IMongoCollection<ProductModel> _collection;
        public ProductRepository()
        {
            _connectionString = "mongodb://localhost:27017";
            // Criar uma instância do MongoClient com a string de conexão
            MongoClient client = new(_connectionString);
            // Obter uma referência ao banco de dados
            IMongoDatabase database = client.GetDatabase("nossa_lojinha");
            _collection = database.GetCollection<ProductModel>("product");
        }

        //criar crud necessário para cada entidade
        public async Task CreateAsync(ProductModel productModel)
        {
            try
            {
                await _collection.InsertOneAsync(productModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task UpdateAsync(ProductModel productModel)
        {
            try
            {
                _ = await _collection.ReplaceOneAsync(x => x.Id == productModel.Id, productModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task RemoveAsync(ProductModel productModel)
        {
            try
            {
                _ = await _collection.DeleteOneAsync(x => x.Id == productModel.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<bool> FindOneAsync(ProductModel productModel)
        {
            var result = await (await _collection.FindAsync(x => x.ProductId == productModel.ProductId)).FirstOrDefaultAsync();
            if (result != null)
                return true;
            else
                return false;
        }

        public async Task<List<ProductModel>> FindAllAsync()
        {
            var products = new List<ProductModel>();
            products = await (await _collection.FindAsync(x => x.Id != null)).ToListAsync();

            return products;
        }

        public async Task<ProductModel> FindByProductNameAsync(string name)
        {
            var product = new ProductModel();
            product = await (await _collection.FindAsync(x => x.ProductName.ToLowerInvariant() == name.ToLowerInvariant())).FirstOrDefaultAsync();

            return product;
        }

        public async Task<ProductModel> FindByProductIdAsync(int code)
        {
            var product = new ProductModel();
            product = await (await _collection.FindAsync(x => x.ProductId == code)).FirstOrDefaultAsync();

            return product;
        }
    }
}
