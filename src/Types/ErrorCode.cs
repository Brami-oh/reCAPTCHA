using System;

namespace Finoaker.Web.Recaptcha
{
    /// <summary>
    /// Error code(s) returned from a failed verification.
    /// </summary>
    [Flags]
    public enum ErrorCode
    {
        /// <summary>
        /// No errors.
        /// </summary>
        None = 0,

        /// <summary>
        /// The secret parameter is missing.
        /// </summary>
        MissingInputSecret = 1,

        /// <summary>
        /// The secret parameter is invalid or malformed.
        /// </summary>
        InvalidInputSecret = 2,
        /// <summary>
        /// The secret parameter is invalid or malformed.
        /// </summary>
        InvalidKeys = InvalidInputSecret,

        /// <summary>
        /// The response parameter is missing.
        /// </summary>
        MissingInputResponse = 4,

        /// <summary>
        /// The response parameter is invalid or malformed.
        /// </summary>
        InvalidInputResponse = 8,

        /// <summary>
        /// The request is invalid or malformed.
        /// </summary>
        BadRequest = 16,

        /// <summary>
        /// The response is no longer valid: either is too old or has been used previously.
        /// </summary>
        TimeoutOrDuplicate = 32
    }
}
