using System.Text.RegularExpressions;

namespace NutriSoftwareV2.Web.Svc
{
    public class Utils
    {
        public static string RemoverMascara(string? pParametro) {
            string pattern = @"\D";

            if (string.IsNullOrWhiteSpace(pParametro))
                return pParametro;

            return Regex.Replace(pParametro, pattern, "");
        }
    }
}
