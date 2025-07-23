using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace firebase_auth.Migrations
{
    /// <inheritdoc />
    public partial class RolePermissionSetUp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Permissions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ReleaseDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DurationMinutes = table.Column<int>(type: "integer", nullable: false),
                    Language = table.Column<string>(type: "text", nullable: true),
                    Country = table.Column<string>(type: "text", nullable: true),
                    Rating = table.Column<double>(type: "double precision", nullable: false),
                    PosterUrl = table.Column<string>(type: "text", nullable: true),
                    TrailerUrl = table.Column<string>(type: "text", nullable: true),
                    Cast = table.Column<List<string>>(type: "text[]", nullable: false),
                    Directors = table.Column<List<string>>(type: "text[]", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    State = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "Id", "Cast", "Country", "CreatedAt", "CreatedBy", "Description", "Directors", "DurationMinutes", "Language", "PosterUrl", "Rating", "ReleaseDate", "State", "Title", "TrailerUrl", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("01ab700c-88de-4efe-b2cf-bea2f852ff90"), new List<string> { "ea", "dolores", "et", "facilis", "aut" }, "Mayotte", new DateTime(2025, 4, 23, 6, 11, 56, 409, DateTimeKind.Utc).AddTicks(5418), "", "Eligendi a est.", new List<string> { "sunt", "accusamus", "ad" }, 147, "en", "https://picsum.photos/640/480/?image=1025", 3.3740443197631134, new DateTime(2021, 1, 28, 22, 44, 42, 634, DateTimeKind.Local).AddTicks(2542), 4, "Practical Soft Table", "https://francesco.name", new DateTime(2025, 4, 23, 6, 11, 56, 409, DateTimeKind.Utc).AddTicks(5423) },
                    { new Guid("0a174afe-1607-4bdc-8805-4a7a14dd197d"), new List<string> { "occaecati", "qui", "quis", "ut", "iste", "veniam" }, "Ukraine", new DateTime(2025, 4, 23, 6, 11, 56, 419, DateTimeKind.Utc).AddTicks(2182), "", "Deleniti possimus iusto fugiat.", new List<string> { "ab", "at", "fuga" }, 82, "en", "https://picsum.photos/640/480/?image=307", 7.200460479478398, new DateTime(2024, 7, 31, 23, 28, 29, 903, DateTimeKind.Local).AddTicks(1760), 4, "Generic Steel Chips", "https://titus.name", new DateTime(2025, 4, 23, 6, 11, 56, 419, DateTimeKind.Utc).AddTicks(2182) },
                    { new Guid("0bb1f535-07df-4250-b2f2-ada70c7e49ac"), new List<string> { "in", "repellendus", "quos", "voluptas", "quis" }, "Hong Kong", new DateTime(2025, 4, 23, 6, 11, 56, 418, DateTimeKind.Utc).AddTicks(9939), "", "Autem quis dolorum non occaecati.", new List<string> { "voluptatibus", "sequi", "libero" }, 144, "en", "https://picsum.photos/640/480/?image=1047", 7.5027849352410589, new DateTime(2024, 12, 15, 6, 49, 26, 442, DateTimeKind.Local).AddTicks(8331), 4, "Handmade Granite Shoes", "http://khalid.info", new DateTime(2025, 4, 23, 6, 11, 56, 418, DateTimeKind.Utc).AddTicks(9940) },
                    { new Guid("0efda013-d623-4e26-808e-ce5d801d0c21"), new List<string> { "enim", "aliquam", "quia" }, "Australia", new DateTime(2025, 4, 23, 6, 11, 56, 418, DateTimeKind.Utc).AddTicks(7929), "", "Autem aperiam temporibus officiis amet laborum.", new List<string> { "eaque" }, 133, "en", "https://picsum.photos/640/480/?image=956", 5.6608665310704662, new DateTime(2022, 9, 24, 16, 47, 51, 518, DateTimeKind.Local).AddTicks(8438), 4, "Ergonomic Steel Gloves", "http://theresa.info", new DateTime(2025, 4, 23, 6, 11, 56, 418, DateTimeKind.Utc).AddTicks(7929) },
                    { new Guid("107a6a0c-ee3d-43c9-890d-781d057775cc"), new List<string> { "cupiditate", "rem", "magni", "optio" }, "Lebanon", new DateTime(2025, 4, 23, 6, 11, 56, 418, DateTimeKind.Utc).AddTicks(9513), "", "Distinctio earum est modi similique voluptas earum.", new List<string> { "debitis", "reiciendis" }, 168, "en", "https://picsum.photos/640/480/?image=621", 1.9876937043962823, new DateTime(2024, 7, 12, 9, 31, 57, 959, DateTimeKind.Local).AddTicks(4271), 4, "Handmade Wooden Chips", "https://richie.biz", new DateTime(2025, 4, 23, 6, 11, 56, 418, DateTimeKind.Utc).AddTicks(9514) },
                    { new Guid("20c265a0-294b-4558-a400-7250855fc1c3"), new List<string> { "nesciunt", "commodi", "sint" }, "Myanmar", new DateTime(2025, 4, 23, 6, 11, 56, 419, DateTimeKind.Utc).AddTicks(55), "", "Assumenda aspernatur ex vitae non in quo dolor eum harum.", new List<string> { "assumenda", "sit" }, 178, "en", "https://picsum.photos/640/480/?image=725", 1.8561467869351831, new DateTime(2020, 10, 31, 10, 7, 5, 810, DateTimeKind.Local), 4, "Generic Concrete Pizza", "https://wendell.info", new DateTime(2025, 4, 23, 6, 11, 56, 419, DateTimeKind.Utc).AddTicks(56) },
                    { new Guid("28b0a778-962f-4cc4-8d3d-953cb9a2020d"), new List<string> { "exercitationem", "beatae", "error", "omnis" }, "Senegal", new DateTime(2025, 4, 23, 6, 11, 56, 418, DateTimeKind.Utc).AddTicks(9598), "", "Fugit fugit blanditiis qui provident eius maxime sint.", new List<string> { "ut" }, 107, "en", "https://picsum.photos/640/480/?image=165", 7.8105480403785332, new DateTime(2018, 8, 26, 17, 44, 49, 593, DateTimeKind.Local).AddTicks(2043), 4, "Awesome Wooden Ball", "https://axel.biz", new DateTime(2025, 4, 23, 6, 11, 56, 418, DateTimeKind.Utc).AddTicks(9598) },
                    { new Guid("3425be72-4d5b-4274-83ea-adbae7a7e6f3"), new List<string> { "tempore", "sunt", "enim" }, "Anguilla", new DateTime(2025, 4, 23, 6, 11, 56, 419, DateTimeKind.Utc).AddTicks(1746), "", "Et ea eaque sint.", new List<string> { "et", "debitis", "est" }, 106, "en", "https://picsum.photos/640/480/?image=379", 4.1243773592159769, new DateTime(2023, 12, 24, 16, 36, 36, 903, DateTimeKind.Local).AddTicks(6639), 4, "Incredible Cotton Sausages", "http://polly.net", new DateTime(2025, 4, 23, 6, 11, 56, 419, DateTimeKind.Utc).AddTicks(1746) },
                    { new Guid("35d5b1e8-27af-4425-9697-86612826d3b4"), new List<string> { "distinctio", "debitis", "nemo", "corporis", "fugiat", "veritatis", "saepe" }, "Rwanda", new DateTime(2025, 4, 23, 6, 11, 56, 418, DateTimeKind.Utc).AddTicks(8010), "", "Et at eos natus natus nisi nisi at consequatur.", new List<string> { "dolorum", "laboriosam", "est" }, 148, "en", "https://picsum.photos/640/480/?image=632", 8.6695401050173047, new DateTime(2019, 10, 26, 1, 25, 15, 806, DateTimeKind.Local).AddTicks(5896), 4, "Awesome Concrete Fish", "https://dallas.com", new DateTime(2025, 4, 23, 6, 11, 56, 418, DateTimeKind.Utc).AddTicks(8010) },
                    { new Guid("41e263f5-0464-4afd-adaf-58314356477e"), new List<string> { "natus", "cumque", "voluptatibus", "velit", "voluptatem", "hic" }, "Spain", new DateTime(2025, 4, 23, 6, 11, 56, 419, DateTimeKind.Utc).AddTicks(1824), "", "Qui beatae labore veniam itaque blanditiis officiis.", new List<string> { "deserunt", "doloremque" }, 144, "en", "https://picsum.photos/640/480/?image=251", 3.7378945237483796, new DateTime(2018, 4, 11, 5, 51, 11, 858, DateTimeKind.Local).AddTicks(2828), 4, "Unbranded Cotton Salad", "https://elody.biz", new DateTime(2025, 4, 23, 6, 11, 56, 419, DateTimeKind.Utc).AddTicks(1824) },
                    { new Guid("4340b7c2-77c2-4740-aa7d-4dc53050c9fc"), new List<string> { "rem", "doloremque", "sit", "non", "quis", "eum" }, "Faroe Islands", new DateTime(2025, 4, 23, 6, 11, 56, 418, DateTimeKind.Utc).AddTicks(8234), "", "Debitis facilis porro dolor totam illum non sed incidunt.", new List<string> { "quidem", "vitae" }, 97, "en", "https://picsum.photos/640/480/?image=409", 1.9902396764268702, new DateTime(2022, 9, 30, 5, 3, 41, 952, DateTimeKind.Local).AddTicks(1020), 4, "Intelligent Frozen Hat", "https://johathan.com", new DateTime(2025, 4, 23, 6, 11, 56, 418, DateTimeKind.Utc).AddTicks(8235) },
                    { new Guid("45797d12-58ba-4099-88fa-e5008cbde9a1"), new List<string> { "blanditiis", "provident", "possimus" }, "Iraq", new DateTime(2025, 4, 23, 6, 11, 56, 418, DateTimeKind.Utc).AddTicks(8152), "", "Recusandae et laboriosam totam deserunt.", new List<string> { "eum" }, 155, "en", "https://picsum.photos/640/480/?image=232", 7.7891384573344755, new DateTime(2021, 11, 29, 15, 5, 7, 733, DateTimeKind.Local).AddTicks(2925), 4, "Tasty Wooden Gloves", "https://oral.org", new DateTime(2025, 4, 23, 6, 11, 56, 418, DateTimeKind.Utc).AddTicks(8153) },
                    { new Guid("467ab48a-9936-47ad-8c4f-dd41d190036d"), new List<string> { "ad", "sunt", "qui", "odit", "deleniti" }, "Jamaica", new DateTime(2025, 4, 23, 6, 11, 56, 418, DateTimeKind.Utc).AddTicks(8472), "", "Odio saepe ut dolores dolor.", new List<string> { "et" }, 98, "en", "https://picsum.photos/640/480/?image=376", 1.0278399883468889, new DateTime(2015, 9, 2, 18, 13, 6, 948, DateTimeKind.Local).AddTicks(5713), 4, "Handcrafted Cotton Shirt", "http://dorcas.info", new DateTime(2025, 4, 23, 6, 11, 56, 418, DateTimeKind.Utc).AddTicks(8473) },
                    { new Guid("4d19eb6a-0171-4f3c-896c-d2683eaf383f"), new List<string> { "hic", "praesentium", "ad", "quisquam", "magni", "sequi" }, "Mozambique", new DateTime(2025, 4, 23, 6, 11, 56, 418, DateTimeKind.Utc).AddTicks(9852), "", "Et voluptas vel nihil est commodi dolorum.", new List<string> { "consequatur" }, 76, "en", "https://picsum.photos/640/480/?image=945", 9.9498424073999097, new DateTime(2020, 11, 12, 4, 31, 40, 599, DateTimeKind.Local).AddTicks(6208), 4, "Refined Concrete Mouse", "http://nolan.org", new DateTime(2025, 4, 23, 6, 11, 56, 418, DateTimeKind.Utc).AddTicks(9852) },
                    { new Guid("4e731ab8-647e-4481-9cc9-2c7d035099ee"), new List<string> { "autem", "rerum", "fugit" }, "French Polynesia", new DateTime(2025, 4, 23, 6, 11, 56, 419, DateTimeKind.Utc).AddTicks(828), "", "Non ullam et.", new List<string> { "distinctio", "nisi", "minus" }, 140, "en", "https://picsum.photos/640/480/?image=1056", 5.1058691742420512, new DateTime(2016, 9, 7, 2, 43, 18, 932, DateTimeKind.Local).AddTicks(8313), 4, "Fantastic Concrete Car", "http://barry.biz", new DateTime(2025, 4, 23, 6, 11, 56, 419, DateTimeKind.Utc).AddTicks(828) },
                    { new Guid("556632e9-ee9a-4453-b590-1d191d5cdb89"), new List<string> { "ipsa", "amet", "maiores", "minus", "hic" }, "Djibouti", new DateTime(2025, 4, 23, 6, 11, 56, 419, DateTimeKind.Utc).AddTicks(357), "", "Est id natus et ipsa soluta.", new List<string> { "vel" }, 95, "en", "https://picsum.photos/640/480/?image=992", 9.5534374516425835, new DateTime(2015, 8, 12, 22, 14, 30, 95, DateTimeKind.Local).AddTicks(9873), 4, "Gorgeous Wooden Bacon", "http://adelia.org", new DateTime(2025, 4, 23, 6, 11, 56, 419, DateTimeKind.Utc).AddTicks(358) },
                    { new Guid("5a9da924-f75b-4f83-af5e-b29f1223ae18"), new List<string> { "dolores", "veniam", "est", "aliquam" }, "Lao People's Democratic Republic", new DateTime(2025, 4, 23, 6, 11, 56, 418, DateTimeKind.Utc).AddTicks(8769), "", "Aut ipsum qui.", new List<string> { "porro" }, 138, "en", "https://picsum.photos/640/480/?image=403", 4.9846128550106048, new DateTime(2015, 9, 15, 2, 19, 25, 685, DateTimeKind.Local).AddTicks(5856), 4, "Small Plastic Sausages", "https://austin.org", new DateTime(2025, 4, 23, 6, 11, 56, 418, DateTimeKind.Utc).AddTicks(8770) },
                    { new Guid("5d786f1d-d5f5-4166-bafc-6a5729e3f45e"), new List<string> { "labore", "esse", "dolor" }, "Jordan", new DateTime(2025, 4, 23, 6, 11, 56, 419, DateTimeKind.Utc).AddTicks(1509), "", "Delectus cupiditate quis sit fugiat sequi sunt.", new List<string> { "ipsa", "voluptates", "qui" }, 156, "en", "https://picsum.photos/640/480/?image=515", 4.8910199979032543, new DateTime(2019, 5, 20, 23, 9, 32, 548, DateTimeKind.Local).AddTicks(6310), 4, "Generic Soft Table", "https://alessandra.info", new DateTime(2025, 4, 23, 6, 11, 56, 419, DateTimeKind.Utc).AddTicks(1509) },
                    { new Guid("69849795-5c8c-41d2-a8ef-74da71645fc2"), new List<string> { "est", "beatae", "qui", "incidunt", "ea", "omnis", "sit" }, "Niue", new DateTime(2025, 4, 23, 6, 11, 56, 418, DateTimeKind.Utc).AddTicks(9174), "", "Adipisci aliquid cumque est et.", new List<string> { "quod", "harum", "eum" }, 96, "en", "https://picsum.photos/640/480/?image=662", 1.7446939675713788, new DateTime(2023, 10, 6, 2, 18, 12, 410, DateTimeKind.Local).AddTicks(4264), 4, "Handmade Granite Fish", "https://elna.name", new DateTime(2025, 4, 23, 6, 11, 56, 418, DateTimeKind.Utc).AddTicks(9174) },
                    { new Guid("6acc018d-d453-4339-b2cd-87a71db7313e"), new List<string> { "qui", "cupiditate", "aut" }, "Slovakia (Slovak Republic)", new DateTime(2025, 4, 23, 6, 11, 56, 418, DateTimeKind.Utc).AddTicks(7341), "", "Voluptatibus quia officiis sint porro itaque est.", new List<string> { "esse", "libero", "repellendus" }, 103, "en", "https://picsum.photos/640/480/?image=860", 3.5180668486254612, new DateTime(2015, 9, 18, 5, 39, 32, 237, DateTimeKind.Local).AddTicks(5732), 4, "Intelligent Granite Sausages", "http://lea.name", new DateTime(2025, 4, 23, 6, 11, 56, 418, DateTimeKind.Utc).AddTicks(7342) },
                    { new Guid("727e14bc-547e-4f68-aab4-b3218627bc1d"), new List<string> { "voluptatum", "ducimus", "iure", "laboriosam", "aliquid" }, "Ecuador", new DateTime(2025, 4, 23, 6, 11, 56, 419, DateTimeKind.Utc).AddTicks(1233), "", "Cumque quibusdam assumenda illo.", new List<string> { "est", "tempore" }, 132, "en", "https://picsum.photos/640/480/?image=554", 1.0947815715586238, new DateTime(2024, 1, 26, 8, 26, 4, 479, DateTimeKind.Local).AddTicks(3650), 4, "Ergonomic Soft Pizza", "https://hyman.name", new DateTime(2025, 4, 23, 6, 11, 56, 419, DateTimeKind.Utc).AddTicks(1233) },
                    { new Guid("78364df5-1784-460d-8ca3-6ba27dd892cc"), new List<string> { "optio", "blanditiis", "distinctio" }, "Switzerland", new DateTime(2025, 4, 23, 6, 11, 56, 419, DateTimeKind.Utc).AddTicks(145), "", "Voluptatibus odit veritatis voluptatem quia optio et possimus.", new List<string> { "dolorem" }, 158, "en", "https://picsum.photos/640/480/?image=830", 1.6764278201476355, new DateTime(2024, 8, 26, 12, 26, 22, 938, DateTimeKind.Local).AddTicks(2667), 4, "Unbranded Metal Gloves", "https://jakob.com", new DateTime(2025, 4, 23, 6, 11, 56, 419, DateTimeKind.Utc).AddTicks(145) },
                    { new Guid("8d3653f3-71b6-48f8-b1a0-2aeeaf74a5ef"), new List<string> { "cumque", "suscipit", "repudiandae", "eius" }, "Eritrea", new DateTime(2025, 4, 23, 6, 11, 56, 418, DateTimeKind.Utc).AddTicks(9755), "", "Libero ut ut aliquam esse deleniti similique sapiente est debitis.", new List<string> { "blanditiis", "dignissimos", "ratione" }, 104, "en", "https://picsum.photos/640/480/?image=734", 9.981618813595647, new DateTime(2024, 5, 2, 8, 19, 34, 903, DateTimeKind.Local).AddTicks(2294), 4, "Practical Steel Cheese", "http://vinnie.com", new DateTime(2025, 4, 23, 6, 11, 56, 418, DateTimeKind.Utc).AddTicks(9755) },
                    { new Guid("9233c6b9-e60e-4d04-b269-a5981cf91e94"), new List<string> { "dolorem", "quos", "ipsam" }, "Tanzania", new DateTime(2025, 4, 23, 6, 11, 56, 419, DateTimeKind.Utc).AddTicks(2032), "", "Totam nostrum veniam.", new List<string> { "et", "porro" }, 120, "en", "https://picsum.photos/640/480/?image=605", 4.3519597856093215, new DateTime(2020, 7, 10, 8, 57, 0, 821, DateTimeKind.Local).AddTicks(282), 4, "Gorgeous Frozen Bacon", "https://xzavier.org", new DateTime(2025, 4, 23, 6, 11, 56, 419, DateTimeKind.Utc).AddTicks(2032) },
                    { new Guid("93776869-e749-4976-8efc-1d774f4bc2ce"), new List<string> { "adipisci", "nihil", "expedita", "voluptas", "et", "facilis", "autem" }, "Aruba", new DateTime(2025, 4, 23, 6, 11, 56, 419, DateTimeKind.Utc).AddTicks(903), "", "Ut rerum nesciunt dolorum laboriosam ipsa dolorem odit facere omnis.", new List<string> { "ipsa" }, 177, "en", "https://picsum.photos/640/480/?image=983", 7.3607124705775311, new DateTime(2024, 12, 9, 5, 46, 53, 156, DateTimeKind.Local).AddTicks(3101), 4, "Awesome Wooden Towels", "https://cory.net", new DateTime(2025, 4, 23, 6, 11, 56, 419, DateTimeKind.Utc).AddTicks(903) },
                    { new Guid("9e18c30e-5150-4041-a7c9-a4225365f497"), new List<string> { "eligendi", "aliquam", "mollitia", "et", "et", "aliquam" }, "United States Minor Outlying Islands", new DateTime(2025, 4, 23, 6, 11, 56, 418, DateTimeKind.Utc).AddTicks(7642), "", "Ut nihil quibusdam tempore sed totam delectus odio.", new List<string> { "odit", "omnis" }, 61, "en", "https://picsum.photos/640/480/?image=588", 6.5386440625600457, new DateTime(2016, 7, 15, 9, 57, 22, 314, DateTimeKind.Local).AddTicks(9669), 4, "Incredible Soft Chair", "https://leonora.biz", new DateTime(2025, 4, 23, 6, 11, 56, 418, DateTimeKind.Utc).AddTicks(7643) },
                    { new Guid("ab98e21f-b122-4ef3-b785-08e8feda65ad"), new List<string> { "commodi", "laudantium", "ea", "rerum", "dolores" }, "Belize", new DateTime(2025, 4, 23, 6, 11, 56, 418, DateTimeKind.Utc).AddTicks(6041), "", "Debitis incidunt autem quam quia culpa.", new List<string> { "rerum", "enim" }, 114, "en", "https://picsum.photos/640/480/?image=24", 2.0577940681032088, new DateTime(2018, 8, 28, 9, 37, 25, 59, DateTimeKind.Local).AddTicks(5801), 4, "Intelligent Fresh Bike", "https://alberto.name", new DateTime(2025, 4, 23, 6, 11, 56, 418, DateTimeKind.Utc).AddTicks(6044) },
                    { new Guid("adeac3a9-a870-4d80-8eeb-37c12356c7d3"), new List<string> { "tempora", "cupiditate", "laborum", "temporibus", "eligendi" }, "Antigua and Barbuda", new DateTime(2025, 4, 23, 6, 11, 56, 419, DateTimeKind.Utc).AddTicks(1403), "", "Maxime omnis quaerat quia quis doloribus voluptatem.", new List<string> { "maiores" }, 164, "en", "https://picsum.photos/640/480/?image=93", 9.7157165886376973, new DateTime(2023, 4, 22, 18, 0, 21, 412, DateTimeKind.Local).AddTicks(2784), 4, "Licensed Granite Mouse", "http://idella.org", new DateTime(2025, 4, 23, 6, 11, 56, 419, DateTimeKind.Utc).AddTicks(1403) },
                    { new Guid("afd44a2e-e7c2-4610-b174-7d2a8ffa9c69"), new List<string> { "sed", "velit", "atque", "molestiae" }, "Bermuda", new DateTime(2025, 4, 23, 6, 11, 56, 418, DateTimeKind.Utc).AddTicks(9266), "", "Et sequi quis eos cum quo.", new List<string> { "deleniti" }, 105, "en", "https://picsum.photos/640/480/?image=394", 4.8016912979642967, new DateTime(2018, 6, 10, 3, 53, 49, 682, DateTimeKind.Local).AddTicks(8481), 4, "Rustic Cotton Cheese", "http://henderson.biz", new DateTime(2025, 4, 23, 6, 11, 56, 418, DateTimeKind.Utc).AddTicks(9266) },
                    { new Guid("b02dd64b-2ecd-40f3-93d4-20b0bc7605c5"), new List<string> { "placeat", "dolores", "aut", "laudantium", "ullam", "et", "consequatur" }, "Democratic People's Republic of Korea", new DateTime(2025, 4, 23, 6, 11, 56, 419, DateTimeKind.Utc).AddTicks(1591), "", "Voluptatem voluptas deserunt dolorum repudiandae doloribus.", new List<string> { "quae", "natus", "voluptatibus" }, 169, "en", "https://picsum.photos/640/480/?image=502", 1.1383336447629535, new DateTime(2023, 11, 20, 3, 36, 53, 490, DateTimeKind.Local).AddTicks(9347), 4, "Refined Steel Keyboard", "https://roscoe.info", new DateTime(2025, 4, 23, 6, 11, 56, 419, DateTimeKind.Utc).AddTicks(1592) },
                    { new Guid("c007e441-2803-49a8-8ccc-e452fad1525f"), new List<string> { "quisquam", "voluptates", "aliquam", "sequi", "eos", "quo" }, "Mali", new DateTime(2025, 4, 23, 6, 11, 56, 419, DateTimeKind.Utc).AddTicks(1910), "", "Quis nihil fuga facilis labore.", new List<string> { "ipsum", "laborum" }, 117, "en", "https://picsum.photos/640/480/?image=803", 8.5576942044900086, new DateTime(2020, 8, 25, 7, 18, 17, 285, DateTimeKind.Local).AddTicks(6540), 4, "Intelligent Metal Chicken", "https://beau.name", new DateTime(2025, 4, 23, 6, 11, 56, 419, DateTimeKind.Utc).AddTicks(1910) },
                    { new Guid("c045b5a7-b5c3-4685-92c5-cb2cd7e9ac8f"), new List<string> { "perferendis", "quisquam", "quam", "architecto" }, "El Salvador", new DateTime(2025, 4, 23, 6, 11, 56, 419, DateTimeKind.Utc).AddTicks(260), "", "Consectetur molestiae pariatur asperiores fugit eos et debitis quia.", new List<string> { "assumenda", "molestiae", "ipsum" }, 122, "en", "https://picsum.photos/640/480/?image=844", 8.2107913516187025, new DateTime(2020, 10, 9, 8, 3, 39, 892, DateTimeKind.Local).AddTicks(340), 4, "Refined Soft Towels", "https://hailee.biz", new DateTime(2025, 4, 23, 6, 11, 56, 419, DateTimeKind.Utc).AddTicks(260) },
                    { new Guid("c352b7c2-f9d4-47d3-a9c8-ec615848d926"), new List<string> { "ut", "sequi", "soluta", "sit", "omnis", "repellendus" }, "Cape Verde", new DateTime(2025, 4, 23, 6, 11, 56, 419, DateTimeKind.Utc).AddTicks(726), "", "Explicabo sed nihil velit fugit et velit doloremque veritatis reiciendis.", new List<string> { "corrupti", "accusamus" }, 115, "en", "https://picsum.photos/640/480/?image=532", 8.6627206003112427, new DateTime(2019, 10, 14, 14, 4, 43, 209, DateTimeKind.Local).AddTicks(5162), 4, "Tasty Wooden Sausages", "https://maritza.com", new DateTime(2025, 4, 23, 6, 11, 56, 419, DateTimeKind.Utc).AddTicks(726) },
                    { new Guid("c4e370b6-c78d-4b4b-9a53-f056d9763e8c"), new List<string> { "qui", "sequi", "non", "deleniti" }, "Ecuador", new DateTime(2025, 4, 23, 6, 11, 56, 418, DateTimeKind.Utc).AddTicks(8681), "", "Consequatur corporis aliquid atque dignissimos possimus rerum.", new List<string> { "voluptate", "veniam" }, 128, "en", "https://picsum.photos/640/480/?image=344", 3.0298223636041319, new DateTime(2017, 12, 1, 13, 31, 54, 516, DateTimeKind.Local).AddTicks(8782), 4, "Generic Granite Keyboard", "http://tiara.com", new DateTime(2025, 4, 23, 6, 11, 56, 418, DateTimeKind.Utc).AddTicks(8681) },
                    { new Guid("c8cb224d-aba0-4d45-950b-d20a24efe2c6"), new List<string> { "sed", "qui", "perspiciatis", "eos", "cum", "iure" }, "Canada", new DateTime(2025, 4, 23, 6, 11, 56, 419, DateTimeKind.Utc).AddTicks(1311), "", "Modi laborum modi ex nihil officiis sit id nam.", new List<string> { "non" }, 76, "en", "https://picsum.photos/640/480/?image=790", 3.030982345732653, new DateTime(2022, 8, 17, 20, 29, 44, 149, DateTimeKind.Local).AddTicks(4997), 4, "Generic Granite Car", "https://cara.org", new DateTime(2025, 4, 23, 6, 11, 56, 419, DateTimeKind.Utc).AddTicks(1311) },
                    { new Guid("cc0aac27-7270-4cb4-a34d-9e9ac1aa668c"), new List<string> { "dolor", "sunt", "omnis", "repellat", "nemo", "sed", "distinctio" }, "Equatorial Guinea", new DateTime(2025, 4, 23, 6, 11, 56, 418, DateTimeKind.Utc).AddTicks(9060), "", "Illum et perspiciatis neque et.", new List<string> { "sequi" }, 60, "en", "https://picsum.photos/640/480/?image=221", 7.5920919447792343, new DateTime(2021, 4, 25, 2, 34, 17, 652, DateTimeKind.Local).AddTicks(280), 4, "Licensed Granite Towels", "https://romaine.name", new DateTime(2025, 4, 23, 6, 11, 56, 418, DateTimeKind.Utc).AddTicks(9061) },
                    { new Guid("cd752882-f49c-430a-b9c8-f16bd05c2b56"), new List<string> { "id", "dolorum", "labore", "earum", "quo", "excepturi", "consequatur" }, "France", new DateTime(2025, 4, 23, 6, 11, 56, 419, DateTimeKind.Utc).AddTicks(1110), "", "Quia iste architecto perferendis voluptatem sit tempora est.", new List<string> { "consequatur" }, 118, "en", "https://picsum.photos/640/480/?image=85", 6.5393721277377468, new DateTime(2015, 10, 2, 20, 49, 5, 140, DateTimeKind.Local).AddTicks(703), 4, "Small Rubber Gloves", "https://charles.org", new DateTime(2025, 4, 23, 6, 11, 56, 419, DateTimeKind.Utc).AddTicks(1110) },
                    { new Guid("d3f26485-dddc-4c09-a637-eb20a9a4c8d3"), new List<string> { "ut", "est", "et", "dolor", "optio", "fuga", "aperiam" }, "Samoa", new DateTime(2025, 4, 23, 6, 11, 56, 418, DateTimeKind.Utc).AddTicks(7827), "", "In facilis rerum et iste.", new List<string> { "ea", "ipsum", "porro" }, 113, "en", "https://picsum.photos/640/480/?image=764", 5.8439559066519031, new DateTime(2017, 10, 28, 21, 52, 47, 573, DateTimeKind.Local).AddTicks(6532), 4, "Ergonomic Fresh Chips", "http://gabrielle.net", new DateTime(2025, 4, 23, 6, 11, 56, 418, DateTimeKind.Utc).AddTicks(7828) },
                    { new Guid("d61b6dd0-43b0-47a2-a21b-31b9af4e6c06"), new List<string> { "beatae", "molestias", "molestias" }, "Micronesia", new DateTime(2025, 4, 23, 6, 11, 56, 419, DateTimeKind.Utc).AddTicks(2110), "", "Voluptatibus cumque veritatis debitis.", new List<string> { "et" }, 179, "en", "https://picsum.photos/640/480/?image=482", 4.6791240924529545, new DateTime(2021, 7, 16, 2, 1, 58, 206, DateTimeKind.Local).AddTicks(5045), 4, "Refined Plastic Sausages", "https://candelario.net", new DateTime(2025, 4, 23, 6, 11, 56, 419, DateTimeKind.Utc).AddTicks(2110) },
                    { new Guid("d8e12eb4-3bc4-404b-9b58-fc8542a8d374"), new List<string> { "optio", "aperiam", "maxime", "qui", "sint", "voluptas" }, "Guatemala", new DateTime(2025, 4, 23, 6, 11, 56, 419, DateTimeKind.Utc).AddTicks(1029), "", "Et ea nobis.", new List<string> { "eligendi", "rem", "consequatur" }, 115, "en", "https://picsum.photos/640/480/?image=478", 3.7716814561258385, new DateTime(2021, 1, 30, 13, 9, 47, 589, DateTimeKind.Local).AddTicks(4725), 4, "Gorgeous Plastic Hat", "https://rita.org", new DateTime(2025, 4, 23, 6, 11, 56, 419, DateTimeKind.Utc).AddTicks(1030) },
                    { new Guid("de94b9fa-df58-4454-9701-dcedb4a00b10"), new List<string> { "eum", "omnis", "laboriosam", "et", "mollitia", "dicta" }, "Kuwait", new DateTime(2025, 4, 23, 6, 11, 56, 418, DateTimeKind.Utc).AddTicks(8369), "", "Cupiditate aliquid vero molestiae quibusdam harum eligendi fugiat tenetur.", new List<string> { "ut", "quasi", "quasi" }, 66, "en", "https://picsum.photos/640/480/?image=378", 2.8137303871386496, new DateTime(2025, 3, 30, 13, 38, 22, 197, DateTimeKind.Local).AddTicks(3550), 4, "Handcrafted Concrete Pizza", "https://rico.net", new DateTime(2025, 4, 23, 6, 11, 56, 418, DateTimeKind.Utc).AddTicks(8370) },
                    { new Guid("df19fb21-2ee1-493c-837d-680fb5ee8f6f"), new List<string> { "soluta", "fuga", "nihil", "molestias", "aut", "quae" }, "Nigeria", new DateTime(2025, 4, 23, 6, 11, 56, 418, DateTimeKind.Utc).AddTicks(6922), "", "Ab temporibus dolor autem non iure similique.", new List<string> { "consequatur" }, 62, "en", "https://picsum.photos/640/480/?image=1075", 1.7593031889849295, new DateTime(2024, 2, 7, 7, 55, 1, 616, DateTimeKind.Local).AddTicks(4316), 4, "Refined Plastic Bacon", "https://mallory.info", new DateTime(2025, 4, 23, 6, 11, 56, 418, DateTimeKind.Utc).AddTicks(6923) },
                    { new Guid("df377ad6-0e2d-4237-a1bd-c4140c652f4f"), new List<string> { "mollitia", "quia", "rerum" }, "Georgia", new DateTime(2025, 4, 23, 6, 11, 56, 418, DateTimeKind.Utc).AddTicks(8558), "", "Deleniti occaecati quia nesciunt omnis earum omnis optio eaque.", new List<string> { "ratione", "aspernatur" }, 100, "en", "https://picsum.photos/640/480/?image=256", 2.9087275055096811, new DateTime(2020, 4, 9, 4, 2, 27, 892, DateTimeKind.Local).AddTicks(1713), 4, "Refined Cotton Mouse", "http://shaun.biz", new DateTime(2025, 4, 23, 6, 11, 56, 418, DateTimeKind.Utc).AddTicks(8559) },
                    { new Guid("eadd08a2-26ef-44eb-a922-a8aebefe3807"), new List<string> { "debitis", "aliquid", "ullam", "minus", "voluptas" }, "Iran", new DateTime(2025, 4, 23, 6, 11, 56, 418, DateTimeKind.Utc).AddTicks(7534), "", "Adipisci tempora quo earum explicabo modi omnis laborum.", new List<string> { "aperiam", "libero", "fugiat" }, 158, "en", "https://picsum.photos/640/480/?image=950", 1.2998497126390438, new DateTime(2024, 11, 1, 2, 15, 59, 174, DateTimeKind.Local).AddTicks(628), 4, "Ergonomic Plastic Chair", "http://griffin.biz", new DateTime(2025, 4, 23, 6, 11, 56, 418, DateTimeKind.Utc).AddTicks(7534) },
                    { new Guid("ec12d0d3-1e39-4bca-af24-dd3158c4a2ef"), new List<string> { "omnis", "saepe", "hic", "nobis", "eos" }, "Trinidad and Tobago", new DateTime(2025, 4, 23, 6, 11, 56, 418, DateTimeKind.Utc).AddTicks(7124), "", "Corrupti pariatur omnis aut pariatur et sunt.", new List<string> { "magnam", "vero" }, 125, "en", "https://picsum.photos/640/480/?image=523", 6.9575613823269506, new DateTime(2020, 12, 24, 6, 37, 26, 251, DateTimeKind.Local).AddTicks(8199), 4, "Rustic Frozen Bike", "https://everett.name", new DateTime(2025, 4, 23, 6, 11, 56, 418, DateTimeKind.Utc).AddTicks(7124) },
                    { new Guid("f39e7790-e51a-4be5-9a48-eb32a9453599"), new List<string> { "laudantium", "quo", "ducimus" }, "Saint Vincent and the Grenadines", new DateTime(2025, 4, 23, 6, 11, 56, 418, DateTimeKind.Utc).AddTicks(8845), "", "Minima provident et omnis sit iure nulla impedit qui sunt.", new List<string> { "magnam" }, 113, "en", "https://picsum.photos/640/480/?image=163", 2.3130585140690068, new DateTime(2023, 7, 4, 9, 12, 40, 614, DateTimeKind.Local).AddTicks(5254), 4, "Small Concrete Table", "http://hilma.name", new DateTime(2025, 4, 23, 6, 11, 56, 418, DateTimeKind.Utc).AddTicks(8845) },
                    { new Guid("f3be8980-039e-412f-ba39-4a49ed9ba797"), new List<string> { "quaerat", "incidunt", "est", "vel", "quo" }, "Iceland", new DateTime(2025, 4, 23, 6, 11, 56, 419, DateTimeKind.Utc).AddTicks(455), "", "Aliquid temporibus et omnis nam totam veritatis.", new List<string> { "qui", "similique" }, 107, "en", "https://picsum.photos/640/480/?image=578", 9.4780139837933195, new DateTime(2017, 7, 10, 6, 10, 16, 924, DateTimeKind.Local).AddTicks(284), 4, "Generic Steel Sausages", "https://emmalee.org", new DateTime(2025, 4, 23, 6, 11, 56, 419, DateTimeKind.Utc).AddTicks(455) },
                    { new Guid("f741dddc-36ce-4db9-81f4-2d3989882966"), new List<string> { "cupiditate", "perferendis", "maxime", "est", "nobis", "dicta", "voluptatem" }, "Greece", new DateTime(2025, 4, 23, 6, 11, 56, 418, DateTimeKind.Utc).AddTicks(9348), "", "Aut deserunt in et aut architecto quos suscipit in.", new List<string> { "nam", "vitae" }, 74, "en", "https://picsum.photos/640/480/?image=1045", 7.9965380628817337, new DateTime(2015, 7, 14, 5, 1, 29, 487, DateTimeKind.Local).AddTicks(5246), 4, "Intelligent Frozen Gloves", "http://jennifer.name", new DateTime(2025, 4, 23, 6, 11, 56, 418, DateTimeKind.Utc).AddTicks(9348) },
                    { new Guid("f969900b-8ba7-4192-a014-f6f5df7fa68d"), new List<string> { "ut", "accusantium", "similique", "et" }, "Taiwan", new DateTime(2025, 4, 23, 6, 11, 56, 418, DateTimeKind.Utc).AddTicks(8965), "", "Praesentium non non omnis ad consequuntur quas quam.", new List<string> { "labore", "perferendis" }, 71, "en", "https://picsum.photos/640/480/?image=968", 5.4364651380717657, new DateTime(2017, 7, 21, 5, 21, 18, 60, DateTimeKind.Local).AddTicks(9126), 4, "Awesome Fresh Fish", "https://marques.org", new DateTime(2025, 4, 23, 6, 11, 56, 418, DateTimeKind.Utc).AddTicks(8965) },
                    { new Guid("f99ae8d7-70a8-4cbc-84bb-1c1b95f74ce1"), new List<string> { "odit", "dolore", "enim" }, "Iceland", new DateTime(2025, 4, 23, 6, 11, 56, 418, DateTimeKind.Utc).AddTicks(7235), "", "Expedita alias quisquam vel impedit fugit ducimus quaerat ut.", new List<string> { "et", "cumque", "quos" }, 103, "en", "https://picsum.photos/640/480/?image=71", 9.6705201834514547, new DateTime(2018, 11, 10, 6, 17, 44, 124, DateTimeKind.Local).AddTicks(7444), 4, "Practical Metal Pants", "http://fabiola.name", new DateTime(2025, 4, 23, 6, 11, 56, 418, DateTimeKind.Utc).AddTicks(7236) }
                });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Code", "Description", "Name", "State" },
                values: new object[,]
                {
                    { new Guid("44444444-4444-4444-4444-444444444444"), "CreateContent", "Permission to create content", "CreateContent", 4 },
                    { new Guid("55555555-5555-5555-5555-555555555555"), "EditContent", "Permission to edit content", "EditContent", 4 },
                    { new Guid("66666666-6666-6666-6666-666666666666"), "DeleteContent", "Permission to delete content", "DeleteContent", 4 },
                    { new Guid("77777777-7777-7777-7777-777777777777"), "ViewContent", "Permission to view content", "ViewContent", 4 }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Description", "Name", "State" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Administrator role with full permissions", "admin", 4 },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "Editor role with editing permissions", "moderator", 4 },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "User role with viewing permissions", "user", 4 },
                    { new Guid("99999999-9999-9999-9999-999999999999"), "Root", "root", 4 }
                });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { new Guid("44444444-4444-4444-4444-444444444444"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("66666666-6666-6666-6666-666666666666"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("77777777-7777-7777-7777-777777777777"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("77777777-7777-7777-7777-777777777777"), new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("77777777-7777-7777-7777-777777777777"), new Guid("33333333-3333-3333-3333-333333333333") },
                    { new Guid("44444444-4444-4444-4444-444444444444"), new Guid("99999999-9999-9999-9999-999999999999") },
                    { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("99999999-9999-9999-9999-999999999999") },
                    { new Guid("66666666-6666-6666-6666-666666666666"), new Guid("99999999-9999-9999-9999-999999999999") },
                    { new Guid("77777777-7777-7777-7777-777777777777"), new Guid("99999999-9999-9999-9999-999999999999") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Movies");

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("44444444-4444-4444-4444-444444444444"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("66666666-6666-6666-6666-666666666666"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("77777777-7777-7777-7777-777777777777"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("22222222-2222-2222-2222-222222222222") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("77777777-7777-7777-7777-777777777777"), new Guid("22222222-2222-2222-2222-222222222222") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("77777777-7777-7777-7777-777777777777"), new Guid("33333333-3333-3333-3333-333333333333") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("44444444-4444-4444-4444-444444444444"), new Guid("99999999-9999-9999-9999-999999999999") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("99999999-9999-9999-9999-999999999999") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("66666666-6666-6666-6666-666666666666"), new Guid("99999999-9999-9999-9999-999999999999") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("77777777-7777-7777-7777-777777777777"), new Guid("99999999-9999-9999-9999-999999999999") });

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"));

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Permissions");
        }
    }
}
