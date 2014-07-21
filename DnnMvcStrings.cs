using System.Collections.Generic;

namespace BigfootDNN
{
    public class DnnMvcStrings
    {
        private readonly DnnMvcApplication App;
        public DnnMvcStrings(DnnMvcApplication app)
        {
            App = app;
        }

        public string PostValRequired { get { return App.GetLocalString("MVC.PostValRequired", "Expected value not found: "); } }
        public string PostRequestOnly { get { return App.GetLocalString("MVC.POSTRequestOnly", "Invalid GET Request"); } }
        public string GetRequestOnly { get { return App.GetLocalString("MVC.GETRequestOnly", "Invalid POST Request"); } }
        public string AuthenticatedOnly { get { return App.GetLocalString("MVC.AuthenticatedOnly", "Must be logged in to perform this action"); } }
        public string AdminOnly { get { return App.GetLocalString("MVC.AdminOnly", "Insufficient rights. Must be an administrator."); } }
        public string HostOnly { get { return App.GetLocalString("MVC.HostOnly", "Insufficient rights. Must be a super user account."); } }
        public string UnableToFindControl { get { return App.GetLocalString("MVC.UnableToFindControl", "Unable to find control at {0} or {1}"); } }
        public string ControllerNotFound { get { return App.GetLocalString("MVC.ControllerNotFound", "Controller: {0}, {1} not found"); } }
        public string ActionNotFound { get { return App.GetLocalString("MVC.ActionNotFound", "Action {0}.{1} not found"); } }

