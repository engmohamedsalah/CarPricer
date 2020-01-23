using System;

namespace CarPricer
{
    
    // WARNING: These instructions conflict with the instructions below.
    //E.g., Start with the current value of the car,
    // then adjust for age, take that result
    // then adjust for miles,
    // then collision, and
    // finally previous owner. 
    public class PriceDeterminator
    {
        const int MAX_COLLISIONS = 5;
        const int MAX_AGE = 120;
        const int MAX_MILES = 150000;
        public decimal DetermineCarPrice(Car car)
        {
            if(car == null) throw new ArgumentException(nameof(car));
            // WARNING: These instructions conflict with the instructions above.
            //Each factor should be off of the result of the previous value in the order of
            //    1. AGE
            //    2. MILES
            //    3. PREVIOUS OWNER
            //    4. COLLISION
            var value = car.PurchaseValue;
            value = ApplyAgeReduction(car.AgeInMonths, value);
            value = ApplyMilesReduction(car.NumberOfMiles, value);
            //if previous owner, had a positive effect,
            //then it should be applied AFTER step 4.
            //If a negative effect, then BEFORE step 4.
            value = ApplyPreviousOwnersReduction(car.NumberOfPreviousOwners, value);
            value = ApplyCollisionReduction(car.NumberOfCollisions, value);
            value = ApplyPreviousOwnersBonus(car.NumberOfPreviousOwners, value);

            return value;
        }

        public decimal ApplyAgeReduction(int ageInMonths, decimal currentValue)
        {
            if(currentValue < 0) throw new ArgumentException(nameof(currentValue));
            if(ageInMonths < 0) throw new ArgumentException(nameof(ageInMonths));
            //Given the number of months of how old the car is,
            //reduce its value one-half (0.5) percent.
            //After 10 years, it's value cannot be reduced further by age.
            //This is not cumulative.
            if (ageInMonths > MAX_AGE) ageInMonths = MAX_AGE;
            var percentage = 1 - (ageInMonths * 0.005m);
            return currentValue * percentage;
        }

        public decimal ApplyMilesReduction(int numberOfMiles, decimal currentValue)
        {
            if(currentValue < 0) throw new ArgumentException(nameof(currentValue));
            if(numberOfMiles < 0) throw new ArgumentException(nameof(numberOfMiles));
            //For every 1,000 miles on the car,
            //reduce its value by one-fifth of a percent(0.2).
            //Do not consider remaining miles. After 150,000 miles,
            //it's value cannot be reduced further by miles.
            if (numberOfMiles > MAX_MILES) numberOfMiles = MAX_MILES;
            var thousandsOfMiles = Math.Floor(numberOfMiles / 1000m);
            var percentage = 1 - (thousandsOfMiles * 0.002m);
            return currentValue * percentage;
        }

        public decimal ApplyPreviousOwnersReduction(int previousOwners, decimal currentValue)
        {
            if(currentValue < 0) throw new ArgumentException(nameof(currentValue));
            if(previousOwners < 0) throw new ArgumentException(nameof(previousOwners));
            //If the car has had more than 2 previous owners,
            //reduce its value by twenty-five(25) percent.
            if (previousOwners > 2) return currentValue * 0.75m;
            return currentValue;
        }

        public decimal ApplyCollisionReduction(int numberOfCollisions, decimal currentValue)
        {
            if(currentValue < 0) throw new ArgumentException(nameof(currentValue));
            if(numberOfCollisions < 0) throw new ArgumentException(nameof(numberOfCollisions));
            //For every reported collision the car has been in, remove two (2) 
            //percent of it's value up to five (5) collisions.
            if (numberOfCollisions == 0) return currentValue;
            if (numberOfCollisions > MAX_COLLISIONS) numberOfCollisions = MAX_COLLISIONS ;
            var percentage = 1 - (numberOfCollisions * 0.02m);
            return currentValue * percentage;
        }
        public decimal ApplyPreviousOwnersBonus(int previousOwners, decimal currentValue)
        {
            if(currentValue < 0) throw new ArgumentException(nameof(currentValue));
            if(previousOwners < 0) throw new ArgumentException(nameof(previousOwners));
            //If the car has had no previous owners,
            //add ten(10) percent of the FINAL car value at the end.
            if (previousOwners == 0) return currentValue * 1.1m;
            return currentValue;
        }
    }
}