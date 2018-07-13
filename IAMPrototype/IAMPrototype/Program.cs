using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices.Protocols;
using System.Data.SqlClient;
using System.Data;

namespace IAMPrototype
{
    class Program
    {
        static void Main(string[] args)
        {
            //TODO: Need this data from Optum
            String server = "";
            String portNumber = "";
            String username = "";
            String password = "";

            Boolean result = LdapConnection(server, portNumber, username, password);

            Console.WriteLine("Connection Test: " + result);
            Console.ReadKey();

        }

        static Boolean LdapConnection(String server, String portNumber, String username, String password)
        {
            // server and port must be correct
            using (var ldap = new LdapConnection(server + ":" + portNumber))
            {
                // pass the username as a distinguishedName
                var dn = string.Format("cn={0},cn=users,dc=company,dc=com", username);

                // passing null for the domain
                var credentials = new System.Net.NetworkCredential(dn, password, null);
                ldap.AuthType = AuthType.Basic;

                try
                {
                    ldap.Bind(credentials);
                    iamDatabaseQuery(ldap);
                    return true;
                }
                catch (LdapException ex)
                {
                    Console.WriteLine(ex.StackTrace);
                    return false;
                }
            }
        }

        static Boolean iamDatabaseQuery(System.DirectoryServices.Protocols.LdapConnection ldap)
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection("IAMCONNECTIONSTRING");  //TODO: Update this
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT * FROM IAMDATABASETABLENAME";  //TODO: Update this
                cmd.CommandType = CommandType.Text;
                cmd.Connection = sqlConnection;

                sqlConnection.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                // Data is accessible through the DataReader object here.

                //TODO:Display Data to Screen

                sqlConnection.Close();
                return true;
            }  catch (Exception ex) //TODO Hndle exceptions better
            {
                Console.WriteLine(ex.StackTrace);
                return false;
            }
        }


    }
}
