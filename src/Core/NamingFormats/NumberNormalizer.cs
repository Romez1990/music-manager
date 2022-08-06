using Core.IocContainer;

namespace Core.NamingFormats {
    [Service]
    public class NumberNormalizer : INumberNormalizer {
        public string Normalize(int number, int numberLength) =>
            number.ToString().PadLeft(numberLength, '0');
    }
}
