using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiGateway.PostgreSql.Models
{
    public enum SubscriptionType
    {
        Month,
        Anual
    }
    public class Subscriptions
    {
        [Key]
        public Guid SubscriptionId { get; set; }

        public string UserId { get; set; }
        public SubscriptionType SubscriptionType { get; set; }

        public DateTime StartDate { get; set; }

        [NotMapped] // No se mapea a la base de datos directamente
        public DateTime EndDate
        {
            get
            {

                switch (SubscriptionType)
                {
                    case SubscriptionType.Month:
                        return StartDate.AddMonths(1);
                    case SubscriptionType.Anual:
                        return StartDate.AddYears(1);
                    default:
                        return DateTime.MaxValue;
                }
            }

        }
    }
}
