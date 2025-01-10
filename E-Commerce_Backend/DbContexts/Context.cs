using E_Commerce_Backend.Entity;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Backend.DbContexts
{
    public class Context : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<AllProduct> AllProducts { get; set; }
        public DbSet<Order> Orders { get; set; }

        //public DbSet<Category> Categories { get; set; }
        //public DbSet<Product> Products { get; set; }

        public Context(DbContextOptions<Context> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CartItem>()
                .Property(ci => ci.Price)
                .HasPrecision(18, 3);

            modelBuilder.Entity<Order>()
                .Property(ci => ci.TotalAmount)
                .HasPrecision(18, 3);

            modelBuilder.Entity<AllProduct>()
                .Property(ci => ci.Price)
                .HasPrecision(18, 3);

            modelBuilder.Entity<CartItem>()
                .Property(ci => ci.Amount)
                .HasPrecision(18, 3);

            modelBuilder.Entity<AllProduct>()
                .OwnsOne(p => p.Rating, rating =>
                {
                    rating.Property(r => r.Rate).HasPrecision(18, 3);

                    rating.HasData(
                        new
                        {
                            AllProductId = 1,
                            Rate = 3.9m,
                            Count = 120
                        },
                        new
                        {
                            AllProductId = 2,
                            Rate = 4.1m,
                            Count = 259
                        },
                        new
                        {
                            AllProductId = 3,
                            Rate = 4.7m,
                            Count = 500
                        },
                        new
                        {
                            AllProductId = 4,
                            Rate = 2.1m,
                            Count = 430
                        },
                        new
                        {
                            AllProductId = 5,
                            Rate = 4.6m,
                            Count = 400
                        },
                        new
                        {
                            AllProductId = 6,
                            Rate = 3.9m,
                            Count = 70
                        },
                        new
                        {
                            AllProductId = 7,
                            Rate = 3m,
                            Count = 400
                        },
                        new
                        {
                            AllProductId = 8,
                            Rate = 1.9m,
                            Count = 100
                        },
                        new
                        {
                            AllProductId = 9,
                            Rate = 3.3m,
                            Count = 203
                        },
                        new
                        {
                            AllProductId = 10,
                            Rate = 2.9m,
                            Count = 470
                        },
                        new
                        {
                            AllProductId = 11,
                            Rate = 4.8m,
                            Count = 319
                        },
                        new
                        {
                            AllProductId = 12,
                            Rate = 4.8m,
                            Count = 400
                        },
                        new
                        {
                            AllProductId = 13,
                            Rate = 2.9m,
                            Count = 250
                        },
                        new
                        {
                            AllProductId = 14,
                            Rate = 2.2m,
                            Count = 140
                        },
                        new
                        {
                            AllProductId = 15,
                            Rate = 2.6m,
                            Count = 235
                        },
                        new
                        {
                            AllProductId = 16,
                            Rate = 2.9m,
                            Count = 340
                        },
                        new
                        {
                            AllProductId = 17,
                            Rate = 3.8m,
                            Count = 679
                        },
                        new
                        {
                            AllProductId = 18,
                            Rate = 4.7m,
                            Count = 130
                        },
                        new
                        {
                            AllProductId = 19,
                            Rate = 4.5m,
                            Count = 146
                        },
                        new
                        {
                            AllProductId = 20,
                            Rate = 3.6m,
                            Count = 145
                        }
                    );
                });

            // Cart and CartItem Relationship (One-to-Many)
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            // CartItem and AllProduct Relationship (Many-to-One)
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.AllProduct)
                .WithMany(p => p.CartItems)
                .HasForeignKey(ci => ci.AllProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Product and Category Relationship (Many-to-One)
            //modelBuilder.Entity<Product>()
            //    .HasOne(p => p.Category)
            //    .WithMany(c => c.Products)
            //    .HasForeignKey(p => p.CategoryId)
            //    .OnDelete(DeleteBehavior.Restrict);

            // User and Cart Relationship (One-to-One)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Cart)
                .WithOne(c => c.User)
                .HasForeignKey<Cart>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Order and Cart (One-to-One)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Cart)
                .WithOne(c => c.Order)
                .HasForeignKey<Order>(o => o.CartId)
                .OnDelete(DeleteBehavior.Restrict); 

            // Order and CartItems (One-to-Many)
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Order)
                .WithMany(o => o.CartItems)
                .HasForeignKey(ci => ci.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            // User and Order relationship (One-to-Many)
            modelBuilder.Entity<Order>()
                .HasOne(u => u.User)
                .WithMany(c => c.Orders)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Remove any unique constraint or index on UserId in the Cart table.
            modelBuilder.Entity<Cart>()
                .HasIndex(c => c.UserId)
                .IsUnique(false); // Allow multiple carts for the same user

            modelBuilder.Entity<AllProduct>()
                .Property(p => p.Id)
                .HasColumnName("id");

            modelBuilder.Entity<AllProduct>()
                .Property(p => p.Title)
                .HasColumnName("title");

            modelBuilder.Entity<AllProduct>()
                .Property(p => p.Price)
                .HasColumnName("price");

            modelBuilder.Entity<AllProduct>()
                .Property(p => p.Description)
                .HasColumnName("description");

            modelBuilder.Entity<AllProduct>()
                .Property(p => p.Category)
                .HasColumnName("category");

            modelBuilder.Entity<AllProduct>()
                .Property(p => p.Image)
                .HasColumnName("image");

            modelBuilder.Entity<AllProduct>()
                .HasData(
                new AllProduct()
                {
                    Id = 1,
                    Title = "Fjallraven - Foldsack No. 1 Backpack, Fits 15 Laptops",
                    Price = 109.95m,
                    Description = "Your perfect pack for everyday use and walks in the forest. Stash your laptop (up to 15 inches) in the padded sleeve, your everyday",
                    Category = "men's clothing",
                    Image = "https://fakestoreapi.com/img/81fPKd-2AYL._AC_SL1500_.jpg"
                },
                new AllProduct()
                {
                    Id = 2,
                    Title = "Mens Casual Premium Slim Fit T-Shirts",
                    Price = 22.3m,
                    Description = "Slim-fitting style, contrast raglan long sleeve, three-button henley placket, light weight & soft fabric for breathable and comfortable wearing. And Solid stitched shirts with round neck made for durability and a great fit for casual fashion wear and diehard baseball fans. The Henley style round neckline includes a three-button placket.",
                    Category = "men's clothing",
                    Image = "https://fakestoreapi.com/img/71-3HjGNDUL._AC_SY879._SX._UX._SY._UY_.jpg"
                },
                new AllProduct()
                {
                    Id = 3,
                    Title = "Mens Cotton Jacket",
                    Price = 55.99m,
                    Description = "great outerwear jackets for Spring/Autumn/Winter, suitable for many occasions, such as working, hiking, camping, mountain/rock climbing, cycling, traveling or other outdoors. Good gift choice for you or your family member. A warm hearted love to Father, husband or son in this thanksgiving or Christmas Day.",
                    Category = "men's clothing",
                    Image = "https://fakestoreapi.com/img/71li-ujtlUL._AC_UX679_.jpg"
                },
                new AllProduct()
                {
                    Id = 4,
                    Title = "Mens Casual Slim Fit",
                    Price = 15.99m,
                    Description = "The color could be slightly different between on the screen and in practice. / Please note that body builds vary by person, therefore, detailed size information should be reviewed below on the product description.",
                    Category = "men's clothing",
                    Image = "https://fakestoreapi.com/img/71YXzeOuslL._AC_UY879_.jpg"
                },
                new AllProduct()
                {
                    Id = 5,
                    Title = "John Hardy Women's Legends Naga Gold & Silver Dragon Station Chain Bracelet",
                    Price = 695m,
                    Description = "From our Legends Collection, the Naga was inspired by the mythical water dragon that protects the ocean's pearl. Wear facing inward to be bestowed with love and abundance, or outward for protection.",
                    Category = "jewelery",
                    Image = "https://fakestoreapi.com/img/71pWzhdJNwL._AC_UL640_QL65_ML3_.jpg"
                },
                new AllProduct()
                {
                    Id = 6,
                    Title = "Solid Gold Petite Micropave",
                    Price = 168m,
                    Description = "Satisfaction Guaranteed. Return or exchange any order within 30 days.Designed and sold by Hafeez Center in the United States. Satisfaction Guaranteed. Return or exchange any order within 30 days.",
                    Category = "jewelery",
                    Image = "https://fakestoreapi.com/img/61sbMiUnoGL._AC_UL640_QL65_ML3_.jpg"
                },
                new AllProduct()
                {
                    Id = 7,
                    Title = "White Gold Plated Princess",
                    Price = 9.99m,
                    Description = "Classic Created Wedding Engagement Solitaire Diamond Promise Ring for Her. Gifts to spoil your love more for Engagement, Wedding, Anniversary, Valentine's Day...",
                    Category = "jewelery",
                    Image = "https://fakestoreapi.com/img/71YAIFU48IL._AC_UL640_QL65_ML3_.jpg"
                },
                new AllProduct()
                {
                    Id = 8,
                    Title = "Pierced Owl Rose Gold Plated Stainless Steel Double",
                    Price = 10.99m,
                    Description = "Rose Gold Plated Double Flared Tunnel Plug Earrings. Made of 316L Stainless Steel",
                    Category = "jewelery",
                    Image = "https://fakestoreapi.com/img/51UDEzMJVpL._AC_UL640_QL65_ML3_.jpg"
                },
                new AllProduct()
                {
                    Id = 9,
                    Title = "WD 2TB Elements Portable External Hard Drive - USB 3.0",
                    Price = 64m,
                    Description = "USB 3.0 and USB 2.0 Compatibility Fast data transfers Improve PC Performance High Capacity; Compatibility Formatted NTFS for Windows 10, Windows 8.1, Windows 7; Reformatting may be required for other operating systems; Compatibility may vary depending on user’s hardware configuration and operating system",
                    Category = "electronics",
                    Image = "https://fakestoreapi.com/img/61IBBVJvSDL._AC_SY879_.jpg"
                },
                new AllProduct()
                {
                    Id = 10,
                    Title = "SanDisk SSD PLUS 1TB Internal SSD - SATA III 6 Gb/s",
                    Price = 109m,
                    Description = "Easy upgrade for faster boot up, shutdown, application load and response (As compared to 5400 RPM SATA 2.5” hard drive; Based on published specifications and internal benchmarking tests using PCMark vantage scores) Boosts burst write performance, making it ideal for typical PC workloads The perfect balance of performance and reliability Read/write speeds of up to 535MB/s/450MB/s (Based on internal testing; Performance may vary depending upon drive capacity, host device, OS and application.)",
                    Category = "electronics",
                    Image = "https://fakestoreapi.com/img/61U7T1koQqL._AC_SX679_.jpg"
                },
                new AllProduct()
                {
                    Id = 11,
                    Title = "Silicon Power 256GB SSD 3D NAND A55 SLC Cache Performance Boost SATA III 2.5",
                    Price = 109m,
                    Description = "3D NAND flash are applied to deliver high transfer speeds Remarkable transfer speeds that enable faster bootup and improved overall system performance. The advanced SLC Cache Technology allows performance boost and longer lifespan 7mm slim design suitable for Ultrabooks and Ultra-slim notebooks. Supports TRIM command, Garbage Collection technology, RAID, and ECC (Error Checking & Correction) to provide the optimized performance and enhanced reliability.",
                    Category = "electronics",
                    Image = "https://fakestoreapi.com/img/71kWymZ+c+L._AC_SX679_.jpg"
                },
                new AllProduct()
                {
                    Id = 12,
                    Title = "WD 4TB Gaming Drive Works with Playstation 4 Portable External Hard Drive",
                    Price = 114m,
                    Description = "Expand your PS4 gaming experience, Play anywhere Fast and easy, setup Sleek design with high capacity, 3-year manufacturer's limited warranty",
                    Category = "electronics",
                    Image = "https://fakestoreapi.com/img/61mtL65D4cL._AC_SX679_.jpg"
                },
                new AllProduct()
                {
                    Id = 13,
                    Title = "Acer SB220Q bi 21.5 inches Full HD (1920 x 1080) IPS Ultra-Thin",
                    Price = 599,
                    Description = "21. 5 inches Full HD (1920 x 1080) widescreen IPS display And Radeon free Sync technology. No compatibility for VESA Mount Refresh Rate: 75Hz - Using HDMI port Zero-frame design | ultra-thin | 4ms response time | IPS panel Aspect ratio - 16: 9. Color Supported - 16. 7 million colors. Brightness - 250 nit Tilt angle -5 degree to 15 degree. Horizontal viewing angle-178 degree. Vertical viewing angle-178 degree 75 hertz",
                    Category = "electronics",
                    Image = "https://fakestoreapi.com/img/81QpkIctqPL._AC_SX679_.jpg"
                },
                new AllProduct()
                {
                    Id = 14,
                    Title = "Samsung 49-Inch CHG90 144Hz Curved Gaming Monitor (LC49HG90DMNXZA) – Super Ultrawide Screen QLED",
                    Price = 999.99m,
                    Description = "49 INCH SUPER ULTRAWIDE 32:9 CURVED GAMING MONITOR with dual 27 inch screen side by side QUANTUM DOT (QLED) TECHNOLOGY, HDR support and factory calibration provides stunningly realistic and accurate color and contrast 144HZ HIGH REFRESH RATE and 1ms ultra fast response time work to eliminate motion blur, ghosting, and reduce input lag",
                    Category = "electronics",
                    Image = "https://fakestoreapi.com/img/81Zt42ioCgL._AC_SX679_.jpg"
                },
                new AllProduct()
                {
                    Id = 15,
                    Title = "BIYLACLESEN Women's 3-in-1 Snowboard Jacket Winter Coats",
                    Price = 56.99m,
                    Description = "Note:The Jackets is US standard size, Please choose size as your usual wear Material: 100% Polyester; Detachable Liner Fabric: Warm Fleece. Detachable Functional Liner: Skin Friendly, Lightweigt and Warm.Stand Collar Liner jacket, keep you warm in cold weather. Zippered Pockets: 2 Zippered Hand Pockets, 2 Zippered Pockets on Chest (enough to keep cards or keys)and 1 Hidden Pocket Inside.Zippered Hand Pockets and Hidden Pocket keep your things secure. Humanized Design: Adjustable and Detachable Hood and Adjustable cuff to prevent the wind and water,for a comfortable fit. 3 in 1 Detachable Design provide more convenience, you can separate the coat and inner as needed, or wear it together. It is suitable for different season and help you adapt to different climates",
                    Category = "women's clothing",
                    Image = "https://fakestoreapi.com/img/51Y5NI-I5jL._AC_UX679_.jpg"
                },
                new AllProduct()
                {
                    Id = 16,
                    Title = "Lock and Love Women's Removable Hooded Faux Leather Moto Biker Jacket",
                    Price = 29.95m,
                    Description = "100% POLYURETHANE(shell) 100% POLYESTER(lining) 75% POLYESTER 25% COTTON (SWEATER), Faux leather material for style and comfort / 2 pockets of front, 2-For-One Hooded denim style faux leather jacket, Button detail on waist / Detail stitching at sides, HAND WASH ONLY / DO NOT BLEACH / LINE DRY / DO NOT IRON",
                    Category = "women's clothing",
                    Image = "https://fakestoreapi.com/img/81XH0e8fefL._AC_UY879_.jpg"
                },
                new AllProduct()
                {
                    Id = 17,
                    Title = "Rain Jacket Women Windbreaker Striped Climbing Raincoats",
                    Price = 39.99m,
                    Description = "Lightweight perfet for trip or casual wear---Long sleeve with hooded, adjustable drawstring waist design. Button and zipper front closure raincoat, fully stripes Lined and The Raincoat has 2 side pockets are a good size to hold all kinds of things, it covers the hips, and the hood is generous but doesn't overdo it.Attached Cotton Lined Hood with Adjustable Drawstrings give it a real styled look.",
                    Category = "women's clothing",
                    Image = "https://fakestoreapi.com/img/71HblAHs5xL._AC_UY879_-2.jpg"
                },
                new AllProduct()
                {
                    Id = 18,
                    Title = "MBJ Women's Solid Short Sleeve Boat Neck V",
                    Price = 9.85m,
                    Description = "95% RAYON 5% SPANDEX, Made in USA or Imported, Do Not Bleach, Lightweight fabric with great stretch for comfort, Ribbed on sleeves and neckline / Double stitching on bottom hem",
                    Category = "women's clothing",
                    Image = "https://fakestoreapi.com/img/71z3kpMAYsL._AC_UY879_.jpg"
                },
                new AllProduct()
                {
                    Id = 19,
                    Title = "Opna Women's Short Sleeve Moisture",
                    Price = 7.95m,
                    Description = "100% Polyester, Machine wash, 100% cationic polyester interlock, Machine Wash & Pre Shrunk for a Great Fit, Lightweight, roomy and highly breathable with moisture wicking fabric which helps to keep moisture away, Soft Lightweight Fabric with comfortable V-neck collar and a slimmer fit, delivers a sleek, more feminine silhouette and Added Comfort",
                    Category = "women's clothing",
                    Image = "https://fakestoreapi.com/img/51eg55uWmdL._AC_UX679_.jpg"
                },
                new AllProduct()
                {
                    Id = 20,
                    Title = "DANVOUY Womens T Shirt Casual Cotton Short",
                    Price = 12.99m,
                    Description = "95%Cotton,5%Spandex, Features: Casual, Short Sleeve, Letter Print,V-Neck,Fashion Tees, The fabric is soft and has some stretch., Occasion: Casual/Office/Beach/School/Home/Street. Season: Spring,Summer,Autumn,Winter.",
                    Category = "women's clothing",
                    Image = "https://fakestoreapi.com/img/61pHAEJ4NML._AC_UX679_.jpg"
                });

            base.OnModelCreating(modelBuilder);
        }

    }
}

//new AllProduct()
//{
//    Id = ,
//    Title = ,
//    Price = ,
//    Description = ,
//    Category = ,
//    Image = ,
//    Rating = new Rating
//    {
//        Rate = ,
//        Count =
//    }
//}




