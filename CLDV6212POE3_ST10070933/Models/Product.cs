using Microsoft.AspNetCore.Mvc;

namespace CLDV6212POE3_ST10070933.Models
{
    public class Product
    {
        public int Id { get; set; }              
        public string Name { get; set; }        
        public string Description { get; set; }  
        public decimal Price { get; set; }       
        public string Category { get; set; }     
        public int StockLevel { get; set; }      
        public string ImageUrl { get; set; }     

       
        public DateTime CreatedDate { get; set; }   
        public DateTime? ModifiedDate { get; set; }  
    }
}
