using Microsoft.AspNetCore.Mvc;

namespace CLDV6212POE3_ST10070933.Models
{
   
        public class Orders
        {
            public int Id { get; set; }                
            public int CustomerProfileId { get; set; } 
            public DateTime OrderDate { get; set; }     
            public decimal TotalAmount { get; set; }    

         
        }
    
}
