using System.Linq;

namespace Core.UserConfig {
    public class UserConfig {
        public string GeniusApiToken { get; set; }

        public bool Equals(UserConfig that) =>
            GetType()
                .GetProperties()
                .All(property => {
                    var thisValue = property.GetValue(this);
                    var thatValue = property.GetValue(that);
                    if (thisValue is null) {
                        return thatValue is null;
                    }
                    return thisValue.Equals(thatValue);
                });
    }
}
