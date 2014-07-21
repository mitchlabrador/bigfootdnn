/* *********************************************************************************
 * TNValidate Fluent Validation Library
 * https://tnvalidate.codeplex.com/
 * Copyright (C) TN Datakonsult AB 2009
 * http://www.tn-data.se/
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 * *********************************************************************************/

using System.Collections.Generic;

namespace BigfootDNN.Model.Validation
{
    internal class LangCache
    {
        public static Dictionary<string, string> LangStrings;


        /// ******************************************************************
        /// <summary>
        /// Fetch an item from the language defintion XML files.
        /// 
        /// Also caches the result
        /// </summary>
        /// <param name="StringKey">The string identifier</param>
        /// <returns>Returns "" if not found</returns>
        public static string FetchItem(string StringKey)
        {
            LoadLanguageDefinition();
            return LangStrings[StringKey];
        }

        /// *****************************************************************
        /// <summary>
        /// Load the entire language definition XML file into the LanguageDict 
        /// dictionary. LanguageDict acts like a cache/lookup-table for all the 
        /// language strings.
        /// </summary>
        private static void LoadLanguageDefinition()
        {
            if (LangStrings != null)
                return;
            // Load the language if not present
            LangStrings = new Dictionary<string, string>();

            // Add the english language
            LangStrings.Add("int_IsLessThanOrEqual", "must be less than or equal to {1}");
            LangStrings.Add("int_IsLessThan", "must be less than {1}");
            LangStrings.Add("int_IsGreaterThanOrEqual", "{0} must be greater than or equal to {1}");
            LangStrings.Add("int_IsGreaterThan", "{0} must be greater than {1}");
            LangStrings.Add("int_Equals", "{0} must be {1}");
            LangStrings.Add("int_Between", "{0} must be between {1} and {2}");
            LangStrings.Add("int_IsZero", "{0} must be zero");
            LangStrings.Add("string_IsEmpty", "{0} must be empty");
            LangStrings.Add("string_IsLongerThan", "{0} is too short (min {1} characters)");
            LangStrings.Add("string_IsShorterThan", "{0} is too long (max {1} characters)");
            LangStrings.Add("string_IsEmail", "'{0}' is not a valid email address");
            LangStrings.Add("string_IsURL", "'{0}' is not a valid URL");
            LangStrings.Add("string_IsDate", "'{0}' is not a valid date");
            LangStrings.Add("string_IsInteger", "'{0}' is not a valid integer value");
            LangStrings.Add("string_IsDecimal", "'{0}' is not a valid decimal value");
            LangStrings.Add("string_HasALengthBetween", "'{0}' must have a length between {1} and {2} characters");
            LangStrings.Add("string_StartsWith", "'{0}' must start with '{1}'");
            LangStrings.Add("string_EndsWith", "'{0}' must end with '{1}'");
            LangStrings.Add("string_Contains", "'{0}' must contain '{1}'");
            LangStrings.Add("string_IsLength", "'{0}' must consist of '{1}' characters");
            LangStrings.Add("string_IsCreditCard", "'{0}' must be a valid credit card number");
            LangStrings.Add("date_IsNotAFutureDate", "'{0}' must not be in the future");
            LangStrings.Add("date_IsNotAPastDate", "'{0}' must not be in the past");
            LangStrings.Add("date_IsNotMinMaxValue", "'{0}' must not be a minimum or maximum value");
            LangStrings.Add("date_IsLaterThan", "'{0}' must be later than {1}");
            LangStrings.Add("date_IsEarlierThan", "'{0}' must be earlier than {1}");
            LangStrings.Add("not_int_IsLessThanOrEqual", "{0} must not be less than or equal to {1}");
            LangStrings.Add("not_int_IsLessThan", "{0} must not be less than {1}");
            LangStrings.Add("not_int_IsGreaterThanOrEqual", "{0} must not be greater than or equal to {1}");
            LangStrings.Add("not_int_IsGreaterThan", "{0} must not be greater than {1}");
            LangStrings.Add("not_int_Equals", "{0} must not be {1}");
            LangStrings.Add("not_int_Between", "{0} must not be between {1} and {2}");
            LangStrings.Add("not_int_IsZero", "{0} must not be zero");
            LangStrings.Add("not_string_IsEmpty", "{0} must not be empty");
            LangStrings.Add("not_string_IsLongerThan", "{0} is too long (max {1} characters)");
            LangStrings.Add("not_string_IsShorterThan", "{0} is too short (min {1} characters)");
            LangStrings.Add("not_string_IsEmail", "'{0}' can not be an email address");
            LangStrings.Add("not_string_IsURL", "'{0}' can not be an URL");
            LangStrings.Add("not_string_IsDate", "'{0}' can not be a date");
            LangStrings.Add("not_string_IsInteger", "'{0}' can not be an integer value");
            LangStrings.Add("not_string_IsDecimal", "'{0}' can not be a decimal value");
            LangStrings.Add("not_string_HasALengthBetween", "'{0}' cannot have a length between {1} and {2} characters");
            LangStrings.Add("not_string_StartsWith", "'{0}' must not start with '{1}'");
            LangStrings.Add("not_string_EndsWith", "'{0}' must not end with '{1}'");
            LangStrings.Add("not_string_Contains", "'{0}' must not contain '{1}'");
            LangStrings.Add("not_string_IsLength", "'{0}' must not consist of '{1}' characters");
            LangStrings.Add("not_string_IsCreditCard", "'{0}' must not be a valid credit card number");
            LangStrings.Add("not_date_IsNotAFutureDate", "'{0}' must be in the future");
            LangStrings.Add("not_date_IsNotAPastDate", "'{0}' must be in the past");
            LangStrings.Add("not_date_IsNotMinMaxValue", "'{0}' must be either a minimum or maximum value");
            LangStrings.Add("not_date_IsLaterThan", "'{0}' must not be later than {1}");
            LangStrings.Add("not_date_IsEarlierThan", "'{0}' must not be earlier than {1}");
        }
    }
}