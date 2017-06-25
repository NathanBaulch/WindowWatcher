using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;

namespace WindowWatcher
{
    public class LogRepository
    {
        private readonly string _connectionString;
        private SQLiteConnection _connection;
        private int _connectionCount;

        public LogRepository(string connectionString)
        {
            Guard.ArgumentNotNullOrEmptyString(connectionString, "connectionString");
            _connectionString = connectionString;
        }

        public IDisposable OpenConnection()
        {
            return OpenConnection(false);
        }

        public IDisposable OpenConnection(bool transacted)
        {
            return new OpenConnectionDisposable(this, transacted);
        }

        private void BeginConnection()
        {
            if (_connectionCount == 0)
            {
                _connection = new SQLiteConnection(_connectionString);
                _connection.Open();
            }

            _connectionCount++;
        }

        private void EndConnection()
        {
            _connectionCount--;

            if (_connectionCount == 0)
            {
                _connection.Dispose();
                _connection = null;
            }
        }

        public void CreateTable()
        {
            using (OpenConnection())
            {
                using (var cmd = new SQLiteCommand(
                    "create table LOG (" +
                    "START datetime not null, " +
                    "END datetime not null, " +
                    "PROCESS varchar(256) not null, " +
                    "CAPTION varchar(1024) not null, " +
                    "IDLE integer not null, " +
                    "ACTIVE integer not null, " +
                    "CATEGORY varchar(32))", _connection))
                {
                    cmd.ExecuteNonQuery();
                }

                using (var cmd = new SQLiteCommand(
                    "create unique index LOG_INDEX " +
                    "on LOG (START desc, END desc, PROCESS, CAPTION)", _connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public bool Select(Log log)
        {
            Guard.ArgumentNotNull(log, "log");
            Guard.ArgumentNotNull(log.Start, "log.Start");
            Guard.ArgumentNotNull(log.End, "log.End");
            Guard.ArgumentNotNull(log.Process, "log.Process");
            Guard.ArgumentNotNull(log.Caption, "log.Caption");

            using (OpenConnection())
            using (var cmd = new SQLiteCommand(
                "select IDLE, ACTIVE, CATEGORY " +
                "from LOG " +
                "where START = ? " +
                "and END = ? " +
                "and PROCESS = ? " +
                "and CAPTION = ?", _connection))
            {
                cmd.Parameters.AddWithValue("START", FormatDate(log.Start.Value));
                cmd.Parameters.AddWithValue("END", FormatDate(log.End.Value));
                cmd.Parameters.AddWithValue("PROCESS", log.Process);
                cmd.Parameters.AddWithValue("CAPTION", log.Caption);

                using (var reader = cmd.ExecuteReader(CommandBehavior.SingleRow))
                {
                    var found = reader.Read();

                    if (found)
                    {
                        FillLog(log, reader);
                    }

                    return found;
                }
            }
        }

        public void Insert(Log log)
        {
            Guard.ArgumentNotNull(log, "log");
            Guard.ArgumentNotNull(log.Start, "log.Start");
            Guard.ArgumentNotNull(log.End, "log.End");
            Guard.ArgumentNotNull(log.Process, "log.Process");
            Guard.ArgumentNotNull(log.Caption, "log.Caption");
            Guard.ArgumentNotNull(log.Idle, "log.Idle");
            Guard.ArgumentNotNull(log.Active, "log.Active");

            using (OpenConnection())
            using (var cmd = new SQLiteCommand(
                "insert into LOG (START, END, PROCESS, CAPTION, IDLE, ACTIVE, CATEGORY) " +
                "values (?, ?, ?, ?, ?, ?, ?)", _connection))
            {
                cmd.Parameters.AddWithValue("START", FormatDate(log.Start.Value));
                cmd.Parameters.AddWithValue("END", FormatDate(log.End.Value));
                cmd.Parameters.AddWithValue("PROCESS", log.Process);
                cmd.Parameters.AddWithValue("CAPTION", log.Caption);
                cmd.Parameters.AddWithValue("IDLE", log.Idle);
                cmd.Parameters.AddWithValue("ACTIVE", log.Active);
                cmd.Parameters.AddWithValue("CATEGORY", log.Category);

                var rowsEffected = cmd.ExecuteNonQuery();
                Debug.Assert(rowsEffected == 1);
            }
        }

        public void UpdateCategory(Log log)
        {
            Guard.ArgumentNotNull(log, "log");
            Guard.ArgumentNotNull(log.Start, "log.Start");
            Guard.ArgumentNotNull(log.End, "log.End");
            Guard.ArgumentNotNull(log.Process, "log.Process");
            Guard.ArgumentNotNull(log.Caption, "log.Caption");
            Guard.ArgumentNotNull(log.Idle, "log.Idle");
            Guard.ArgumentNotNull(log.Active, "log.Active");

            using (OpenConnection())
            using (var cmd = new SQLiteCommand(
                "update LOG " +
                "set CATEGORY = ? " +
                "where START = ? " +
                "and END = ? " +
                "and PROCESS = ? " +
                "and CAPTION = ?", _connection))
            {
                cmd.Parameters.AddWithValue("CATEGORY", log.Category);
                cmd.Parameters.AddWithValue("START", FormatDate(log.Start.Value));
                cmd.Parameters.AddWithValue("END", FormatDate(log.End.Value));
                cmd.Parameters.AddWithValue("PROCESS", log.Process);
                cmd.Parameters.AddWithValue("CAPTION", log.Caption);
                var rowsEffected = cmd.ExecuteNonQuery();
                Debug.Assert(rowsEffected == 1);
            }
        }

        public void Delete(Log log)
        {
            Guard.ArgumentNotNull(log, "log");
            Guard.ArgumentNotNull(log.Start, "log.Start");
            Guard.ArgumentNotNull(log.End, "log.End");
            Guard.ArgumentNotNull(log.Process, "log.Process");
            Guard.ArgumentNotNull(log.Idle, "log.Idle");

            using (OpenConnection())
            using (var cmd = new SQLiteCommand(
                "delete LOG " +
                "where START = ? " +
                "and END = ? " +
                "and PROCESS = ? " +
                "and CAPTION = ?", _connection))
            {
                cmd.Parameters.AddWithValue("START", FormatDate(log.Start.Value));
                cmd.Parameters.AddWithValue("END", FormatDate(log.End.Value));
                cmd.Parameters.AddWithValue("PROCESS", log.Process);
                cmd.Parameters.AddWithValue("CAPTION", log.Caption);
                var rowsEffected = cmd.ExecuteNonQuery();
                Debug.Assert(rowsEffected == 1);
            }
        }

        public DataTable ExecuteQuery(string query)
        {
            Guard.ArgumentNotNull(query, "query");

            var table = new DataTable();

            using (OpenConnection())
            using (var adapter = new SQLiteDataAdapter(query, _connection))
            {
                try
                {
                    adapter.Fill(table);
                }
                catch (SQLiteException ex)
                {
                    throw new DataException(ex.Message, ex);
                }
            }

            return table;
        }

        public IEnumerable<Log> GetClassified()
        {
            using (OpenConnection())
            {
                using (var cmd = new SQLiteCommand(
                    "select PROCESS, CAPTION, CATEGORY " +
                    "from LOG " +
                    "where CATEGORY is not null " +
                    "and CATEGORY <> '' " +
                    "order by START, END", _connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        yield return CreateLog(reader);
                    }
                }
            }
        }

        public IEnumerable<Log> GetByFilter(string filter, IDictionary<string, object> parameters)
        {
            Guard.ArgumentNotNull(filter, "filter");
            Guard.ArgumentNotNull(parameters, "parameters");

            if (!filter.EndsWith(" "))
            {
                filter += " ";
            }

            using (OpenConnection())
            {
                using (var cmd = new SQLiteCommand(
                    "select START, END, PROCESS, CAPTION, IDLE, ACTIVE, CAPTION " +
                    "from LOG " +
                    "where " + filter +
                    "order by START, END, PROCESS, CAPTION", _connection))
                {
                    foreach (var pair in parameters)
                    {
                        cmd.Parameters.AddWithValue(pair.Key, pair.Value);
                    }

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return CreateLog(reader);
                        }
                    }
                }
            }
        }

        public IEnumerable<Log> GetSummary(DateTime from, DateTime to)
        {
            using (OpenConnection())
            {
                using (var cmd = new SQLiteCommand(
                    "select max(START) as START, max(END) as END, PROCESS, CAPTION, " +
                    "sum(IDLE) as IDLE, sum(ACTIVE) as ACTIVE, max(CATEGORY) as CATEGORY " +
                    "from LOG " +
                    "where START >= ? " +
                    "and END < ? " +
                    "and (PROCESS <> '' or CAPTION <> '') " +
                    "group by PROCESS, CAPTION " +
                    "order by sum(IDLE) + sum(ACTIVE) desc " +
                    "limit 1000", _connection))
                {
                    cmd.Parameters.AddWithValue("START", FormatDate(from));
                    cmd.Parameters.AddWithValue("END", FormatDate(to));

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return CreateLog(reader);
                        }
                    }
                }
            }
        }

