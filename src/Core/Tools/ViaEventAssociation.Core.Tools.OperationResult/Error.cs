namespace ViaEventAssociation.Core.Tools.OperationResult {
    public class Error {
        public int Code { get; }
        public string Message { get; }

        private Error(int code, string message) {
            Code = code;
            Message = message;
        }

        // Application-Specific Errors
        public static Error NoError => new Error((int) ErrorCode.NoError, GetMessage(ErrorCode.NoError));
        public static Error BadRequest => new Error((int) ErrorCode.BadRequest, GetMessage(ErrorCode.BadRequest));
        public static Error Unauthorized => new Error((int) ErrorCode.Unauthorized, GetMessage(ErrorCode.Unauthorized));
        public static Error Forbidden => new Error((int) ErrorCode.Forbidden, GetMessage(ErrorCode.Forbidden));
        public static Error NotFound => new Error((int) ErrorCode.NotFound, GetMessage(ErrorCode.NotFound));
        public static Error Teapot => new Error((int) ErrorCode.Teapot, GetMessage(ErrorCode.Teapot));
        public static Error InternalServerError => new Error((int) ErrorCode.InternalServerError, GetMessage(ErrorCode.InternalServerError));
        public static Error InvalidEmail => new Error((int) ErrorCode.InvalidEmail, GetMessage(ErrorCode.InvalidEmail));
        public static Error InvalidDateTime => new Error((int) ErrorCode.InvalidDateTime, GetMessage(ErrorCode.InvalidDateTime));
        public static Error DuplicateUID => new Error((int) ErrorCode.DuplicateUID, GetMessage(ErrorCode.DuplicateUID));
        public static Error EventFull => new Error((int) ErrorCode.EventFull, GetMessage(ErrorCode.EventFull));
        public static Error PaymentRequired => new Error((int) ErrorCode.PaymentRequired, GetMessage(ErrorCode.PaymentRequired));
        public static Error EventCancelled => new Error((int) ErrorCode.EventCancelled, GetMessage(ErrorCode.EventCancelled));
        public static Error AccessDenied => new Error((int) ErrorCode.AccessDenied, GetMessage(ErrorCode.AccessDenied));
        public static Error ResourceNotAvailable => new Error((int) ErrorCode.ResourceNotAvailable, GetMessage(ErrorCode.ResourceNotAvailable));
        public static Error ValidationFailed => new Error((int) ErrorCode.ValidationFailed, GetMessage(ErrorCode.ValidationFailed));
        public static Error RateLimitExceeded => new Error((int) ErrorCode.RateLimitExceeded, GetMessage(ErrorCode.RateLimitExceeded));

        public static List<Error> MultipleErrors(params ErrorCode[] codes) {
            return codes.Select(code => new Error((int) code, GetMessage(code))).ToList();
        }

        // Method to convert Exception to a generic Error
        public static Error Exception(Exception exception) => new Error((int) ErrorCode.InternalServerError, exception.Message);

        public enum ErrorCode {
            NoError = 0,

            // HTTP Error Codes
            BadRequest = 400,
            Unauthorized = 401,
            Forbidden = 403,
            NotFound = 404,
            Teapot = 418,
            InternalServerError = 500,

            // Application-Specific Error Codes
            InvalidEmail = 1001,
            InvalidDateTime = 1002,
            DuplicateUID = 1003,
            EventFull = 1004,
            PaymentRequired = 1005,
            EventCancelled = 1006,
            AccessDenied = 1007,
            ResourceNotAvailable = 1008,
            ValidationFailed = 1009,
            RateLimitExceeded = 1010
        }

        private static readonly Dictionary<ErrorCode, string> Messages = new Dictionary<ErrorCode, string> {
            {ErrorCode.NoError, "No error"},
            // HTTP Error Messages
            {ErrorCode.BadRequest, "The request could not be understood by the server due to malformed syntax."},
            {ErrorCode.Unauthorized, "The request requires user authentication."},
            {ErrorCode.Forbidden, "The server understood the request, but is refusing to fulfill it."},
            {ErrorCode.NotFound, "The server has not found anything matching the Request-URI."},
            {ErrorCode.Teapot, "I'm a teapot. The requested entity body is short and stout. Tip me over and pour me out."},
            {ErrorCode.InternalServerError, "The server encountered an unexpected condition which prevented it from fulfilling the request."},
            // Application-Specific Error Messages
            {ErrorCode.InvalidEmail, "The email address provided is invalid."},
            {ErrorCode.InvalidDateTime, "The date or time provided does not match the expected format or is out of range."},
            {ErrorCode.DuplicateUID, "The UID provided already exists."},
            {ErrorCode.EventFull, "The event has reached its capacity limit. No more tickets can be sold."},
            {ErrorCode.PaymentRequired, "Payment information is required to complete this operation."},
            {ErrorCode.EventCancelled, "The event has been cancelled."},
            {ErrorCode.AccessDenied, "You do not have permission to perform this action."},
            {ErrorCode.ResourceNotAvailable, "The requested resource is not available."},
            {ErrorCode.ValidationFailed, "Provided data did not pass validation checks."},
            {ErrorCode.RateLimitExceeded, "Too many requests. Please try again later."}
        };

        private static string GetMessage(ErrorCode code) {
            return Messages.GetValueOrDefault(code, "An unexpected error occurred.");
        }
    }
}