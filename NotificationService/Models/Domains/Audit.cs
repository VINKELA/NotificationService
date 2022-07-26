using NotificationService.Models.Domains;
using NotificationService.Models.Enums;

namespace SSOService.Models.Domains
{
    public class Audit : Base
    {
        public string UserId { get; set; }
        public AuditType Type { get; set; }
        public string TableName { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
        public string AffectedColumns { get; set; }
        public string PrimaryKey { get; set; }
    }
}
