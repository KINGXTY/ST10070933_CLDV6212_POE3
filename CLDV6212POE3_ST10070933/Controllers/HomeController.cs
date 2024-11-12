using CLDV6212POE3_ST10070933.Models;
using CLDV6212POE3_ST10070933.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Azure.Data.Tables;

namespace CLDV6212POE1_ST10070933.Controllers
{
    public class HomeController : Controller
    {
        private readonly BlobService _blobService;
        private readonly FileService _fileService;
        private readonly QueueService _queueService;
        private readonly TableService _tableService;
        private readonly CustomerService _customerService;
        private readonly OrderServices _orderServices;
        private readonly ProductService _productService;

        public HomeController(BlobService blobService, FileService fileService, QueueService queueService, TableService tableService, CustomerService customerService, OrderServices orderServices, ProductService productService )
        {
            _blobService = blobService;
            _fileService = fileService;
            _queueService = queueService;
            _tableService = tableService;
            _customerService = customerService;
            _orderServices = orderServices;
            _productService = productService;

        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Products()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    await _blobService.UploadBlobAsync("images", file.FileName, stream);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomerProfile(string firstName, string lastName, string email, string phoneNumber)
        {
            var profile = new CustomerProfile
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PhoneNumber = phoneNumber
            };

            await _tableService.AddEntityAsync("CustomerProfiles", profile);
            await _customerService.InsertCustomerAsync(profile);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ProcessOrder(int orderId)
        {
            var message = $"Order {orderId} processed at {DateTime.UtcNow}";
            await _queueService.SendMessageAsync("orders", message);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UploadContract(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    await _fileService.UploadFileAsync("contracts", "directory", file.FileName, stream);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> NewOrder(int CustomerProfileId, DateTime OrderDate, decimal TotalAmount)
        {
            var newOrder = new Orders
            {
                CustomerProfileId = CustomerProfileId,
                OrderDate = OrderDate,
                TotalAmount = TotalAmount
            };

            await _orderServices.InsertOrderAsync(newOrder);

            return RedirectToAction("Products"); 
        }
        [HttpPost]
        public async Task<IActionResult> CreateProduct(string name, string description, decimal price, string category, int stockLevel, string imageUrl)
        {
            var newProduct = new Product
            {
                Name = name,
                Description = description,
                Price = price,
                Category = category,
                StockLevel = stockLevel,                
                CreatedDate = DateTime.UtcNow 
            };

            await _productService.InsertProductAsync(newProduct); 
            return RedirectToAction("Products"); 
        }

    }
}