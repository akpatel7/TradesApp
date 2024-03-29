﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradesDataAccessServices
{
    public sealed class DataReader
    {
        private DateTime defaultDate;

        public DataReader(SqlDataReader reader)
        {
            this.defaultDate = DateTime.MinValue;
            this.reader = reader;
        }

        public int GetInt32(String column)
        {
            int data = 0;

            if (DoesFieldExists(reader, column))
                data = (reader.IsDBNull(reader.GetOrdinal(column)))
                                    ? (int)0 : (int)reader[column];

            return data;
        }

        public short GetInt16(String column)
        {
            short data = 0;

            if (DoesFieldExists(reader, column))
                data = (reader.IsDBNull(reader.GetOrdinal(column)))
                                  ? (short)0 : (short)reader[column];
            return data;
        }

        public float GetFloat(String column)
        {
            float data = 0;

            if (DoesFieldExists(reader, column))
                data = (reader.IsDBNull(reader.GetOrdinal(column)))
                            ? 0 : float.Parse(reader[column].ToString());

            return data;
        }

        public bool GetBoolean(String column)
        {
            bool data = (!reader.IsDBNull(reader.GetOrdinal(column))) && (bool)reader[column];
            return data;
        }

        public string GetString(String column)
        {
            string data = string.Empty;

            if (DoesFieldExists(reader, column))
                data = Convert.ToString(reader[column]);

            return data;

        }

        public DateTime GetDateTime(String column)
        {
            DateTime data = defaultDate;

            if (DoesFieldExists(reader, column))
                data = (reader.IsDBNull(reader.GetOrdinal(column)))
                               ? defaultDate : (DateTime)reader[column];

            return data;
        }

        public bool Read()
        {
            return this.reader.Read();
        }

        private bool DoesFieldExists(IDataReader reader, string fieldName)
        {
            reader.GetSchemaTable().DefaultView.RowFilter = string.Format("ColumnName= '{0}'", fieldName);
            return (reader.GetSchemaTable().DefaultView.Count > 0);
        }

        private SqlDataReader reader;

    }
}
