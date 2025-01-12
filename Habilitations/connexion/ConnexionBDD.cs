﻿using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Habilitations.connexion
{
    public class ConnexionBDD
    {
        private MySqlConnection connection;
        private MySqlCommand command;
        private MySqlDataReader reader;
        private static ConnexionBDD instance = null;

        private ConnexionBDD(string chaineConnection)
        {
            try
            {
                this.connection = new MySqlConnection(chaineConnection);
                this.connection.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Application.Exit();
            }
        }

        public static ConnexionBDD GetInstance(string chaineConnection)
        {
            if (ConnexionBDD.instance is null)
            {
                ConnexionBDD.instance = new ConnexionBDD(chaineConnection);
            }
            return ConnexionBDD.instance;
        }

        public void ReqUpdate(string chaineRequete, Dictionary<string, object>parameters)
        {
            try
            {
                this.command = new MySqlCommand(chaineRequete, this.connection);
                if (!(parameters is null))
                {
                    foreach (KeyValuePair<string, object> parameter in parameters)
                    {
                        command.Parameters.Add(new MySqlParameter(parameter.Key, parameter.Value));
                    }
                }
                command.Prepare();
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void ReqSelect(string chaineRequete, Dictionary<string, object> parameters)
        {
            try
            {
                this.command = new MySqlCommand(chaineRequete, connection);
                if (!(parameters is null))
                {
                    foreach(KeyValuePair<string, object> parameter in parameters)
                    {
                        command.Parameters.Add(new MySqlParameter(parameter.Key, parameter.Value));
                    }
                }
                command.Prepare();
                this.reader = command.ExecuteReader();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public Boolean Read()
        {
            if (reader is null)
            {
                return false;
            }
            try
            {
                return reader.Read();
            }
            catch
            {
                return false;
            }
        }

        public object Field(String nomDeChamp)
        {
            if (reader is null)
            {
                return null;
            }
            try
            {
                return reader[nomDeChamp];
            }
            catch
            {
                return null;
            }
        }

        public void Close()
        {
            if (!(reader is null))
            {
                reader.Close();
            }
        }
    }
}
