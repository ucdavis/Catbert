using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAESDO.Catbert.Core.Domain;
using CAESDO.Catbert.Data;

namespace CAESDO.Catbert.BLL
{
    public class UnitAssociationBLL : GenericBLL<UnitAssociation, int>
    {
        public static bool AssociationExists(string login, string application, string unitFIS)
        {
            return UnitAssociationBLL.Queryable.Where(assoc => assoc.User.LoginID == login
                && assoc.Unit.FISCode == unitFIS
                && assoc.Application.Name == application).Any();
        }

        public static UnitAssociation GetUnitAssociation(string login, string application, string unitFIS)
        {
            return UnitAssociationBLL.Queryable.Where(assoc => assoc.User.LoginID == login
                && assoc.Unit.FISCode == unitFIS
                && assoc.Application.Name == application).SingleOrDefault();
        }

        /// <summary>
        /// Associate the unit identified by unitFIS -- only within the context of the given application 
        /// </summary>
        public static bool AssociateUnit(string login, string application, string unitFIS, string trackingUserName)
        {
            //Get the user and unit and application and make sure they exist
            User user = UserBLL.GetUser(login);
            Unit unit = UnitBLL.GetByFIS(unitFIS);
            Application app = ApplicationBLL.GetByName(application);

            if (user == null || unit == null || app == null) return false;

            //Get the association between this unit and this user in this application
            var unitAssociation = UnitAssociationBLL.GetUnitAssociation(login, application, unitFIS);

            //if the association doesn't exist, create it
            if (unitAssociation == null)
            {
                unitAssociation = new UnitAssociation() { Application = app, Unit = unit, User = user, Inactive = false };
            }
            else if (unitAssociation.Inactive == true) //if the association exists and is inactive, activate it
            {
                unitAssociation.Inactive = false;
            }
            else  //the association exists and is already active -- so ignore and return
            {
                return false;
            }

            Tracking tracking = TrackingBLL.GetTrackingInstance(trackingUserName, TrackingTypes.User, TrackingActions.Change);
            tracking.Comments = string.Format("Unit {0} associated with user {1}", unit.ID, user.ID);

            using (var ts = new TransactionScope())
            {
                EnsurePersistent( unitAssociation);  //persist the unitAssociation
                TrackingBLL.EnsurePersistent( tracking);

                ts.CommittTransaction();
            }

            return true;
        }

        /// <summary>
        /// Unassociate the identified unit
        /// </summary>
        public static bool UnassociateUnit(string login, string application, string unitFIS, string trackingUserName)
        {
            //Get the user and unit and make sure they exist
            User user = UserBLL.GetUser(login);
            Unit unit = UnitBLL.GetByFIS(unitFIS);
            Application app = ApplicationBLL.GetByName(application);

            if (user == null || unit == null || app == null) return false;

            //Get the association between this unit and this user in this application
            var unitAssociation = GetUnitAssociation(login, application, unitFIS);

            if (unitAssociation == null)
            {
                return false; //If there isn't an association, return false
            }
            else if (unitAssociation.Inactive == false) //If there is an association and it is active, set it inactive
            {
                unitAssociation.Inactive = true;
            }
            else //If there is an association and it is already inactive, we don't need to do anything
            {
                return false;
            }

            Tracking tracking = TrackingBLL.GetTrackingInstance(trackingUserName, TrackingTypes.User, TrackingActions.Change);
            tracking.Comments = string.Format("Unit {0} unassociated from user {1}", unit.ID, user.ID);

            using (var ts = new TransactionScope())
            {
                EnsurePersistent(unitAssociation);
                TrackingBLL.EnsurePersistent(tracking);

                ts.CommittTransaction();
            }

            return true;
        }
    }
}
