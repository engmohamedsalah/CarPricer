using CarPricer;
using NUnit.Framework;

namespace CarPricerTest
{

    [TestFixture()]
    public class UnitTests
    {
        [TestCase(24, 50000, 44000)]
        [TestCase(120, 50000, 20000)]
        [TestCase(150, 50000, 20000)]
        public void Should_calculate_age_price_reduction(int age, decimal price, decimal expected)
        {
            var actual = new PriceDeterminator().ApplyAgeReduction(age, price);
            Assert.AreEqual(expected, actual);
        }

        [TestCase(1000, 50000, 49900)]
        [TestCase(2000, 50000, 49800)]
        [TestCase(2239, 50000, 49800)]
        [TestCase(150000, 50000, 35000)]
        [TestCase(200000, 50000, 35000)]
        public void Should_calculate_miles_price_reduction(int miles, decimal price, decimal expected)
        {
            var actual = new PriceDeterminator().ApplyMilesReduction(miles, price);
            Assert.AreEqual(expected, actual);
        }

        [TestCase(0, 50000, 50000)]
        [TestCase(1, 50000, 50000)]
        [TestCase(2, 50000, 50000)]
        [TestCase(3, 50000, 37500)]
        [TestCase(5, 50000, 37500)]
        public void Should_calculate_previous_owners_price_reduction(int owners, decimal price, decimal expected)
        {
            var actual = new PriceDeterminator().ApplyPreviousOwnersReduction(owners, price);
            Assert.AreEqual(expected, actual);
        }

        [TestCase(0, 50000, 50000)]
        [TestCase(1, 50000, 49000)]
        [TestCase(2, 50000, 48000)]
        [TestCase(5, 50000, 45000)]
        public void Should_calculate_collision_reduction(int collisons, decimal price, decimal expected)
        {
            var actual = new PriceDeterminator().ApplyCollisionReduction(collisons, price);
            Assert.AreEqual(expected, actual);
        }

        [TestCase(3, 50000, 50000)]
        [TestCase(2, 50000, 50000)]
        [TestCase(1, 50000, 50000)]
        [TestCase(0, 50000, 55000)]
        public void Should_calculate_previous_owners_bonus(int owners, decimal price, decimal expected)
        {
            var actual = new PriceDeterminator().ApplyPreviousOwnersBonus(owners, price);
            Assert.AreEqual(expected, actual);
        }

        [Test()]
        public void CalculateCarValue()
        {
            AssertCarValue(25313.40m, 35000m, 3 * 12, 50000, 1, 1);
            AssertCarValue(19688.20m, 35000m, 3 * 12, 150000, 1, 1);
            AssertCarValue(19688.20m, 35000m, 3 * 12, 250000, 1, 1);
            AssertCarValue(20090.00m, 35000m, 3 * 12, 250000, 1, 0);
            AssertCarValue(21657.02m, 35000m, 3 * 12, 250000, 0, 1);
        }

        private static void AssertCarValue(decimal expectValue, decimal purchaseValue,
        int ageInMonths, int numberOfMiles, int numberOfPreviousOwners, int
        numberOfCollisions)
        {
            Car car = new Car
            {
                AgeInMonths = ageInMonths,
                NumberOfCollisions = numberOfCollisions,
                NumberOfMiles = numberOfMiles,
                NumberOfPreviousOwners = numberOfPreviousOwners,
                PurchaseValue = purchaseValue
            };
            PriceDeterminator priceDeterminator = new PriceDeterminator();
            var carPrice = priceDeterminator.DetermineCarPrice(car);
            Assert.AreEqual(expectValue, carPrice);
        }
    }
}