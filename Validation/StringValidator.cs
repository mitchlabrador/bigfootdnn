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

using System;
using System.Text;
using System.Text.RegularExpressions;

namespace BigfootDNN.Model.Validation
{
    /// ********************************************************************
    /// <summary>
    /// String validation handler.
    /// </summary>
    public class StringValidator : ValidatorBase<StringValidator, string>
    {
        /// ********************************************************************
        /// <summary>
        /// Initializes a new instance of the <see cref="StringValidator"/> class.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="fieldName"></param>
        /// <param name="validatorObj"></param>
        public StringValidator(string value, string fieldName, Validator validatorObj) : base(value, fieldName, validatorObj)
        {
        }

        /// ********************************************************************
        /// <summary>
        /// Checks if the string is empty.
        /// </summary>
        /// <param name="ErrorMessage"></param>
        /// <returns>My instance to allow me to chain multiple validations together</returns>
        public StringValidator IsEmpty(string ErrorMessage)
        {
            SetResult((Value ?? string.Empty).Length != 0, string.Format(ErrorMessage, FieldName), ValidationErrorCode.StringIsEmpty);
            return this;
        }

        /// <summary>
        /// Checks if the string is empty.
        /// </summary>
        /// <returns>My instance to allow me to chain multiple validations together</returns>
        public StringValidator IsEmpty()
        {
            IsEmpty(ValidatorObj.LookupLanguageString("string_IsEmpty", NegateNextValidationResult));
            return this;
        }

        /// <summary>
        /// Checks if the string is not empty.
        /// </summary>
        /// <returns>My instance to allow me to chain multiple validations together</returns>
        public StringValidator IsNotEmpty()
        {
            var ErrorMessage = ValidatorObj.LookupLanguageString("string_IsEmpty", true);
            SetResult((Value ?? string.Empty).Length == 0, string.Format(ErrorMessage, FieldName), ValidationErrorCode.StringIsEmpty);
            return this;
        }

        /// <summary>
        /// Checks if the string is not empty.
        /// </summary>
        /// <returns>My instance to allow me to chain multiple validations together</returns>
        public StringValidator IsNotEmpty(string ErrorMessage)
        {
            SetResult((Value ?? string.Empty).Length == 0, string.Format(ErrorMessage, FieldName), ValidationErrorCode.StringIsEmpty);
            return this;
        }

        /// ********************************************************************
        /// <summary>
        /// Checks if the string has the correct length
        /// </summary>
        /// <param name="RequiredLength">The required length.</param>
        /// <param name="ErrorMessage"></param>
        /// <returns>My instance to allow me to chain multiple validations together</returns>
        public StringValidator IsLength(int RequiredLength, string ErrorMessage)
        {
            SetResult(Value.Length != RequiredLength, string.Format(ErrorMessage, FieldName, RequiredLength.ToString()), ValidationErrorCode.StringIsLength);
            return this;
        }

        /// <summary>
        /// Checks if the string has the correct length
        /// </summary>
        /// <param name="RequiredLength">The required length.</param>
        /// <returns>
        /// My instance to allow me to chain multiple validations together
        /// </returns>
        public StringValidator IsLength(int RequiredLength)
        {
            IsLength(RequiredLength, ValidatorObj.LookupLanguageString("string_IsLength", NegateNextValidationResult));
            return this;
        }

        /// ********************************************************************
        /// <summary>
        /// Checks if the string has more characters than the specified minimum.
        /// </summary>
        /// <param name="MinLength">The minimum length.</param>
        /// <param name="ErrorMessage"></param>
        /// <returns>My instance to allow me to chain multiple validations together</returns>
        public StringValidator IsLongerThan(int MinLength, string ErrorMessage)
        {
            SetResult(Value.Length <= MinLength, string.Format(ErrorMessage, FieldName, MinLength.ToString()), ValidationErrorCode.StringIsLongerThan);
            return this;
        }

        /// <summary>
        /// Checks if the string has more characters than the specified minimum.
        /// </summary>
        /// <param name="MinLength">The minimum length.</param>
        /// <returns>
        /// My instance to allow me to chain multiple validations together
        /// </returns>
        public StringValidator IsLongerThan(int MinLength)
        {
            IsLongerThan(MinLength, ValidatorObj.LookupLanguageString("string_IsLongerThan", NegateNextValidationResult));
            return this;
        }

