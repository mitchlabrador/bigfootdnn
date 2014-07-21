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
using System.Collections.Generic;

namespace BigfootDNN.Model.Validation
{
    /// **************************************************************************
    /// <summary>
    /// The main class for the validation library. Create an instance of this per
    /// set of data that you wish to validate. This class is not threadsafe.
    /// </summary>
    public class Validator
    {
        
        /// <summary>
        /// Collection of our validation results.
        /// </summary>
        private readonly IList<ValidatorResult> validatorResults = new List<ValidatorResult>();

        /// *****************************************************************
        /// <summary>
        /// Default constructor; sets us to collect one error per field and
        /// generate error messages in English.
        /// </summary>
        public Validator()
        {
            Mode = ErrorMode.OneErrorPerField;
        }

        /// *****************************************************************
        /// <summary>
        /// Constructor for setting both language of generated error messages
        /// and error collection mode.
        /// </summary>
        /// <param name="validationLanguage"></param>
        /// <param name="mode"></param>
        public Validator(ErrorMode mode)
        {
            Mode = mode;
        }

        /// ******************************************************************
        /// <summary>
        /// Return a list of the validation results.
        /// </summary>
        public IList<ValidatorResult> ValidatorResults
        {
            get { return validatorResults; }
        }

        /// <summary>
        /// Indicates if we should only collect one error per field name.
        /// </summary>
        public ErrorMode Mode { get; set; }

        /// ******************************************************************
        /// <summary>
        /// Clear the validation results and start over.
        /// </summary>
        public void Clear()
        {
            ValidatorResults.Clear();
        }

        /// ******************************************************************
        /// <summary>
        /// Get the number of validation errors/failures.
        /// </summary>
        /// <returns></returns>
        public int ErrorCount()
        {
            var count = 0;
            foreach (var result in validatorResults)
            {
                if (result.Level == ValidatorResultLevel.Error)
                    count++;
            }
            return count;
        }

        /// ******************************************************************
        /// <summary>
        /// Get the number of validation warnings.
        /// </summary>
        /// <returns></returns>
        public int WarningCount()
        {
            var count = 0;
            foreach (var result in validatorResults)
            {
                if (result.Level == ValidatorResultLevel.Warning)
                    count++;
            }
            return count;
        }

        /// ******************************************************************
        /// <summary>
        /// Check if we have any validation errors.
        /// </summary>
        /// <returns>False if no errors, true if we have errors</returns>
        public bool HasErrors()
        {
            return ErrorCount() != 0;
        }

        /// ******************************************************************
        /// <summary>
        /// Check if we have any validation warnings.
        /// </summary>
        /// <returns>False if no warnings, true if we have warnings</returns>
        public bool HasWarnings()
        {
            return WarningCount() != 0;
        }

        /// ******************************************************************
        /// <summary>
        /// Start to validate an int.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public NumericValidator<int> That(int value, string fieldName)
        {
            return new NumericValidator<int>(value, fieldName, this);
        }

        /// ******************************************************************
        /// <summary>
        /// Start to validate an uint.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public NumericValidator<uint> That(uint value, string fieldName)
        {
            return new NumericValidator<uint>(value, fieldName, this);
        }

        /// ******************************************************************
        /// <summary>
        /// Start to validate a short.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public NumericValidator<short> That(short value, string fieldName)
        {
            return new NumericValidator<short>(value, fieldName, this);
        }

        /// ******************************************************************
        /// <summary>
        /// Start to validate a ushort.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public NumericValidator<ushort> That(ushort value, string fieldName)
        {
            return new NumericValidator<ushort>(value, fieldName, this);
        }

        /// ******************************************************************
        /// <summary>
        /// Start to validate a long.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public NumericValidator<long> That(long value, string fieldName)
        {
            return new NumericValidator<long>(value, fieldName, this);
        }

        /// ******************************************************************
        /// <summary>
        /// Start to validate an ulong.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public NumericValidator<ulong> That(ulong value, string fieldName)
        {
            return new NumericValidator<ulong>(value, fieldName, this);
        }
        
        /// ******************************************************************
        /// <summary>
        /// Start to validate a byte.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public NumericValidator<byte> That(byte value, string fieldName)
        {
            return new NumericValidator<byte>(value, fieldName, this);
        }

        /// ******************************************************************
        /// <summary>
        /// Start to validate an sbyte.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public NumericValidator<sbyte> That(sbyte value, string fieldName)
        {
            return new NumericValidator<sbyte>(value, fieldName, this);
        }

        /// ******************************************************************
        /// <summary>
        /// Start to validate a decimal.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public NumericValidator<decimal> That(decimal value, string fieldName)
        {
            return new NumericValidator<decimal>(value, fieldName, this);
        }

        /// ******************************************************************
        /// <summary>
        /// Start to validate a float.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public NumericValidator<float> That(float value, string fieldName)
        {
            return new NumericValidator<float>(value, fieldName, this);
        }

        /// ******************************************************************
        /// <summary>
        /// Start to validate a double.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public NumericValidator<double> That(double value, string fieldName)
        {
            return new NumericValidator<double>(value, fieldName, this);
        }

        /// ******************************************************************
        /// <summary>
        /// Start to validate a string.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public StringValidator That(string value, string fieldName)
        {
            return new StringValidator(value, fieldName, this);
        }

        /// ******************************************************************
        /// <summary>
        /// Start to validate a DateTime.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public DateValidator That(DateTime value, string fieldName)
        {
            return new DateValidator(value, fieldName, this);
        }

        /// ***********************************************************
        /// <summary>
        /// Add a new validation error to the list of validation errors
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="FieldName"></param>
        /// <param name="errorCode"></param>
        public void AddValidationError(string Message, string FieldName, int errorCode)
        {
            AddValidationError(Message, FieldName, ValidatorResultLevel.Error, errorCode);
        }

        /// ***********************************************************
        /// <summary>
        /// Add a new validation error to the list of validation errors
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="FieldName"></param>
        /// <param name="Level"></param>
        /// <param name="errorCode"></param>
        public void AddValidationError(string Message, string FieldName, ValidatorResultLevel Level, int errorCode)
        {
            // Should we only allow one error per fieldname?
            if (Mode == ErrorMode.OneErrorPerField)
            {
                // Check if an error for this fieldname already exists
                foreach (var Error in ValidatorResults)
                    if (Error.FieldName == FieldName)
                        return;
            }

            // If we get here, add the new item.
            ValidatorResults.Add(new ValidatorResult(Message, FieldName, Level, errorCode));
        }


        /// *********************************************************************
        /// <summary>
        /// Looks up a string in the internationalization dictionary.
        /// </summary>
        /// <param name="KeyName"></param>
        /// <param name="Negated"></param>
        /// <returns></returns>
        internal string LookupLanguageString(string KeyName, bool Negated)
        {
            return LangCache.FetchItem(Negated ? "not_" + KeyName : KeyName);
        }

    }
}
