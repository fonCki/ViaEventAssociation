using System.Runtime.InteropServices.JavaScript;

namespace ViaEventAssociation.Core.Tools.OperationResult {
    public class Error {
        public int Code { get; }
        public string Message { get; }
        public Error Next { get; private set; }


        // Factory methods for creating specific errors
        // public static Error NoError => new Error(0, "No error");
        // public static Error BadRequest => new Error(400, "The request could not be understood by the server due to malformed syntax.");
        // public static Error Unauthorized => new Error(401, "The request requires user authentication.");
        // public static Error Forbidden => new Error(403, "The server understood the request, but is refusing to fulfill it.");
        // public static Error NotFound => new Error(404, "The server has not found anything matching the Request-URI.");
        // public static Error Teapot => new Error(418, "I'm a teapot. The requested entity body is short and stout. Tip me over and pour me out.");
        // public static Error InternalServerError => new Error(500, "The server encountered an unexpected condition which prevented it from fulfilling the request.");
        public static Error InvalidEmail => new Error(1001, "The email address provided is invalid.");
        // public static Error InvalidDateTime => new Error(1002, "The date or time provided does not match the expected format or is out of range.");
        // public static Error DuplicateUID => new Error(1003, "The UID provided already exists.");
        // public static Error EventFull => new Error(1004, "The event has reached its capacity limit. No more tickets can be sold.");
        // public static Error PaymentRequired => new Error(1005, "Payment information is required to complete this operation.");
        // public static Error EventCancelled => new Error(1006, "The event has been cancelled.");
        // public static Error AccessDenied => new Error(1007, "You do not have permission to perform this action.");
        // public static Error ResourceNotAvailable => new Error(1008, "The requested resource is not available.");
        // public static Error ValidationFailed => new Error(1009, "Provided data did not pass validation checks.");
        // public static Error RateLimitExceeded => new Error(1010, "Too many requests. Please try again later.");
        public static Error InvalidDateTimeRange => new Error(1011, "The start time must be before the end time.");
        public static Error InvalidOrganizerName => new Error(1012, "The organizer name cannot be empty.");
        public static Error Unknown => new Error(9999, "An unknown error occurred.");
        public static Error BlankString => new Error(1013, "The provided string cannot be blank.");

        public static Error TooLongString => new Error(1014, "The provided string is too long.");
        public static Error TooShortString => new Error(1015, "The provided string is too short.");
        public static Error EventStatusInvalid => new Error(1016, "The event status is invalid for this operation.");
        public static Error NullString => new Error(1017, "The provided string cannot be null.");
        public static Result NullDateTime => new Error(1018, "The provided date or time cannot be null.");

        // Method to convert Exception to a generic Error
        public static Error FromException(Exception exception) { return new Error(500, exception.Message); }


        private Error(int code, string message) {
            Code = code;
            Message = message;
            Next = null;
        }

        private void Append(Error error) {
            if (this.Next == null) {
                this.Next = error;
            } else {
                this.Next.Append(error);
            }
        }

        public static Error Add(HashSet<Error> errors) {
            // Create a new error with the first error in the chain
            var error = errors.First();
            // Add the rest of the errors to the chain
            foreach (var e in errors.Skip(1)) {
                error.Append(e);
            }
            return error;
        }

        public static Error Add(Error error) {
            var newError = new Error(error.Code, error.Message);
            return newError;
        }

        public int Count() {
            return Next == null ? 1 : 1 + Next.Count();
        }

        public IEnumerable<Error> GetAllErrors() {
            var errors = new List<Error> { this };
            var current = this.Next;
            while (current != null) {
                errors.Add(current);
                current = current.Next;
            }
            return errors;
        }

        public override string ToString() {
            // If there are multiple errors, return a string with all of them
            if (Next != null) {
                return $"{Message}\n{Next}";
            }
            return Message;
        }

    }
}
