using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserContactsManager.Bll.Repository.Settings;

public class SqlDbConnectionString
{
    private string connectionString;

    public string ConnectionString
    {
        get { return connectionString; }
        set { connectionString = value; }
    }
    public SqlDbConnectionString(string connectionString)
    {
        ConnectionString = connectionString;

    }
}