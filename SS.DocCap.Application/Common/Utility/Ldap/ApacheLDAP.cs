using Microsoft.Extensions.Configuration;
using Novell.Directory.Ldap;
using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SS.DocCap.Application.Common.Utility.Ldap
{
    public class ApacheLDAP
    {
        private static string _userName;
        private static string _userPassword;
        private static string _Server ;
        private static string  _Port;
        private static string _Domain;
        private static string _ServiceAccountDn;
        private static string _ServiceAccountUserName;
        private static string _ServiceAccountPassword;
        private static string _SearchBase;


        public ApacheLDAP(IConfiguration iConfiguration)
        {
            var configurationManager = iConfiguration.GetSection("ConfigurationManager");
            if (configurationManager != null)
            {
                _Server = configurationManager.GetSection("LdapServer").Value;
                _Port = configurationManager.GetSection("Port").Value;
                _Domain = configurationManager.GetSection("Domain").Value;
                _ServiceAccountDn = configurationManager.GetSection("ServiceAccountDn").Value;
                _ServiceAccountUserName = configurationManager.GetSection("ServiceAccountUserName").Value;
                _ServiceAccountPassword = configurationManager.GetSection("ServiceAccountPassword").Value;
                _SearchBase = configurationManager.GetSection("SearchBase").Value;
            }
        }

        public bool ValidateCredentials(string userName, string userPassword)
        {


            _userName = userName;
            _userPassword = userPassword;

            LdapDirectoryIdentifier ldi = new LdapDirectoryIdentifier(_Server, Convert.ToInt32(_Port));
            System.DirectoryServices.Protocols.LdapConnection ldapConnection =
                new System.DirectoryServices.Protocols.LdapConnection(ldi);
            Console.WriteLine("LdapConnection is created successfully.");
            ldapConnection.AuthType = AuthType.Basic;
            ldapConnection.SessionOptions.ProtocolVersion = 3;
            string UserNameString = "uid=" + _userName + ",ou=people,o=" + _Domain + ",dc=com";
            NetworkCredential nc = new NetworkCredential(UserNameString, _userPassword); //password


            try
            {
                //We use using so we dispose the object as soon as it goes out of scope 
                using (System.DirectoryServices.Protocols.LdapConnection connection = new System.DirectoryServices.Protocols.LdapConnection(ldi))
                {

                    ldapConnection.Bind(nc);
                    ldapConnection.Dispose();

                }

                return true;

            }
            catch (System.DirectoryServices.Protocols.LdapException ldapException)
            {
                return false;
            }



        }//End of ValidateCredentials


      
        public string getUserdetails(string username)
        {

            string user = "";
           // user = Search.getUser("uid", username);
            return user;
        }

        public string getUsergroup(string usergroup)
        {

            string user = "";
            user = Search.ForGroup("ou", usergroup);
            return user;
        }

        public List<LdapEntry> getAlluserlist(string searchfiltre)
        {
            List<LdapEntry> users = null;
            users = Search.ForUsers(searchfiltre);

            return users;

        }

        //public bool UpdateLDAPEntity(string UserName, List<TBL_BMS_SMS_CONFIGURATION> tBL_BMS_SMS_CONFIGURATION)
        //{
        //    string ldapHost = Settings.Server;
        //    int ldapPort = Settings.Port;
        //    String loginDN = Settings.ServiceAccountDn;
        //    String password = Settings.ServiceAccountPassword;
        //    String dn = "uid = " + UserName + ",ou = people," + Settings.SearchBase;

        //    try
        //    {
        //        Console.WriteLine("Connecting to:" + ldapHost);
        //        Novell.Directory.Ldap.LdapConnection conn = new Novell.Directory.Ldap.LdapConnection();
        //        ArrayList modList = new ArrayList();

        //        String mobile = "";
        //        String productType = "";
        //        LdapAttribute attribute = null;
        //        LdapModification[] mods = null;

        //        //Update
        //        foreach (var item in tBL_BMS_SMS_CONFIGURATION)
        //        {
        //            mobile = item.MOBILE_NUMBER;
        //            productType = item.TBL_BMS_PRODUCT_SETUP.PRODUCT_ID;

        //            attribute = new LdapAttribute("description", productType);
        //            modList.Add(new LdapModification(LdapModification.ADD, attribute));

        //            attribute = new LdapAttribute("mobile", mobile);
        //            modList.Add(new LdapModification(LdapModification.ADD, attribute));

        //            mods = new LdapModification[modList.Count];
        //            mods = (LdapModification[])modList.ToArray(typeof(LdapModification));

        //        }
        //        conn.Connect(ldapHost, ldapPort);
        //        conn.Bind(loginDN, password);
        //        conn.Modify(dn, mods);
        //        Console.WriteLine(" Entry: " + dn + "Modified Successfully");
        //        conn.Disconnect();
        //        return true;

        //    }
        //    catch (Novell.Directory.Ldap.LdapException e)
        //    {
        //        Console.WriteLine("Error:" + e.LdapErrorMessage);
        //        return false;
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("Error:" + e.Message);
        //        return false;
        //    }
        //}

        //public bool ChangePasswordInLDAP(string UserName, string new_passsword)
        //{
        //    string ldapHost = Settings.Server;
        //    int ldapPort = Settings.Port;
        //    String loginDN = Settings.ServiceAccountDn;
        //    String password = Settings.ServiceAccountPassword;
        //    String dn = "uid = " + UserName + ",ou = people," + Settings.SearchBase;

        //    try
        //    {
        //        Console.WriteLine("Connecting to:" + ldapHost);
        //        Novell.Directory.Ldap.LdapConnection conn = new Novell.Directory.Ldap.LdapConnection();
        //        ArrayList modList = new ArrayList();

        //        LdapAttribute attribute = new LdapAttribute("userPassword", new_passsword);
        //        modList.Add(new LdapModification(LdapModification.REPLACE, attribute));

        //        LdapModification[] mods = new LdapModification[modList.Count];
        //        mods = (LdapModification[])modList.ToArray(typeof(LdapModification));

        //        conn.Connect(ldapHost, ldapPort);
        //        conn.Bind(loginDN, password);
        //        conn.Modify(dn, mods);
        //        Console.WriteLine(" Entry: " + dn + "Modified Successfully");
        //        conn.Disconnect();
        //        return true;

        //    }
        //    catch (Novell.Directory.Ldap.LdapException e)
        //    {
        //        Console.WriteLine("Error:" + e.LdapErrorMessage);
        //        return false;
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("Error:" + e.Message);
        //        return false;
        //    }
        //}
    }
}
