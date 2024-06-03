using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Novell.Directory.Ldap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS.DocCap.Application.Common.Utility.Ldap
{
    public class Search
    {

        private static string _userName;
        private static string _userPassword;
        private static string _Server;
        private static string _Port;
        private static string _Domain;
        private static string _ServiceAccountDn;
        private static string _ServiceAccountUserName;
        private static string _ServiceAccountPassword;
        private static string _SearchBase;
        public Search(IConfiguration iConfiguration)
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


        /// <summary>
        /// Finds a User Object
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string getUser(string key, string value)
        {
            return ForLdapEntry(null, null, String.Format("(&(objectClass=person)({0}={1}))", key, value));
            //return ForLdapEntry(null, null, String.Format("(&(&(objectClass=user)(!(objectClass=computer)))({0}={1}))", key, value));


        }

        public static string getUser(string key, string value, string[] PropertiesToLoad)
        {
            return ForLdapEntry(null, null, String.Format("(&(objectClass=person)({0}={1}))", key, value), PropertiesToLoad);
            //return ForLdapEntry(null, null, String.Format("(&(&(objectClass=user)(!(objectClass=computer)))({0}={1}))", key, value), PropertiesToLoad);
        }

        /// <summary>
        /// Finds Users
        /// </summary>
        /// <returns>List</returns>
        public static List<LdapEntry> ForUsers(string filterString)
        {
            return ForLdapEntries(null, null, filterString);
        }

        public static List<LdapEntry> ForUsers(string[] PropertiesToLoad)
        {
            return ForLdapEntries(null, null, "(&(objectClass=user)(!(objectClass=computer)))", PropertiesToLoad);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ForGroup(string key, string value)
        {
            return ForLdapEntry(null, null, String.Format("(&(objectClass=group)({0}={1}))", key, value));
        }

        public static List<LdapEntry> ForGroups()
        {
            return ForLdapEntries(null, null, "objectClass=user");
        }

        /// <summary>
        /// Finds Users
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static List<LdapEntry> ForUsers(string key, string value)
        {
            return ForLdapEntries(null, null, "(&(&(objectClass=user)(!(objectClass=computer)))(" + key + "=" + value + "))");
        }

        /// <summary>
        /// Finds ldap entry
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ForLdapEntry(string key, string value)
        {
            return ForLdapEntry(key, value, string.Format("{0}={1}", key, value));
        }

        /// <summary>
        /// Finds ldap entry
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static string ForLdapEntry(string key, string value, string filter)
        {
            return ForLdapEntry(key, value, filter, null);
        }

        /// <summary>
        /// Finds ldap entry
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="filter">The ADO query filter</param>
        /// <param name="attributes">Properties to load</param>
        /// <returns></returns>
        public static string ForLdapEntry(string key, string value, string filter, string[] attributes)
        {
            ValidateSetup();
            LdapEntry searchResult = null;
            string output = "";
            List<Attribs> resultRows = new List<Attribs>();

            //string[] resultOut = null;
            LdapConnection conn = new LdapConnection();
            conn.Connect(_Server, Convert.ToInt32(_Port));

            conn.Bind(_ServiceAccountDn, _ServiceAccountPassword);

            //Search
            LdapSearchResults results = (LdapSearchResults)conn.Search(_SearchBase, //search base
                                                    LdapConnection.ScopeSub, //scope 
                                                    filter, //filter
                                                    attributes, //attributes 
                                                    false); //types only 


            //string[] resultOut = new string[20];
            while (results.HasMore())
            {
                searchResult = null;


                try
                {
                    searchResult = results.Next();
                    // break;
                }
                catch (LdapException e)
                {
                    Console.WriteLine(e.Message);
                }
                Console.WriteLine("\n" + searchResult.Dn);
                LdapAttributeSet attributeSet = searchResult.GetAttributeSet();
                System.Collections.IEnumerator ienum = attributeSet.GetEnumerator();
                while (ienum.MoveNext())
                {
                    LdapAttribute attribute = (LdapAttribute)ienum.Current;

                    Attribs attribs = new Attribs();
                    attribs.type = attribute.Name;
                    attribs.value = attribute.StringValue;
                    // Console.WriteLine(attributeName + "value:" + attributeVal);
                    resultRows.Add(attribs);

                }


            }
            conn.Disconnect();
            output = JsonConvert.SerializeObject(resultRows);
            return output;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// 

        public static List<LdapEntry> ForLdapEntries(string key, string value)
        {
            return ForLdapEntries(key, value, String.Format("{0}={1}"), null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static List<LdapEntry> ForLdapEntries(string key, string value, string filter)
        {
            return ForLdapEntries(key, value, filter, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="filter"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public static List<LdapEntry> ForLdapEntries(string key, string value, string filter, string[] attributes)
        {
            //  ValidateSetup();
            List<LdapEntry> searchResults = new List<LdapEntry>();
            LdapConnection conn = new LdapConnection();
            conn.Connect(_Server,Convert.ToInt32(_Port));
            conn.Bind(_ServiceAccountDn, _ServiceAccountPassword);

            LdapSearchResults results = (LdapSearchResults)conn.Search(_SearchBase, //search base
                                                    LdapConnection.ScopeSub, //scope 
                                                    filter, //filter
                                                    attributes, //attributes 
                                                    false); //types only 

            while (results.HasMore())
            {
                LdapEntry nextEntry = null;
                try
                {
                    nextEntry = results.Next();
                    if (nextEntry != null)
                        searchResults.Add(nextEntry);
                }
                catch (LdapException)
                {
                    //Console.WriteLine(e.Message);
                    continue;
                }
            }
            conn.Disconnect();
            return searchResults;
        }

        private static void ValidateSetup()
        {
            if (_Server == null || _SearchBase == null)
                throw new Exception("Must specify required parameters in the Settings.cs class");
        }
    }
}
