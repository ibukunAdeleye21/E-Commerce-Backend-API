IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241016083703_AddDataSeedandTables'
)
BEGIN
    CREATE TABLE [AllProducts] (
        [id] int NOT NULL IDENTITY,
        [title] nvarchar(200) NOT NULL,
        [price] decimal(18,3) NOT NULL,
        [description] nvarchar(max) NOT NULL,
        [category] nvarchar(max) NOT NULL,
        [image] nvarchar(max) NOT NULL,
        [Rating_Rate] decimal(18,3) NOT NULL,
        [Rating_Count] int NOT NULL,
        CONSTRAINT [PK_AllProducts] PRIMARY KEY ([id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241016083703_AddDataSeedandTables'
)
BEGIN
    CREATE TABLE [Users] (
        [Id] int NOT NULL IDENTITY,
        [Firstname] nvarchar(100) NOT NULL,
        [Lastname] nvarchar(100) NOT NULL,
        [Email] nvarchar(100) NOT NULL,
        [Phonenumber] nvarchar(100) NOT NULL,
        [Password] nvarchar(100) NOT NULL,
        [PasswordResetGuid] nvarchar(max) NOT NULL,
        [PasswordResetGuidCreate] datetime2 NULL,
        [PasswordResetGuidExpiry] datetime2 NULL,
        [IsUsed] bit NOT NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241016083703_AddDataSeedandTables'
)
BEGIN
    CREATE TABLE [Carts] (
        [Id] int NOT NULL IDENTITY,
        [UserId] int NOT NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_Carts] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Carts_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241016083703_AddDataSeedandTables'
)
BEGIN
    CREATE TABLE [Orders] (
        [OrderId] int NOT NULL IDENTITY,
        [UserId] int NOT NULL,
        [CartId] int NOT NULL,
        [TotalAmount] decimal(18,3) NOT NULL,
        [OrderDate] datetime2 NOT NULL,
        [Status] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Orders] PRIMARY KEY ([OrderId]),
        CONSTRAINT [FK_Orders_Carts_CartId] FOREIGN KEY ([CartId]) REFERENCES [Carts] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Orders_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241016083703_AddDataSeedandTables'
)
BEGIN
    CREATE TABLE [CartItems] (
        [Id] int NOT NULL IDENTITY,
        [CartId] int NOT NULL,
        [AllProductId] int NOT NULL,
        [OrderId] int NULL,
        [Price] decimal(18,3) NOT NULL,
        [Quantity] int NOT NULL,
        [Amount] decimal(18,3) NOT NULL,
        CONSTRAINT [PK_CartItems] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_CartItems_AllProducts_AllProductId] FOREIGN KEY ([AllProductId]) REFERENCES [AllProducts] ([id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_CartItems_Carts_CartId] FOREIGN KEY ([CartId]) REFERENCES [Carts] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_CartItems_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([OrderId]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241016083703_AddDataSeedandTables'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'id', N'category', N'description', N'image', N'price', N'title', N'Rating_Count', N'Rating_Rate') AND [object_id] = OBJECT_ID(N'[AllProducts]'))
        SET IDENTITY_INSERT [AllProducts] ON;
    EXEC(N'INSERT INTO [AllProducts] ([id], [category], [description], [image], [price], [title], [Rating_Count], [Rating_Rate])
    VALUES (1, N''men''''s clothing'', N''Your perfect pack for everyday use and walks in the forest. Stash your laptop (up to 15 inches) in the padded sleeve, your everyday'', N''https://fakestoreapi.com/img/81fPKd-2AYL._AC_SL1500_.jpg'', 109.95, N''Fjallraven - Foldsack No. 1 Backpack, Fits 15 Laptops'', 120, 3.9),
    (2, N''men''''s clothing'', N''Slim-fitting style, contrast raglan long sleeve, three-button henley placket, light weight & soft fabric for breathable and comfortable wearing. And Solid stitched shirts with round neck made for durability and a great fit for casual fashion wear and diehard baseball fans. The Henley style round neckline includes a three-button placket.'', N''https://fakestoreapi.com/img/71-3HjGNDUL._AC_SY879._SX._UX._SY._UY_.jpg'', 22.3, N''Mens Casual Premium Slim Fit T-Shirts'', 259, 4.1),
    (3, N''men''''s clothing'', N''great outerwear jackets for Spring/Autumn/Winter, suitable for many occasions, such as working, hiking, camping, mountain/rock climbing, cycling, traveling or other outdoors. Good gift choice for you or your family member. A warm hearted love to Father, husband or son in this thanksgiving or Christmas Day.'', N''https://fakestoreapi.com/img/71li-ujtlUL._AC_UX679_.jpg'', 55.99, N''Mens Cotton Jacket'', 500, 4.7),
    (4, N''men''''s clothing'', N''The color could be slightly different between on the screen and in practice. / Please note that body builds vary by person, therefore, detailed size information should be reviewed below on the product description.'', N''https://fakestoreapi.com/img/71YXzeOuslL._AC_UY879_.jpg'', 15.99, N''Mens Casual Slim Fit'', 430, 2.1),
    (5, N''jewelery'', N''From our Legends Collection, the Naga was inspired by the mythical water dragon that protects the ocean''''s pearl. Wear facing inward to be bestowed with love and abundance, or outward for protection.'', N''https://fakestoreapi.com/img/71pWzhdJNwL._AC_UL640_QL65_ML3_.jpg'', 695.0, N''John Hardy Women''''s Legends Naga Gold & Silver Dragon Station Chain Bracelet'', 400, 4.6),
    (6, N''jewelery'', N''Satisfaction Guaranteed. Return or exchange any order within 30 days.Designed and sold by Hafeez Center in the United States. Satisfaction Guaranteed. Return or exchange any order within 30 days.'', N''https://fakestoreapi.com/img/61sbMiUnoGL._AC_UL640_QL65_ML3_.jpg'', 168.0, N''Solid Gold Petite Micropave'', 70, 3.9),
    (7, N''jewelery'', N''Classic Created Wedding Engagement Solitaire Diamond Promise Ring for Her. Gifts to spoil your love more for Engagement, Wedding, Anniversary, Valentine''''s Day...'', N''https://fakestoreapi.com/img/71YAIFU48IL._AC_UL640_QL65_ML3_.jpg'', 9.99, N''White Gold Plated Princess'', 400, 3.0),
    (8, N''jewelery'', N''Rose Gold Plated Double Flared Tunnel Plug Earrings. Made of 316L Stainless Steel'', N''https://fakestoreapi.com/img/51UDEzMJVpL._AC_UL640_QL65_ML3_.jpg'', 10.99, N''Pierced Owl Rose Gold Plated Stainless Steel Double'', 100, 1.9),
    (9, N''electronics'', N''USB 3.0 and USB 2.0 Compatibility Fast data transfers Improve PC Performance High Capacity; Compatibility Formatted NTFS for Windows 10, Windows 8.1, Windows 7; Reformatting may be required for other operating systems; Compatibility may vary depending on user’s hardware configuration and operating system'', N''https://fakestoreapi.com/img/61IBBVJvSDL._AC_SY879_.jpg'', 64.0, N''WD 2TB Elements Portable External Hard Drive - USB 3.0'', 203, 3.3),
    (10, N''electronics'', N''Easy upgrade for faster boot up, shutdown, application load and response (As compared to 5400 RPM SATA 2.5” hard drive; Based on published specifications and internal benchmarking tests using PCMark vantage scores) Boosts burst write performance, making it ideal for typical PC workloads The perfect balance of performance and reliability Read/write speeds of up to 535MB/s/450MB/s (Based on internal testing; Performance may vary depending upon drive capacity, host device, OS and application.)'', N''https://fakestoreapi.com/img/61U7T1koQqL._AC_SX679_.jpg'', 109.0, N''SanDisk SSD PLUS 1TB Internal SSD - SATA III 6 Gb/s'', 470, 2.9),
    (11, N''electronics'', N''3D NAND flash are applied to deliver high transfer speeds Remarkable transfer speeds that enable faster bootup and improved overall system performance. The advanced SLC Cache Technology allows performance boost and longer lifespan 7mm slim design suitable for Ultrabooks and Ultra-slim notebooks. Supports TRIM command, Garbage Collection technology, RAID, and ECC (Error Checking & Correction) to provide the optimized performance and enhanced reliability.'', N''https://fakestoreapi.com/img/71kWymZ+c+L._AC_SX679_.jpg'', 109.0, N''Silicon Power 256GB SSD 3D NAND A55 SLC Cache Performance Boost SATA III 2.5'', 319, 4.8),
    (12, N''electronics'', N''Expand your PS4 gaming experience, Play anywhere Fast and easy, setup Sleek design with high capacity, 3-year manufacturer''''s limited warranty'', N''https://fakestoreapi.com/img/61mtL65D4cL._AC_SX679_.jpg'', 114.0, N''WD 4TB Gaming Drive Works with Playstation 4 Portable External Hard Drive'', 400, 4.8),
    (13, N''electronics'', N''21. 5 inches Full HD (1920 x 1080) widescreen IPS display And Radeon free Sync technology. No compatibility for VESA Mount Refresh Rate: 75Hz - Using HDMI port Zero-frame design | ultra-thin | 4ms response time | IPS panel Aspect ratio - 16: 9. Color Supported - 16. 7 million colors. Brightness - 250 nit Tilt angle -5 degree to 15 degree. Horizontal viewing angle-178 degree. Vertical viewing angle-178 degree 75 hertz'', N''https://fakestoreapi.com/img/81QpkIctqPL._AC_SX679_.jpg'', 599.0, N''Acer SB220Q bi 21.5 inches Full HD (1920 x 1080) IPS Ultra-Thin'', 250, 2.9),
    (14, N''electronics'', N''49 INCH SUPER ULTRAWIDE 32:9 CURVED GAMING MONITOR with dual 27 inch screen side by side QUANTUM DOT (QLED) TECHNOLOGY, HDR support and factory calibration provides stunningly realistic and accurate color and contrast 144HZ HIGH REFRESH RATE and 1ms ultra fast response time work to eliminate motion blur, ghosting, and reduce input lag'', N''https://fakestoreapi.com/img/81Zt42ioCgL._AC_SX679_.jpg'', 999.99, N''Samsung 49-Inch CHG90 144Hz Curved Gaming Monitor (LC49HG90DMNXZA) – Super Ultrawide Screen QLED'', 140, 2.2),
    (15, N''women''''s clothing'', N''Note:The Jackets is US standard size, Please choose size as your usual wear Material: 100% Polyester; Detachable Liner Fabric: Warm Fleece. Detachable Functional Liner: Skin Friendly, Lightweigt and Warm.Stand Collar Liner jacket, keep you warm in cold weather. Zippered Pockets: 2 Zippered Hand Pockets, 2 Zippered Pockets on Chest (enough to keep cards or keys)and 1 Hidden Pocket Inside.Zippered Hand Pockets and Hidden Pocket keep your things secure. Humanized Design: Adjustable and Detachable Hood and Adjustable cuff to prevent the wind and water,for a comfortable fit. 3 in 1 Detachable Design provide more convenience, you can separate the coat and inner as needed, or wear it together. It is suitable for different season and help you adapt to different climates'', N''https://fakestoreapi.com/img/51Y5NI-I5jL._AC_UX679_.jpg'', 56.99, N''BIYLACLESEN Women''''s 3-in-1 Snowboard Jacket Winter Coats'', 235, 2.6),
    (16, N''women''''s clothing'', N''100% POLYURETHANE(shell) 100% POLYESTER(lining) 75% POLYESTER 25% COTTON (SWEATER), Faux leather material for style and comfort / 2 pockets of front, 2-For-One Hooded denim style faux leather jacket, Button detail on waist / Detail stitching at sides, HAND WASH ONLY / DO NOT BLEACH / LINE DRY / DO NOT IRON'', N''https://fakestoreapi.com/img/81XH0e8fefL._AC_UY879_.jpg'', 29.95, N''Lock and Love Women''''s Removable Hooded Faux Leather Moto Biker Jacket'', 340, 2.9),
    (17, N''women''''s clothing'', N''Lightweight perfet for trip or casual wear---Long sleeve with hooded, adjustable drawstring waist design. Button and zipper front closure raincoat, fully stripes Lined and The Raincoat has 2 side pockets are a good size to hold all kinds of things, it covers the hips, and the hood is generous but doesn''''t overdo it.Attached Cotton Lined Hood with Adjustable Drawstrings give it a real styled look.'', N''https://fakestoreapi.com/img/71HblAHs5xL._AC_UY879_-2.jpg'', 39.99, N''Rain Jacket Women Windbreaker Striped Climbing Raincoats'', 679, 3.8),
    (18, N''women''''s clothing'', N''95% RAYON 5% SPANDEX, Made in USA or Imported, Do Not Bleach, Lightweight fabric with great stretch for comfort, Ribbed on sleeves and neckline / Double stitching on bottom hem'', N''https://fakestoreapi.com/img/71z3kpMAYsL._AC_UY879_.jpg'', 9.85, N''MBJ Women''''s Solid Short Sleeve Boat Neck V'', 130, 4.7),
    (19, N''women''''s clothing'', N''100% Polyester, Machine wash, 100% cationic polyester interlock, Machine Wash & Pre Shrunk for a Great Fit, Lightweight, roomy and highly breathable with moisture wicking fabric which helps to keep moisture away, Soft Lightweight Fabric with comfortable V-neck collar and a slimmer fit, delivers a sleek, more feminine silhouette and Added Comfort'', N''https://fakestoreapi.com/img/51eg55uWmdL._AC_UX679_.jpg'', 7.95, N''Opna Women''''s Short Sleeve Moisture'', 146, 4.5),
    (20, N''women''''s clothing'', N''95%Cotton,5%Spandex, Features: Casual, Short Sleeve, Letter Print,V-Neck,Fashion Tees, The fabric is soft and has some stretch., Occasion: Casual/Office/Beach/School/Home/Street. Season: Spring,Summer,Autumn,Winter.'', N''https://fakestoreapi.com/img/61pHAEJ4NML._AC_UX679_.jpg'', 12.99, N''DANVOUY Womens T Shirt Casual Cotton Short'', 145, 3.6)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'id', N'category', N'description', N'image', N'price', N'title', N'Rating_Count', N'Rating_Rate') AND [object_id] = OBJECT_ID(N'[AllProducts]'))
        SET IDENTITY_INSERT [AllProducts] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241016083703_AddDataSeedandTables'
)
BEGIN
    CREATE INDEX [IX_CartItems_AllProductId] ON [CartItems] ([AllProductId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241016083703_AddDataSeedandTables'
)
BEGIN
    CREATE INDEX [IX_CartItems_CartId] ON [CartItems] ([CartId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241016083703_AddDataSeedandTables'
)
BEGIN
    CREATE INDEX [IX_CartItems_OrderId] ON [CartItems] ([OrderId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241016083703_AddDataSeedandTables'
)
BEGIN
    CREATE INDEX [IX_Carts_UserId] ON [Carts] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241016083703_AddDataSeedandTables'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Orders_CartId] ON [Orders] ([CartId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241016083703_AddDataSeedandTables'
)
BEGIN
    CREATE INDEX [IX_Orders_UserId] ON [Orders] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241016083703_AddDataSeedandTables'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20241016083703_AddDataSeedandTables', N'8.0.8');
END;
GO

COMMIT;
GO

