using System;
using System.Web;

namespace BigfootDNN
{
    /// <summary>
    /// This is the viewmodel used for error view
    /// </summary>
    public class ErrorViewModel
    {

        public string ErrorMessage { get; set; }
        public string ErrorDetail { get; set; }
        public string ReturnUrl { get; set; }

        public static ErrorViewModel Create(Exception ex, string ReturnUrl)
        {
            var msg = "";
            var detail = "";
            if (ex != null)
            {
                if (ex.InnerException != null)
                {
                    msg = ex.InnerException.Message;
                    detail = ex.InnerException.ToString();
                }
                else
                {
                    msg = ex.Message;
                    detail = ex.ToString();
                }
            }
            else
            {
                msg = "An unknown error has ocurred!";
            }
            return Create(msg, detail, ReturnUrl);
        }

        public static ErrorViewModel Create(string ErrorMessage, string ErrorDetail, string ReturnUrl)
        {
            var co = new ErrorViewModel
            {
                ErrorMessage = HttpUtility.HtmlEncode(ErrorMessage)
            };

            // encode and replace the return char with <br /> for display
            ErrorDetail = HttpUtility.HtmlEncode(ErrorDetail);
            ErrorDetail = ErrorDetail.Replace(Environment.NewLine, "<br />");

            co.ErrorDetail = ErrorDetail;
            co.ReturnUrl = ReturnUrl;
            return co;
        }

    }

}