        public IEnumerable<string> GetDistinctProcesses(DateTime from, DateTime to)
        {
            return GetDistinct("PROCESS", from, to);
        }

        public IEnumerable<string> GetDistinctCaptions(DateTime from, DateTime to)
        {
            return GetDistinct("CAPTION", from, to);
        }

        private IEnumerable<string> GetDistinct(string field, DateTime from, DateTime to)
        {
            using (OpenConnection())
            {
                using (var cmd = new SQLiteCommand(
                    string.Format(
                        "select {0} " +
                        "from LOG " +
                        "where START >= ? " +
                        "and END < ? " +
                        "and {0} is not null " +
                        "group by {0} " +
                        "order by lower({0})", field), _connection))
                {
                    cmd.Parameters.AddWithValue("START", FormatDate(from));
                    cmd.Parameters.AddWithValue("END", FormatDate(to));

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return (string) reader[0];
                        }
                    }
                }
            }
        }

        private static Log CreateLog(IDataRecord record)
        {
            var log = new Log();
            FillLog(log, record);
            return log;
        }

        private static void FillLog(Log log, IDataRecord record)
        {
            if (TryGetRecordValue(record, "START", out var value))
            {
                var str = value as string;
                log.Start = str != null ? DateTime.Parse(str) : (DateTime?) value;
            }

            if (TryGetRecordValue(record, "END", out value))
            {
                var str = value as string;
                log.End = str != null ? DateTime.Parse(str) : (DateTime?) value;
            }

            if (TryGetRecordValue(record, "PROCESS", out value))
            {
                log.Process = (string) value;
            }

            if (TryGetRecordValue(record, "CAPTION", out value))
            {
                log.Caption = (string) value;
            }

            if (TryGetRecordValue(record, "IDLE", out value))
            {
                log.Idle = (int?) (long?) value;
            }

            if (TryGetRecordValue(record, "ACTIVE", out value))
            {
                log.Active = (int?) (long?) value;
            }

            if (TryGetRecordValue(record, "CATEGORY", out value))
            {
                log.Category = value as string;
            }
        }

        private static bool TryGetRecordValue(IDataRecord record, string name, out object value)
        {
            var ordinal = record.GetOrdinal(name);
            var hasValue = ordinal >= 0;
            value = hasValue ? record[ordinal] : null;
            return hasValue;
        }

        private static string FormatDate(DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH:mm:ss.fffffff");
        }

        #region Nested type: OpenConnectionDisposable

        private class OpenConnectionDisposable : IDisposable
        {
            private readonly LogRepository _repository;
            private readonly SQLiteTransaction _transaction;

            public OpenConnectionDisposable(LogRepository repository, bool transacted)
            {
                _repository = repository;
                _repository.BeginConnection();

                if (transacted)
                {
                    _transaction = _repository._connection.BeginTransaction();
                }
            }

            #region IDisposable Members

            public void Dispose()
            {
                _transaction?.Commit();
                _repository.EndConnection();
            }

            #endregion
        }

        #endregion
    }
}