namespace Notification.Helpers
{
    public static class StringHelper
    {
        public static int? ToInt(this string? @string)
        {
            if (string.IsNullOrEmpty(@string))
            {
                return null;
            }

            if (!int.TryParse(@string, out int result))
            {
                return null;
            }

            return result;
        }

    }
}
