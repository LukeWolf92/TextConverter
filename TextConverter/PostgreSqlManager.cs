using System;
using Npgsql;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace TextConverter
{
    class PostgreSqlManager
    {
        private static NpgsqlCommand SqlCommand;
        private static NpgsqlConnection SqlConnection = new NpgsqlConnection();
        private static string ConnectionString;        

        public PostgreSqlManager(MqttCfgSettingsOrganiser mqttCfgSettings)
        {
            // PostgeSQL-style connection string
            ConnectionString = String.Format("Server={0};Port={1};" + "User Id={2};Password={3};Database={4};",
                mqttCfgSettings.DbServer,
                mqttCfgSettings.DbPort,
                mqttCfgSettings.DbUser,
                mqttCfgSettings.DbPassword,
                mqttCfgSettings.DbName);

            // Making connection with Npgsql provider
            SqlConnection = new NpgsqlConnection(ConnectionString);
        }

        public void OpenConnection()
        {
            SqlConnection.Open();
        }

        public void CloseConnection()
        {
            SqlConnection.Close();
        }

        public bool CheckingDuplicate(Measurements measure, MqttCfgSettingsOrganiser mqttCfgSettings)
        {
            // OPENS CONNECTION
            SqlConnection.Open();

            string sql = "SELECT MeasureTimeStamp, " +
                "MachineType, " +
                "MachineNumber, " +
                "Part, " +
                "PartNumber, " +
                "ValueKind, " +
                "MeasureValue, " +
                "TextValue " +
                "FROM public.qcmachines " +
                "WHERE MeasureTimeStamp = '" + measure.TimeStamp + "' and " +
                "MachineType = '" + measure.MachineType + "' and " +
                "MachineModel = '" + measure.MachineModel + "' and " +
                "MachineNumber = '" + measure.MachineNumber + "' and " +
                "Part = '" + measure.Part + "' and " +
                "PartNumber = '" + measure.PartNumber + "' and " +
                "ValueKind = '" + measure.ValueKind + "' and " +
                "MeasureValue = '" + measure.Value + "' and " +
                "TextValue = '" + measure.TextValue + "';";

            // Define a query
            NpgsqlCommand command = new NpgsqlCommand(sql, SqlConnection);

            // Execute the query and obtain a result set
            NpgsqlDataReader DataReader = command.ExecuteReader();

            bool IsDuplicate;
            if (DataReader.HasRows)
                IsDuplicate = true;
            else
                IsDuplicate = false;

            // CLOSES CONNECTION
            SqlConnection.Close();

            return IsDuplicate;
        }
        public void StoreIntoPostgreSQL(Measurements measure, MqttCfgSettingsOrganiser mqttCfgSettings)
        {
            try
            {
                // OPENS CONNECTION
                SqlConnection.Open();
                string sql = "INSERT INTO QCMachines " +
                    "(MeasureTimeStamp, " +
                    "MachineType, " +
                    "MachineModel, " +
                    "MachineNumber, " +
                    "Part, " +
                    "PartNumber, " +
                    "ValueKind, " +
                    "MeasureValue, " +
                    "TextValue) " +
                    "VALUES ('" + measure.TimeStamp + "',"
                    + "'" + measure.MachineType + "',"
                    + "'" + measure.MachineModel + "',"
                    + "'" + measure.MachineNumber + "',"
                    + "'" + measure.Part + "',"
                    + "'" + measure.PartNumber + "',"
                    + "'" + measure.ValueKind + "',"
                    + "'" + measure.Value + "',"
                    + "'" + measure.TextValue + "');";
               
                SqlCommand = new NpgsqlCommand(sql, SqlConnection);

                SqlCommand.CommandType = System.Data.CommandType.Text;
                // EXECUTES QUERY
                SqlCommand.ExecuteNonQuery();

                // CLOSES CONNECTION
                SqlConnection.Close();
            }
            catch (Exception PostgreError)
            {
                Console.WriteLine("ERROR: Failed to store measurement on PostgreSQL DB:\n" + PostgreError);
            }

        }
    }
}
