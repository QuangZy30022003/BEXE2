using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Data.Enities.Feedback
{
    public class FeedbackCreateDto
    {
        public int? ProductId { get; set; }
        public int? WorkshopId { get; set; }
        [Range(1, 5)]
        public int Rating { get; set; }

        [StringLength(1000)]
        public string? Comment { get; set; }
    }

    public class FeedbackDto
    {
        public int FeedbackId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime? CreatedAt { get; set; }

        // Thông tin người dùng (ẩn các thông tin nhạy cảm)
        public int UserId { get; set; }
        public string UserFullName { get; set; } = string.Empty;

        // Nếu phản hồi về sản phẩm
        public int? ProductId { get; set; }

        // Nếu phản hồi về workshop
        public int? WorkshopId { get; set; }
        public string? WorkshopName { get; set; }
    }

}
