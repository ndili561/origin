using System;
using System.Linq;
using MVC5RealWorld.Models.DB;
using MVC5RealWorld.Models.ViewModel;
using System.Collections.Generic;

namespace MVC5RealWorld.Models.EntityManager
{
    public class UserManager
    {
        public void AddUserAccount(UserSignUpView user) {

            using (DemoDBEntities db = new DemoDBEntities()) {

                User SU = new User();
                SU.LoginName = user.LoginName;
                SU.PasswordEncryptedText = user.Password;
                SU.RowCreatedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1;
                SU.RowModifiedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1; ;
                SU.RowCreatedDateTime = DateTime.Now;
                SU.RowMOdifiedDateTime = DateTime.Now;

                db.Users.Add(SU);
                db.SaveChanges();

                SYSUserProfile SUP = new SYSUserProfile();
                SUP.SYSUserID = SU.UserID;
                SUP.FirstName = user.FirstName;
                SUP.LastName = user.LastName;
                SUP.Gender = user.Gender;
                SUP.RowCreatedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1;
                SUP.RowModifiedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1;
                SUP.RowCreatedDateTime = DateTime.Now;
                SUP.RowModifiedDateTime = DateTime.Now;

                db.SYSUserProfiles.Add(SUP);
                db.SaveChanges();


                if (user.LOOKUPRoleID > 0) {
                    SYSUserRole SUR = new SYSUserRole();
                    SUR.LOOKUPRoleID = user.LOOKUPRoleID;
                    SUR.SYSUserID = user.SYSUserID;
                    SUR.IsActive = true;
                    SUR.RowCreatedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1;
                    SUR.RowModifiedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1;
                    SUR.RowCreatedDateTime = DateTime.Now;
                    SUR.RowModifiedDateTime = DateTime.Now;

                    db.SYSUserRoles.Add(SUR);
                    db.SaveChanges();
                }
            }
        }

        public void UpdateUserAccount(UserProfileView user) {

            using (DemoDBEntities db = new DemoDBEntities()) {
                using (var dbContextTransaction = db.Database.BeginTransaction()) {
                    try {

                        User SU = db.Users.Find(user.SYSUserID);
                        SU.LoginName = user.LoginName;
                        SU.PasswordEncryptedText = user.Password;
                        SU.RowCreatedSYSUserID = user.SYSUserID;
                        SU.RowModifiedSYSUserID = user.SYSUserID;
                        SU.RowCreatedDateTime = DateTime.Now;
                        SU.RowMOdifiedDateTime = DateTime.Now;

                        db.SaveChanges();

                        var userProfile = db.SYSUserProfiles.Where(o => o.SYSUserID == user.SYSUserID);
                        if (userProfile.Any()) {
                            SYSUserProfile SUP = userProfile.FirstOrDefault();
                            SUP.SYSUserID = SU.UserID;
                            SUP.FirstName = user.FirstName;
                            SUP.LastName = user.LastName;
                            SUP.Gender = user.Gender;
                            SUP.RowCreatedSYSUserID = user.SYSUserID;
                            SUP.RowModifiedSYSUserID = user.SYSUserID;
                            SUP.RowCreatedDateTime = DateTime.Now;
                            SUP.RowModifiedDateTime = DateTime.Now;

                            db.SaveChanges();
                        }

                        if (user.LOOKUPRoleID > 0) {
                            var userRole = db.SYSUserRoles.Where(o => o.SYSUserID == user.SYSUserID);
                            SYSUserRole SUR = null;
                            if (userRole.Any()) {
                                SUR = userRole.FirstOrDefault();
                                SUR.LOOKUPRoleID = user.LOOKUPRoleID;
                                SUR.SYSUserID = user.SYSUserID;
                                SUR.IsActive = true;
                                SUR.RowCreatedSYSUserID = user.SYSUserID;
                                SUR.RowModifiedSYSUserID = user.SYSUserID;
                                SUR.RowCreatedDateTime = DateTime.Now;
                                SUR.RowModifiedDateTime = DateTime.Now;
                            }
                            else {
                                SUR = new SYSUserRole();
                                SUR.LOOKUPRoleID = user.LOOKUPRoleID;
                                SUR.SYSUserID = user.SYSUserID;
                                SUR.IsActive = true;
                                SUR.RowCreatedSYSUserID = user.SYSUserID;
                                SUR.RowModifiedSYSUserID = user.SYSUserID;
                                SUR.RowCreatedDateTime = DateTime.Now;
                                SUR.RowModifiedDateTime = DateTime.Now;
                                db.SYSUserRoles.Add(SUR);
                            }

                            db.SaveChanges();
                        }
                        dbContextTransaction.Commit();
                    }
                    catch {
                        dbContextTransaction.Rollback();
                    }
                }
            }
        }
        public bool IsLoginNameExist(string loginName) {
            using (DemoDBEntities db = new DemoDBEntities()) {
                return db.Users.Where(o => o.LoginName.Equals(loginName)).Any();
            }
        }

