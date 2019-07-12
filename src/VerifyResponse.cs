using System;
using System.Runtime.Serialization;
using System.Globalization;
using System.Linq;

namespace Finoaker.Web.Recaptcha
{
    /// <summary>
    /// Contains response information from the reCAPTCHA Verification service.
    /// </summary>
    [DataContract]
    public class VerifyResponse
    {
        /// <summary>
        /// Status of the verification.
        /// </summary>
        [DataMember(Name = "success", IsRequired = true)]
        public bool Success { get; private set; }

        /// <summary>
        /// (V3 only) The score for this request (0.0 - 1.0). 
        /// </summary>
        [DataMember(Name = "score")]
        public decimal? Score { get; private set; }

        /// <summary>
        /// (V3 only) The action name for this request (important to verify)
        /// </summary>
        [DataMember(Name = "action")]
        public string Action { get; private set; }

        /// <summary>
        /// The hostname of the site where the reCAPTCHA was solved.
        /// </summary>
        [DataMember(Name = "hostname")]
        public string Hostname { get; private set; }

        /// <summary>
        /// Any errors that were returned in verfication.
        /// </summary>
        [DataMember(Name = "error-codes")]
        private string[] _errorCodes = null;

        /// <summary>
        /// Timestamp of the challenge load in ISO format.
        /// </summary>
        [DataMember(Name = "challenge_ts")]
        private string _challengeTimestamp = null;

        /// <summary>
        /// Timestamp of the challenge load.
        /// </summary>
        [IgnoreDataMember]
        public DateTime? ChallengeTimeStamp
            => string.IsNullOrEmpty(_challengeTimestamp) ? (DateTime?)null : DateTime.Parse(_challengeTimestamp, null, DateTimeStyles.RoundtripKind);

        /// <summary>
        /// Any errors that were returned in verification.
        /// </summary>
        [IgnoreDataMember]
        public ErrorCode? ErrorCodes
            => !Success && _errorCodes != null && _errorCodes.Length > 0 ? _errorCodes.Select(code => (ErrorCode)Enum.Parse(typeof(ErrorCode), code.Replace("-", string.Empty), true)).Aggregate((a, e) => a | e) : ErrorCode.None;
    }
}