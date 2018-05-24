using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fault_handling_system.Models;

namespace Fault_handling_system.Services
{
    public class ReportParsingProgress
    {
        public bool hasRfaId = false;
        public bool hasZoneId = false;
        public bool hasEtrTypeId = false;
        public bool hasEtrStatusId = false;

        public bool HasEverything()
        {
            return hasRfaId
             && hasZoneId
             && hasEtrTypeId
             && hasEtrStatusId;
        }

        public string MissingFields()
        {
            string missingFields = "";
            if (!hasRfaId)
                missingFields += "RfaId ";
            if (!hasZoneId)
                missingFields += "Zone ";
            if (!hasEtrTypeId)
                missingFields += "EtrType ";
            if (!hasEtrStatusId)
                missingFields += "EtrStatus ";
            return missingFields;
        }
    }
}