        // Validation
        public void LoadValidationLanguage()
        {
            var langStrings = new Dictionary<string, string>
            {
                {"int_IsLessThanOrEqual", App.GetLocalString("MVCVal.int.IsLessThanOrEqual", "must be less than or equal to {1}") },
                {"int_IsLessThan", App.GetLocalString("MVCVal.int.IsLessThan", "must be less than {1}")},
                {"int_IsGreaterThanOrEqual", App.GetLocalString("MVCVal.int.IsGreaterThanOrEqual", "{0} must be greater than or equal to {1}") },
                {"int_IsGreaterThan", App.GetLocalString("MVCVal.int.IsGreaterThan", "{0} must be greater than {1}") },
                {"int_Equals", App.GetLocalString("MVCVal.int.Equals", "{0} must be {1}") },
                {"int_Between", App.GetLocalString("MVCVal.int.Between", "{0} must be between {1} and {2}") },
                {"int_IsZero", App.GetLocalString("MVCVal.int.IsZero", "{0} must be zero") },
                {"string_IsEmpty", App.GetLocalString("MVCVal.string.IsEmpty", "{0} must be empty") },
                {"string_IsLongerThan", App.GetLocalString("MVCVal.string.IsLongerThan", "{0} is too short (min {1} characters)") },
                {"string_IsShorterThan", App.GetLocalString("MVCVal.string.IsShorterThan", "{0} is too long (max {1} characters)") },
                {"string_IsEmail", App.GetLocalString("MVCVal.string.IsEmail", "'{0}' is not a valid email address") },
                {"string_IsURL", App.GetLocalString("MVCVal.string.IsURL", "'{0}' is not a valid URL") },
                {"string_IsDate", App.GetLocalString("MVCVal.string.IsDate", "'{0}' is not a valid date") },
                {"string_IsInteger", App.GetLocalString("MVCVal.string.IsInteger", "'{0}' is not a valid integer value") },
                {"string_IsDecimal", App.GetLocalString("MVCVal.string.IsDecimal", "'{0}' is not a valid decimal value") },
                {"string_HasALengthBetween", App.GetLocalString("MVCVal.string.HasALengthBetween", "'{0}' must have a length between {1} and {2} characters") },
                {"string_StartsWith", App.GetLocalString("MVCVal.string.StartsWith", "'{0}' must start with '{1}'") },
                {"string_EndsWith", App.GetLocalString("MVCVal.string.EndsWith", "'{0}' must end with '{1}'") },
                {"string_Contains", App.GetLocalString("MVCVal.string.Contains", "'{0}' must contain '{1}'") },
                {"string_IsLength", App.GetLocalString("MVCVal.string.IsLength", "'{0}' must consist of '{1}' characters") },
                {"string_IsCreditCard", App.GetLocalString("MVCVal.string.IsCreditCard", "'{0}' must be a valid credit card number") },
                {"date_IsNotAFutureDate", App.GetLocalString("MVCVal.date.IsNotAFutureDate", "'{0}' must not be in the future") },
                {"date_IsNotAPastDate", App.GetLocalString("MVCVal.date.IsNotAPastDate", "'{0}' must not be in the past") },
                {"date_IsNotMinMaxValue", App.GetLocalString("MVCVal.date.IsNotMinMaxValue", "'{0}' must not be a minimum or maximum value") },
                {"date_IsLaterThan", App.GetLocalString("MVCVal.date.IsLaterThan", "'{0}' must be later than {1}") },
                {"date_IsEarlierThan", App.GetLocalString("MVCVal.date.IsEarlierThan", "'{0}' must be earlier than {1}") },
                {"not_int_IsLessThanOrEqual", App.GetLocalString("MVCVal.not.int.IsLessThanOrEqual", "{0} must not be less than or equal to {1}") },
                {"not_int_IsLessThan", App.GetLocalString("MVCVal.not.int.IsLessThan", "{0} must not be less than {1}") },
                {"not_int_IsGreaterThanOrEqual", App.GetLocalString("MVCVal.not.int.IsGreaterThanOrEqual", "{0} must not be greater than or equal to {1}") },
                {"not_int_IsGreaterThan", App.GetLocalString("MVCVal.not.int.IsGreaterThan", "{0} must not be greater than {1}") },
                {"not_int_Equals", App.GetLocalString("MVCVal.not.int.Equals", "{0} must not be {1}") },
                {"not_int_Between", App.GetLocalString("MVCVal.not.int.Between", "{0} must not be between {1} and {2}") },
                {"not_int_IsZero", App.GetLocalString("MVCVal.not.int.IsZero", "{0} must not be zero") },
                {"not_string_IsEmpty", App.GetLocalString("MVCVal.not.string.IsEmpty", "{0} must not be empty") },
                {"not_string_IsLongerThan", App.GetLocalString("MVCVal.not.string.IsLongerThan", "{0} is too long (max {1} characters)") },
                {"not_string_IsShorterThan", App.GetLocalString("MVCVal.not.string.IsShorterThan", "{0} is too short (min {1} characters)") },
                {"not_string_IsEmail", App.GetLocalString("MVCVal.not.string.IsEmail", "'{0}' can not be an email address") },
                {"not_string_IsURL", App.GetLocalString("MVCVal.not.string.IsURL", "'{0}' can not be an URL") },
                {"not_string_IsDate", App.GetLocalString("MVCVal.not.string.IsDate", "'{0}' can not be a date") },
                {"not_string_IsInteger", App.GetLocalString("MVCVal.not.string.IsInteger", "'{0}' can not be an integer value") },
                {"not_string_IsDecimal", App.GetLocalString("MVCVal.not.string.IsDecimal", "'{0}' can not be a decimal value") },
                {"not_string_HasALengthBetween", App.GetLocalString("MVCVal.not.string.HasALengthBetween", "'{0}' cannot have a length between {1} and {2} characters") },
                {"not_string_StartsWith", App.GetLocalString("MVCVal.not.string.StartsWith", "'{0}' must not start with '{1}'") },
                {"not_string_EndsWith", App.GetLocalString("MVCVal.not.string.EndsWith", "'{0}' must not end with '{1}'") },
                {"not_string_Contains", App.GetLocalString("MVCVal.not.string.Contains", "'{0}' must not contain '{1}'") },
                {"not_string_IsLength", App.GetLocalString("MVCVal.not.string.IsLength", "'{0}' must not consist of '{1}' characters") },
                {"not_string_IsCreditCard", App.GetLocalString("MVCVal.not.string.IsCreditCard", "'{0}' must not be a valid credit card number") },
                {"not_date_IsNotAFutureDate", App.GetLocalString("MVCVal.not.date.IsNotAFutureDate", "'{0}' must be in the future") },
                {"not_date_IsNotAPastDate", App.GetLocalString("MVCVal.not.date.IsNotAPastDate", "'{0}' must be in the past") },
                {"not_date_IsNotMinMaxValue", App.GetLocalString("MVCVal.not.date.IsNotMinMaxValue", "'{0}' must be either a minimum or maximum value") },
                {"not_date_IsLaterThan", App.GetLocalString("MVCVal.not.date.IsLaterThan", "'{0}' must not be later than {1}") },
                {"not_date_IsEarlierThan", App.GetLocalString("MVCVal.not.date.IsEarlierThan", "'{0}' must not be earlier than {1}") }, 
            };

            // Load it into the validation strings
            Model.Validation.LangCache.LangStrings = langStrings;

        }
    }
}