        /// ********************************************************************
        /// <summary>
        /// Checks that the string has less characters than the provided maximum.
        /// </summary>
        /// <param name="MaxLength">The maximum length.</param>
        /// <param name="ErrorMessage"></param>
        /// <returns>My instance to allow me to chain multiple validations together </returns>
        public StringValidator IsShorterThan(int MaxLength, string ErrorMessage)
        {
            SetResult(Value.Length >= MaxLength, string.Format(ErrorMessage, FieldName, MaxLength.ToString()), ValidationErrorCode.StringIsShorterThan);
            return this;
        }

        /// <summary>
        /// Checks that the string has less characters than the provided maximum.
        /// </summary>
        /// <param name="MaxLength">The maximum length.</param>
        /// <returns>
        /// My instance to allow me to chain multiple validations together
        /// </returns>
        public StringValidator IsShorterThan(int MaxLength)
        {
            IsShorterThan(MaxLength, ValidatorObj.LookupLanguageString("string_IsShorterThan", NegateNextValidationResult));
            return this;
        }

        /// ********************************************************************
        /// <summary>
        /// Checks that the string matches the supplied regular expression.
        /// </summary>
        /// <param name="RegularExpression">The regular expression to check against.</param>
        /// <param name="ErrorMessage">The Error Message.</param>
        /// <returns>My instance to allow me to chain multiple validations together </returns>
        public StringValidator MatchRegex(string RegularExpression, string ErrorMessage)
        {
            Regex Reg = new Regex(RegularExpression);
            SetResult(!Reg.IsMatch(Value), ErrorMessage, ValidationErrorCode.StringMatchRegex);
            return this;
        }

        /// ********************************************************************
        /// <summary>
        /// Checks that the string matches the supplied regular expression.
        /// </summary>
        /// <param name="RegularExpression">The regular expression to check against.</param>
        /// <param name="regexOptions">Any options for the regular expression.</param>
        /// <param name="ErrorMessage">The Error Message.</param>
        /// <returns>My instance to allow me to chain multiple validations together </returns>
        public StringValidator MatchRegex(string RegularExpression, RegexOptions regexOptions, string ErrorMessage)
        {
            Regex Reg = new Regex(RegularExpression, regexOptions);
            SetResult(!Reg.IsMatch(Value), ErrorMessage, ValidationErrorCode.StringMatchRegex);
            return this;
        }

        /// ********************************************************************
        /// <summary>
        /// Checks that the string is an email address.
        /// </summary>
        /// <param name="ErrorMessage"></param>
        /// <returns>
        /// My instance to allow me to chain multiple validations together
        /// </returns>
        public StringValidator IsEmail(string ErrorMessage)
        {
            Regex Reg = new Regex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+$", RegexOptions.IgnoreCase);
            SetResult(!Reg.IsMatch(Value), string.Format(ErrorMessage, FieldName, Value.ToString()), ValidationErrorCode.StringIsEmail);
            return this;
        }
        
        /// <summary>
        /// Checks that the string is an email address.
        /// </summary>
        /// <returns>
        /// My instance to allow me to chain multiple validations together
        /// </returns>
        public StringValidator IsEmail()
        {
            IsEmail(ValidatorObj.LookupLanguageString("string_IsEmail", NegateNextValidationResult));
            return this;
        }

        /// ********************************************************************
        /// <summary>
        /// Checks that the string is a URL.
        /// </summary>
        /// <param name="ErrorMessage"></param>
        /// <returns>
        /// My instance to allow me to chain multiple validations together
        /// </returns>
        public StringValidator IsURL(string ErrorMessage)
        {
            Regex Reg = new Regex(@"^\w+://(?:[\w-]+(?:\:[\w-]+)?\@)?(?:[\w-]+\.)+[\w-]+(?:\:\d+)?[\w- ./?%&=\+]*$", RegexOptions.IgnoreCase);
            SetResult(!Reg.IsMatch(Value), string.Format(ErrorMessage, FieldName, Value.ToString()), ValidationErrorCode.StringIsURL);
            return this;
        }

        /// <summary>
        /// Checks that the string is a URL.
        /// </summary>
        /// <returns>
        /// My instance to allow me to chain multiple validations together
        /// </returns>
        public StringValidator IsURL()
        {
            IsURL(ValidatorObj.LookupLanguageString("string_IsURL", NegateNextValidationResult));
            return this;
        }

