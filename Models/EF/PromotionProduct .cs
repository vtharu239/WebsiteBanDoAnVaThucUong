﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebsiteBanDoAnVaThucUong.Models.EF
{
    [Table("PromotionProduct")]
    public class PromotionProduct
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        [ForeignKey("Promotion")]
        public int PromotionId { get; set; }
        public bool IsBuyProduct { get; set; } // Thêm trường này
        public virtual Promotion Promotion { get; set; }
        public virtual Product Product { get; set; }
    }
}