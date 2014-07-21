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

namespace BigfootDNN.Model.Validation
{
    /// ********************************************************************
    /// <summary>
    /// Date validation handlers.
    /// </summary>
    public class DateValidator : ValidatorBase<DateValidator, DateTime>
    {
        /// ********************************************************************
        /// <summary>
        /// Initializes a new instance of the <see cref="DateValidator"/> class. 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="fieldName"></param>
        /// <param name="validatorObj"></param>
        public DateValidator(DateTime value, string fieldName, Validator validatorObj) : base(value, fieldName, validatorObj)
        {
        }

        /// *****************************************************************
        /// <summary>
        /// Checks that the date provided is not in the future (has a date
        /// part that is later than today).
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns>My instance to allow me to chain multiple validations together</returns>
        public DateValidator IsNotAFutureDate(string errorMessage)
        {
            SetResult(Value > DateTime.Now, string.Format(errorMessage, FieldName), ValidationErrorCode.DateIsNotAFutureDate);
            return this;
        }

        /// <summary>
        /// Checks that the date provided is not in the future (has a date
        /// part that is not later than today).
        /// </summary>
        /// <returns>My instance to allow me to chain multiple validations together</returns>
        public DateValidator IsNotAFutureDate()
        {
            IsNotAFutureDate(ValidatorObj.LookupLanguageString("date_IsNotAFutureDate", NegateNextValidationResult));
            return this;
        }

        /// *****************************************************************
        /// <summary>
        /// Checks that the date provided is not in the past (has a date part
        /// that is not earlier than today).
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns>My instance to allow me to chain multiple validations together</returns>
        public DateValidator IsNotAPastDate(string errorMessage)
        {
            SetResult(Value < DateTime.Now, string.Format(errorMessage, FieldName), ValidationErrorCode.DateIsNotAPastDate);
            return this;
        }

        /// <summary>
        /// Checks that the date provided is not in the past (has a date part
        /// that is not earlier than today).
        /// </summary>
        /// <returns>My instance to allow me to chain multiple validations together</returns>
        public DateValidator IsNotAPastDate()
        {
            IsNotAPastDate(ValidatorObj.LookupLanguageString("date_IsNotAPastDate", NegateNextValidationResult));
            return this;
        }

        /// *****************************************************************
        /// <summary>
        /// Checks that the date is not DateTime.Min or DateTime.Max.
        /// </summary>
        /// <param name="ErrorMessage"></param>
        /// <returns>My instance to allow me to chain multiple validations together</returns>
        public DateValidator IsNotMinMaxValue(string ErrorMessage)
        {
            SetResult((Value == DateTime.MinValue || Value == DateTime.MaxValue), string.Format(ErrorMessage, FieldName), ValidationErrorCode.DateIsNotMinMaxValue);
            return this;
        }

        /// <summary>
        /// Checks that the date is not DateTime.Min or DateTime.Max.
        /// </summary>
        /// <returns>My instance to allow me to chain multiple validations together</returns>
        public DateValidator IsNotMinMaxValue()
        {
            IsNotMinMaxValue(ValidatorObj.LookupLanguageString("date_IsNotMinMaxValue", NegateNextValidationResult));
            return this;
        }

        /// *****************************************************************
        /// <summary>
        /// Checks that the date is ealier than the provided check date.
        /// </summary>
        /// <param name="CheckDateValue"></param>
        /// <param name="ErrorMessage"></param>
        /// <returns>My instance to allow me to chain multiple validations together</returns>
        public DateValidator IsEarlierThan(DateTime CheckDateValue, string ErrorMessage)
        {
            SetResult(Value >= CheckDateValue, string.Format(ErrorMessage, FieldName, CheckDateValue), ValidationErrorCode.DateIsEarlierThan);
            return this;
        }

        /// <summary>
        /// Checks that the date is ealier than the provided check date.
        /// </summary>
        /// <param name="CheckDateValue"></param>
        /// <returns>My instance to allow me to chain multiple validations together</returns>
        public DateValidator IsEarlierThan(DateTime CheckDateValue)
        {
            IsEarlierThan(CheckDateValue, ValidatorObj.LookupLanguageString("date_IsEarlierThan", NegateNextValidationResult));
            return this;
        }

        /// *****************************************************************
        /// <summary>
        /// Checks that the date is later than the provided check date.
        /// </summary>
        /// <param name="CheckDateValue"></param>
        /// <param name="ErrorMessage"></param>
        /// <returns>My instance to allow me to chain multiple validations together</returns>
        public DateValidator IsLaterThan(DateTime CheckDateValue, string ErrorMessage)
        {
            SetResult(Value <= CheckDateValue, string.Format(ErrorMessage, FieldName, CheckDateValue), ValidationErrorCode.DateIsLaterThan);
            return this;
        }

        /// <summary>
        /// Checks that the date is later than the provided check date.
        /// </summary>
        /// <param name="CheckDateValue"></param>
        /// <returns>My instance to allow me to chain multiple validations together</returns>
        public DateValidator IsLaterThan(DateTime CheckDateValue)
        {
            IsLaterThan(CheckDateValue, ValidatorObj.LookupLanguageString("date_IsLaterThan", NegateNextValidationResult));
            return this;
        }
    }
}