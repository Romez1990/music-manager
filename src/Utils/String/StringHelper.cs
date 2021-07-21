namespace Utils.String {
    public static class StringHelper {
        public static string Capitalize(this string @string) =>
            @string[0].ToString().ToUpper() + @string[1..];
    }
}
