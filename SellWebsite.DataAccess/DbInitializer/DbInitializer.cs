using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using SellWebsite.DataAccess.Data;
using SellWebsite.Models.Models;
using SellWebsite.Utility;

namespace SellWebsite.DataAccess.DbInitializer
{
    //Đây là một cách thức để tạo những database cần thiết ở đây thì ta tạo ra các role
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;
        public DbInitializer(ApplicationDbContext applicationDbContext, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _db = applicationDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        //Tạo dữ liệu trong database nếu dữ liệu chưa được tạo 
        public async void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Any())
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception)
            {

            }

            //Dòng if dưới đây kiểm tra xem trong database đã có role phù hợp hay chưa. Nếu chưa sẽ tự động tạo các role trong database khi mở trang register
            //Ở đây chọn 1 role mồi là customer nếu không có role customer thì tạo hết :v xử lý vậy cho lẹ ứng dụng nào mà chẳng có khách hàng
            if (!_roleManager.RoleExistsAsync(SD.Role_Employee).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Boss)).GetAwaiter().GetResult();

                //Tạo thêm một tài khoản Admin ban đầu để tránh việc không có ai để tạo người dùng
                _userManager.CreateAsync(new ApplicationUser()
                {
                    UserName = "dattranphu1114@gmail.com",
                    Email = "dattranphu1114@gmail.com",
                    Name = "Trần Phú Đạt",
                    PhoneNumber = "0985950723",
                    StreetAddress = "Tăng Bạc Hổ",
                    City = "Quy Nhơn",
                    State = "Bình Định",
                    Country = "Việt Nam",
                    Zipcode = "70500",
                    LockoutEnabled = false,
                }, @"Iloveuzienoi1114@").GetAwaiter().GetResult();

                ApplicationUser user = _db.ApplicationUsers.FirstOrDefault(p => p.Email == "dattranphu1114@gmail.com")!;

                _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();

                // Những dữ liệu khác
                var categories = new List<Category>()
            {
                    new Category()
                    {
                        Name = "Informational",

                        Description = "I'm too lazy to write it down, I can't edit it anymore",

                    },
                    new Category()
                    {
                        Name = "Comics",
                          Description = "I'm too lazy to write it down, I can't edit it anymore",

                    },
                    new Category()
                    {
                        Name = "Entertainment",
                          Description = "These are websites that provide entertainment content, such as movie streaming websites, gaming websites, or music streaming websites.",

                    },
                    new Category()
                    {
                        Name = "Social-media",
                         Description = "These are websites that allow users to interact and connect with each other, such as Facebook, Twitter, or Instagram.",

                    },
                    new Category()
                    {
                        Name = "Blog",
                        Description = "These are personal or business websites that provide the latest posts and information on a specific topic.",

                    },
                    new Category()
                    {
                        Name = "Manga",
                        Description = "I'm too lazy to write it down, I can't edit it anymore",

                    },
                    new Category()
                    {
                        Name = "Tool",

                        Description = "These are websites that provide online tools or services for users, such as Google, Dropbox, or GitHub.",

                    },
                    new Category()
                    {
                        Name = "Learning",
                        Description = "These are websites that provide online courses or study materials, such as Coursera, edX, or Udemy.",
                    },
                    new Category()
                    {
                        Name = "Food",
                        Description = "I'm too lazy to write it down, I can't edit it anymore",

                    },
                    new Category()
                    {
                        Name = "Exhibition",
                        Description = "Phòng trưng bày",

                    },
                    new Category()
                    {
                        Name = "Travel",
                        Description = "I'm too lazy to write it down, I can't edit it anymore",
                    },
                    new Category()
                    {
                        Name = "Sports",
                        Description = "I'm too lazy to write it down, I can't edit it anymore",

                    },
                    new Category()
                    {
                        Name = "Business",
                        Description = "A huge list of the best business website templates is built to serve any company, from construction to business consulting and financial services. All templates are mobile-friendly and feature both one-page and multi-page setups. Whether you are bringing a fresh project or redesigning your current website, these templates have you covered. They are powerful enough to meet any firm and organization owner’s needs and requirements.",
                                        }
            };
                _db.Categories.AddRangeAsync(categories).GetAwaiter().GetResult();
                _db.SaveChangesAsync().GetAwaiter().GetResult();

                var companys = new List<Company>()
                {
                    new Company(){
                        Name = "Thư viện Hồ Chí Minh",
                        PhoneNumber= "1234567890",
                        Email="TVHCM@gmail.com",
                        Address="Tp. Hồ Chí Minh"
                    },
                    new Company(){
                        Name = "Thư viện Hà Nội",
                        PhoneNumber= "0987654321",
                        Email="TVHN@gmail.com",
                        Address="Thủ đô Hà Nội"
                    }                    ,
                    new Company(){
                        Name = "Thư viện Quy Nhơn",
                        PhoneNumber= "5555555555",
                        Email="TVQN@gmail.com",
                        Address="Tp. Quy Nhơn"
                    }
                };
                _db.Companies.AddRangeAsync(companys).GetAwaiter().GetResult();
                _db.SaveChangesAsync().GetAwaiter().GetResult();

                var products = new List<Product>()
            {
                new Product()
                {
                    Title = "DarkAndLight",
                    Author = "TranPhuDat",
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                   Description = "Glint is a modern and stylish digital agency HTML template. Designed for creative designers, agencies, freelancers, photographers, or any creative profession.",

                    Image = "/assets/Images/products/DarkAndLight.png",
                    Categories= _db.Categories.Where(p=>p.Name == "Tool"|| p.Name =="Blog"||p.Name == "Exhibition"||p.Name=="Entertainment" ).ToList(),

                },
                new Product()
                {
                    Title = "Dotnet Universe",
                    Author = "TranPhuDat",
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    Description = "Glint is a modern and stylish digital agency HTML template. Designed for creative designers, agencies, freelancers, photographers, or any creative profession.",
                   Image = "/assets/Images/products/DotnetUniverse.png",
                    Categories= _db.Categories.Where(p=>p.Name == "Informational"|| p.Name =="Social-media"||p.Name == "Manga"||p.Name=="Learning"||p.Name=="Blog" ).ToList(),

                },
                 new Product()
                {
                    Title = "Food Shop",
                    Author = "TranPhuDat",
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                   Description = "Glint is a modern and stylish digital agency HTML template. Designed for creative designers, agencies, freelancers, photographers, or any creative profession.",
                     Image = "/assets/Images/products/FoodShop.png",
                    Categories= _db.Categories.Where(p=>p.Name == "Food"|| p.Name =="Business"||p.Name == "Exhibition" ).ToList(),

                },
                  new Product()
                {
                    Title = "Start Boostrap",
                    Author = "TranPhuDat",
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    Description = "Glint is a modern and stylish digital agency HTML template. Designed for creative designers, agencies, freelancers, photographers, or any creative profession.",
                   Image = "/assets/Images/products/StartBoostrap.png",
                    Categories= _db.Categories.Where(p=>p.Name == "Tool"|| p.Name =="Comics"||p.Name == "Business"||p.Name=="Exhibition" ).ToList(),

                },
                   new Product()
                {
                    Title = "Cyborg",
                    Author = "TranPhuDat",
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    Description = "Glint is a modern and stylish digital agency HTML template. Designed for creative designers, agencies, freelancers, photographers, or any creative profession.",
                  Image = "/assets/Images/products/Cyborg.png",
                    Categories = _db.Categories.Where(p=>p.Name == "Entertainment"|| p.Name =="Social-media"||p.Name == "Manga" ).ToList(),
                }
            };
                _db.Products.AddRangeAsync(products).GetAwaiter().GetResult();
                _db.SaveChangesAsync().GetAwaiter().GetResult();

            }
            return;
        }
    }
}
