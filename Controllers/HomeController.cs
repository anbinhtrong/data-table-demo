using DataTableSample.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DataTableSample.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult DataTableDemo()
        {
            return View();
        }

        public IActionResult FixedColumn()
        {
            return View();
        }

        public IActionResult AjaxDataDemo()
        {
            return View();
        }

        public IActionResult ServerSideLoadDemo()
        {
            return View();
        }

        public IActionResult IndividualColumnSearching()
        {
            return View();
        }

        public IActionResult StateSavingDemo()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            var products = new[]
            {
                new { id = 1, name = "Apple", category = "Fruit", price = 10m, stock = 150, rating = 4.5, status = "In Stock", dateAdded = "2024-01-15", supplier = "Fresh Farms Inc" },
                new { id = 2, name = "Milk", category = "Dairy", price = 20m, stock = 80, rating = 4.8, status = "In Stock", dateAdded = "2024-01-14", supplier = "Dairy Corp" },
                new { id = 3, name = "Orange", category = "Fruit", price = 8m, stock = 200, rating = 4.3, status = "In Stock", dateAdded = "2024-01-13", supplier = "Citrus Growers" },
                new { id = 4, name = "Cheese", category = "Dairy", price = 15m, stock = 45, rating = 4.6, status = "Low Stock", dateAdded = "2024-01-12", supplier = "Artisan Cheese Co" },
                new { id = 5, name = "Banana", category = "Fruit", price = 5m, stock = 300, rating = 4.4, status = "In Stock", dateAdded = "2024-01-11", supplier = "Tropical Fruit Ltd" },
                new { id = 6, name = "Yogurt", category = "Dairy", price = 12m, stock = 120, rating = 4.7, status = "In Stock", dateAdded = "2024-01-10", supplier = "Health Foods" },
                new { id = 7, name = "Carrot", category = "Vegetable", price = 6m, stock = 250, rating = 4.2, status = "In Stock", dateAdded = "2024-01-09", supplier = "Organic Veggies" },
                new { id = 8, name = "Butter", category = "Dairy", price = 18m, stock = 60, rating = 4.9, status = "In Stock", dateAdded = "2024-01-08", supplier = "Premium Dairy" },
                new { id = 9, name = "Broccoli", category = "Vegetable", price = 7m, stock = 0, rating = 4.1, status = "Out of Stock", dateAdded = "2024-01-07", supplier = "Green Harvest" },
                new { id = 10, name = "Cream", category = "Dairy", price = 25m, stock = 35, rating = 4.8, status = "Low Stock", dateAdded = "2024-01-06", supplier = "Luxury Dairy" },
                new { id = 11, name = "Grapes", category = "Fruit", price = 12m, stock = 180, rating = 4.6, status = "In Stock", dateAdded = "2024-01-05", supplier = "Vineyard Direct" },
                new { id = 12, name = "Spinach", category = "Vegetable", price = 4m, stock = 220, rating = 4.3, status = "In Stock", dateAdded = "2024-01-04", supplier = "Fresh Greens Co" },
                new { id = 13, name = "Tomato", category = "Vegetable", price = 5m, stock = 310, rating = 4.2, status = "In Stock", dateAdded = "2024-01-03", supplier = "Garden Fresh" },
                new { id = 14, name = "Lettuce", category = "Vegetable", price = 3m, stock = 290, rating = 4.4, status = "In Stock", dateAdded = "2024-01-02", supplier = "Salad Farms" },
                new { id = 15, name = "Potatoes", category = "Vegetable", price = 8m, stock = 400, rating = 4.5, status = "In Stock", dateAdded = "2024-01-01", supplier = "Root Vegetables Inc" }
            };

            return Json(new { data = products });
        }

        [HttpPost]
        public IActionResult GetProductsServerSide([FromForm] DataTableRequest request)
        {
            // Sample data source
            var allProducts = new List<ProductDto>
            {
                new ProductDto { Id = 1, Name = "Apple", Category = "Fruit", Price = 10m, Stock = 150, Rating = 4.5, Status = "In Stock", DateAdded = "2024-01-15", Supplier = "Fresh Farms Inc" },
                new ProductDto { Id = 2, Name = "Milk", Category = "Dairy", Price = 20m, Stock = 80, Rating = 4.8, Status = "In Stock", DateAdded = "2024-01-14", Supplier = "Dairy Corp" },
                new ProductDto { Id = 3, Name = "Orange", Category = "Fruit", Price = 8m, Stock = 200, Rating = 4.3, Status = "In Stock", DateAdded = "2024-01-13", Supplier = "Citrus Growers" },
                new ProductDto { Id = 4, Name = "Cheese", Category = "Dairy", Price = 15m, Stock = 45, Rating = 4.6, Status = "Low Stock", DateAdded = "2024-01-12", Supplier = "Artisan Cheese Co" },
                new ProductDto { Id = 5, Name = "Banana", Category = "Fruit", Price = 5m, Stock = 300, Rating = 4.4, Status = "In Stock", DateAdded = "2024-01-11", Supplier = "Tropical Fruit Ltd" },
                new ProductDto { Id = 6, Name = "Yogurt", Category = "Dairy", Price = 12m, Stock = 120, Rating = 4.7, Status = "In Stock", DateAdded = "2024-01-10", Supplier = "Health Foods" },
                new ProductDto { Id = 7, Name = "Carrot", Category = "Vegetable", Price = 6m, Stock = 250, Rating = 4.2, Status = "In Stock", DateAdded = "2024-01-09", Supplier = "Organic Veggies" },
                new ProductDto { Id = 8, Name = "Butter", Category = "Dairy", Price = 18m, Stock = 60, Rating = 4.9, Status = "In Stock", DateAdded = "2024-01-08", Supplier = "Premium Dairy" },
                new ProductDto { Id = 9, Name = "Broccoli", Category = "Vegetable", Price = 7m, Stock = 0, Rating = 4.1, Status = "Out of Stock", DateAdded = "2024-01-07", Supplier = "Green Harvest" },
                new ProductDto { Id = 10, Name = "Cream", Category = "Dairy", Price = 25m, Stock = 35, Rating = 4.8, Status = "Low Stock", DateAdded = "2024-01-06", Supplier = "Luxury Dairy" },
                new ProductDto { Id = 11, Name = "Grapes", Category = "Fruit", Price = 12m, Stock = 180, Rating = 4.6, Status = "In Stock", DateAdded = "2024-01-05", Supplier = "Vineyard Direct" },
                new ProductDto { Id = 12, Name = "Spinach", Category = "Vegetable", Price = 4m, Stock = 220, Rating = 4.3, Status = "In Stock", DateAdded = "2024-01-04", Supplier = "Fresh Greens Co" },
                new ProductDto { Id = 13, Name = "Tomato", Category = "Vegetable", Price = 5m, Stock = 310, Rating = 4.2, Status = "In Stock", DateAdded = "2024-01-03", Supplier = "Garden Fresh" },
                new ProductDto { Id = 14, Name = "Lettuce", Category = "Vegetable", Price = 3m, Stock = 290, Rating = 4.4, Status = "In Stock", DateAdded = "2024-01-02", Supplier = "Salad Farms" },
                new ProductDto { Id = 15, Name = "Potatoes", Category = "Vegetable", Price = 8m, Stock = 400, Rating = 4.5, Status = "In Stock", DateAdded = "2024-01-01", Supplier = "Root Vegetables Inc" },
                new ProductDto { Id = 16, Name = "Watermelon", Category = "Fruit", Price = 15m, Stock = 90, Rating = 4.7, Status = "In Stock", DateAdded = "2023-12-31", Supplier = "Melon Farms" },
                new ProductDto { Id = 17, Name = "Broccoli Sprouts", Category = "Vegetable", Price = 9m, Stock = 60, Rating = 4.2, Status = "In Stock", DateAdded = "2023-12-30", Supplier = "Sprout Factory" },
                new ProductDto { Id = 18, Name = "Cheddar Cheese", Category = "Dairy", Price = 22m, Stock = 50, Rating = 4.8, Status = "In Stock", DateAdded = "2023-12-29", Supplier = "Cheese Masters" },
                new ProductDto { Id = 19, Name = "Strawberry", Category = "Fruit", Price = 14m, Stock = 110, Rating = 4.6, Status = "In Stock", DateAdded = "2023-12-28", Supplier = "Berry Bliss" },
                new ProductDto { Id = 20, Name = "Kale", Category = "Vegetable", Price = 5m, Stock = 180, Rating = 4.3, Status = "In Stock", DateAdded = "2023-12-27", Supplier = "Green Leafy Co" }
            };

            var query = allProducts.AsQueryable();
            var totalRecords = allProducts.Count;

            // Search logic
            if (!string.IsNullOrEmpty(request.Search?.Value))
            {
                var searchValue = request.Search.Value.ToLower();
                query = query.Where(x => x.Name.ToLower().Contains(searchValue)
                                      || x.Category.ToLower().Contains(searchValue)
                                      || x.Supplier.ToLower().Contains(searchValue));
            }

            var filteredRecords = query.Count();

            // Sorting logic
            if (request.Order != null && request.Order.Count > 0)
            {
                var orderColumn = request.Order[0].Column;
                var orderDir = request.Order[0].Dir.ToLower() == "desc" ? 1 : 0;

                query = orderColumn switch
                {
                    0 => orderDir == 1 ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id),
                    1 => orderDir == 1 ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
                    2 => orderDir == 1 ? query.OrderByDescending(x => x.Category) : query.OrderBy(x => x.Category),
                    3 => orderDir == 1 ? query.OrderByDescending(x => x.Price) : query.OrderBy(x => x.Price),
                    4 => orderDir == 1 ? query.OrderByDescending(x => x.Stock) : query.OrderBy(x => x.Stock),
                    5 => orderDir == 1 ? query.OrderByDescending(x => x.Rating) : query.OrderBy(x => x.Rating),
                    _ => query.OrderBy(x => x.Id)
                };
            }

            // Pagination
            var data = query.Skip(request.Start).Take(request.Length).ToList();

            var response = new DataTableResponse<ProductDto>
            {
                Draw = request.Draw,
                RecordsTotal = totalRecords,
                RecordsFiltered = filteredRecords,
                Data = data
            };

            return Json(response);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
