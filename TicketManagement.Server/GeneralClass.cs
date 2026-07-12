namespace TicketManagement.Server
{
    public static class GeneralClass
    {
        

        public static int GetTestID { get; set; }
        public static int GetSyllabusID { get; set; }
        public static int GetChapterID { get; set; }
        public static bool IsStrongPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            if (password.Length < 8 || password.Length > 20)
                return false;

            bool hasUpper = password.Any(char.IsUpper);
            bool hasLower = password.Any(char.IsLower);
            bool hasDigit = password.Any(char.IsDigit);
            bool hasSpecial = password.Any(ch => !char.IsLetterOrDigit(ch));

            return hasUpper && hasLower && hasDigit && hasSpecial;
        }
    }
}
