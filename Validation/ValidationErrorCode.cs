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
    /// <summary>
    /// Default error codes for the various validation failures that we can get.
    /// </summary>
   public static class ValidationErrorCode
   {
      public const int StringIsEmpty = 101;
      public const int StringIsLength = 102;
      public const int StringIsLongerThan = 103;
      public const int StringIsShorterThan = 104;
      public const int StringMatchRegex = 105;
      public const int StringIsEmail = 106;
      public const int StringIsURL = 107;
      public const int StringIsDate = 108;
      public const int StringIsInteger = 109;
      public const int StringIsDecimal = 110;
      public const int StringHasALengthBetween = 111;
      public const int StringStartsWith = 112;
      public const int StringEndsWith = 113;
      public const int StringContains = 114;
      public const int StringIsCreditCard = 115;

      public const int NumericIsLessThanOrEqual = 201;
      public const int NumericIsGreaterThanOrEqual = 202;
      public const int NumericIsGreaterThan = 203;
      public const int NumericIsLessThan = 204;
      public const int NumericEquals = 205;
      public const int NumericBetween = 206;
      public const int NumericIsZero = 207;

      public const int DateIsNotAFutureDate = 301;
      public const int DateIsNotAPastDate = 302;
      public const int DateIsNotMinMaxValue = 303;
      public const int DateIsEarlierThan = 304;
      public const int DateIsLaterThan = 305;
   }
}
﻿