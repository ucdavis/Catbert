using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace CAESDO.Catbert.Core.Domain
{
    public static class ValidateBO<T>
    {
        public static bool isValid(T obj)
        {
            return Validation.Validate<T>(obj).IsValid;
        }

        public static ValidationResults GetValidationResults(T obj)
        {
            return Validation.Validate<T>(obj);
        }

        public static string GetValidationResultsAsString(T obj)
        {
            StringBuilder ErrorString = new StringBuilder();

            foreach (ValidationResult r in GetValidationResults(obj))
            {
                ErrorString.AppendLine(string.Format("{0}, {1}", r.Key, r.Message));
            }

            return ErrorString.ToString();
        }
    }
}
