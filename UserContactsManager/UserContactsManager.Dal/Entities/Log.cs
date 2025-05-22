using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserContactsManager.Dal.Entities;

public class Log
{
    public long LogId { get; set; }
    public string Message { get; set; }
    public DateTime Timestamp { get; set; }
    public string Level { get; set; }
    public string Exception { get; set; }
    public string RequestPath { get; set; }
    public string RequestMethod { get; set; }
    public int StatusCode { get; set; }
    public long UserId { get; set; }
    public User UserName { get; set; }
}
