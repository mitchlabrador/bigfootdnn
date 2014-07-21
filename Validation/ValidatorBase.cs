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

namespace BigfootDNN.Model.Validation
{
    /// ********************************************************************
    /// <summary>
    /// This class serves as the base for a creating a validator for a
    /// particular data type. It provides various common utilities.
    /// </summary>
    /// <typeparam name="TValue">
    /// </typeparam>
    public abstract class ValidatorBase<TValidator, TValue> where TValidator : ValidatorBase<TValidator, TValue>
    {
        /// ********************************************************************
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="fieldName"></param>
        /// <param name="validatorObj"></param>
        protected ValidatorBase(TValue value, string fieldName, Validator validatorObj)
        {
            Value = value;
            FieldName = fieldName;
            ValidatorObj = validatorObj;
        }

        /// ********************************************************************
        /// <summary>
        /// return the value property
        /// </summary>
        public TValue Value { get; set; }

        /// <summary>
        /// The name of the field being tested
        /// </summary>
        public string FieldName { get; private set; }

        /// <summary>
        /// Reference to our validator caller class.
        /// </summary>
        protected Validator ValidatorObj { get; set; }

        /// <summary>
        /// TRUE if the next validation operation should be negated
        /// </summary>
        protected bool NegateNextValidationResult { get; set; }

        /// <summary>
        /// The level of the next failure.
        /// </summary>
        protected ValidatorResultLevel NextFailureResultLevel = ValidatorResultLevel.Error;

        /// <summary>
        /// The code of the next error.
        /// </summary>
        protected int? NextErrorCode = null;

        /// ********************************************************************
        /// <summary>
        /// Set the result of a check.
        /// </summary>
        /// <param name="Result"></param>
        /// <param name="ErrorMsg"></param>
        public void SetResult(bool Result, string ErrorMsg)
        {
            SetResult(Result, ErrorMsg, 0);
        }

        /// ********************************************************************
        /// <summary>
        /// Set the result of a check.
        /// </summary>
        /// <param name="Result"></param>
        /// <param name="ErrorMsg"></param>
        /// <param name="ErrorCode"></param>
        public void SetResult(bool Result, string ErrorMsg, int ErrorCode)
        {
            if (Result ^ NegateNextValidationResult)
                ValidatorObj.AddValidationError(ErrorMsg, FieldName, NextFailureResultLevel,
                    NextErrorCode ?? ErrorCode);

            // Reset the negate flag and warning level.
            NegateNextValidationResult = false;
            NextFailureResultLevel = ValidatorResultLevel.Error;
            NextErrorCode = null;
        }

        /// ********************************************************************
        /// <summary>
        /// Negates the next validation check.
        /// </summary>
        /// <returns></returns>
        public TValidator Not()
        {
            NegateNextValidationResult = true;
            return (TValidator)this;
        }

        /// ********************************************************************
        /// <summary>
        /// Sets the level of the next validation result to just be warning,
        /// rather than error.
        /// </summary>
        /// <returns></returns>
        public TValidator WarnUnless()
        {
            NextFailureResultLevel = ValidatorResultLevel.Warning;
            return (TValidator)this;
        }

        /// ********************************************************************
        /// <summary>
        /// Sets the code for the next validation result.
        /// </summary>
        /// <returns></returns>
        public TValidator WithErrorCode(int code)
        {
           NextErrorCode = code;
           return (TValidator)this;
        }
    }
}