        /// ********************************************************************
        /// <summary>
        /// Checks if the string can be parsed as a date under the current locale.
        /// </summary>
        /// <param name="ErrorMessage"></param>
        /// <returns>
        /// My instance to allow me to chain multiple validations together
        /// </returns>
        public StringValidator IsDate(string ErrorMessage)
        {
            DateTime Date;
            SetResult(!DateTime.TryParse(Value, out Date), string.Format(ErrorMessage, FieldName, Value.ToString()), ValidationErrorCode.StringIsDate);
            return this;
        }

        /// <summary>
        /// Checks if the string can be parsed as a date under the current locale.
        /// </summary>
        /// <returns>
        /// My instance to allow me to chain multiple validations together
        /// </returns>
        public StringValidator IsDate()
        {
            IsDate(ValidatorObj.LookupLanguageString("string_IsDate", NegateNextValidationResult));
            return this;
        }

        /// ********************************************************************
        /// <summary>
        /// Checks if the string can be parsed as an integer.
        /// </summary>
        /// <param name="ErrorMessage"></param>
        /// <returns>
        /// My instance to allow me to chain multiple validations together
        /// </returns>
        public StringValidator IsInteger(string ErrorMessage)
        {
            int tmp;
            SetResult(!int.TryParse(Value, out tmp), string.Format(ErrorMessage, FieldName, Value.ToString()), ValidationErrorCode.StringIsInteger);
            return this;
        }

        /// <summary>
        /// Checks if the string can be parsed as an integer.
        /// </summary>
        /// <returns>
        /// My instance to allow me to chain multiple validations together
        /// </returns>
        public StringValidator IsInteger()
        {
            IsInteger(ValidatorObj.LookupLanguageString("string_IsInteger", NegateNextValidationResult));
            return this;
        }

        /// ********************************************************************
        /// <summary>
        /// Checks if the string can be parsed as a deciaml under the current locale.
        /// </summary>
        /// <param name="ErrorMessage"></param>
        /// <returns>
        /// My instance to allow me to chain multiple validations together
        /// </returns>
        public StringValidator IsDecimal(string ErrorMessage)
        {
            decimal tmp;
            SetResult(!decimal.TryParse(Value, out tmp), string.Format(ErrorMessage, FieldName, Value.ToString()), ValidationErrorCode.StringIsDecimal);
            return this;
        }

        /// <summary>
        /// Checks if the string can be parsed as a deciaml under the current locale.
        /// </summary>
        /// <returns>
        /// My instance to allow me to chain multiple validations together
        /// </returns>
        public StringValidator IsDecimal()
        {
            IsDecimal(ValidatorObj.LookupLanguageString("string_IsDecimal", NegateNextValidationResult));
            return this;
        }

        /// ********************************************************************
        /// <summary>
        /// Checks that the length of the string is within the provided minimum
        /// and maximum length limits.
        /// </summary>
        /// <param name="MinLength"></param>
        /// <param name="MaxLength"></param>
        /// <param name="ErrorMessage"></param>
        /// <returns>
        /// My instance to allow me to chain multiple validations together
        /// </returns>
        public StringValidator HasALengthBetween(int MinLength, int MaxLength, string ErrorMessage)
        {
            SetResult(Value.Length < MinLength || Value.Length > MaxLength, string.Format(ErrorMessage, FieldName, MinLength, MaxLength), ValidationErrorCode.StringHasALengthBetween);
            return this;
        }

        /// <summary>
        /// Checks that the length of the string is within the provided minimum
        /// and maximum length limits.
        /// </summary>
        /// <param name="MinLength"></param>
        /// <param name="MaxLength"></param>
        /// <returns>
        /// My instance to allow me to chain multiple validations together
        /// </returns>
        public StringValidator HasALengthBetween(int MinLength, int MaxLength)
        {
            HasALengthBetween(MinLength, MaxLength, ValidatorObj.LookupLanguageString("string_HasALengthBetween", NegateNextValidationResult));
            return this;
        }

        /// ********************************************************************
        /// <summary>
        /// Checks that the string starts with StartValue.
        /// </summary>
        /// <param name="StartValue"></param>
        /// <param name="ErrorMessage"></param>
        /// <returns>
        /// My instance to allow me to chain multiple validations together
        /// </returns>
        public StringValidator StartsWith(string StartValue, string ErrorMessage)
        {
            SetResult(!Value.StartsWith(StartValue), string.Format(ErrorMessage, FieldName, StartValue), ValidationErrorCode.StringStartsWith);
            return this;
        }

