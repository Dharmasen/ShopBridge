using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ShopBridge.Models;

namespace ShopBridge.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {

        private readonly ILogger<InventoryController> _logger;
        private readonly IConfiguration config;


        public InventoryController(ILogger<InventoryController> logger, IConfiguration configuration)
        {
            _logger = logger;
            this.config = configuration;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                using (var database = GetShopBridgeDbContext())
                {
                    var inventoryList = new List<InventoryModel>();
                    var result = database.InventoryItems.ToList();
                    foreach(var item in result){
                        var data = new InventoryModel();
                        data.Id = item.Id;
                        data.Category = item.Category;
                        data.Price = item.Price;
                        data.Name = item.Name;
                        data.Description = item.Description;
                        inventoryList.Add(data);
                    }
                    return new OkObjectResult(inventoryList);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.ToString());
                throw;
            }
        }


        [HttpPost]
        public async Task<IActionResult> Post()
        {
            try
            {
                using (var database = GetShopBridgeDbContext())
                {
                    var body = await new StreamReader(Request.Body).ReadToEndAsync();
                    var request = JsonConvert.DeserializeObject<InventoryModel>(body);
                    if (request.Id == 0)
                    {
                        var inventoryData = new InventoryItems
                        {
                            Name = request.Name,
                            Description = request.Description,
                            Price = request.Price,
                            Category = request.Category
                        };
                        database.InventoryItems.Add(inventoryData);
                        await database.SaveChangesAsync();
                    }
                    else
                    {
                        var updateInventoryData = database.InventoryItems.Where(c => c.Id == request.Id).ToList();
                        foreach(var data in updateInventoryData){
                            data.Name = request.Name;
                            data.Description = request.Description;
                            data.Price = request.Price;
                            data.Category = request.Category;
                            database.Entry(data).State = EntityState.Modified;
                            await database.SaveChangesAsync();
                        }
                    }
                    return new OkResult();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.ToString());
                throw;
            }
        }


        [HttpDelete]
        public async Task<IActionResult> Delete(int Id)
        {
            try
            {
                using (var database = GetShopBridgeDbContext())
                {
                  var result = database.InventoryItems.Where(c=>c.Id==Id).FirstOrDefault();
                    database.InventoryItems.RemoveRange(result);
                    await database.SaveChangesAsync();
                    return new OkResult();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.ToString());
                throw;
            }
        }

        private ShopBridgeDbContext GetShopBridgeDbContext()
        {
            return new ShopBridgeDbContext(config["Db:ConnectionString"]);
        }
    }
}
