using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using projetGestionProduit.Models;

namespace projetGestionProduit.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ProductController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        // GET: ProductController
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.Include(p => p.Category).ToListAsync();
            return View(products);
        }

        // GET: ProductController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: ProductController/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductDto productDTO)
        {
            if (productDTO.ImageFileName == null)
            {
                ModelState.AddModelError("ImageFile", "the image file is required");
                // Reinitialize Categories to avoid NullReferenceException
                ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", productDTO.CategoryId);


                return View(productDTO);
            }
            if (!ModelState.IsValid)
            {
                string fileExtension = Path.GetExtension(productDTO.ImageFileName.FileName);

                // Generate a unique file name for the image using GUID and the extracted file extension
                string newFileName = Guid.NewGuid().ToString() + fileExtension;

                // Save the image file
                string imageFullPath = Path.Combine(_environment.WebRootPath, "products", newFileName);
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    productDTO.ImageFileName.CopyTo(stream);
                }


                var productModel = new Product
                {
                    CategoryId = productDTO.CategoryId,
                    CreatedAt = DateTime.Now,
                    ImageFileName = newFileName,
                    Price = productDTO.Price,
                    ProductName = productDTO.ProductName,
                };

                //product.CategoryId = Convert.ToInt32(Request.Form["CategoryId"]);
                _context.Add(productModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", productDTO.CategoryId);
            return View(productDTO);
        }

        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            // Map your Product entity to ProductDto
            var productDto = new ProductDto
            {
                ProductName = product.ProductName,
                Price = product.Price,
                CategoryId = product.CategoryId,
            };

            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            ViewData["ProductId"] = product.ProductId;
            ViewData["ImageFileName"] = product.ImageFileName;
            return View(productDto);
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductDto productDto)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            if (productDto.ImageFileName == null)
            {
                ModelState.AddModelError("ImageFile", "the image file is required");
                ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
                ViewData["ProductId"] = product.ProductId;
                ViewData["ImageFileName"] = product.ImageFileName;


                return View(productDto);
            }

            if (!ModelState.IsValid)
            {
                ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
                ViewData["ProductId"] = product.ProductId;
                ViewData["ImageFileName"] = product.ImageFileName;
                try
                {
                    var existingProduct = await _context.Products.FindAsync(id);
                    if (existingProduct == null)
                    {
                        return NotFound();
                    }
                    string fileExtension = Path.GetExtension(productDto.ImageFileName.FileName);

                    // Generate a unique file name for the image using GUID and the extracted file extension
                    string newFileName = Guid.NewGuid().ToString() + fileExtension;

                    // Save the image file
                    string imageFullPath = Path.Combine(_environment.WebRootPath, "products", newFileName);
                    using (var stream = System.IO.File.Create(imageFullPath))
                    {
                        productDto.ImageFileName.CopyTo(stream);
                    }


                    // Update other properties of the product
                    existingProduct.ProductName = productDto.ProductName;
                    existingProduct.Price = productDto.Price;
                    existingProduct.CategoryId = productDto.CategoryId;
                    existingProduct.ImageFileName = newFileName;
                    _context.Update(existingProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

            }


            return RedirectToAction(nameof(Index));
        }


        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}
