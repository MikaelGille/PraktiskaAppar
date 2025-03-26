using Northwind.EntityModels; // För NorthwindDatabaseContext

namespace Northwind.UnitTests
{
    public class EntityModelTests
    {
        [Fact]
        public void DatabaseConnectTest()
        {
            using NorthwindDatabaseContext db = new();

            Assert.True(db.Database.CanConnect());
        }

        [Fact]
        public void CategoryCountTest()
        {
            using NorthwindDatabaseContext db = new();
            int expected = 8;
            int actual = db.Categories.Count();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ProductIdTest()
        {
            using NorthwindDatabaseContext db = new();
            string expected = "Tofu";
            Product? productWithId14 = db.Products.Find(14);
            string actual = productWithId14?.ProductName ?? string.Empty;

            Assert.Equal(expected, actual);
        }
    }
}
