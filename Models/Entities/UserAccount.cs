using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace  ThirdWebseite.Models.Entities;

[Table("users")]
public class UserAccount
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("UserID")]
    public int UserID { get; set; }

    [Column("Username")]
    [MaxLength(100)]
    public string? UserName { get; set; }

    [Column("Email")]
    [MaxLength(255)]
    public string? Email { get; set; }

    [Column("PasswordHash")]
    [MaxLength(255)]
    public string? PasswordHash { get; set; }

    [Column("Role")]
    [MaxLength(50)]
    public string? Role { get; set; }

}
