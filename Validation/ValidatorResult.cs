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
    /// ****************************************************************
    /// <summary>
    /// Represents a validation failutre.
    /// </summary>
    public class ValidatorResult
    {
        /// ******************************************************************
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidatorResult">class</see>. 
        /// </summary>
        /// <param name="ErrorMessage"></param>
        /// <param name="fieldName"></param>
        /// <param name="level"></param>
        /// <param name="errorCode"></param>
        public ValidatorResult(string ErrorMessage, string fieldName, ValidatorResultLevel level, int errorCode)
        {
            ValidationMessage = ErrorMessage;
            FieldName = fieldName;
            Level = level;
            ErrorCode = errorCode;
        }

        /// <summary>
        /// The error message
        /// </summary>
        public string ValidationMessage { get; private set; }

        /// <summary>
        /// The field name that caused the error
        /// </summary>
        public string FieldName { get; private set; }

        /// <summary>
        /// Validation failure level; is it an error or warning?
        /// </summary>
        public ValidatorResultLevel Level { get; private set; }

        /// <summary>
        /// Validation error code, as defined in <see>ErrorCodes</see>.
        /// </summary>
        public int ErrorCode { get; private set; }
    }
}