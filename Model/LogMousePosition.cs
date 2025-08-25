using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common;

namespace Model
{
    public class LogMousePosition : BaseEntity
    {
        public LogMousePosition() {
        }

        public LogMousePosition(string position)
        {
            Position = position; 
        }

        [Key]
        public int MousePositionId { get; set; }
        
        public string Position { get; set; }

        public string PosX
        {
            get
            {
                var arr = Position.Split(',');
                return arr[0];
            }
        }

        public string PosY
        {
            get
            {
                var arr = Position.Split(',');
                return arr[1];
            }
        }

        [ForeignKey("UserSessionId")]
        public virtual LogUserSession UserSession { get; set; }

        public int? UserSessionId { get; set; }

    }
}