        public string GetUserPassword(string loginName) {
            using (DemoDBEntities db = new DemoDBEntities()) {
                var user = db.Users.Where(o => o.LoginName.ToLower().Equals(loginName));
                if (user.Any())
                    return user.FirstOrDefault().PasswordEncryptedText;
                else
                    return string.Empty;
            }
        }

        public string[] GetRolesForUser(string loginName) {
            using (DemoDBEntities db = new DemoDBEntities()) {
                User SU = db.Users.Where(o => o.LoginName.ToLower().Equals(loginName))?.FirstOrDefault();
                if (SU != null) {
                    var roles = from q in db.SYSUserRoles
                                join r in db.LOOKUPRoles on q.LOOKUPRoleID equals r.LOOKUPRoleID
                                select r.RoleName;

                    if (roles != null) {
                        return roles.ToArray();
                    }
                }

                return new string[] { };
            }
        }
        public bool IsUserInRole(string loginName, string roleName) {
            using (DemoDBEntities db = new DemoDBEntities()) {
                User SU = db.Users.Where(o => o.LoginName.ToLower().Equals(loginName))?.FirstOrDefault();
                if (SU != null) {
                    var roles = from q in db.SYSUserRoles
                                join r in db.LOOKUPRoles on q.LOOKUPRoleID equals r.LOOKUPRoleID
                                where r.RoleName.Equals(roleName) && q.SYSUserID.Equals(SU.UserID)
                                select r.RoleName;

                    if (roles != null) {
                        return roles.Any();
                    }
                }

                return false;
            }
        }

        public List<LOOKUPAvailableRole> GetAllRoles() {
            using (DemoDBEntities db = new DemoDBEntities()) {
                var roles = db.LOOKUPRoles.Select(o => new LOOKUPAvailableRole {
                    LOOKUPRoleID = o.LOOKUPRoleID,
                    RoleName = o.RoleName,
                    RoleDescription = o.RoleDescription
                }).ToList();

                return roles;
            }
        }

        public int GetUserID(string loginName) {
            using (DemoDBEntities db = new DemoDBEntities()) {
                var user = db.Users.Where(o => o.LoginName.Equals(loginName));
                if (user.Any())
                    return user.FirstOrDefault().UserID;
            }
            return 0;
        }
        public List<UserProfileView> GetAllUserProfiles() {
            List<UserProfileView> profiles = new List<UserProfileView>();
            using(DemoDBEntities db = new DemoDBEntities()) {
                UserProfileView UPV;
                var users = db.Users.ToList();

                foreach(User u in db.Users) {
                    UPV = new UserProfileView();
                    UPV.SYSUserID = u.UserID;
                    UPV.LoginName = u.LoginName;
                    UPV.Password = u.PasswordEncryptedText;

                    var SUP = db.SYSUserProfiles.Find(u.UserID);
                    if(SUP != null) {
                        UPV.FirstName = SUP.FirstName;
                        UPV.LastName = SUP.LastName;
                        UPV.Gender = SUP.Gender;
                    }

                    var SUR = db.SYSUserRoles.Where(o => o.SYSUserID.Equals(u.UserID));
                    if (SUR.Any()) {
                        var userRole = SUR.FirstOrDefault();
                        UPV.LOOKUPRoleID = userRole.LOOKUPRoleID;
                        UPV.RoleName = userRole.LOOKUPRole.RoleName;
                        UPV.IsRoleActive = userRole.IsActive;
                    }                    

                    profiles.Add(UPV);
                }
            }

            return profiles;
        }

