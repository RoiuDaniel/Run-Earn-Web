using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public enum ItemType
    {
        Accessory,
        Clothing,

    }
    public class ShopItem
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }

        [Required]
        public int Price { get; set; }

        [DisplayName("Image")]
        public string ImgSource { get; set; }

        public ItemType Type { get; set; }
    }
}