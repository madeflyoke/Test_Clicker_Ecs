using System.Linq;

namespace Core.Utils
{
    public static class FormulasUtils
    {
        public static double CalculateNextLevelPrice(int currentLevel, double basePrice)
        {
            return (currentLevel+1) * basePrice;
        }

        public static double CalculateIncome(int currentLevel, double baseIncome, int incomeMultipliersPercentsSum)
        {
            return currentLevel * baseIncome * (1 + incomeMultipliersPercentsSum/100f);
        }
    }
}
