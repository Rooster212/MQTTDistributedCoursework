using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class ClientStorage
    {
        private readonly SQLiteConnection _connection;

        private const string CreateStateTablesString = @"
            CREATE TABLE State (
                connected boolean,
                ID integer NOT NULL PRIMARY KEY AUTOINCREMENT,
                AsOfDate datetime,
                HostLocation varchar(2048) NOT NULL,
                HostPort integer NOT NULL,
                NextPacketID integer NOT NULL
            )";
        private const string CreateStateSubscriptionsTablesString = @"
            CREATE TABLE StateSubscriptions (
                Subscription_ID integer NOT NULL,
                State_ID integer NOT NULL,
                CONSTRAINT StateSubscriptions_pk PRIMARY KEY (Subscription_ID,State_ID),
                FOREIGN KEY (Subscription_ID) REFERENCES Subscription (ID),
                FOREIGN KEY (State_ID) REFERENCES State (ID)
            )";
        private const string CreateSubscriptionsTablesString = @"
            CREATE TABLE Subscription (
                ID integer NOT NULL PRIMARY KEY AUTOINCREMENT,
                TopicName integer NOT NULL
            )";

        public ClientStorage()
        {
            // check for DB file
            if (!File.Exists(ConfigurationManager.AppSettings["DBLocation"]))
            {
                SQLiteConnection.CreateFile(ConfigurationManager.AppSettings["DBLocation"]);
            }

            // create connection
            _connection = new SQLiteConnection
            {
                ConnectionString = new DbConnectionStringBuilder()
                {
                    {"Data Source", ConfigurationManager.AppSettings["DBLocation"]},
                    {"Version", "3"}
                }.ConnectionString
            };

            // open connection
            _connection.Open();
            
            SQLiteCommand checkForTablesCommand = new SQLiteCommand(@"SELECT name FROM sqlite_master WHERE type='table'",_connection);
            var results = checkForTablesCommand.ExecuteReader();
            if (!results.HasRows)
            {
                // create tables
                SQLiteCommand createStateTableCommand = new SQLiteCommand(CreateStateTablesString, _connection);
                createStateTableCommand.ExecuteNonQuery();
                SQLiteCommand createSubscriptionCommand = new SQLiteCommand(CreateSubscriptionsTablesString, _connection);
                createSubscriptionCommand.ExecuteNonQuery();
                SQLiteCommand createLinkTableCommand = new SQLiteCommand(CreateStateSubscriptionsTablesString, _connection);
                createLinkTableCommand.ExecuteNonQuery();
            }
        }
    }
}
