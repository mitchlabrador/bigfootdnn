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
    /// Numeric validation handlers.
    /// </summary>
    /// <typeparam name="TValue">
    /// </typeparam>
    public class NumericValidator<TValue> : ValidatorBase<NumericValidator<TValue>, TValue> where TValue : struct, IComparable<TValue>, IEquatable<TValue>
    {
        /// ********************************************************************
        /// <summary>
        /// Initializes a new instance of the <see cref="NumericValidator"/> class.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="fieldName"></param>
        /// <param name="validatorObj"></param>
        public NumericValidator(TValue value, string fieldName, Validator validatorObj) : base(value, fieldName, validatorObj)
        {
        }

        /// ********************************************************************
        /// <summary>
        /// Checks that the value is less than or equal to the provided value.
        /// </summary>
        /// <param name="lessThanValue"></param>
        /// <param name="ErrorMessage"></param>
        /// <returns>My instance to allow me to chain multiple validations together</returns>
        public NumericValidator<TValue> IsLessThanOrEqual(TValue lessThanValue, string ErrorMessage)
        {
            SetResult(Value.CompareTo(lessThanValue) > 0, string.Format(ErrorMessage, FieldName, lessThanValue.ToString()), ValidationErrorCode.NumericIsLessThanOrEqual);
            return this;
        }

        /// <summary>
        /// Checks that the value is less than or equal to the provided value.
        /// </summary>
        /// <param name="lessThanValue"></param>
        /// <returns>My instance to allow me to chain multiple validations together</returns>
        public NumericValidator<TValue> IsLessThanOrEqual(TValue lessThanValue)
        {
            IsLessThanOrEqual(lessThanValue, ValidatorObj.LookupLanguageString("int_IsLessThanOrEqual", NegateNextValidationResult));
            return this;
        }

        /// ********************************************************************
        /// <summary>
        /// Checks that the value is greater than or equal to the provided value.
        /// </summary>
        /// <param name="GreaterThanValue"></param>
        /// <param name="ErrorMessage"></param>
        /// <returns>My instance to allow me to chain multiple validations together</returns>
        public NumericValidator<TValue> IsGreaterThanOrEqual(TValue GreaterThanValue, string ErrorMessage)
        {
            SetResult(Value.CompareTo(GreaterThanValue) < 0, string.Format(ErrorMessage, FieldName, GreaterThanValue.ToString()), ValidationErrorCode.NumericIsGreaterThanOrEqual);
            return this;
        }

        /// <summary>
        /// Checks that the value is greater than or equal to the provided value.
        /// </summary>
        /// <param name="GreaterThanValue"></param>
        /// <returns>My instance to allow me to chain multiple validations together</returns>
        public NumericValidator<TValue> IsGreaterThanOrEqual(TValue GreaterThanValue)
        {
            IsGreaterThanOrEqual(GreaterThanValue, ValidatorObj.LookupLanguageString("int_IsGreaterThanOrEqual", NegateNextValidationResult));
            return this;
        }

        /// ********************************************************************
        /// <summary>
        /// Checks that the value is greater than the provided value.
        /// </summary>
        /// <param name="GreaterThanValue"></param>
        /// <param name="ErrorMessage"></param>
        /// <returns>My instance to allow me to chain multiple validations together</returns>
        public NumericValidator<TValue> IsGreaterThan(TValue GreaterThanValue, string ErrorMessage)
        {
            SetResult(Value.CompareTo(GreaterThanValue) <= 0, string.Format(ErrorMessage, FieldName, GreaterThanValue.ToString()), ValidationErrorCode.NumericIsGreaterThan);
            return this;
        }       
        
        /// <summary>
        /// Checks that the value is greater than the provided value.
        /// </summary>
        /// <param name="GreaterThanValue"></param>
        /// <returns>My instance to allow me to chain multiple validations together</returns>
        public NumericValidator<TValue> IsGreaterThan(TValue GreaterThanValue)
        {
            IsGreaterThan(GreaterThanValue, ValidatorObj.LookupLanguageString("int_IsGreaterThan", NegateNextValidationResult));
            return this;
        }

        /// ********************************************************************
        /// <summary>
        /// Checks that the value is less than the provided value.
        /// </summary>
        /// <param name="LessThanValue"></param>
        /// <param name="ErrorMessage"></param>
        /// <returns>My instance to allow me to chain multiple validations together</returns>
        public NumericValidator<TValue> IsLessThan(TValue LessThanValue, string ErrorMessage)
        {
            SetResult(Value.CompareTo(LessThanValue) >= 0, string.Format(ErrorMessage, FieldName, LessThanValue.ToString()), ValidationErrorCode.NumericIsLessThan);
            return this;
        }

        /// <summary>
        /// Checks that the value is less than the provided value.
        /// </summary>
        /// <param name="LessThanValue"></param>
        /// <returns>My instance to allow me to chain multiple validations together</returns>
        public NumericValidator<TValue> IsLessThan(TValue LessThanValue)
        {
            IsLessThan(LessThanValue, ValidatorObj.LookupLanguageString("int_IsLessThan", NegateNextValidationResult));
            return this;
        }

        /// ********************************************************************
        /// <summary>
        /// Checks that the value is the same as the provided value.
        /// </summary>
        /// <param name="EqualValue"></param>
        /// <param name="ErrorMessage"></param>
        /// <returns>My instance to allow me to chain multiple validations together</returns>
        public NumericValidator<TValue> Equals(TValue EqualValue, string ErrorMessage)
        {
            SetResult(!Value.Equals(EqualValue), string.Format(ErrorMessage, FieldName, EqualValue.ToString()), ValidationErrorCode.NumericEquals);
            return this;
        }

        /// <summary>
        /// Checks that the value is the same as the provided value.
        /// </summary>
        /// <param name="EqualValue"></param>
        /// <returns>My instance to allow me to chain multiple validations together</returns>
        public NumericValidator<TValue> Equals(TValue EqualValue)
        {
            Equals(EqualValue, ValidatorObj.LookupLanguageString("int_Equals", NegateNextValidationResult));
            return this;
        }

        /// ********************************************************************
        /// <summary>
        /// Checks that the value is within the provided range.
        /// </summary>
        /// <param name="StartValue"></param>
        /// <param name="EndValue"></param>
        /// <param name="ErrorMessage"></param>
        /// <returns>My instance to allow me to chain multiple validations together</returns>
        public NumericValidator<TValue> Between(TValue StartValue, TValue EndValue, string ErrorMessage)
        {
            SetResult((Value.CompareTo(StartValue) < 0 || Value.CompareTo(EndValue) > 0), string.Format(ErrorMessage, FieldName, StartValue.ToString(), EndValue.ToString()), ValidationErrorCode.NumericBetween);
            return this;
        }        
        
        /// <summary>
        /// Checks that the value is within the provided range.
        /// </summary>
        /// <param name="StartValue"></param>
        /// <param name="EndValue"></param>
        /// <returns>My instance to allow me to chain multiple validations together</returns>
        public NumericValidator<TValue> Between(TValue StartValue, TValue EndValue)
        {
            Between(StartValue, EndValue, ValidatorObj.LookupLanguageString("int_Between", NegateNextValidationResult));
            return this;
        }

        /// ********************************************************************
        /// <summary>
        /// Checks that the value is zero.
        /// </summary>
        /// <param name="ErrorMessage"></param>
        /// <returns>My instance to allow me to chain multiple validations together</returns>
        public NumericValidator<TValue> IsZero(string ErrorMessage)
        {
            SetResult(!Value.Equals(new TValue()), string.Format(ErrorMessage, FieldName), ValidationErrorCode.NumericIsZero);
            return this;
        }
        
        /// <summary>
        /// Checks that the value is zero.
        /// </summary>
        /// <returns>My instance to allow me to chain multiple validations together</returns>
        public NumericValidator<TValue> IsZero()
        {
            IsZero(ValidatorObj.LookupLanguageString("int_IsZero", NegateNextValidationResult));
            return this;
        }
    }
}