using ECommerce.Domin.Contract.DataSeeding;
using ECommerce.Domin.Models;
using ECommerce.Domin.Models.OrderModule;
using ECommerce.Domin.Models.ProudctModule;
using ECommerce.Persistence.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Data.DataSeed
{
    public class DataIni : IDataini
    {
        private readonly StoreDbContext _dbContext;

        public DataIni(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task InitilizeAsync()
        {
            try
            {
                var HasProducts = await _dbContext.Products.AnyAsync();
                var HasTypes    = await _dbContext.ProductTypes.AnyAsync();
                var HasBrands   = await _dbContext.ProductBrands.AnyAsync();
                var HasDelivery = await _dbContext.Set<DeliveryMethod>().AnyAsync();

                if (HasProducts && HasBrands && HasTypes && HasDelivery) return;

                if (!HasBrands)
                    await SeedDataFromJsonAsync<ProductBrand ,int>("brands.json", _dbContext.ProductBrands);

                if (!HasTypes)
                    await SeedDataFromJsonAsync<ProductType ,int>("types.json", _dbContext.ProductTypes);
                _dbContext.SaveChanges();

                if (!HasProducts)
                    await SeedDataFromJsonAsync<Product ,int>("products.json", _dbContext.Products);

                if (!HasDelivery)
                    await SeedDataFromJsonAsync<DeliveryMethod ,int>("delivery.json", _dbContext.Set<DeliveryMethod>());

                _dbContext.SaveChanges();

            }
            catch (Exception ex)
            {

                Console.WriteLine($"Data Seed Failed : {ex}");
            }
        }
        private async Task SeedDataFromJsonAsync<T , TKey>(string fileName , DbSet<T> dbSet) where T : BaseEntity<TKey>
        {
            // Get File Path
            var path = @"..\ECommerce.Persistence\Data\DataSeed\JSONFiles\" + fileName;
            if (!File.Exists(path)) throw new FileNotFoundException($"{fileName} Not Found");

            try
            {
                using var DataStream = File.OpenRead(path);

                var Data = JsonSerializer.Deserialize<List<T>>(DataStream , new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });
                if (Data is not null )
                {
                    await dbSet.AddRangeAsync(Data);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to Read Data from Json: {ex}");
            }
        }
    }
}