        /// <summary>
        /// Checks that the string starts with StartValue.
        /// </summary>
        /// <param name="StartValue"></param>
        /// <returns>
        /// My instance to allow me to chain multiple validations together
        /// </returns>
        public StringValidator StartsWith(string StartValue)
        {
            StartsWith(StartValue, ValidatorObj.LookupLanguageString("string_StartsWith", NegateNextValidationResult));
            return this;
        }

        /// ********************************************************************
        /// <summary>
        /// Checks that the string ends with EndValue.
        /// </summary>
        /// <param name="EndValue"></param>
        /// <param name="ErrorMessage"></param>
        /// <returns>
        /// My instance to allow me to chain multiple validations together
        /// </returns>
        public StringValidator EndsWith(string EndValue, string ErrorMessage)
        {
            SetResult(!Value.EndsWith(EndValue), string.Format(ErrorMessage, FieldName, EndValue), ValidationErrorCode.StringEndsWith);
            return this;
        }

        /// <summary>
        /// Checks that the string ends with EndValue.
        /// </summary>
        /// <param name="EndValue"></param>
        /// <returns>
        /// My instance to allow me to chain multiple validations together
        /// </returns>
        public StringValidator EndsWith(string EndValue)
        {
            EndsWith(EndValue, ValidatorObj.LookupLanguageString("string_EndsWith", NegateNextValidationResult));
            return this;
        }

        /// ********************************************************************
        /// <summary>
        /// Checks that the string contains the specified CompareValue.
        /// </summary>
        /// <param name="CompareValue"></param>
        /// <param name="ErrorMessage"></param>
        /// <returns>
        /// My instance to allow me to chain multiple validations together
        /// </returns>
        public StringValidator Contains(string CompareValue, string ErrorMessage)
        {
            SetResult(!Value.Contains(CompareValue), string.Format(ErrorMessage, FieldName, CompareValue), ValidationErrorCode.StringContains);
            return this;
        }

        /// <summary>
        /// Checks that the string contains the specified CompareValue.
        /// </summary>
        /// <param name="CompareValue"></param>
        /// <returns>
        /// My instance to allow me to chain multiple validations together
        /// </returns>
        public StringValidator Contains(string CompareValue)
        {
            Contains(CompareValue, ValidatorObj.LookupLanguageString("string_Contains", NegateNextValidationResult));
            return this;
        }


        /// <summary>
        /// Checks if the string is a valid credit card.
        /// </summary>
        /// <returns>My instance to allow me to chain multiple validations together</returns>
        public StringValidator IsCreditCard()
        {
            IsCreditCard(ValidatorObj.LookupLanguageString("string_IsCreditCard", NegateNextValidationResult));
            return this;
        }

        /// ********************************************************************
        /// <summary>
        /// Checks if the string is a valid credit card.
        /// </summary>
        /// <param name="ErrorMessage"></param>
        /// <returns>My instance to allow me to chain multiple validations together</returns>
        public StringValidator IsCreditCard(string ErrorMessage)
        {
            // Allow only digits
            const string allowed = "0123456789";
            int i;
            // Clean the input
            StringBuilder cleanNumber = new StringBuilder();
            for (i = 0; i < Value.Length; i++)
            {
                if (allowed.IndexOf(Value.Substring(i, 1)) >= 0)
                    cleanNumber.Append(Value.Substring(i, 1));
            }
            // Credit card length must be greater than 13 and smaller than 16
            if (cleanNumber.Length < 13 || cleanNumber.Length > 16)
            {
                SetResult(true, string.Format(ErrorMessage, FieldName), ValidationErrorCode.StringIsCreditCard);
                return this;
            }
            

            for (i = cleanNumber.Length + 1; i <= 16; i++)
                cleanNumber.Insert(0, "0");

            int multiplier, digit, sum, total = 0;
            string number = cleanNumber.ToString();

            for (i = 1; i <= 16; i++)
            {
                multiplier = 1 + (i % 2);
                digit = int.Parse(number.Substring(i - 1, 1));
                sum = digit * multiplier;
                if (sum > 9)
                    sum -= 9;
                total += sum;
            }
          
            /*SET RESULT*/
            SetResult(!(total % 10 == 0), string.Format(ErrorMessage, FieldName), ValidationErrorCode.StringIsCreditCard);
            return this;
        }

    }
}
