using System;

namespace tobin.GusessCsvFieldsAndtypesGen
{
    internal class DeterimeTypes
    {
        public static GTypes DetermineTypeHelper(GTypes previousType, string value)
        {
            if (previousType == GTypes.String)
            {
                return previousType;
            }

            GTypes testType;

            if (DateTime.TryParse(value, out _))
            {
                testType = GTypes.Date;
            }

            else if (int.TryParse(value, out _))
            {
                testType = GTypes.INT;
            }
            else if (long.TryParse(value, out _))
            {
                testType = GTypes.LONG;
            }

            else if (decimal.TryParse(value, out _))
            {
                testType = GTypes.DECIMAL;
            }

            else
            {
                testType = GTypes.String;
            }

            return (int) testType > (int) previousType ? testType : previousType;
        }
    }
}