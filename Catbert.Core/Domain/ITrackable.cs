using System;
using System.Collections.Generic;
using System.Text;

namespace CAESDO.Catbert.Core.Domain
{
    public interface ITrackable
    {
        bool isTracked();

        bool arePropertiesTracked();
    }
}
