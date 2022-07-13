using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.Services.Blogging
{
    public class BlogArticle
    {
        [Key]
        [Column("article_id")]
        public int ArticleId { get; set; }

        [Column("title")]
        [StringLength(40)]
        public string Title { get; set; }

        [Column("text")]
        [StringLength(255)]
        public string Text { get; set; }

        [Column("publish_date")]
        public DateTime PublishDate { get; set; }

        [Column("employee_id")]
        public int EmployeeId { get; set; }
    }
}
