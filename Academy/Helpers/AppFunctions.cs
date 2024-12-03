namespace Academy.Helpers
{
    public static class AppFunctions
    {
        public static string NormalizeArabicText(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;

            return input
                .Replace("أ", "ا")
                .Replace("إ", "ا")
                .Replace("آ", "ا")
                .Replace("ى", "ي")
                .Replace("ئ", "ي")
                .Replace("ؤ", "و")
                .Replace("ة", "ه")
                .Replace("َ", "")  // إزالة الفتحة
                .Replace("ً", "")  // إزالة التنوين بالفتح
                .Replace("ُ", "")  // إزالة الضمة
                .Replace("ٌ", "")  // إزالة التنوين بالضم
                .Replace("ِ", "")  // إزالة الكسرة
                .Replace("ٍ", "")  // إزالة التنوين بالكسر
                .Replace("ْ", "")  // إزالة السكون
                .Replace("ّ", ""); // إزالة الشدة
        }
    }
}
