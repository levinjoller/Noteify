using Noteify.Web.Services.Interfaces;

namespace Noteify.Web.Services
{
    public class ErrorService
    {
        public static string GetErrorMessage(int statusCode, string customErrorMessage)
        {
            if (!string.IsNullOrEmpty(customErrorMessage))
            {
                return customErrorMessage;
            }

            return statusCode switch
            {
                401 => "Anfrage wurde Aufgrund fehlender Autentifizierung abgelehnt.",
                403 or 405 => "Sie sind nicht authorisiert auf diese Ressource zuzugriffen.",
                404 => "Die angefragte Seite existiert nicht, überprüfen Sie die URL.",
                500 => "Ein interner Server Fehler ist aufgetreten.",
                _ => ""
            };
        }
    }
}