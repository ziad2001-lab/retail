using inventory_UI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace inventory_UI.Controllers
{
    public class ProductController : Controller
    {
        Uri BaseAddress = new Uri("https://localhost:7237/api");
        //Uri BaseAddress = new Uri("https://localhost:44378/api");
        //Uri BaseAddress = new Uri("https://localhost:52105/api");

        private readonly HttpClient _httpClient;
        public ProductController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = BaseAddress;
        }

        [HttpGet]

        public IActionResult Index()
        {
            List<ProductViewModel> productList = new List<ProductViewModel>();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress +
                "/Product/GetProduct").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                productList = JsonConvert.DeserializeObject<List<ProductViewModel>>(data);

            }
            return View(productList);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(ProductViewModel Model)
        {
            try
            {
                string Data = JsonConvert.SerializeObject(Model);
                StringContent Content = new StringContent(Data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress +
                    "/Product/AddProduct", Content).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["Succes"] = "Product created";
                    return RedirectToAction("Index");
                }
                if(response.StatusCode == HttpStatusCode.Conflict)
                {
                    TempData["Error"] = "A product with the same name already exists.";

                }


            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;

                return View();

            }
            return View();
        }
        public IActionResult Edit(int  id)
        {
            try
            {
                ProductViewModel product = new ProductViewModel();
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress +
                     "/Product/SearchProduct/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    product = JsonConvert.DeserializeObject<ProductViewModel>(data);
                }
               
                return View(product);
            }
            catch (Exception ex)
            {
                TempData["Error"]= ex.Message;
                return View();
            }
        }
        [HttpPost]
        public IActionResult Edit(ProductViewModel model)
        {
            try
            {
                string data = JsonConvert.SerializeObject(model);
                StringContent Content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PutAsync(_httpClient.BaseAddress +
                     "/Product/UpdateProduct", Content).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["Succes"] = "Product edited";
                    return RedirectToAction("Index");
                }
                if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    TempData["Error"] = "A product with the same name already exists.";

                }
                return View();
            }
            catch (Exception ex)
            {

                TempData["Error"] = ex.Message;
                return View(); 
            }


        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            try
            {
                ProductViewModel product = new ProductViewModel();
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress +
                     "/Product/SearchProduct/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    product = JsonConvert.DeserializeObject<ProductViewModel>(data);
                }
                return View(product);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View();
            }
        }
       [HttpPost,ActionName("Delete")]
        public IActionResult DeletedConfimred( int id)
     {
            try
            {
                HttpResponseMessage response = _httpClient.DeleteAsync(_httpClient.BaseAddress +
                           "/Product/DeleteProduct/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["Succes"] = "Product Deleted";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View();

            }
            return View();
     }


        [HttpGet]
        public IActionResult details(int id)
        {
            try
            {
                ProductViewModel product = new ProductViewModel();
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress +
                     "/Product/SearchProduct/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    product = JsonConvert.DeserializeObject<ProductViewModel>(data);
                }
                return View(product);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View();
            }

        }
        [HttpPost]
        public IActionResult details(ProductViewModel model)
        {
            try
            {
                string data = JsonConvert.SerializeObject(model);
                return View();
            }
            catch (Exception ex)
            {

                TempData["Error"] = ex.Message;
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Search(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return RedirectToAction("Index");
            }

            List<ProductViewModel> productList = new List<ProductViewModel>();

            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress +
                    "/Product/SearchProductbyname/" + searchTerm);

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON array into a List<ProductViewModel>
                productList = JsonConvert.DeserializeObject<List<ProductViewModel>>(data);
            }

            return View(productList);
        }




    }
}


