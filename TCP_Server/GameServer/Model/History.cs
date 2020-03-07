using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model
{
    class History
    {
        public History(int id, int userid, int totalcount, int wincount)
        {
            Id = id;
            UserId = userid;
            TotalCount = totalcount;
            WinCount = wincount;
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public int TotalCount { get; set; }
        public int WinCount { get; set; }
    }
}