        public UserDataView GetUserDataView(string loginName) {
            UserDataView UDV = new UserDataView();
            List<UserProfileView> profiles = GetAllUserProfiles();
            List<LOOKUPAvailableRole> roles = GetAllRoles();

            int? userAssignedRoleID = 0, userID = 0;
            string userGender = string.Empty;

            userID = GetUserID(loginName);
            using (DemoDBEntities db = new DemoDBEntities()) {
                userAssignedRoleID = db.SYSUserRoles.Where(o => o.SYSUserID == userID)?.FirstOrDefault().LOOKUPRoleID;
                userGender = db.SYSUserProfiles.Where(o => o.SYSUserID == userID)?.FirstOrDefault().Gender;
            }

            List<Gender> genders = new List<Gender>();
            genders.Add(new Gender { Text = "Male", Value = "M" });
            genders.Add(new Gender { Text = "Female", Value = "F" });

            UDV.UserProfile = profiles;
            UDV.UserRoles = new UserRoles { SelectedRoleID = userAssignedRoleID, UserRoleList = roles };
            UDV.UserGender = new UserGender { SelectedGender = userGender, Gender = genders };
            return UDV;
        }

        public UserProfileView GetUserProfile(int userID) {
            UserProfileView UPV = new UserProfileView();
            using (DemoDBEntities db = new DemoDBEntities()) {
                var user = db.Users.Find(userID);
                if (user != null) {
                    UPV.SYSUserID = user.UserID;
                    UPV.LoginName = user.LoginName;
                    UPV.Password = user.PasswordEncryptedText;

                    var SUP = db.SYSUserProfiles.Find(userID);
                    if (SUP != null) {
                        UPV.FirstName = SUP.FirstName;
                        UPV.LastName = SUP.LastName;
                        UPV.Gender = SUP.Gender;
                    }

                    var SUR = db.SYSUserRoles.Find(userID);
                    if (SUR != null) {
                        UPV.LOOKUPRoleID = SUR.LOOKUPRoleID;
                        UPV.RoleName = SUR.LOOKUPRole.RoleName;
                        UPV.IsRoleActive = SUR.IsActive;
                    }
                }
            }
            return UPV;
        }

        public void DeleteUser(int userID) {
            using (DemoDBEntities db = new DemoDBEntities()) {
                using (var dbContextTransaction = db.Database.BeginTransaction()) {
                    try {

                        var SUR = db.SYSUserRoles.Where(o => o.SYSUserID == userID);
                        if (SUR.Any()) {
                            db.SYSUserRoles.Remove(SUR.FirstOrDefault());
                            db.SaveChanges();
                        }

                        var SUP = db.SYSUserProfiles.Where(o => o.SYSUserID == userID);
                        if (SUP.Any()) {
                            db.SYSUserProfiles.Remove(SUP.FirstOrDefault());
                            db.SaveChanges();
                        }

                        var SU = db.Users.Where(o => o.UserID == userID);
                        if (SU.Any()) {
                            db.Users.Remove(SU.FirstOrDefault());
                            db.SaveChanges();
                        }

                        dbContextTransaction.Commit();
                    }
                    catch {
                        dbContextTransaction.Rollback();
                    }
                }
            }
        }
    }
}