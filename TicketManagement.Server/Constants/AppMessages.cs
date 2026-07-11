namespace TicketManagement.Server.Constants
{
    public static class AppMessages
    {
        public const string EmailRequired = "Email is required.";

        public const string InvalidOTP = "Invalid OTP.";

        public const string OTPExpired = "OTP has expired.";

        public const string EmailVerified = "Email verified successfully.";

        public const string OTPSent = "OTP sent successfully.";

        public const string RegistrationSuccess = "Registration completed successfully.";

        public const string LoginSuccess = "Login successful.";

        public const string InvalidCredentials = "Invalid email or password.";

        public const string Unauthorized = "Unauthorized access.";

        public const string UserNotFound = "User not found.";

        public const string EmailAlreadyExists = "Email is already registered.";

        //return BadRequest(new
        //{
        //    message = AppMessages.EmailRequired
        //    });
        //return BadRequest(new
        //{
        //    message = "Email is required."
        //});


    }
}
